using Microsoft.EntityFrameworkCore;
using RecipeBox.Data;
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

        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i=> i.Ingredient)
                                   .ToList();
        }

        public Recipe? GetById(int id)
        {
            return _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i => i.Ingredient)
                                   .FirstOrDefault(rec => rec.Id == id);
        }

        public IEnumerable<Recipe> GetByName(string name)
        {
            return _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i => i.Ingredient)
                                   .Where(rec => rec.Name == name).ToList();
        }

        public Recipe Create(Recipe recipe)
        {
            recipe.Id = 0;
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            return recipe;
        }

        public bool Update(int id, Recipe newRecipe)
        {

            var recipe = _context.Recipes
                        .Include(r=>r.RecipeIngredients)
                        .FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return false; // не нашли

            newRecipe.Id = recipe.Id;

            _context.Entry(recipe).CurrentValues.SetValues(newRecipe);

            recipe.RecipeIngredients.Clear();
            foreach (var recIng in newRecipe.RecipeIngredients)
            {
                recipe.RecipeIngredients.Add(recIng);
            }

            _context.SaveChanges();

            return true;

        }

        public bool Delete(int id)
        {

            var recipe = _context.Recipes.FirstOrDefault(rec => rec.Id == id);
            if (recipe == null)
                return false;
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return true;

        }

        
    }
}
