using Microsoft.EntityFrameworkCore;
using VictuzAppMVC.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace VictuzAppMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
             
            // api controller hier toevoegen
            // builder.Services.AddScoped<ActiviteitenAPIController>(); // API Controller toevoegen (nog niet nodig)

            // Configuratie voor JSON om cyclische referenties te negeren
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Voeg services toe aan de container
            builder.Services.AddControllersWithViews();

            // Voeg de databasecontext toe met de juiste connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<VictuzAppContext>(options =>
                options.UseSqlServer(connectionString));

            // Voeg Swagger services toe (nog niet nodig voor API)
            // builder.Services.AddEndpointsApiExplorer();

            // builder.Services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo
            //     {
            //         Title = "VictuzAppMVC API",
            //         Version = "v1",
            //         Description = "API voor het beheren van activiteiten en aanmeldingen bij VictuzAppMVC",
            //         Contact = new OpenApiContact
            //         {
            //             Name = "Support Team",
            //             Email = "support@victuzapp.com"
            //         }
            //     });
            // });

            var app = builder.Build();

            // Configureer de HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // Gedetailleerde foutpagina tijdens ontwikkeling

                // Swagger configuratie voor API-documentatie (nog niet nodig)
                // app.UseSwagger();
                // app.UseSwaggerUI(c =>
                // {
                //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "VictuzAppMVC API v1");
                //     c.RoutePrefix = "swagger";
                // });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // API routes (nog niet nodig)
            // app.MapControllers();

            // MVC routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
