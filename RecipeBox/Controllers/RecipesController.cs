using Microsoft.AspNetCore.Mvc;
using RecipeBox.DTOs.Recipe;
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
        public IActionResult Post(CreateRecipeRequest recipe)
        {
            RecipeResponse? result = _recipeService.Create(recipe);
            return result == null ? BadRequest() : CreatedAtAction(nameof(GetByID),new {id = result.Id}, result);
        }

        [HttpPut("{id}")]
        public IActionResult PutByID(int id, UpdateRecipeRequest recipe)
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
        public IActionResult GetByName([FromQuery] string name)
        {
            return Ok(_recipeService.GetByName(name));
        }
    }
}
