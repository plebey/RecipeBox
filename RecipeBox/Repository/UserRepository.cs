using Microsoft.EntityFrameworkCore;
using RecipeBox.Data;
using RecipeBox.DTOs;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;

namespace RecipeBox.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DBContextRecipeBox _context;
        public UserRepository(DBContextRecipeBox context)
        {
            _context = context;
        }
        public async Task<PagedResult<User>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? email)
        {
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(x => x.Email.Contains(email));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var userList = await query
                                   .OrderBy(x => x.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);
            return new PagedResult<User>
            {
                Items = userList,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(rec => rec.Id == id, cancellationToken);
        }
        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(ing => ing.Email == email, cancellationToken);
        }
        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            user.Id = 0;
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }
        public async Task<bool> UpdateAsync(int id, User newUser, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (user == null)
                return false; // не нашли

            newUser.Id = user.Id;

            _context.Entry(user).CurrentValues.SetValues(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(ing => ing.Id == id, cancellationToken);
            if (user == null)
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
