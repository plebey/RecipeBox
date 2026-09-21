using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Common;
using RecipeBox.DTOs.User;
using RecipeBox.Services;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService service)
        {
            _userService = service;
        }

        private IActionResult HandleError(ErrorType? errorType, string errorMsg)
        {
            return errorType switch
            {
                ErrorType.Validation => BadRequest(errorMsg),
                ErrorType.Conflict => Conflict(errorMsg),
                ErrorType.NotFound => NotFound(errorMsg),
                ErrorType.Unauthorized => Unauthorized(errorMsg),
                ErrorType.Forbidden => StatusCode(
                    StatusCodes.Status403Forbidden,
                    errorMsg),

                _ => throw new InvalidOperationException($"Unsupported error type: {errorType}")
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var res = await _userService.LoginAsync(request, cancellationToken);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequest user, CancellationToken cancellationToken)
        {
            var res = await _userService.RegisterUserAsync(user, cancellationToken);
            if (res.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = res.Value.Id }, res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var res = await _userService.GetByIdAsync(id, cancellationToken);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        /*
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken, [FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var res = await _recipeService.GetAllAsync(cancellationToken, page, pageSize, name);
            if (res.IsSuccess)
                return Ok(res.Value);
            return HandleError(res.ErrorType, res.ErrorMsg);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id, CancellationToken cancellationToken)
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


        [HttpPatch("{id}")]
        [Consumes("application/json-patch+json")]
        public async Task<IActionResult> PatchByID(int id, JsonPatchDocument<UpdateRecipeRequest> patchChanges, CancellationToken cancellationToken)
        {
            var fullRecipeResult = await _recipeService.GetForUpdateAsync(id, cancellationToken);

            if (!fullRecipeResult.IsSuccess)
                return HandleError(fullRecipeResult.ErrorType, fullRecipeResult.ErrorMsg);

            var fullRecipe = fullRecipeResult.Value!;

            patchChanges.ApplyTo(fullRecipe, error =>
            {
                ModelState.AddModelError(
                    "JsonPatch",
                    error.ErrorMessage
                );
            });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var res = await _recipeService.UpdateAsync(id, fullRecipe, cancellationToken);
            if (res.IsSuccess)
                return NoContent();
            return HandleError(res.ErrorType, res.ErrorMsg);
        }
        */
    }
}
