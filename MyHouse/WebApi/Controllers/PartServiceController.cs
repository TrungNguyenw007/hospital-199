using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Parts;
using Contract;
using Contract.Identity.UserManager;
using Contract.Parts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/part/")]
    public class PartServiceController :  ControllerBase, IPartService
    {
        private PartService _partService;
        public PartServiceController(PartService partService)
        {
            _partService = partService;
        }
        
      
        [HttpPost]
        public async Task<PartDto> CreateAsync(CreateUpdatePartDto input)
        {
           return  await _partService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<PartDto> UpdateAsync(CreateUpdatePartDto input, Guid id)
        {
            return await _partService.UpdateAsync(input,id);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _partService.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<List<PartDto>> GetListAsync()
        {
            return await _partService.GetListAsync();
        }

        [HttpPost]
        [Route("get-list-by-filter")]
        public async Task<List<PartDto>> GetListAsync(PartFilter input)
        {
            return await _partService.GetListAsync(input);
        }
    }
}