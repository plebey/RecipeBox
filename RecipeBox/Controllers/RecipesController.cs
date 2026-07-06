using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController: ControllerBase
    {

        public RecipesController()
        {

        }

        [HttpGet]
        public IEnumerable<Recipe> GetAll()
        {
            return new List<Recipe>();
        }

        [HttpGet("{id}")]
        public Recipe GetByID (int id)
        {
            return new Recipe(name:"");
        }

        [HttpPost]
        public void Post(Recipe recipe)
        {
            Recipe recip = recipe;
        }

        [HttpPut("{id}")]
        public void PutByID(int id, Recipe recipe)
        {
            //тут еще поиск по id конкретного рецепта будет
            Recipe recip = recipe;
        }

        [HttpDelete("{id}")]
        public void DeleteByID(int id)
        {
            //удаление по id
        }

        [HttpGet("name/{name}")]
        public IEnumerable<Recipe> GetByName([FromQuery] string name)
        {
            return new List<Recipe>();
        }
    }
}
