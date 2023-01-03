using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Identity.UserManager;
using Contract;
using Contract.Identity.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/user/")]
    public class UserController : IUserManagerService
    {
        private UserManagerService _userManagerService;

        public UserController(UserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        [HttpGet]
        [Route("get-list-with-nav")]
        public async Task<List<UserWithNavigationDto>> GetListWithNavigationAsync()
        {
           return  await _userManagerService.GetListWithNavigationAsync();
        }

        [HttpPost]
        [Route("create-with-nav")]
        public async Task<CreateUpdateUserWithNavDto> CreateWithNavigationAsync(CreateUpdateUserWithNavDto input)
        {
            return await _userManagerService.CreateWithNavigationAsync(input);
        }
        
        [HttpPost]
        [Route("update-with-nav/{id}")]
        public async Task<CreateUpdateUserWithNavDto> UpdateWithNavigationAsync(CreateUpdateUserWithNavDto input, Guid id)
        {
            return await _userManagerService.UpdateWithNavigationAsync(input,id);
        }
        
        [HttpPost]
        [Route("delete-with-nav")]
        public async  Task DeleteWithNavigationAsync(Guid id)
        {
             await _userManagerService.DeleteWithNavigationAsync(id);
        }

        [HttpGet]
        public async Task<List<UserDto>> GetListAsync()
        {
            return await _userManagerService.GetListAsync();
        }

        [HttpPost]
        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            return await _userManagerService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<UserDto> UpdateAsync(CreateUpdateUserDto input, Guid id)
        {
            return await _userManagerService.UpdateAsync(input, id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
             await _userManagerService.DeleteAsync(id);
        }
        
        
        [HttpPost]
        [Route("sign-in")]
        public async Task<TokenDto> SignInAsync(UserModel input)
        {
            return await _userManagerService.SignInAsync(input);
        }
        

        [HttpPost]
        [Route("sign-up")]

        public async Task<UserDto> SignUpAsync(CreateUpdateUserDto input)
        {
            return await _userManagerService.SignUpAsync(input);
        }

        // [HttpPost]
        // [Route("update-roles-for-user")]
        // public async Task<ApiResponseBase> UpdateRolesForUser(string userName, List<string> roles)
        // {
        //     return await _userManagerService.UpdateRolesForUser(userName,roles);
        // }

        [HttpPost]
        [Route("update-profile")]
        public async Task<UserProfileModel> UpdateUserProfileAsync(UserProfileModel userProfileModel)
        {
            return await _userManagerService.UpdateUserProfileAsync(userProfileModel);
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<UserPasswordUpdateModel> ChangePasswordAsync(UserPasswordUpdateModel user)
        {
            return await _userManagerService.ChangePasswordAsync(user);
        }
        

        [HttpPost]
        [Route("set-password")]
        public  async Task<UserDto> SetPasswordAsync(UserModel input)
        {
            return await _userManagerService.SetPasswordAsync(input);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<TokenDto> RefreshTokenAsync(TokenModel token)
        {
            return await _userManagerService.RefreshTokenAsync(token);
        }

        
        [HttpPost]
        [Route("upload-file")]
        public bool UploadFile()
        {
            return true;
        }
        
        [HttpGet]
        [Route("xxxxx")]
        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}