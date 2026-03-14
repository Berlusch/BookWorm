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
    public class GenreServiceTests
    {
        private readonly Mock<IGenreRepository> _repositoryMock;
        private readonly GenreService _service;

        // Dijeljeni testni podaci
        private readonly Genre _genreFantasy = new() { Id = 1, Name = "Fantasy", Description = "Fantasy books" };
        private readonly Genre _genreThriller = new() { Id = 2, Name = "Thriller", Description = "Thriller books" };

        public GenreServiceTests()
        {
            _repositoryMock = new Mock<IGenreRepository>();
            _service = new GenreService(_repositoryMock.Object);
        }

        // -----------------------------------------------------------------------
        // GetGenresAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetGenresAsync_ReturnsPagedResult_WithCorrectItems()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            var genres = new List<Genre> { _genreFantasy, _genreThriller }.BuildMock();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(genres);

            // Act
            var result = await _service.GetGenresAsync(paging, sorting, filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Paging.Should().Be(paging);
        }

        [Fact]
        public async Task GetGenresAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(new List<Genre>().BuildMock());

            // Act
            var result = await _service.GetGenresAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetGenresAsync_RespectsPageSize()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 1 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            var genres = new List<Genre> { _genreFantasy, _genreThriller }.BuildMock();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(genres);

            // Act
            var result = await _service.GetGenresAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(2); // ukupno u bazi
        }

        // -----------------------------------------------------------------------
        // GetGenreByIdAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetGenreByIdAsync_ExistingId_ReturnsGenre()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_genreFantasy);

            // Act
            var result = await _service.GetGenreByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Fantasy");
            result.Description.Should().Be("Fantasy books");
        }

        [Fact]
        public async Task GetGenreByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Genre)null!);

            // Act
            var result = await _service.GetGenreByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // -----------------------------------------------------------------------
        // AddGenreAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task AddGenreAsync_ValidGenre_ReturnsAddedGenre()
        {
            // Arrange
            var newGenre = new Genre { Name = "Horror", Description = "Horror books" };
            var savedGenre = new Genre { Id = 3, Name = "Horror", Description = "Horror books" };

            _repositoryMock
                .Setup(r => r.AddAsync(newGenre))
                .ReturnsAsync(savedGenre);

            // Act
            var result = await _service.AddGenreAsync(newGenre);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(3);
            result.Name.Should().Be("Horror");
            result.Description.Should().Be("Horror books");

            _repositoryMock.Verify(r => r.AddAsync(newGenre), Times.Once);
        }

        // -----------------------------------------------------------------------
        // UpdateGenreAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateGenreAsync_ExistingId_UpdatesNameAndDescription()
        {
            // Arrange
            var updateData = new Genre { Name = "Dark Fantasy", Description = "Dark fantasy books" };
            var updatedGenre = new Genre { Id = 1, Name = "Dark Fantasy", Description = "Dark fantasy books" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_genreFantasy);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Genre>()))
                .ReturnsAsync(updatedGenre);

            // Act
            var result = await _service.UpdateGenreAsync(1, updateData);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Dark Fantasy");
            result.Description.Should().Be("Dark fantasy books");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Genre)null!);

            // Act
            var act = async () => await _service.UpdateGenreAsync(99, new Genre { Name = "X", Description = "Y" });

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*99*");
        }

        [Fact]
        public async Task UpdateGenreAsync_NonExistingId_DoesNotCallUpdate()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Genre)null!);

            // Act
            try { await _service.UpdateGenreAsync(99, new Genre { Name = "X", Description = "Y" }); }
            catch (KeyNotFoundException) { }

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Genre>()), Times.Never);
        }

        // -----------------------------------------------------------------------
        // DeleteGenreAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteGenreAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteGenreAsync(1);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteGenreAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}