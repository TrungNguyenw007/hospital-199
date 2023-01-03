using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Contract;
using Contract.Identity.RoleManager;
using Core.Const;
using Core.Exceptions;
using Domain.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Application.Identity.RoleManager
{
    public class RoleManagerServiceService : ServiceBase, IRoleManagerService,ITransientDependency
    {
        private RoleManager<Role> _roleManager;

        public RoleManagerServiceService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }


        public async Task<List<RoleDto>> GetListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesDto = ObjectMapper.Map<List<Role>, List<RoleDto>>(roles);
            
            var count = 1;
            foreach (var item in rolesDto)
            {
                item.Index = count;
                count++;
            }

            return rolesDto;
        }

        public async Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
        {
            var role = ObjectMapper.Map<CreateUpdateRoleDto, Role>(input);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);

            }

            return ObjectMapper.Map<Role,RoleDto>(role);
        }

        public async Task<RoleDto> UpdateAsync(CreateUpdateRoleDto input, Guid id)
        {
            var item = await _roleManager.FindByIdAsync(id.ToString());

            if (item == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            var role = ObjectMapper.Map(input, item);
            var result = await _roleManager.UpdateAsync(role);
            
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);

            }
            
            return  ObjectMapper.Map<Role,RoleDto>(role);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            var result = await _roleManager.DeleteAsync(role);
            
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);

            }
        }

        public async Task<List<RoleClaimDto>> GetClaimListAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)   throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            var claims = await _roleManager.GetClaimsAsync(role);
            List<RoleClaimDto> roleClaim = new List<RoleClaimDto>();

            foreach (var item in claims)
            {
                roleClaim.Add(new RoleClaimDto(){RoleId = roleId , ClaimType = item.Type,ClaimValue = item.Value});
            }
            
            return roleClaim;
        }

        
        public async Task<CreateUpdateClaimRole> CreateClaimAsync(CreateUpdateClaimRole input)
        {
            var role = await _roleManager.FindByIdAsync(input.RoleId.ToString());
            if (role == null) throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            
            var result =  await _roleManager.AddClaimAsync(role, new Claim(input.ClaimType, input.ClaimValue));
            return input;
        }

        public async Task DeleteClaimAsync(RoleClaimModel input)
        {
            var role = await _roleManager.FindByIdAsync(input.RoleId.ToString());
            if (role == null) throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            var result = await _roleManager.RemoveClaimAsync(role, new Claim(input.ClaimType,input.ClaimValue));
            
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Description, HttpStatusCode.BadRequest);
            }
            
        }

        
        
    }
}