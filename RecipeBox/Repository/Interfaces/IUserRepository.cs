using RecipeBox.DTOs;
using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IUserRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        Task<PagedResult<User>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, User user, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}
