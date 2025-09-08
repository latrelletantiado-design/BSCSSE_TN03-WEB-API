namespace setupWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add controller services
            builder.Services.AddControllers();

            // Add Swagger/OpenAPI services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            // Enable Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            // Enable CORS
            app.UseCors("AllowLocalhost");

            app.MapControllers();
            app.Run();
        }
    }
}
