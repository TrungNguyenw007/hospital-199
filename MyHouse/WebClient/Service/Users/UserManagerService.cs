using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Contract;
using Contract.Identity.UserManager;
using Microsoft.AspNetCore.Components.Authorization;
using WebClient.Identity;
using WebClient.RequestHttp;

namespace WebClient.Service.Users
{
    public class UserManagerService : IUserManagerService
    {
        private ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider  _authenticationStateProvider;


        public UserManagerService(
            ILocalStorageService localStorage,
            AuthenticationStateProvider  authenticationStateProvider)
        {
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }


        public async Task<List<UserWithNavigationDto>> GetListWithNavigationAsync()
        {   
             return await RequestClient.GetAPIAsync<List<UserWithNavigationDto>>("user/get-list-with-nav");
        }

        public async Task<CreateUpdateUserWithNavDto> CreateWithNavigationAsync(CreateUpdateUserWithNavDto input)
        {
            return await RequestClient.PostAPIAsync<CreateUpdateUserWithNavDto>("user/create-with-nav", input);
        }

        public async Task<CreateUpdateUserWithNavDto> UpdateWithNavigationAsync(CreateUpdateUserWithNavDto input, Guid id)
        {
            return await RequestClient.PostAPIAsync<CreateUpdateUserWithNavDto>($"user/update-with-nav/{id}", input);
        }

        public async Task DeleteWithNavigationAsync(Guid id)
        {
             await RequestClient.PostAPIAsync<Task>($"user/delete-with-nav/{id}",null);
        }

        public async Task<List<UserDto>> GetListAsync()
        {
            return await RequestClient.GetAPIAsync<List<UserDto>>("user");
        }

        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            return await RequestClient.PostAPIAsync<UserDto>("user",input);
        }

        public async Task<UserDto> UpdateAsync(CreateUpdateUserDto input, Guid id)
        {
            return await RequestClient.PutAPIAsync<UserDto>($"user/{id}",input);

        }

        public async Task DeleteAsync(Guid id)
        {
             await RequestClient.DeleteAPIAsync<Task>($"user/{id}");
        }

        public async Task<TokenDto> SignInAsync(UserModel input)
        {
            var response = await RequestClient.PostAPIAsync<TokenDto>("user/sign-in", input);
            RequestClient.AttachToken(response.AccessToken);
                RequestClient.SetLocalStorageService(_localStorage);
                await _localStorage.SetItemAsync("my-access-token", response.AccessToken);
                await _localStorage.SetItemAsync("my-refresh-token", response.RefreshToken);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(input.UserName);
            
            return response;
        }
        

        public async Task<UserDto> SignUpAsync(CreateUpdateUserDto input)
        {
            return await RequestClient.PostAPIAsync<UserDto>("user/sign-up", input);
        }



        public Task<UserProfileModel> UpdateUserProfileAsync(UserProfileModel userProfileModel)
        {
            throw new NotImplementedException();
        }

        public Task<UserPasswordUpdateModel> ChangePasswordAsync(UserPasswordUpdateModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> SetPasswordAsync(UserModel input)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> RefreshTokenAsync(TokenModel token)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }


        public async Task test()
        { 
            await RequestClient.GetAPIAsync<Task>("test/alo");
        }
    }
}