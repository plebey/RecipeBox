using Moq;
using RecipeBox.Common;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services;

namespace RecipeBox.Tests.UnitTests
{
    public class IngredientServiceTests
    {
        //Заполненный/Пустой PurchaseURL -> успешно
        [Theory]
        [InlineData("http://blabla.com")]
        [InlineData("")]

        //ЧтоТестируем_Условие_ОжидаемыйРезультат
        public async Task Create_WithValidData_ReturnIngredient(string purchaseURL)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Ingredient>(), CancellationToken.None))
                    .ReturnsAsync((Ingredient i, CancellationToken token) => i);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new CreateIngredientRequest { Name = "Перец", Unit = "г", PurchaseURL = purchaseURL };

            var res = await service.CreateAsync(createReq, CancellationToken.None);

            Assert.NotNull(res.Value);
            Assert.Equal("Перец", res.Value.Name);
            Assert.Equal("г", res.Value.Unit);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Ingredient>(), CancellationToken.None), Times.Once);
        }

        //Пустой/пробельный Unit / Имя -> null возврат
        [Theory]
        [InlineData("", "г.")]
        [InlineData("Капуста", "")]
        [InlineData(" ", "г.")]
        [InlineData("Капуста", " ")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public async Task Create_WithEmptyNameUnit_ReturnValidationError(string name, string unit)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Ingredient>(), CancellationToken.None))
                    .ReturnsAsync((Ingredient i, CancellationToken token) => i);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new CreateIngredientRequest { Name = name, Unit = unit };

            var res = await service.CreateAsync(createReq, CancellationToken.None);

            Assert.Null(res.Value);
            Assert.Equal(ErrorType.Validation, res.ErrorType);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Ingredient>(), CancellationToken.None), Times.Never);
        }

        //тест на дубликат через getByName
        [Fact]
        public async Task Create_WithDuplicateName_ReturnErrorConflict()
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Ingredient>(), CancellationToken.None))
                    .ReturnsAsync((Ingredient i, CancellationToken token) => i);
            mockRepo.Setup(repo => repo.GetByNameAsync(It.IsAny<string>(), CancellationToken.None))
                    .ReturnsAsync(new Ingredient("Соль", "г."));

            var service = new IngredientService(mockRepo.Object);
            var createReq = new CreateIngredientRequest { Name = "Соль", Unit = "кг." };
            var res = await service.CreateAsync(createReq, CancellationToken.None);

            Assert.Null(res.Value);
            Assert.Equal(ErrorType.Conflict, res.ErrorType);
            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Ingredient>(), CancellationToken.None), Times.Never);
        }


        //Заполненный/Пустой PurchaseURL -> успешно
        [Theory]
        [InlineData("http://blabla.com")]
        [InlineData("")]

        //ЧтоТестируем_Условие_ОжидаемыйРезультат
        public async Task Update_WithValidData_ReturnTrue(string purchaseURL)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>(), CancellationToken.None))
                    .ReturnsAsync(true);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new UpdateIngredientRequest { Name = "Перец", Unit = "г", PurchaseURL = purchaseURL };

            var res = await service.UpdateAsync(1, createReq, CancellationToken.None);

            Assert.True(res.IsSuccess);
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>(), CancellationToken.None), Times.Once);

        }

        //Пустое имя/юнит
        [Theory]
        [InlineData("", "г.")]
        [InlineData("Капуста", "")]
        [InlineData(" ", "г.")]
        [InlineData("Капуста", " ")]
        [InlineData("", "")]
        [InlineData(" ", " ")]

        public async Task Update_WithEmptyNameUnit_ReturnErrorValidation(string name, string unit)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>(), CancellationToken.None))
                    .ReturnsAsync(false);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new UpdateIngredientRequest { Name = name, Unit = unit};

            var res = await service.UpdateAsync(1, createReq, CancellationToken.None);

            Assert.False(res.IsSuccess);
            Assert.Equal(ErrorType.Validation, res.ErrorType);
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>(), CancellationToken.None), Times.Never);

        }

        //тест на дубликат имени при update
        [Fact]
        public async Task Update_WithDuplicateName_ReturnErrorConflict()
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>(), CancellationToken.None))
                    .ReturnsAsync(false);
            mockRepo.Setup(repo => repo.GetByNameAsync("name", CancellationToken.None))
                    .ReturnsAsync(new Ingredient ("Name", "Unit") { Id = 5 });

            var service = new IngredientService(mockRepo.Object);
            var updateReq = new UpdateIngredientRequest { Name = "  name  ", Unit = "unit" };

            var res = await service.UpdateAsync(1, updateReq, CancellationToken.None);

            Assert.False(res.IsSuccess);
            Assert.Equal(ErrorType.Conflict, res.ErrorType);
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>(), CancellationToken.None), Times.Never);

        }
    }
}
