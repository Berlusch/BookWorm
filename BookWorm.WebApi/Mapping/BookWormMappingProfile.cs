using AutoMapper;
using BookWorm.Model;
using BookWorm.WebApi.DTO;

namespace BookWorm.WebAPI.Mapping
{
    public class BookWormMappingProfile : Profile
    {
        public BookWormMappingProfile()
        {            
            CreateMap<Author, AuthorReadDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
                        
            CreateMap<AuthorInsertUpdateDto, Author>();            
            CreateMap<Author, AuthorInsertUpdateDto>();
        }
    }
}
