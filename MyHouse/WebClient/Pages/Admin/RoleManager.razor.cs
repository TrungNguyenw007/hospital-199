using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Contract.Identity.RoleManager;
using Contract.Identity.UserManager;
using Microsoft.AspNetCore.Components;


namespace WebClient.Pages.Admin
{
    public partial class RoleManager
    {
        private List<RoleDto> Roles = new List<RoleDto>();
        private CreateUpdateRoleDto NewRole = new CreateUpdateRoleDto();
        private CreateUpdateRoleDto EditRole = new CreateUpdateRoleDto();
        private Guid EditRoleId { get; set; }
        [Inject] IMessageService _messageService { get; set; }


        private Modal CreateModal;
        private Modal EditModal;
        private string HeaderTitle = "Role";

        public RoleManager()
        {
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await GetRoles();
                StateHasChanged();
            }
        }

        private async Task GetRoles()
        {
            Roles = await _roleManagerService.GetListAsync();
        }

        private async Task CreateRole()
        {
            try
            {
                await _roleManagerService.CreateAsync(input: NewRole);
                HideNewModal();
                await GetRoles();
            }
            catch (Exception e)
            {
                
            }

        }

        private async Task UpdateRole()
        {
            await _roleManagerService.UpdateAsync(EditRole, EditRoleId);
                HideEditModal();
                await GetRoles();
        }

        private async Task DeleteRole(Guid id)
        {
            await _roleManagerService.DeleteAsync(id);
                HideEditModal();
                await GetRoles();
        }

        private async Task ShowConfirmMessage(Guid id)
        {
            if ( await _messageService.Confirm( "Are you sure you want to confirm?", "Confirmation" ) )
            {
                await DeleteRole(id);
            }
        }

        private void ShowNewModal()
        {
            NewRole = new CreateUpdateRoleDto();
            CreateModal.Show();
        }

        private void HideNewModal()
        {
            CreateModal.Hide();
        }


        private Task ShowEditModal(RoleDto roleModel)
        {
            EditRole = new CreateUpdateRoleDto();
            EditRole = ObjectMapper.Map<RoleDto, CreateUpdateRoleDto>(roleModel);
            EditRoleId = roleModel.Id;
            return EditModal.Show();
        }

        private void HideEditModal()
        {
            EditModal.Hide();
        }
    }
}