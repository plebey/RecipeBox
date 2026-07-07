using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController: ControllerBase
    {
        private IRecipeService _recipeService;
        public RecipesController(IRecipeService service)
        {
            _recipeService = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_recipeService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetByID (int id)
        {
            var recipe = _recipeService.GetById(id);

            return recipe == null ? NotFound() : Ok(recipe);
        }

        [HttpPost]
        public IActionResult Post(Recipe recipe)
        {
            Recipe? result = _recipeService.Create(recipe);
            return result == null ? BadRequest() : Created("api/recipes", result);
        }

        [HttpPut("{id}")]
        public IActionResult PutByID(int id, Recipe recipe)
        {
            bool success = _recipeService.Update(id, recipe);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteByID(int id)
        {
            bool success = _recipeService.Delete(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("search")]
        public IEnumerable<Recipe> GetByName([FromQuery] string name)
        {
            return _recipeService.GetByName(name);
        }
    }
}
