
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
                    .ForMember(dest => dest.BookQuotes, opt => opt.MapFrom(src => src.BookQuotes.Select(q => q.Text).ToList()))
                    .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));
            CreateMap<BookTitleInsertUpdateDto, BookTitle>();
          
            CreateMap<BookQuote, BookQuoteReadDto>()
            .ForMember(dest => dest.BookTitleName, opt => opt.MapFrom(src => src.BookTitle.Title));
            CreateMap<BookQuoteInsertUpdateDto, BookQuote>();
        }
    }

}
