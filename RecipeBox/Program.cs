using Microsoft.EntityFrameworkCore;
using RecipeBox.Data;
using RecipeBox.ExceptionHandle;
using RecipeBox.Repository;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services;
using RecipeBox.Services.Interfaces;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//builder.Services.AddSingleton<IRecipeRepository, LocalRecipeRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IRecipeRepository, SQLRecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddDbContext<DBContextRecipeBox>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSerilog(configuration =>
{
    configuration
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(
            "Logs/log-.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 14,
            restrictedToMinimumLevel: LogEventLevel.Error);
});

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
