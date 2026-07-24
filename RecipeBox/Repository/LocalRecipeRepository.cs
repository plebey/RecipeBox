using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using System.Linq.Expressions;

namespace RecipeBox.Repository
{
    //TODO: Более не актуально, нужен рефакторинг в связи с разделением Ingredients на две таблицы
    //public class LocalRecipeRepository: IRecipeRepository
    //{
    //    private int _next_id = 1;
    //    private List<Recipe> _recipes = new List<Recipe>();

    //    public LocalRecipeRepository() 
    //    {

    //    }

    //    public IEnumerable<Recipe> GetAll()
    //    {
    //        return _recipes;
    //    }

    //    public Recipe? GetById(int id)
    //    {
    //        return _recipes.Where(rec => rec.Id == id).FirstOrDefault();
    //    }

    //    public Recipe? Create(Recipe recipe)
    //    {
    //        recipe.Id = _next_id;
    //        _recipes.Add(recipe);
    //        _next_id++;

    //        return recipe;
    //    }

    //    public bool Update(int id, Recipe recipe)
    //    {
    //        try
    //        {
    //            int index = _recipes.FindIndex(r => r.Id == id);
    //            if (index == -1)
    //                return false; // не нашли

    //            recipe.Id = _recipes[index].Id;
    //            _recipes[index] = recipe;

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }

    //    public bool Delete(int id)
    //    {
    //        try
    //        {
    //            int index = _recipes.FindIndex(rec => rec.Id == id);
    //            if (index == -1)
    //                return false;
    //            _recipes.RemoveAt(index);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }
            
    //    public IEnumerable<Recipe> GetByName(string name)
    //    {
    //        return _recipes.Where(rec => rec.Name == name).ToList();
    //    }
    //}
}
