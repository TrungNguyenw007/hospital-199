using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Contract.Identity.RoleManager;
using Contract.Identity.UserManager;
using Core.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace WebClient.Pages.Admin
{
    public partial class UserManager
    {
        [Inject] IMessageService _messageService { get; set; }
        private List<UserWithNavigationDto> UsersWithNav { get; set; } = new List<UserWithNavigationDto>();
        private CreateUpdateUserWithNavDto NewUser { get; set; } = new CreateUpdateUserWithNavDto();
        private CreateUpdateUserWithNavDto EditUser { get; set; } = new CreateUpdateUserWithNavDto();
        private List<RoleDto> Roles = new List<RoleDto>();
        private List<string> RoleNames = new List<string>();
        private List<string> SelectedRoles = new List<string>();
        private Guid EditUserId { get; set; }

        private Modal CreateModal = new Modal();
        private Modal EditModal = new Modal();
        private string HeaderTitle = "User";


        public UserManager()
        {
        }


        protected override async Task OnInitializedAsync()
        {

        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await GetUsers();
                await GetRoles();
                StateHasChanged();
            }
        }

        private async Task GetUsers()
        {
            UsersWithNav = await _userManagerService.GetListWithNavigationAsync();
        }

        private async Task GetRoles()
        {
            Roles = await _roleManagerService.GetListAsync();
            RoleNames = Roles.Select(x => x.Name).ToList();
        }

        private async Task CreateUser()
        {
            await InvokeAsync(async () =>
            {
                await _userManagerService.CreateWithNavigationAsync(NewUser);
                HideNewModal();
                await GetUsers();
            }, ActionType.Create, true);
        }

        private async Task UpdateUser()
        {
            await _userManagerService.UpdateWithNavigationAsync(EditUser, EditUserId);
            await GetUsers();
            await HideEditModal();
            await GetUsers();
        }

        private async Task DeleteUser(Guid id)
        {
            await _userManagerService.DeleteAsync(id);
            await GetUsers();
        }
        
        
        private async Task ShowConfirmMessage(Guid id)
        {
            if ( await _messageService.Confirm( "Are you sure you want to confirm?", "Confirmation" ) )
            {
                await DeleteUser(id);
            }
        }


        private void ShowNewModal()
        {
            NewUser = new CreateUpdateUserWithNavDto();
            SelectedRoles = new List<string>();
            CreateModal.Show();
        }

        private void HideNewModal()
        {
            NewUser = new CreateUpdateUserWithNavDto();
            CreateModal.Hide();
        }


        private Task ShowEditModal(Guid id)
        {
            EditUser = new CreateUpdateUserWithNavDto();
            SelectedRoles = new List<string>();

            var userWithNavDto = UsersWithNav.FirstOrDefault(x => x.UserDto.Id == id);
            EditUser.Roles = userWithNavDto.RoleNames;
            SelectedRoles = EditUser.Roles;
            EditUserId = id;
            EditUser.User = ObjectMapper.Map<UserDto, CreateUpdateUserDto>(userWithNavDto.UserDto);
            return EditModal.Show();
        }

        public BadgeStyle ChooseColorByNumber(int i)
        {
            switch (i)
            {
                case 1:
                {
                    return BadgeStyle.Info;
                }
                case 2:
                {
                    return BadgeStyle.Primary;
                }
                case 3:
                {
                    return BadgeStyle.Success;
                }
                case 4:
                {
                    return BadgeStyle.Light;
                }
                default:
                {
                    return BadgeStyle.Danger;
                }
            }
        }

        public void OnChooseFile()
        {
            this.StateHasChanged();
        }


        void OnEditSelectedRoles(object value)
        {
            var selectedRoles = ((IEnumerable<string>) value).ToList();
            EditUser.Roles = selectedRoles;
        }

        void OnCreateSelectedRoles(object value)
        {
            var selectedRoles = ((IEnumerable<string>) value).ToList();
            NewUser.Roles = selectedRoles;
        }

        private Task HideEditModal()
        {
            return EditModal.Hide();
        }
    }
}