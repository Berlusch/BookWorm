using Bookworm.Repository;
using Bookworm.Repository.Common;
using BookWorm.DAL;
using BookWorm.Service;
using BookWorm.Service.Common;
using BookWorm.WebAPI.Extensions;
using BookWorm.WebAPI.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BookWorm.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            builder.Services.AddAutoMapper(typeof(BookWormMappingProfile));

            builder.Services.AddScoped<IAuthorService, AuthorService>();           

           
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();


            builder.Services.AddDbContext<BookWormDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // CORS
            builder.Services.AddBookWormCORS();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            
            app.UseCors("AllowViteDev");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}