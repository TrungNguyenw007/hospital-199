using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Identity.Users;

namespace Contract.Identity.UserManager
{
    public interface IUserManagerService
    {
        public Task<List<UserWithNavigationDto>> GetListWithNavigationAsync();
        public Task<CreateUpdateUserWithNavDto> CreateWithNavigationAsync(CreateUpdateUserWithNavDto input);
        public Task<CreateUpdateUserWithNavDto> UpdateWithNavigationAsync(CreateUpdateUserWithNavDto input,Guid id);
        public Task DeleteWithNavigationAsync(Guid id);
        public Task<List<UserDto>> GetListAsync();
        public Task<UserDto> CreateAsync(CreateUpdateUserDto input);
        public Task<UserDto> UpdateAsync(CreateUpdateUserDto input,Guid id);
        public Task DeleteAsync(Guid id);
        public Task<TokenDto> SignInAsync(UserModel input);
        public Task<UserDto> SignUpAsync(CreateUpdateUserDto input);
        // public Task<ApiResponseBase> UpdateRolesForUser(string userName,List<string> roles);
        public Task<UserProfileModel> UpdateUserProfileAsync(UserProfileModel userProfileModel);
        public Task<UserPasswordUpdateModel> ChangePasswordAsync(UserPasswordUpdateModel user);
        public Task<UserDto> SetPasswordAsync(UserModel input);
        public Task<TokenDto> RefreshTokenAsync(TokenModel token);

        public void Logout();

    }
}