using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using FluentAssertions;
using MockQueryable;
using Moq;
using Xunit;

namespace BookWorm.Service.Tests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _repositoryMock;
        private readonly AuthorService _service;

        // Dijeljeni testni podaci
        private readonly Author _authorTolkien = new()
        {
            Id = 1,
            FirstName = "John Ronald Reuel",
            LastName = "Tolkien",
            BirthYear = 1892,
            DeathYear = 1973,
            Biography = "English author and philologist.",
            NationalLiterature = "English"
        };

        private readonly Author _authorOrwell = new()
        {
            Id = 2,
            FirstName = "George",
            LastName = "Orwell",
            BirthYear = 1903,
            DeathYear = 1950,
            Biography = "English novelist and essayist.",
            NationalLiterature = "English"
        };

        public AuthorServiceTests()
        {
            _repositoryMock = new Mock<IAuthorRepository>();
            _service = new AuthorService(_repositoryMock.Object);
        }

        // -----------------------------------------------------------------------
        // GetAuthorsAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAuthorsAsync_ReturnsPagedResult_WithCorrectItems()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            var authors = new List<Author> { _authorTolkien, _authorOrwell }.BuildMock();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(authors);

            // Act
            var result = await _service.GetAuthorsAsync(paging, sorting, filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Paging.Should().Be(paging);
        }

        [Fact]
        public async Task GetAuthorsAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(new List<Author>().BuildMock());

            // Act
            var result = await _service.GetAuthorsAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetAuthorsAsync_RespectsPageSize()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 1 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            var authors = new List<Author> { _authorTolkien, _authorOrwell }.BuildMock();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(authors);

            // Act
            var result = await _service.GetAuthorsAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(2);
        }

        // -----------------------------------------------------------------------
        // GetAuthorByIdAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAuthorByIdAsync_ExistingId_ReturnsAuthor()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_authorTolkien);

            // Act
            var result = await _service.GetAuthorByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.FirstName.Should().Be("John Ronald Reuel");
            result.LastName.Should().Be("Tolkien");
            result.FullName.Should().Be("John Ronald Reuel Tolkien");
        }

        [Fact]
        public async Task GetAuthorByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Author)null!);

            // Act
            var result = await _service.GetAuthorByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // -----------------------------------------------------------------------
        // AddAuthorAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task AddAuthorAsync_ValidAuthor_ReturnsAddedAuthor()
        {
            // Arrange
            var newAuthor = new Author
            {
                FirstName = "Fyodor",
                LastName = "Dostoevsky",
                BirthYear = 1821,
                DeathYear = 1881,
                Biography = "Russian novelist.",
                NationalLiterature = "Russian"
            };
            var savedAuthor = new Author
            {
                Id = 3,
                FirstName = "Fyodor",
                LastName = "Dostoevsky",
                BirthYear = 1821,
                DeathYear = 1881,
                Biography = "Russian novelist.",
                NationalLiterature = "Russian"
            };

            _repositoryMock
                .Setup(r => r.AddAsync(newAuthor))
                .ReturnsAsync(savedAuthor);

            // Act
            var result = await _service.AddAuthorAsync(newAuthor);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(3);
            result.FirstName.Should().Be("Fyodor");
            result.LastName.Should().Be("Dostoevsky");

            _repositoryMock.Verify(r => r.AddAsync(newAuthor), Times.Once);
        }

        [Fact]
        public async Task AddAuthorAsync_LivingAuthor_NullDeathYear_ReturnsAddedAuthor()
        {
            // Arrange
            var newAuthor = new Author
            {
                FirstName = "Living",
                LastName = "Author",
                BirthYear = 1980,
                DeathYear = null,
                Biography = "Still writing.",
                NationalLiterature = "Croatian"
            };
            var savedAuthor = new Author
            {
                Id = 4,
                FirstName = "Living",
                LastName = "Author",
                BirthYear = 1980,
                DeathYear = null,
                Biography = "Still writing.",
                NationalLiterature = "Croatian"
            };

            _repositoryMock
                .Setup(r => r.AddAsync(newAuthor))
                .ReturnsAsync(savedAuthor);

            // Act
            var result = await _service.AddAuthorAsync(newAuthor);

            // Assert
            result.Should().NotBeNull();
            result.DeathYear.Should().BeNull();
            result.BirthYear.Should().Be(1980);
        }

        // -----------------------------------------------------------------------
        // UpdateAuthorAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateAuthorAsync_ExistingId_UpdatesAllFields()
        {
            // Arrange
            var updateData = new Author
            {
                FirstName = "J.R.R.",
                LastName = "Tolkien",
                BirthYear = 1892,
                DeathYear = 1973,
                Biography = "Updated biography.",
                NationalLiterature = "British"
            };
            var updatedAuthor = new Author
            {
                Id = 1,
                FirstName = "J.R.R.",
                LastName = "Tolkien",
                BirthYear = 1892,
                DeathYear = 1973,
                Biography = "Updated biography.",
                NationalLiterature = "British"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_authorTolkien);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Author>()))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await _service.UpdateAuthorAsync(1, updateData);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("J.R.R.");
            result.LastName.Should().Be("Tolkien");
            result.Biography.Should().Be("Updated biography.");
            result.NationalLiterature.Should().Be("British");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthorAsync_SetDeathYearToNull_UpdatesSuccessfully()
        {
            // Arrange
            var updateData = new Author
            {
                FirstName = "George",
                LastName = "Orwell",
                BirthYear = 1903,
                DeathYear = null, 
                Biography = "English novelist.",
                NationalLiterature = "English"
            };
            var updatedAuthor = new Author
            {
                Id = 2,
                FirstName = "George",
                LastName = "Orwell",
                BirthYear = 1903,
                DeathYear = null,
                Biography = "English novelist.",
                NationalLiterature = "English"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(_authorOrwell);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Author>()))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await _service.UpdateAuthorAsync(2, updateData);

            // Assert
            result.DeathYear.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAuthorAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Author)null!);

            // Act
            var act = async () => await _service.UpdateAuthorAsync(99, new Author
            {
                FirstName = "X",
                LastName = "Y",
                Biography = "Z",
                NationalLiterature = "W"
            });

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*99*");
        }

        [Fact]
        public async Task UpdateAuthorAsync_NonExistingId_DoesNotCallUpdate()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Author)null!);

            // Act
            try
            {
                await _service.UpdateAuthorAsync(99, new Author
                {
                    FirstName = "X",
                    LastName = "Y",
                    Biography = "Z",
                    NationalLiterature = "W"
                });
            }
            catch (KeyNotFoundException) { }

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Author>()), Times.Never);
        }

        // -----------------------------------------------------------------------
        // DeleteAuthorAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteAuthorAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAuthorAsync(1);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAuthorAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}