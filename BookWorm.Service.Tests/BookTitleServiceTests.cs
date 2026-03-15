using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using FluentAssertions;
using MockQueryable;
using Moq;
using Xunit;

namespace BookWorm.Service.Tests
{
    public class BookTitleServiceTests
    {
        private readonly Mock<IBookTitleRepository> _bookTitleRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly BookTitleService _service;
        
        private readonly Author _author = new()
        {
            Id = 1,
            FirstName = "John Ronald Reuel",
            LastName = "Tolkien",
            Biography = "English author.",
            NationalLiterature = "English"
        };

        private readonly Language _language = new()
        {
            Id = 1,
            Name = "English"
        };

        private readonly Genre _genreFantasy = new() { Id = 1, Name = "Fantasy", Description = "Fantasy books" };
        private readonly Genre _genreAdventure = new() { Id = 2, Name = "Adventure", Description = "Adventure books" };

        private readonly BookTitle _bookTitleOne;
        private readonly BookTitle _bookTitleTwo;

        public BookTitleServiceTests()
        {
            _bookTitleRepositoryMock = new Mock<IBookTitleRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _service = new BookTitleService(_bookTitleRepositoryMock.Object, _genreRepositoryMock.Object);

            _bookTitleOne = new BookTitle
            {
                Id = 1,
                Title = "The Lord of the Rings",
                Subtitle = "The Fellowship of the Ring",
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreFantasy, _genreAdventure]
            };

