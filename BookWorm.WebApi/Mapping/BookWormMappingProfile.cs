
    using AutoMapper;
    using BookWorm.Model;
    using BookWorm.WebApi.DTO;

    namespace BookWorm.WebAPI.Mapping

{ 
    public class BookWormMappingProfile : Profile
    {
        public BookWormMappingProfile()
        {

            CreateMap<Author, AuthorReadDto>();
            CreateMap<AuthorInsertUpdateDto, Author>();
            CreateMap<Author, AuthorInsertUpdateDto>();

            CreateMap<Language, LanguageReadDto>();
            
            CreateMap<Genre, GenreReadDto>();
            CreateMap<GenreInsertUpdateDto, Genre>();

            CreateMap<BookTitle, BookTitleReadDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Name))
                .ForMember(dest => dest.TagLineText, opt => opt.MapFrom(src => src.TagLine.Text))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));

            CreateMap<BookTitleInsertUpdateDto, BookTitle>();
        }
    }

}
