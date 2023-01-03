using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Identity.RoleManager;
using Contract;
using Contract.Identity.RoleManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/role/")]
    public class RoleController : IRoleManagerService
    {
        public RoleManagerServiceService RoleManagerServiceService;

        public RoleController(RoleManagerServiceService roleManagerServiceService)
        {
            RoleManagerServiceService = roleManagerServiceService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<RoleDto>> GetListAsync()
        {
            return await RoleManagerServiceService.GetListAsync();
        }

        [HttpPost]
        public async Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
        {
            return await RoleManagerServiceService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<RoleDto> UpdateAsync(CreateUpdateRoleDto input, Guid id)
        {
            return await RoleManagerServiceService.UpdateAsync(input, id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
             await RoleManagerServiceService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("get-claims/{roleId}")]
        public async Task<List<RoleClaimDto>> GetClaimListAsync(Guid roleId)
        {
            return await RoleManagerServiceService.GetClaimListAsync(roleId);
        }

        [HttpPost]
        [Route("create-claim")]
        [Authorize("EmployeeOnly")]
        public async Task<CreateUpdateClaimRole> CreateClaimAsync(CreateUpdateClaimRole input)
        {
            return await RoleManagerServiceService.CreateClaimAsync(input);
        }

        [HttpPost]
        [Route("delete-claim")]
        public async Task DeleteClaimAsync(RoleClaimModel input)
        {
             await RoleManagerServiceService.DeleteClaimAsync(input);
        }
    }
}