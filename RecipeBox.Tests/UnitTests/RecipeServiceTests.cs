using Moq;
using RecipeBox.Common;
using RecipeBox.DTOs.Recipe;
using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Tests.UnitTests
{
    public class RecipeServiceTests
    {

        // проверить корректность построения респонза? например, что ингредиенты в рецепте и в респонзе одни и те же
        [Fact]
        public async Task Create_WithTwoIngredients_ReturnsRecipeWithExactTwoIngredients()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                    .ReturnsAsync((Recipe i) => i);
            mockIngrRepo.Setup(s => s.GetByIdAsync(1))
                           .ReturnsAsync(new Ingredient("Ингред1", "Кг."));
            mockIngrRepo.Setup(s => s.GetByIdAsync(3))
                           .ReturnsAsync(new Ingredient("Ингред3", "г."));

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var recIngReq = new List<CreateRecipeIngredientRequest>();
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 2.0m });
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 3, Amount = 2.0m });
            var recReq = new CreateRecipeRequest { Name = "Борщ", RecipeIngredients = recIngReq };

            var res = await service.CreateAsync(recReq);

            Assert.NotNull(res.Value);
            Assert.Equal("Ингред1", res.Value.Ingredients[0].IngredientName);
            Assert.Equal("Ингред3", res.Value.Ingredients[1].IngredientName);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Once);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));

        }

        // проверка при несуществующем ингредиенте (вызов create 0 раз)
        [Fact]
        public async Task Create_WithInvalidIngredients_ReturnsErrorNotFound()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                    .ReturnsAsync((Recipe i) => i);
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Ingredient?)null);

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var recIngReq = new List<CreateRecipeIngredientRequest>();
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 2.0m });
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 3, Amount = 2.0m });
            var recReq = new CreateRecipeRequest { Name = "Борщ" , RecipeIngredients = recIngReq};

            var res = await service.CreateAsync(recReq);

            Assert.Null(res.Value);
            Assert.Equal(ErrorType.NotFound, res.ErrorType);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Never);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Once);

        }

        // проверка со всеми корректными данными и пустым ингредиентом (вызов create 1 раз)
        [Fact]
        public async Task Create_WithEmptyIngredients_ReturnsRecipeResponse()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                    .ReturnsAsync((Recipe i) => i);
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var recReq = new CreateRecipeRequest { Name = "Борщ"};

            var res = await service.CreateAsync(recReq);

            Assert.Equal("Борщ", res.Value.Name);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Once);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Never);

        }

        // проверка со всеми корректными данными и заполненным ингредиентом (вызов create 1 раз)
        [Fact]
        public async Task Create_WithIngredients_ReturnsRecipeResponse()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                    .ReturnsAsync((Recipe i) => i);
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var recIngReq = new List<CreateRecipeIngredientRequest>();
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 2.0m });
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 2, Amount = 3.0m });
            var recReq = new CreateRecipeRequest { Name = "Борщ", RecipeIngredients = recIngReq };

            var res = await service.CreateAsync(recReq);

            Assert.Equal("Борщ", res.Value.Name);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Once);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));

        }

        // проверка на пустые значения имени и с корректными данными (вызов create 0 раз)
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Create_WithEmptyName_ReturnsErrorValidation(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                    .ReturnsAsync((Recipe i) => i);
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var recReq = new CreateRecipeRequest{Name = name};

            var res = await service.CreateAsync(recReq);

            Assert.Null(res.Value);
            Assert.Equal(ErrorType.Validation, res.ErrorType);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Never);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Never);

        }
        // проверка на пустые значения имени и с корректными данными (вызов create 0 раз)
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Update_WithEmptyName_ReturnsErrorValidation(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()))
                    .ReturnsAsync(true);
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Ingredient("Соль", "г"));

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var updRecReq = new UpdateRecipeRequest { Name = name };

            var res = await service.UpdateAsync(1, updRecReq);

            Assert.Equal(ErrorType.Validation, res.ErrorType);
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Never);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }
        // проверка при несуществующем ингредиенте (вызов create 0 раз)
        [Fact]
        public async Task Update_WithInvalidIngredient_ReturnsErrorNotFound()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()))
                    .ReturnsAsync(false);

            var mockIngrRepo = new Mock<IIngredientRepository>();
            var testIngredient = new Ingredient("Соль", "г");
            mockIngrRepo.Setup(s => s.GetByIdAsync(1))
                           .ReturnsAsync(testIngredient);
            mockIngrRepo.Setup(s => s.GetByIdAsync(3))
                           .ReturnsAsync((Ingredient?)null);

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var ingrReqList = new List<CreateRecipeIngredientRequest>();
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 1 , Amount = 1.0m});
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 3 , Amount = 1.0m});
            var updRecReq = new UpdateRecipeRequest { Name = "name", RecipeIngredients = ingrReqList };

            var res = await service.UpdateAsync(1, updRecReq);

            Assert.Equal(ErrorType.NotFound, res.ErrorType);
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Never);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }
        // проверка со всеми корректными данными (вызов create 1 раз)
        [Fact]
        public async Task Update_WithValidIngredient_ReturnsSuccess()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()))
                    .ReturnsAsync(true);

            var mockIngrRepo = new Mock<IIngredientRepository>();
            var testIngredient = new Ingredient("Соль", "г");
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(testIngredient);

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var ingrReqList = new List<CreateRecipeIngredientRequest>();
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 1.0m });
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 3, Amount = 1.0m });
            var updRecReq = new UpdateRecipeRequest { Name = "name", RecipeIngredients = ingrReqList };

            var res = await service.UpdateAsync(1, updRecReq);

            Assert.True(res.IsSuccess);
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Once);
            mockIngrRepo.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        // проверка на возврат пустого значения при пустом/отсутствующем имени
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetByName_WithEmptyNameOrNoMatches_ReturnsErrorValidation(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(Enumerable.Empty<Recipe>());
            var mockIngrRepo = new Mock<IIngredientRepository>();

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);

            var res = await service.GetByNameAsync(name);

            Assert.Equal(ErrorType.Validation, res.ErrorType);
            mockRepo.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
        }

        // проверка на возврат пустого значения при отсутствии совпадений
        [Theory]
        [InlineData("Омлет")]
        public async Task GetByName_WithNoMatches_ReturnsEmptyEnumerableResponse(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(Enumerable.Empty<Recipe>());
            var mockIngrRepo = new Mock<IIngredientRepository>();

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);

            var res = await service.GetByNameAsync(name);


            Assert.True(res.IsSuccess);
            Assert.Equal(Enumerable.Empty<RecipeResponse>(), res.Value);
            mockRepo.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }


        //ТЕСТ НА ДОБАВЛЕНИЕ 2 ОДИНАКОВЫХ ИНГРЕДИЕНТОВ В РЕЦЕПТ
        [Fact]
        public async Task Create_WithTwoSameIngredients_ReturnsErrorValidation()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                    .ReturnsAsync((Recipe i) => i);
            mockIngrRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrRepo.Object);
            var ingrReqList = new List<CreateRecipeIngredientRequest>();
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 1.0m });
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 1.0m });
            var recReq = new CreateRecipeRequest { Name = "recipeName", RecipeIngredients = ingrReqList };

            var res = await service.CreateAsync(recReq);

            Assert.Null(res.Value);
            Assert.Equal(ErrorType.Validation, res.ErrorType);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Never);
            mockIngrRepo.Verify(s => s.GetByIdAsync(1), Times.Once);
        }
    }
}
