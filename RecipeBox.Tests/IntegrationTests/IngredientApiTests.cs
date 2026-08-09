using Microsoft.Extensions.DependencyInjection;
using RecipeBox.Data;
using RecipeBox.DTOs.Ingredients;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace RecipeBox.Tests.IntegrationTests
{
    public class IngredientApiTests
    {
        [Fact]
        public async Task CreateIngredient_WithValidData_Returns201()
        {
            var factory = new CustomWebAppFactory();
            factory.InitDatabase();

            var client = factory.CreateClient();


            var request = new CreateIngredientRequest
            {
                Name = "Sugar",
                Unit = "kg"
            };

            var response = await client.PostAsJsonAsync(
                "/api/ingredients",
                request);

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                              .GetRequiredService<DBContextRecipeBox>();

                var ingredient = db.Ingredients
                                   .SingleOrDefault(i => i.Name == "Sugar");

                Assert.NotNull(ingredient);
                Assert.Equal("kg", ingredient.Unit);
            }

            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);
        }

        [Fact]
        public async Task CreateIngredient_WithDuplicateName_ReturnsErrorConflict()
        {
            var factory = new CustomWebAppFactory();
            factory.InitDatabase();

            var client = factory.CreateClient();


            var request = new CreateIngredientRequest
            {
                Name = "Sugar",
                Unit = "kg"
            };

            var response1 = await client.PostAsJsonAsync(
                "/api/ingredients",
                request);

            var response2 = await client.PostAsJsonAsync(
                "/api/ingredients",
                request);

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                              .GetRequiredService<DBContextRecipeBox>();

                var ingredients = db.Ingredients
                    .Where(i => i.Name == "Sugar")
                    .ToList();

                Assert.Single(ingredients);
            }

            Assert.Equal(
                HttpStatusCode.Created,
                response1.StatusCode);

            Assert.Equal(
                HttpStatusCode.Conflict,
                response2.StatusCode);
        }


        [Fact]
        public async Task CreateIngredient_WithEmptyName_ReturnsErrorValidation()
        {
            var factory = new CustomWebAppFactory();
            factory.InitDatabase();

            var client = factory.CreateClient();


            var request = new CreateIngredientRequest
            {
                Name = "    ",
                Unit = "kg"
            };

            var response = await client.PostAsJsonAsync(
                "/api/ingredients",
                request);

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                              .GetRequiredService<DBContextRecipeBox>();

                var ingredients = db.Ingredients
                    .ToList();

                Assert.Empty(ingredients);
            }

            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }
    }
}
