using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController: ControllerBase
    {
        private IIngredientService _ingredientService;
        public IngredientsController(IIngredientService service)
        {
            _ingredientService = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_ingredientService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var ingredient = _ingredientService.GetById(id);

            return ingredient == null ? NotFound() : Ok(ingredient);
        }

        [HttpPost]
        public IActionResult Post(CreateIngredientRequest ingredient)
        {
            IngredientResponse? result = _ingredientService.Create(ingredient);
            return result == null ? BadRequest() : CreatedAtAction(nameof(GetByID), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult PutByID(int id, UpdateIngredientRequest ingredient)
        {
            bool success = _ingredientService.Update(id, ingredient);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteByID(int id)
        {
            bool success = _ingredientService.Delete(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("search")]
        public IActionResult GetByName([FromQuery] string name)
        {
            return Ok(_ingredientService.GetByName(name));
        }
    }
}
