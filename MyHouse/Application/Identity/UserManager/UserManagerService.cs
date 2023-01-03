using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Contract.Identity.UserManager;
using Core.Const;
using Core.Exceptions;
using Domain.Identity.Roles;
using Domain.Identity.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SqlServ4r.Repository.RoleClaims;
using SqlServ4r.Repository.UserRoles;
using Volo.Abp.DependencyInjection;

namespace Application.Identity.UserManager
{
    public class UserManagerService : ServiceBase, IUserManagerService, ITransientDependency
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private RoleClaimRepository _roleClaimRepository;
        private IConfiguration _configuration;
        private UserRoleRepository _userRoleRepository;

        public UserManagerService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            RoleClaimRepository roleClaimRepository,
            UserRoleRepository userRoleRepository,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _roleClaimRepository = roleClaimRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<UserWithNavigationDto>> GetListWithNavigationAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithNav = new List<UserWithNavigationDto>();
            var count = 1;
            foreach (var item in users)
            {
                var roleNames = (List<string>) await _userManager.GetRolesAsync(item);
                count++;
                usersWithNav.Add(new UserWithNavigationDto()
                    {RoleNames = roleNames, Index = count, UserDto = ObjectMapper.Map<User, UserDto>(item)});
            }

            return usersWithNav;
        }

        public async Task<CreateUpdateUserWithNavDto> CreateWithNavigationAsync(CreateUpdateUserWithNavDto input)
        {
            await CreateAsync(input.User);
            await UpdateRolesForUser(input.User.UserName, input.Roles);
            return input;
        }

        public async Task<CreateUpdateUserWithNavDto> UpdateWithNavigationAsync(CreateUpdateUserWithNavDto input, Guid id)
        {
            var user = input.User; 
            await UpdateAsync(user, id);
            await UpdateRolesForUser(user.UserName, input.Roles);
            return input;
        }

        public async Task DeleteWithNavigationAsync(Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            if (item == null)  throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest); 
          

            item.IsActive = false;
            var userResult = await _userManager.UpdateAsync(item);
            if (!userResult.Succeeded)
            {
                 throw new GlobalException(HttpMessage.CheckInformation, HttpStatusCode.BadRequest);
            }
            
        }

        public async Task<List<UserDto>> GetListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return ObjectMapper.Map<List<User>, List<UserDto>>(users);
        }

        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            var user = ObjectMapper.Map<CreateUpdateUserDto, User>(input);
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);
            }
            
            return ObjectMapper.Map<User,UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(CreateUpdateUserDto input, Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());

            if (item == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            var user = ObjectMapper.Map(input, item);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);
            }
            
            return ObjectMapper.Map<User,UserDto>(user);;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            if (item == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            var result = await _userManager.DeleteAsync(item);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);
            }
            
        }

        public async Task<TokenDto> SignInAsync(UserModel input)
        {
            var user = await _userManager.FindByNameAsync(input.UserName);
            if (user == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            var result = await _userManager.CheckPasswordAsync(user, input.Password);
            if (!result)
            {
                throw new GlobalException(HttpMessage.CheckInformation, HttpStatusCode.BadRequest);
            }

            string accessToken = await GenerateTokenByUser(user);
            string refreshToken = GenerateRefreshToken();
   
            return new TokenDto(){AccessToken = accessToken,RefreshToken = refreshToken};
        }



        public async Task<UserDto> SignUpAsync(CreateUpdateUserDto input)
        {
            
            var user = new User();
            user.UserName = input.UserName;
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                throw new GlobalException(HttpMessage.CheckInformation, HttpStatusCode.BadRequest);
            }
            
            return ObjectMapper.Map<User,UserDto>(user);
        }

        public async Task<ApiResponseBase> UpdateRolesForUser(string userName, List<string> roles)
        {
            var apiResponse = new ApiResponseBase();
            var user = await _userManager.FindByNameAsync(userName);

            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            await _userManager.AddToRolesAsync(user, roles);
            apiResponse.IsSuccess = true;
            apiResponse.Data = new List<string>();
            return apiResponse;
        }

        public Task<UserProfileModel> UpdateUserProfileAsync(UserProfileModel userProfileModel)
        {
            throw new NotImplementedException();
        }

        public async Task<UserPasswordUpdateModel> ChangePasswordAsync(UserPasswordUpdateModel userDto)
        {
            // var user = await _userManager.FindByNameAsync(userDto.UserName);
            // if (user == null)
            // {
            //     return new ApiResponseBase()
            //         {IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = HttpMessage.CheckInformation};
            // }
            //
            // var result = await _userManager.ChangePasswordAsync(user, userDto.CurrentPassword, userDto.NewPassword);
            //
            // return GetApiResponse(result);
            return null;
        }

        public async Task<UserDto> SetPasswordAsync(UserModel input)
        {
            // var user = await _userManager.FindByNameAsync(input.UserName);
            //
            // if (user == null)
            // {
            //     return new ApiResponseBase()
            //         {IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = HttpMessage.CheckInformation};
            // }
            //
            // var result = await _userManager.RemovePasswordAsync(user);
            // if (!result.Succeeded)
            // {
            //     return new ApiResponseBase()
            //         {IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = HttpMessage.CheckInformation};
            // }
            //
            // var nextResult = await _userManager.AddPasswordAsync(user, input.Password);
            //
            // return GetApiResponse(nextResult);
            return null;
        }

        public async Task<TokenDto> RefreshTokenAsync(TokenModel token)
        {
            if (token is null) throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
                

            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || user.RefreshToken != user.RefreshToken) throw new GlobalException(HttpMessage.CheckInformation, HttpStatusCode.BadRequest);
            
            var refreshToken = GenerateRefreshToken();
            var accessToken = await GenerateTokenByUser(user);
            user.RefreshToken = refreshToken;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                 throw new GlobalException(HttpMessage.CheckInformation, HttpStatusCode.BadRequest);
            }

            return new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        private async Task<string> GenerateTokenByUser(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = await _userRoleRepository.GetListAsync(x => x.UserId == user.Id);

            var userRoleClaims = _roleClaimRepository.GetRoleClaimsByRoles(userRole.Select(x => x.RoleId).ToList());
            var userClaims = userRoleClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue));

            List<Claim> claims = new List<Claim>();

            claims.AddRange(userClaims);
            foreach (var item in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(60),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience =
                    false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        private ApiResponseBase GetApiResponse(IdentityResult result)
        {
            var apiResponse = new ApiResponseBase();

            if (!result.Succeeded)
            {
                apiResponse.Message = result.Errors.FirstOrDefault().Description;
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                return apiResponse;
            }

            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            return apiResponse;
        }
    }
}