using Microsoft.AspNetCore.Identity;
using RecipeBox.Common;
using RecipeBox.DTOs;
using RecipeBox.DTOs.Recipe;
using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.DTOs.User;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository,  
                            IPasswordHasher<User> passwordHasher,
                            IJwtTokenService jwtTokenService)
        {
            _repository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        private UserResponse BuildUserResponse(User user)
        {
            UserResponse response = new();
            response.Id = user.Id;
            response.Email = user.Email;
            response.Role = user.Role;
            response.IsBlocked = user.IsBlocked;
            response.CreatedAtUtc = user.CreatedAtUtc;
      
            return response;
        }

        public async Task<Result<PagedResult<UserResponse>>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? email)
        {
            if (page <= 0)
            {
                return Result<PagedResult<UserResponse>>.Failure(
                    ErrorType.Validation,
                    "Page must be greater than 0.");
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                return Result<PagedResult<UserResponse>>.Failure(
                    ErrorType.Validation,
                    "Page size must be between 1 and 100.");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.Trim();
            }

            List<UserResponse> userRes = new List<UserResponse>();
            var pagedUsers = await _repository.GetAllAsync(cancellationToken, page, pageSize, email);

            foreach (User user in pagedUsers.Items)
            {
                userRes.Add(BuildUserResponse(user));
            }
            var userResPaged = new PagedResult<UserResponse>()
            {
                Items = userRes,
                Page = pagedUsers.Page,
                PageSize = pagedUsers.PageSize,
                TotalCount = pagedUsers.TotalCount
            };

            return Result<PagedResult<UserResponse>>.Success(userResPaged);
        }
        public async Task<Result<UserResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return Result<UserResponse>.Failure(ErrorType.Validation,
                                                      $"Id must be greater than 0.");
            var res = await _repository.GetByIdAsync(id, cancellationToken);
            if (res == null)
                return Result<UserResponse>.Failure(ErrorType.NotFound,
                                                      $"User with id {id} not found.");
            return Result<UserResponse>.Success(BuildUserResponse(res));
        }

        private bool ValidateEmailPassLength(string email, string pass)
        {
            if (email.Length > Constraints.UserEmailMaxLength)
                return false;
            if (pass.Length > Constraints.UserPasswordMaxLength || pass.Length < Constraints.UserPasswordMinLength)
                return false;

            return true;
        }

        public async Task<Result<UserResponse>> RegisterUserAsync(CreateUserRequest user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                return Result<UserResponse>.Failure(ErrorType.Validation,
                               "Email and Password must not be empty.");
            }
            var email = user.Email.Trim();

            if (!ValidateEmailPassLength(email, user.Password))
            {
                return Result<UserResponse>.Failure(ErrorType.Validation,
                               "One or more parameters are too long or too short.");
            }

            var duplicate = await _repository.GetByEmailAsync(email, cancellationToken);
            if (duplicate != null)
            {
                return Result<UserResponse>.Failure(ErrorType.Conflict,
                               "Email already exists.");
            }

            User newUser = new User(email, UserRoles.User);
            var passwordHash = _passwordHasher.HashPassword(newUser, user.Password);
            newUser.PasswordHash = passwordHash;

            return Result<UserResponse>.Success(BuildUserResponse(await _repository.CreateAsync(newUser, cancellationToken)));

        }

        private LoginUserResponse BuildLoginResponse(User user, string jwtToken, DateTime expiresAtUtc)
        {
            return new LoginUserResponse()
            {
                AccessToken = jwtToken,
                Email = user.Email,
                ExpiresAtUtc = expiresAtUtc,
                Role = user.Role
            };
        }

        public async Task<Result<LoginUserResponse>> LoginAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(loginUserRequest.Email) || string.IsNullOrWhiteSpace(loginUserRequest.Password))
            {
                return Result<LoginUserResponse>.Failure(ErrorType.Validation,
                               "Email and Password must not be empty.");
            }

            var email = loginUserRequest.Email.Trim();
            if (!ValidateEmailPassLength(email, loginUserRequest.Password))
            {
                return Result<LoginUserResponse>.Failure(ErrorType.Validation,
                               "One or more parameters are too long or too short.");
            }
            var user = await _repository.GetByEmailAsync(email, cancellationToken);
            if (user == null || user.IsBlocked)
                return Result<LoginUserResponse>.Failure(ErrorType.Unauthorized,
                                                      $"Invalid email or password.");


            var verificationResult = _passwordHasher.VerifyHashedPassword(user, 
                                                                            user.PasswordHash, 
                                                                            loginUserRequest.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
                return Result<LoginUserResponse>.Failure(ErrorType.Unauthorized,
                                                      $"Invalid email or password.");
            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var newHashedPass = _passwordHasher.HashPassword(user, loginUserRequest.Password);
                user.PasswordHash = newHashedPass;
                await _repository.SaveChangesAsync(cancellationToken);
            }

            (string jwtToken, DateTime expiresAtUtc) = _jwtTokenService.GenerateToken(user);


            return Result<LoginUserResponse>.Success(BuildLoginResponse(user, jwtToken, expiresAtUtc));
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return Result.Failure(ErrorType.Validation,
                                                      $"Id must be greater than 0.");
            return await _repository.DeleteAsync(id, cancellationToken) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"User with id <{id}> was not found.");
        }
        public async Task<Result<UserResponse>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<UserResponse>.Failure
                                                (ErrorType.Validation,
                                                "Email must not be empty.");

            email = email.Trim();
            var userByEmail = await _repository.GetByEmailAsync(email, cancellationToken);

            if (userByEmail == null)
                return Result<UserResponse>.Failure(ErrorType.NotFound,
                                                      $"User with email {email} not found.");

            var userRes = BuildUserResponse(userByEmail);

            return Result<UserResponse>.Success(userRes);
        }

        public async Task<Result> UpdateEmailAsync(int id, UpdateEmailUserRequest user, CancellationToken cancellationToken)
        {
            return Result.Failure(ErrorType.NotFound,
                               "Not implemented yet.");
        }
        public async Task<Result> UpdatePasswordAsync(int id, UpdatePasswordUserRequest user, CancellationToken cancellationToken)
        {
            return Result.Failure(ErrorType.NotFound,
                               "Not implemented yet.");
        }

        public async Task<Result> ChangeRoleAsync(int id, string newRole, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);
            if (user == null)
                return Result.Failure(ErrorType.NotFound,
                                                      $"User with id {id} not found.");

            switch (newRole) 
            { 
                case UserRoles.User:
                    {
                        user.Role = UserRoles.User;
                        break;
                    }
                case UserRoles.Admin:
                    {
                        user.Role = UserRoles.Admin;
                        break;
                    }
                default:
                    {
                        return Result.Failure(ErrorType.Validation,
                                                      $"Role does not exist.");
                    }
            }
            await _repository.SaveChangesAsync(cancellationToken);
            return Result.Success();    

        }
        public async Task<Result> ChangeBlockStatusAsync(int id, bool isBlocked, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);
            if (user == null)
                return Result.Failure(ErrorType.NotFound,
                                                      $"User with id {id} not found.");
            user.IsBlocked = isBlocked;

            await _repository.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