            _bookTitleTwo = new BookTitle
            {
                Id = 2,
                Title = "The Hobbit",
                Subtitle = null,
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreFantasy]
            };
        }

        // Helper - set GetQuery mock on BookTitleRepository
        private void SetupBookTitleGetQuery(List<BookTitle> bookTitles)
        {
            _bookTitleRepositoryMock
                .Setup(r => r.GetQuery(
                    It.IsAny<PagingParameters>(),
                    It.IsAny<SortingParameters>(),
                    It.IsAny<FilterParameters>()))
                .Returns(bookTitles.BuildMock());
        }

        // Helper - set GetQuery mock on GenreRepository
        private void SetupGenreGetQuery(List<Genre> genres)
        {
            _genreRepositoryMock
                .Setup(r => r.GetQuery(
                    It.IsAny<PagingParameters>(),
                    It.IsAny<SortingParameters>(),
                    It.IsAny<FilterParameters>()))
                .Returns(genres.BuildMock());
        }

        // -----------------------------------------------------------------------
        // GetBookTitlesAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetBookTitlesAsync_ReturnsPagedResult_WithCorrectItems()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupBookTitleGetQuery([_bookTitleOne, _bookTitleTwo]);

            // Act
            var result = await _service.GetBookTitlesAsync(paging, sorting, filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Paging.Should().Be(paging);
        }

        [Fact]
        public async Task GetBookTitlesAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupBookTitleGetQuery([]);

            // Act
            var result = await _service.GetBookTitlesAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetBookTitlesAsync_RespectsPageSize()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 1 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupBookTitleGetQuery([_bookTitleOne, _bookTitleTwo]);

            // Act
            var result = await _service.GetBookTitlesAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetBookTitlesAsync_ItemsIncludeAuthorAndLanguage()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupBookTitleGetQuery([_bookTitleOne]);

            // Act
            var result = await _service.GetBookTitlesAsync(paging, sorting, filter);

            // Assert
            result.Items[0].Author.Should().NotBeNull();
            result.Items[0].Author.FullName.Should().Be("John Ronald Reuel Tolkien");
            result.Items[0].Language.Should().NotBeNull();
            result.Items[0].Language.Name.Should().Be("English");
        }

        [Fact]
        public async Task GetBookTitlesAsync_ItemsIncludeGenres()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            SetupBookTitleGetQuery([_bookTitleOne]);

            // Act
            var result = await _service.GetBookTitlesAsync(paging, sorting, filter);

            // Assert
            result.Items[0].Genres.Should().HaveCount(2);
            result.Items[0].Genres.Should().Contain(g => g.Name == "Fantasy");
            result.Items[0].Genres.Should().Contain(g => g.Name == "Adventure");
        }

        // -----------------------------------------------------------------------
        // GetByIdWithDetailsAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetByIdWithDetailsAsync_ExistingId_ReturnsBookTitle()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(_bookTitleOne);

            // Act
            var result = await _service.GetByIdWithDetailsAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Title.Should().Be("The Lord of the Rings");
            result.Subtitle.Should().Be("The Fellowship of the Ring");
            result.Author.Should().NotBeNull();
            result.Language.Should().NotBeNull();
            result.Genres.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_BookTitleWithNullSubtitle_ReturnsBookTitle()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(2))
                .ReturnsAsync(_bookTitleTwo);

            // Act
            var result = await _service.GetByIdWithDetailsAsync(2);

            // Assert
            result.Should().NotBeNull();
            result!.Subtitle.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(99))
                .ReturnsAsync((BookTitle?)null);

            // Act
            var result = await _service.GetByIdWithDetailsAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // -----------------------------------------------------------------------
        // AddBookTitleAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task AddBookTitleAsync_ValidData_ReturnsAddedBookTitle()
        {
            // Arrange
            var newBookTitle = new BookTitle
            {
                Title = "The Silmarillion",
                Subtitle = null,
                AuthorId = 1,
                LanguageId = 1
            };
            var savedBookTitle = new BookTitle
            {
                Id = 3,
                Title = "The Silmarillion",
                Subtitle = null,
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreFantasy]
            };

            SetupGenreGetQuery([_genreFantasy, _genreAdventure]);

            _bookTitleRepositoryMock
                .Setup(r => r.AddAsync(newBookTitle))
                .ReturnsAsync(savedBookTitle);

            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(3))
                .ReturnsAsync(savedBookTitle);

            // Act
            var result = await _service.AddBookTitleAsync(newBookTitle, [1]);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(3);
            result.Title.Should().Be("The Silmarillion");
            result.Author.Should().NotBeNull();
            result.Genres.Should().HaveCount(1);

            _bookTitleRepositoryMock.Verify(r => r.AddAsync(newBookTitle), Times.Once);
        }

        [Fact]
        public async Task AddBookTitleAsync_AssignsCorrectGenres()
        {
            // Arrange
            var newBookTitle = new BookTitle
            {
                Title = "New Book",
                AuthorId = 1,
                LanguageId = 1
            };
            var savedBookTitle = new BookTitle
            {
                Id = 4,
                Title = "New Book",
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreFantasy, _genreAdventure]
            };

            SetupGenreGetQuery([_genreFantasy, _genreAdventure]);

            _bookTitleRepositoryMock
                .Setup(r => r.AddAsync(newBookTitle))
                .ReturnsAsync(savedBookTitle);

            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(4))
                .ReturnsAsync(savedBookTitle);

            // Act
            var result = await _service.AddBookTitleAsync(newBookTitle, [1, 2]);

            // Assert
            result.Genres.Should().HaveCount(2);
            result.Genres.Should().Contain(g => g.Id == 1);
            result.Genres.Should().Contain(g => g.Id == 2);
        }

        [Fact]
        public async Task AddBookTitleAsync_EmptyGenreIds_AssignsNoGenres()
        {
            // Arrange
            var newBookTitle = new BookTitle
            {
                Title = "No Genre Book",
                AuthorId = 1,
                LanguageId = 1
            };
            var savedBookTitle = new BookTitle
            {
                Id = 5,
                Title = "No Genre Book",
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = []
            };

            SetupGenreGetQuery([_genreFantasy, _genreAdventure]);

            _bookTitleRepositoryMock
                .Setup(r => r.AddAsync(newBookTitle))
                .ReturnsAsync(savedBookTitle);

            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(5))
                .ReturnsAsync(savedBookTitle);

            // Act
            var result = await _service.AddBookTitleAsync(newBookTitle, []);

            // Assert
            result.Genres.Should().BeEmpty();
        }

        // -----------------------------------------------------------------------
        // UpdateBookTitleAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateBookTitleAsync_ExistingId_UpdatesAllFields()
        {
            // Arrange
            var updateData = new BookTitle
            {
                Title = "Updated Title",
                Subtitle = "Updated Subtitle",
                AuthorId = 1,
                LanguageId = 1
            };
            var updatedBookTitle = new BookTitle
            {
                Id = 1,
                Title = "Updated Title",
                Subtitle = "Updated Subtitle",
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreFantasy]
            };

            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(_bookTitleOne);

            SetupGenreGetQuery([_genreFantasy, _genreAdventure]);

            _bookTitleRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<BookTitle>()))
                .ReturnsAsync(updatedBookTitle);

            _bookTitleRepositoryMock
                .SetupSequence(r => r.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(_bookTitleOne)
                .ReturnsAsync(updatedBookTitle);

            // Act
            var result = await _service.UpdateBookTitleAsync(1, updateData, [1]);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Updated Title");
            result.Subtitle.Should().Be("Updated Subtitle");

            _bookTitleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<BookTitle>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBookTitleAsync_UpdatesGenres()
        {
            // Arrange
            var updateData = new BookTitle
            {
                Title = "The Lord of the Rings",
                Subtitle = "The Fellowship of the Ring",
                AuthorId = 1,
                LanguageId = 1
            };
            var updatedBookTitle = new BookTitle
            {
                Id = 1,
                Title = "The Lord of the Rings",
                Subtitle = "The Fellowship of the Ring",
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreAdventure]
            };

            SetupGenreGetQuery([_genreFantasy, _genreAdventure]);

            _bookTitleRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<BookTitle>()))
                .ReturnsAsync(updatedBookTitle);

            _bookTitleRepositoryMock
                .SetupSequence(r => r.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(_bookTitleOne)
                .ReturnsAsync(updatedBookTitle);

            // Act
            var result = await _service.UpdateBookTitleAsync(1, updateData, [2]);

            // Assert
            result.Genres.Should().HaveCount(1);
            result.Genres.Should().Contain(g => g.Name == "Adventure");
        }

        [Fact]
        public async Task UpdateBookTitleAsync_ClearsSubtitle_WhenSetToNull()
        {
            // Arrange
            var updateData = new BookTitle
            {
                Title = "The Lord of the Rings",
                Subtitle = null,
                AuthorId = 1,
                LanguageId = 1
            };
            var updatedBookTitle = new BookTitle
            {
                Id = 1,
                Title = "The Lord of the Rings",
                Subtitle = null,
                AuthorId = 1,
                Author = _author,
                LanguageId = 1,
                Language = _language,
                Genres = [_genreFantasy]
            };

            SetupGenreGetQuery([_genreFantasy]);

            _bookTitleRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<BookTitle>()))
                .ReturnsAsync(updatedBookTitle);

            _bookTitleRepositoryMock
                .SetupSequence(r => r.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(_bookTitleOne)
                .ReturnsAsync(updatedBookTitle);

            // Act
            var result = await _service.UpdateBookTitleAsync(1, updateData, [1]);

            // Assert
            result.Subtitle.Should().BeNull();
        }

        [Fact]
        public async Task UpdateBookTitleAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(99))
                .ReturnsAsync((BookTitle?)null);

            // Act
            var act = async () => await _service.UpdateBookTitleAsync(99, new BookTitle
            {
                Title = "X",
                AuthorId = 1,
                LanguageId = 1
            }, [1]);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*99*");
        }

        [Fact]
        public async Task UpdateBookTitleAsync_NonExistingId_DoesNotCallUpdate()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(99))
                .ReturnsAsync((BookTitle?)null);

            // Act
            try
            {
                await _service.UpdateBookTitleAsync(99, new BookTitle
                {
                    Title = "X",
                    AuthorId = 1,
                    LanguageId = 1
                }, [1]);
            }
            catch (KeyNotFoundException) { }

            // Assert
            _bookTitleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<BookTitle>()), Times.Never);
        }

        // -----------------------------------------------------------------------
        // DeleteBookTitleAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteBookTitleAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteBookTitleAsync(1);

            // Assert
            result.Should().BeTrue();
            _bookTitleRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteBookTitleAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _bookTitleRepositoryMock
                .Setup(r => r.DeleteAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteBookTitleAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}