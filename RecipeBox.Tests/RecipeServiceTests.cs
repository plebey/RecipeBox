using Moq;
using RecipeBox.DTOs.Recipe;
using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Tests
{
    public class RecipeServiceTests
    {

        // проверить корректность построения респонза? например, что ингредиенты в рецепте и в респонзе одни и те же
        [Fact]
        public void Create_WithTwoIngredients_ReturnsRecipeWithExactTwoIngredients()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrService = new Mock<IIngredientService>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Recipe>()))
                    .Returns((Recipe i) => i);
            mockIngrService.Setup(s => s.GetByIdDomain(1))
                           .Returns(new Ingredient("Ингред1", "Кг."));
            mockIngrService.Setup(s => s.GetByIdDomain(3))
                           .Returns(new Ingredient("Ингред3", "г."));

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var recIngReq = new List<CreateRecipeIngredientRequest>();
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 2.0m });
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 3, Amount = 2.0m });
            var recReq = new CreateRecipeRequest { Name = "Борщ", RecipeIngredients = recIngReq };

            var res = service.Create(recReq);

            Assert.Equal("Ингред1", res.Ingredients[0].IngredientName);
            Assert.Equal("Ингред3", res.Ingredients[1].IngredientName);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Recipe>()), Times.Once);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Exactly(2));

        }

        // проверка при несуществующем ингредиенте (вызов create 0 раз)
        [Fact]
        public void Create_WithInvalidIngredients_ReturnsNull()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrService = new Mock<IIngredientService>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Recipe>()))
                    .Returns((Recipe i) => i);
            mockIngrService.Setup(s => s.GetByIdDomain(It.IsAny<int>()))
                           .Returns((Ingredient?)null);

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var recIngReq = new List<CreateRecipeIngredientRequest>();
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 2.0m });
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 3, Amount = 2.0m });
            var recReq = new CreateRecipeRequest { Name = "Борщ" , RecipeIngredients = recIngReq};

            var res = service.Create(recReq);

            Assert.Null(res);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Recipe>()), Times.Never);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Once);

        }

        // проверка со всеми корректными данными и пустым ингредиентом (вызов create 1 раз)
        [Fact]
        public void Create_WithEmptyIngredients_ReturnsRecipeResponse()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrService = new Mock<IIngredientService>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Recipe>()))
                    .Returns((Recipe i) => i);
            mockIngrService.Setup(s => s.GetByIdDomain(It.IsAny<int>()))
                           .Returns(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var recReq = new CreateRecipeRequest { Name = "Борщ"};

            var res = service.Create(recReq);

            Assert.Equal("Борщ", res.Name);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Recipe>()), Times.Once);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Never);

        }

        // проверка со всеми корректными данными и заполненным ингредиентом (вызов create 1 раз)
        [Fact]
        public void Create_WithIngredients_ReturnsRecipeResponse()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrService = new Mock<IIngredientService>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Recipe>()))
                    .Returns((Recipe i) => i);
            mockIngrService.Setup(s => s.GetByIdDomain(It.IsAny<int>()))
                           .Returns(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var recIngReq = new List<CreateRecipeIngredientRequest>();
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 2.0m });
            recIngReq.Add(new CreateRecipeIngredientRequest { IngredientId = 2, Amount = 3.0m });
            var recReq = new CreateRecipeRequest { Name = "Борщ", RecipeIngredients = recIngReq };

            var res = service.Create(recReq);

            Assert.Equal("Борщ", res.Name);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Recipe>()), Times.Once);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Exactly(2));

        }

        // проверка на пустые значения имени и с корректными данными (вызов create 0 раз)
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" ")]
        public void Create_WithEmptyName_ReturnsNull(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            var mockIngrService = new Mock<IIngredientService>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Recipe>()))
                    .Returns((Recipe i) => i);
            mockIngrService.Setup(s => s.GetByIdDomain(It.IsAny<int>()))
                           .Returns(new Ingredient("Ингред1", "Кг."));

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var recReq = new CreateRecipeRequest{Name = name};

            var res = service.Create(recReq);

            Assert.Null(res);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Recipe>()), Times.Never);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Never);

        }
        // проверка на пустые значения имени и с корректными данными (вызов create 0 раз)
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Update_WithEmptyName_ReturnsNull(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<Recipe>()))
                    .Returns(true);
            var mockIngrService = new Mock<IIngredientService>();
            mockIngrService.Setup(s => s.GetByIdDomain(It.IsAny<int>()))
                           .Returns((Ingredient i) => i);

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var updRecReq = new UpdateRecipeRequest { Name = name };

            var res = service.Update(1, updRecReq);

            Assert.False(res);
            mockRepo.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Never);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Never);
        }
        // проверка при несуществующем ингредиенте (вызов create 0 раз)
        [Fact]
        public void Update_WithInvalidIngredient_ReturnsNull()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<Recipe>()))
                    .Returns(false);

            var mockIngrService = new Mock<IIngredientService>();
            var testIngredient = new Ingredient("Соль", "г");
            mockIngrService.Setup(s => s.GetByIdDomain(1))
                           .Returns(testIngredient);
            mockIngrService.Setup(s => s.GetByIdDomain(3))
                           .Returns((Ingredient?)null);

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var ingrReqList = new List<CreateRecipeIngredientRequest>();
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 1 , Amount = 1.0m});
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 3 , Amount = 1.0m});
            var updRecReq = new UpdateRecipeRequest { Name = "name", RecipeIngredients = ingrReqList };

            var res = service.Update(1, updRecReq);

            Assert.False(res);
            mockRepo.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Never);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Exactly(2));
        }
        // проверка со всеми корректными данными (вызов create 1 раз)
        [Fact]
        public void Update_WithValidIngredient_ReturnsTrue()
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<Recipe>()))
                    .Returns(true);

            var mockIngrService = new Mock<IIngredientService>();
            var testIngredient = new Ingredient("Соль", "г");
            mockIngrService.Setup(s => s.GetByIdDomain(It.IsAny<int>()))
                           .Returns(testIngredient);

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);
            var ingrReqList = new List<CreateRecipeIngredientRequest>();
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 1, Amount = 1.0m });
            ingrReqList.Add(new CreateRecipeIngredientRequest { IngredientId = 3, Amount = 1.0m });
            var updRecReq = new UpdateRecipeRequest { Name = "name", RecipeIngredients = ingrReqList };

            var res = service.Update(1, updRecReq);

            Assert.True(res);
            mockRepo.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Once);
            mockIngrService.Verify(s => s.GetByIdDomain(It.IsAny<int>()), Times.Exactly(2));
        }

        // проверка на возврат пустого значения при пустом/отсутствующем имени
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetByName_WithEmptyNameOrNoMatches_ReturnsEmptyEnumerableResponse(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.GetByName(It.IsAny<string>()))
                    .Returns(Enumerable.Empty<Recipe>);
            var mockIngrService = new Mock<IIngredientService>();

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);

            var res = service.GetByName(name);

            Assert.Equal(Enumerable.Empty<RecipeResponse>(), res);
            mockRepo.Verify(repo => repo.GetByName(It.IsAny<string>()), Times.Never);
        }

        // проверка на возврат пустого значения при отсуюствии совпадений
        [Theory]
        [InlineData("Омлет")]
        public void GetByName_WithNoMatches_ReturnsEmptyEnumerableResponse(string name)
        {
            var mockRepo = new Mock<IRecipeRepository>();
            mockRepo.Setup(repo => repo.GetByName(It.IsAny<string>()))
                    .Returns(Enumerable.Empty<Recipe>);
            var mockIngrService = new Mock<IIngredientService>();

            var service = new RecipeService(mockRepo.Object, mockIngrService.Object);

            var res = service.GetByName(name);

            Assert.Equal(Enumerable.Empty<RecipeResponse>(), res);
            mockRepo.Verify(repo => repo.GetByName(It.IsAny<string>()), Times.Once);
        }
    }
}
