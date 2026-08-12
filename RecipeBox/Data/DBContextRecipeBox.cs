using Microsoft.EntityFrameworkCore;
using RecipeBox.Common;
using RecipeBox.Models;

namespace RecipeBox.Data
{
    public class DBContextRecipeBox: DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DBContextRecipeBox(DbContextOptions<DBContextRecipeBox> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId});

            modelBuilder.Entity<RecipeIngredient>()
                .Property(ri => ri.Amount)
                .HasPrecision(Constraints.IngredientAmountDigitsLength, Constraints.IngredientAmountAfterComma);


            modelBuilder.Entity<RecipeIngredient>()
                .ToTable(t => t.HasCheckConstraint(
                                "CK_RecipeIngredient_Amount_Positive",
                                "[Amount] > 0"));


            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ingredient>()
                .HasIndex(i => i.Name)
                .IsUnique();

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Name)
                .HasMaxLength(Constraints.IngredientNameMaxLength)
                .IsRequired();

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Unit)
                .HasMaxLength(Constraints.IngredientUnitMaxLength)
                .IsRequired();

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.PurchaseURL)
                .HasMaxLength(Constraints.IngredientURLMaxLength);

            modelBuilder.Entity<Recipe>()
                .Property(i => i.Name)
                .HasMaxLength(Constraints.RecipeNameMaxLength)
                .IsRequired();

            modelBuilder.Entity<Recipe>()
                .Property(i => i.RecipeURL)
                .HasMaxLength(Constraints.RecipeURLMaxLength);

            modelBuilder.Entity<Recipe>()
                .Property(i => i.Description)
                .HasMaxLength(Constraints.RecipeDescriptionMaxLength);

        }
    }
}
