using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contract.Parts;
using Core.Const;
using Core.Exceptions;
using Domain.Parts;
using Microsoft.EntityFrameworkCore;
using SqlServ4r.Repository.Parts;
using Volo.Abp.DependencyInjection;

namespace Application.Parts
{
    public class PartService : ServiceBase, IPartService, ITransientDependency
    {
        public PartRepository _partRepository;

        public PartService(PartRepository partRepository)
        {
            _partRepository = partRepository;
        }


        public async Task<PartDto> CreateAsync(CreateUpdatePartDto input)
        {
            var part = ObjectMapper.Map<CreateUpdatePartDto, Part>(input);
            await _partRepository.AddAsync(part);
            return ObjectMapper.Map<Part,PartDto>(part);
        }
        

        public async Task<PartDto> UpdateAsync(CreateUpdatePartDto input, Guid id)
        {
            var part = await _partRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (part == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }
            part = ObjectMapper.Map(input, part);
            _partRepository.Update(part);
            
            return ObjectMapper.Map<Part,PartDto>(part);
        }

        public async Task DeleteAsync(Guid id)
        {
            var part = await _partRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (part == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }
            
            part.IsDeletion = true;
            _partRepository.Update(part);
        }

        public async Task<List<PartDto>> GetListAsync()
        {
            var parts = await _partRepository.GetListAsync(x=>x.IsDeletion == false);
            var partsDto = ObjectMapper.Map<List<Part>,List<PartDto>>(parts);
            AttachIndex(partsDto);
            return partsDto;
        }

        public async Task<List<PartDto>> GetListAsync(PartFilter input)
        {
          var parts =  await _partRepository.GetQueryable().Where(x => x.IsDeletion == false)
                .WhereIf(!input.TextFilter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.TextFilter))
                .WhereIf(input.IsActive != null, x => x.IsActive == input.IsActive).ToListAsync();

          var partsDto = ObjectMapper.Map<List<Part>,List<PartDto>>(parts);
            AttachIndex(partsDto);
            return partsDto;
        }
    }
}