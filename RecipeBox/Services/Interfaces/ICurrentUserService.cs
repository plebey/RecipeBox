namespace RecipeBox.Services.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }

    }
}
