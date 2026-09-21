using RecipeBox.Common;
using RecipeBox.DTOs;
using RecipeBox.DTOs.Recipe;
using RecipeBox.DTOs.User;

namespace RecipeBox.Services.Interfaces
{
    public interface IUserService
    {

        Task<Result<PagedResult<UserResponse>>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name);
        Task<Result<UserResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<UserResponse>> RegisterUserAsync(CreateUserRequest user, CancellationToken cancellationToken);
        Task<Result<LoginUserResponse>> LoginAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken);
        Task<Result> UpdateEmailAsync(int id, UpdateEmailUserRequest user, CancellationToken cancellationToken);
        Task<Result> UpdatePasswordAsync(int id, UpdatePasswordUserRequest user, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Result<UserResponse>> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task<Result> ChangeRoleAsync(int id, string newRole, CancellationToken cancellationToken);
        Task<Result> ChangeBlockStatusAsync(int id, bool isBlocked, CancellationToken cancellationToken);

    }
}
