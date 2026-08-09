using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Common;
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

        private IActionResult HandleError(ErrorType? errorType, string errorMsg)
        {
            return errorType switch
            {
                ErrorType.Validation => BadRequest(errorMsg),
                ErrorType.Conflict => Conflict(errorMsg),
                ErrorType.NotFound => NotFound(errorMsg),

                _ => throw new InvalidOperationException($"Unsupported error type: {errorType}")
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _ingredientService.GetAllAsync();
            if (res.IsSuccess)
                return Ok(res.Value);

            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var res = await _ingredientService.GetByIdAsync(id);
            if (res.IsSuccess)
                return Ok(res.Value);

            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateIngredientRequest ingredient)
        {
            var res = await _ingredientService.CreateAsync(ingredient);
            if (res.IsSuccess)
                return CreatedAtAction(nameof(GetByID), new { id = res.Value.Id }, res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutByID(int id, UpdateIngredientRequest ingredient)
        {
            var res = await _ingredientService.UpdateAsync(id, ingredient);

            if (res.IsSuccess)
                return NoContent();

            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            var res = await _ingredientService.DeleteAsync(id);
            if (res.IsSuccess)
                return NoContent();

            return HandleError(res.ErrorType, res.ErrorMsg);

        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var res = await _ingredientService.GetByNameAsync(name);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }
    }
}
