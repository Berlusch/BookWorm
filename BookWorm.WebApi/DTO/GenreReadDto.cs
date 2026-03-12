namespace BookWorm.WebApi.DTO
{
    public class GenreReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}