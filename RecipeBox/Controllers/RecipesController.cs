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
        public IActionResult GetAll()
        {
            var res = _recipeService.GetAll();
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpGet("{id}")]
        public IActionResult GetByID (int id)
        {
            var res = _recipeService.GetById(id);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPost]
        public IActionResult Post(CreateRecipeRequest recipe)
        {
            var res = _recipeService.Create(recipe);
            if (res.IsSuccess)
                return CreatedAtAction(nameof(GetByID), new { id = res.Value.Id }, res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPut("{id}")]
        public IActionResult PutByID(int id, UpdateRecipeRequest recipe)
        {
            var res = _recipeService.Update(id, recipe);
            if (res.IsSuccess)
                return NoContent();
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteByID(int id)
        {
            var res = _recipeService.Delete(id);
            if (res.IsSuccess)
                return NoContent();
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpGet("search")]
        public IActionResult GetByName([FromQuery] string name)
        {
            var res = _recipeService.GetByName(name);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }
    }
}
