namespace BookWorm.WebAPI.Extensions

{
    public static class ServiceCollectionCorsExtensions
    {
        public static void AddCarsProjectCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowViteDev", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}
