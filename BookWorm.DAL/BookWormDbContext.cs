using BookWorm.Model;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.DAL
{
    public class BookWormDbContext(DbContextOptions<BookWormDbContext> options) : DbContext(options)
    {
        public DbSet<BookTitle> BookTitles { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<TagLine> TagLines { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:1 BookTitle → TagLine
            modelBuilder.Entity<BookTitle>()
                .HasOne(b => b.TagLine)
                .WithOne(t => t.BookTitle)
                .HasForeignKey<TagLine>(t => t.BookTitleId)
                .IsRequired();

            // 1:N BookTitle → Author
            modelBuilder.Entity<BookTitle>()
                .HasOne(b => b.Author)
                .WithMany(a => a.BookTitles)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // N:1 BookTitle → Language
            modelBuilder.Entity<BookTitle>()
                .HasOne(b => b.Language)
                .WithMany(l => l.BookTitles)
                .HasForeignKey(b => b.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-many BookTitle ↔ Genre
            modelBuilder.Entity<BookTitle>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.BookTitles)
                .UsingEntity<Dictionary<string, object>>(
                    "BookTitleGenre",
                    b => b.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.Cascade),
                    g => g.HasOne<BookTitle>().WithMany().HasForeignKey("BookTitleId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("BookTitleId", "GenreId")
                );

            // Configurations for properties
            modelBuilder.Entity<BookTitle>().Property(b => b.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Author>().Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Author>().Property(a => a.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Author>().Property(a => a.Biography).IsRequired();
            modelBuilder.Entity<Author>().Property(a => a.NationalLiterature).IsRequired();
            modelBuilder.Entity<TagLine>().Property(t => t.Text).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<Genre>().Property(g => g.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Language>().Property(l => l.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Language>().HasData(
                    new Language { Id = 1, Name = "English" },
                    new Language { Id = 2, Name = "Croatian" },
                    new Language { Id = 3, Name = "German" },
                    new Language { Id = 4, Name = "French" },
                    new Language { Id = 5, Name = "Spanish" },
                    new Language { Id = 6, Name = "Italian" },
                    new Language { Id = 7, Name = "Japanese" }
                );
            
        }
    }
}