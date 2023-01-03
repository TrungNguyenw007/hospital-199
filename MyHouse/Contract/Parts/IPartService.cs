using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.Identity.UserManager;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Contract.Parts
{
    public interface IPartService
    {
        Task<PartDto> CreateAsync(CreateUpdatePartDto input);
        Task<PartDto> UpdateAsync(CreateUpdatePartDto input,Guid id);
        Task DeleteAsync(Guid id);
        Task<List<PartDto>> GetListAsync();
        
        Task<List<PartDto>> GetListAsync(PartFilter input);

    }
}