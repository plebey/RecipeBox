using Moq;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services;

namespace RecipeBox.Tests
{
    public class IngredientServiceTests
    {
        //Заполненный/Пустой PurchaseURL -> успешно
        [Theory]
        [InlineData("http://blabla.com")]
        [InlineData("")]

        //ЧтоТестируем_Условие_ОжидаемыйРезультат
        public void Create_WithValidData_ReturnIngredient(string purchaseURL)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Ingredient>()))
                    .Returns((Ingredient i) => i);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new CreateIngredientRequest { Name = "Перец", Unit = "г", PurchaseURL = purchaseURL };

            var res = service.Create(createReq);

            Assert.NotNull(res);
            Assert.Equal("Перец", res.Name);
            Assert.Equal("г", res.Unit);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Ingredient>()), Times.Once);
        }

        //Пустой/пробельный Unit / Имя -> null возврат
        [Theory]
        [InlineData("", "г.")]
        [InlineData("Капуста", "")]
        [InlineData(" ", "г.")]
        [InlineData("Капуста", " ")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void Create_WithEmptyNameUnit_ReturnNull(string name, string unit)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Ingredient>()))
                    .Returns((Ingredient i) => i);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new CreateIngredientRequest { Name = name, Unit = unit };

            var res = service.Create(createReq);

            Assert.Null(res);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Ingredient>()), Times.Never);
        }

        //тест на дубликат через getByName
        [Fact]
        public void Create_WithDuplicateName_ReturnNull()
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.Create(It.IsAny<Ingredient>()))
                    .Returns((Ingredient i) => i);
            mockRepo.Setup(repo => repo.GetByName(It.IsAny<string>()))
                    .Returns(new Ingredient("Соль", "г."));

            var service = new IngredientService(mockRepo.Object);
            var createReq = new CreateIngredientRequest { Name = "Соль", Unit = "кг." };
            var res = service.Create(createReq);

            Assert.Null(res);
            mockRepo.Verify(repo => repo.Create(It.IsAny<Ingredient>()), Times.Never);
        }


        //Заполненный/Пустой PurchaseURL -> успешно
        [Theory]
        [InlineData("http://blabla.com")]
        [InlineData("")]

        //ЧтоТестируем_Условие_ОжидаемыйРезультат
        public void Update_WithValidData_ReturnTrue(string purchaseURL)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<Ingredient>()))
                    .Returns(true);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new UpdateIngredientRequest { Name = "Перец", Unit = "г", PurchaseURL = purchaseURL };

            var res = service.Update(1, createReq);

            Assert.True(res);
            mockRepo.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Ingredient>()), Times.Once);

        }

        //Пустое имя/юнит
        [Theory]
        [InlineData("", "г.")]
        [InlineData("Капуста", "")]
        [InlineData(" ", "г.")]
        [InlineData("Капуста", " ")]
        [InlineData("", "")]
        [InlineData(" ", " ")]

        public void Update_WithEmptyNameUnit_ReturnFalse(string name, string unit)
        {
            var mockRepo = new Mock<IIngredientRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<Ingredient>()))
                    .Returns(false);

            var service = new IngredientService(mockRepo.Object);
            var createReq = new UpdateIngredientRequest { Name = name, Unit = unit};

            var res = service.Update(1, createReq);

            Assert.False(res);
            mockRepo.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Ingredient>()), Times.Never);

        }
    }
}
