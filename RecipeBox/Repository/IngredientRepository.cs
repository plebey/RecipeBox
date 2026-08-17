using Microsoft.EntityFrameworkCore;
using RecipeBox.Data;
using RecipeBox.DTOs;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;

namespace RecipeBox.Repository
{
    public class IngredientRepository: IIngredientRepository
    {
        private readonly DBContextRecipeBox _context;
        public IngredientRepository(DBContextRecipeBox context)
        {
            _context = context;
        }

        public async Task<PagedResult<Ingredient>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name)
        {
            IQueryable<Ingredient> query = _context.Ingredients;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var ingredList = await query
                                   .OrderBy(x => x.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);
            return new PagedResult<Ingredient>
                                {
                                    Items = ingredList,
                                    Page = page,
                                    PageSize = pageSize,
                                    TotalCount = totalCount
                                };
        }

        public Task<Ingredient?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Ingredients.FirstOrDefaultAsync(rec => rec.Id == id, cancellationToken);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingred, CancellationToken cancellationToken)
        {
            ingred.Id = 0;
            _context.Ingredients.Add(ingred);
            await _context.SaveChangesAsync(cancellationToken);

            return ingred;
        }

        public async Task<bool> UpdateAsync(int id, Ingredient newIngred, CancellationToken cancellationToken)
        {
            var ingred = await _context.Ingredients.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (ingred == null)
                return false; // не нашли
 
            newIngred.Id = ingred.Id;

            _context.Entry(ingred).CurrentValues.SetValues(newIngred);
            await _context.SaveChangesAsync(cancellationToken);

            return true;

        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {

            var ingred =await _context.Ingredients.FirstOrDefaultAsync(ing => ing.Id == id, cancellationToken);
            if (ingred == null)
                return false;
            _context.Ingredients.Remove(ingred);
            await _context.SaveChangesAsync(cancellationToken);

            return true; 
        }

        public Task<Ingredient?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Ingredients.FirstOrDefaultAsync(ing => ing.Name == name, cancellationToken);
        }
    }
}
