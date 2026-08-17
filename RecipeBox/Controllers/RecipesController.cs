using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Common;
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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken, [FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var res = await _recipeService.GetAllAsync(cancellationToken, page, pageSize, name);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID (int id, CancellationToken cancellationToken)
        {
            var res = await _recipeService.GetByIdAsync(id, cancellationToken);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateRecipeRequest recipe, CancellationToken cancellationToken)
        {
            var res = await _recipeService.CreateAsync(recipe, cancellationToken);
            if (res.IsSuccess)
                return CreatedAtAction(nameof(GetByID), new { id = res.Value.Id }, res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutByID(int id, UpdateRecipeRequest recipe, CancellationToken cancellationToken)
        {
            var res = await _recipeService.UpdateAsync(id, recipe, cancellationToken);
            if (res.IsSuccess)
                return NoContent();
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByID(int id, CancellationToken cancellationToken)
        {
            var res = await _recipeService.DeleteAsync(id, cancellationToken);
            if (res.IsSuccess)
                return NoContent();
            return HandleError(res.ErrorType, res.ErrorMsg);
        }
        /*
        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken cancellationToken)
        {
            var res = await _recipeService.GetByNameAsync(name, cancellationToken);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }
        */
    }
}
