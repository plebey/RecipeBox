namespace RecipeBox.DTOs.User
{
    public class LoginUserResponse
    {
        public string AccessToken {  get; set; }
        public string TokenType { get; set; } = "Bearer";
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
