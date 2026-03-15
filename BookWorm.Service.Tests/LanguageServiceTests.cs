using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using FluentAssertions;
using MockQueryable;
using Moq;
using Xunit;

namespace BookWorm.Service.Tests
{
    public class LanguageServiceTests
    {
        private readonly Mock<ILanguageRepository> _repositoryMock;
        private readonly LanguageService _service;
       
        private readonly Language _languageEnglish = new() { Id = 1, Name = "English" };
        private readonly Language _languageCroatian = new() { Id = 2, Name = "Croatian" };

        public LanguageServiceTests()
        {
            _repositoryMock = new Mock<ILanguageRepository>();
            _service = new LanguageService(_repositoryMock.Object);
        }

        // -----------------------------------------------------------------------
        // GetLanguagesAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetLanguagesAsync_ReturnsPagedResult_WithCorrectItems()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            var languages = new List<Language> { _languageEnglish, _languageCroatian }.BuildMock();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(languages);

            // Act
            var result = await _service.GetLanguagesAsync(paging, sorting, filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Paging.Should().Be(paging);
        }

        [Fact]
        public async Task GetLanguagesAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 10 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(new List<Language>().BuildMock());

            // Act
            var result = await _service.GetLanguagesAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetLanguagesAsync_RespectsPageSize()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 1, PageSize = 1 };
            var sorting = new SortingParameters();
            var filter = new FilterParameters();

            var languages = new List<Language> { _languageEnglish, _languageCroatian }.BuildMock();

            _repositoryMock
                .Setup(r => r.GetQuery(paging, sorting, filter))
                .Returns(languages);

            // Act
            var result = await _service.GetLanguagesAsync(paging, sorting, filter);

            // Assert
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(2); 
        }

        // -----------------------------------------------------------------------
        // GetLanguageByIdAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetLanguageByIdAsync_ExistingId_ReturnsLanguage()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_languageEnglish);

            // Act
            var result = await _service.GetLanguageByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("English");
        }

        [Fact]
        public async Task GetLanguageByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Language)null!);

            // Act
            var result = await _service.GetLanguageByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // -----------------------------------------------------------------------
        // AddLanguageAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task AddLanguageAsync_ValidLanguage_ReturnsAddedLanguage()
        {
            // Arrange
            var newLanguage = new Language { Name = "German" };
            var savedLanguage = new Language { Id = 3, Name = "German" };

            _repositoryMock
                .Setup(r => r.AddAsync(newLanguage))
                .ReturnsAsync(savedLanguage);

            // Act
            var result = await _service.AddLanguageAsync(newLanguage);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(3);
            result.Name.Should().Be("German");

            _repositoryMock.Verify(r => r.AddAsync(newLanguage), Times.Once);
        }

        // -----------------------------------------------------------------------
        // UpdateLanguageAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateLanguageAsync_ExistingId_UpdatesAndReturnsLanguage()
        {
            // Arrange
            var updateData = new Language { Name = "British English" };
            var updatedLanguage = new Language { Id = 1, Name = "British English" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(_languageEnglish);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Language>()))
                .ReturnsAsync(updatedLanguage);

            // Act
            var result = await _service.UpdateLanguageAsync(1, updateData);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("British English");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Language>()), Times.Once);
        }

        [Fact]
        public async Task UpdateLanguageAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Language)null!);

            // Act
            var act = async () => await _service.UpdateLanguageAsync(99, new Language { Name = "X" });

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*99*");
        }

        [Fact]
        public async Task UpdateLanguageAsync_NonExistingId_DoesNotCallUpdate()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Language)null!);

            // Act
            try { await _service.UpdateLanguageAsync(99, new Language { Name = "X" }); }
            catch (KeyNotFoundException) { }

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Language>()), Times.Never);
        }

        // -----------------------------------------------------------------------
        // DeleteLanguageAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteLanguageAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteLanguageAsync(1);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteLanguageAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.DeleteAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteLanguageAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}