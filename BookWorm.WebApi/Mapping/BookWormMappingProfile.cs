
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
        }
    }

}
