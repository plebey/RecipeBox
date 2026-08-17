using Microsoft.EntityFrameworkCore;
using RecipeBox.Data;
using RecipeBox.DTOs;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;

namespace RecipeBox.Repository
{
    public class SQLRecipeRepository: IRecipeRepository
    {
        private readonly DBContextRecipeBox _context;
        public SQLRecipeRepository(DBContextRecipeBox context)
        {
            _context = context;
        }

        public async Task<PagedResult<Recipe>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name)
        {
            IQueryable<Recipe> query = _context.Recipes;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var recipes = await query
                                 .Include(r => r.RecipeIngredients)
                                 .ThenInclude(i => i.Ingredient)
                                 .OrderBy(r => r.Id)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync(cancellationToken);
            return new PagedResult<Recipe> { Items = recipes , Page = page, PageSize = pageSize, TotalCount = totalCount};
        }

        public Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i => i.Ingredient)
                                   .FirstOrDefaultAsync(rec => rec.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Recipe>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i => i.Ingredient)
                                   .Where(rec => rec.Name == name).ToListAsync(cancellationToken);
        }

        public async Task<Recipe> CreateAsync(Recipe recipe, CancellationToken cancellationToken)
        {
            recipe.Id = 0;
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync(cancellationToken);

            return recipe;
        }

        public async Task<bool> UpdateAsync(int id, Recipe newRecipe, CancellationToken cancellationToken)
        {

            var recipe = await _context.Recipes
                        .Include(r=>r.RecipeIngredients)
                        .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (recipe == null)
                return false; // не нашли

            newRecipe.Id = recipe.Id;

            _context.Entry(recipe).CurrentValues.SetValues(newRecipe);

            recipe.RecipeIngredients.Clear();
            foreach (var recIng in newRecipe.RecipeIngredients)
            {
                recipe.RecipeIngredients.Add(recIng);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;

        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {

            var recipe = await _context.Recipes.FirstOrDefaultAsync(rec => rec.Id == id, cancellationToken);
            if (recipe == null)
                return false;
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }

        
    }
}
