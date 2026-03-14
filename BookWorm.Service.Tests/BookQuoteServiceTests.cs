using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service;
using FluentAssertions;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace BookWorm.Service.Tests
{
    public class BookQuoteServiceTests
    {
        private readonly Mock<IBookQuoteRepository> _repositoryMock;
        private readonly BookQuoteService _service;

        // Dijeljeni testni podaci
        private readonly BookTitle _bookTitle = new()
        {
            Id = 1,
            Title = "The Lord of the Rings"
        };

        private readonly BookQuote _quoteOne;
        private readonly BookQuote _quoteTwo;

        public BookQuoteServiceTests()
        {
            _repositoryMock = new Mock<IBookQuoteRepository>();
            _service = new BookQuoteService(_repositoryMock.Object);

            _quoteOne = new BookQuote
            {
                Id = 1,
                Text = "Not all those who wander are lost.",
                BookTitleId = 1,
                BookTitle = _bookTitle
            };

            _quoteTwo = new BookQuote
            {
                Id = 2,
                Text = "Even the smallest person can change the course of the future.",
                BookTitleId = 1,
                BookTitle = _bookTitle
            };
        }

        // Helper - postavlja GetQuery mock s listom citata
        private void SetupGetQuery(List<BookQuote> quotes)
        {
            _repositoryMock
                .Setup(r => r.GetQuery(
                    It.IsAny<PagingParameters>(),
                    It.IsAny<SortingParameters>(),
                    It.IsAny<FilterParameters>()))
                .Returns(quotes.BuildMock());
        }

        // -----------------------------------------------------------------------
        // GetBookQuotesAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetBookQuotesAsync_ReturnsPagedResult_WithCorrectItems()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupGetQuery(new List<BookQuote> { _quoteOne, _quoteTwo });

            // Act
            var result = await _service.GetBookQuotesAsync(paging, sorting, filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Paging.Should().Be(paging);
        }

        [Fact]
        public async Task GetBookQuotesAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupGetQuery(new List<BookQuote>());

            // Act
            var result = await _service.GetBookQuotesAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetBookQuotesAsync_RespectsPageSize()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 1 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupGetQuery(new List<BookQuote> { _quoteOne, _quoteTwo });

            // Act
            var result = await _service.GetBookQuotesAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetBookQuotesAsync_ItemsIncludeBookTitle()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupGetQuery(new List<BookQuote> { _quoteOne });

            // Act
            var result = await _service.GetBookQuotesAsync(paging, sorting, filter);

            // Assert
            result.Items[0].BookTitle.Should().NotBeNull();
            result.Items[0].BookTitle.Title.Should().Be("The Lord of the Rings");
        }

        // -----------------------------------------------------------------------
        // GetBookQuoteByIdAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetBookQuoteByIdAsync_ExistingId_ReturnsBookQuote()
        {
            // Arrange
            SetupGetQuery(new List<BookQuote> { _quoteOne, _quoteTwo });

            // Act
            var result = await _service.GetBookQuoteByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("Not all those who wander are lost.");
            result.BookTitle.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBookQuoteByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            SetupGetQuery(new List<BookQuote> { _quoteOne, _quoteTwo });

            // Act
            var act = async () => await _service.GetBookQuoteByIdAsync(99);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*99*");
        }

        // -----------------------------------------------------------------------
        // AddBookQuoteAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task AddBookQuoteAsync_ValidQuote_ReturnsAddedQuoteWithBookTitle()
        {
            // Arrange
            var newQuote = new BookQuote
            {
                Text = "In the beginning was the Word.",
                BookTitleId = 1
            };
            var savedQuote = new BookQuote
            {
                Id = 3,
                Text = "In the beginning was the Word.",
                BookTitleId = 1,
                BookTitle = _bookTitle
            };

            _repositoryMock
                .Setup(r => r.AddAsync(newQuote))
                .ReturnsAsync(savedQuote);

            SetupGetQuery(new List<BookQuote> { _quoteOne, _quoteTwo, savedQuote });

            // Act
            var result = await _service.AddBookQuoteAsync(newQuote);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(3);
            result.Text.Should().Be("In the beginning was the Word.");
            result.BookTitle.Should().NotBeNull();

            _repositoryMock.Verify(r => r.AddAsync(newQuote), Times.Once);
        }

        // -----------------------------------------------------------------------
        // UpdateBookQuoteAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateBookQuoteAsync_ExistingId_UpdatesTextAndBookTitleId()
        {
            // Arrange
            var updateData = new BookQuote
            {
                Text = "Updated quote text.",
                BookTitleId = 1
            };
            var updatedQuote = new BookQuote
            {
                Id = 1,
                Text = "Updated quote text.",
                BookTitleId = 1,
                BookTitle = _bookTitle
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_quoteOne);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<BookQuote>()))
                .ReturnsAsync(updatedQuote);

            SetupGetQuery(new List<BookQuote> { updatedQuote, _quoteTwo });

            // Act
            var result = await _service.UpdateBookQuoteAsync(1, updateData);

            // Assert
            result.Should().NotBeNull();
            result.Text.Should().Be("Updated quote text.");
            result.BookTitle.Should().NotBeNull();

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<BookQuote>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBookQuoteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((BookQuote)null!);

            // Act
            var act = async () => await _service.UpdateBookQuoteAsync(99, new BookQuote { Text = "X", BookTitleId = 1 });

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*99*");
        }

        [Fact]
        public async Task UpdateBookQuoteAsync_NonExistingId_DoesNotCallUpdate()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((BookQuote)null!);

            // Act
            try { await _service.UpdateBookQuoteAsync(99, new BookQuote { Text = "X", BookTitleId = 1 }); }
            catch (KeyNotFoundException) { }

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<BookQuote>()), Times.Never);
        }

        // -----------------------------------------------------------------------
        // DeleteBookQuoteAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteBookQuoteAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteBookQuoteAsync(1);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteBookQuoteAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteBookQuoteAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}