using RecipeBox.Services.Interfaces;
using System.Security.Claims;

namespace RecipeBox.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (int.TryParse(value, out var userId))
                    return userId;

                return null;
            }
        }

        public string? Role
        {
            get
            {
                return _httpContextAccessor
                        .HttpContext?
                        .User
                        .FindFirst(ClaimTypes.Role)?
                        .Value;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return _httpContextAccessor
                    .HttpContext?
                    .User
                    .Identity?
                    .IsAuthenticated == true;
            }
        }


    }
}
