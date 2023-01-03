using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contract;
using Contract.PartReviewDetails;
using Core.Const;
using Core.Exceptions;
using Domain.PartReviewDetails;
using SqlServ4r.Repository.PartReviewDetails;
using SqlServ4r.Repository.PartReviews;
using Volo.Abp.DependencyInjection;

namespace Application.PartReviewDetails
{
    public class PartReviewDetailService:ServiceBase,IPartReviewDetailService,ITransientDependency
    {
        private PartReviewDetailRepository _partReviewDetailRepository;

        
        public PartReviewDetailService(PartReviewDetailRepository partReviewDetailRepository)
        {
            _partReviewDetailRepository = partReviewDetailRepository;
        }
        
        public async Task<PartReviewDetailDto> CreateAsync(CreateUpdatePartReviewDetailDto input)
        {
            var detail = ObjectMapper.Map<CreateUpdatePartReviewDetailDto,PartReviewDetail>(input);
            await _partReviewDetailRepository.AddAsync(detail);
            return ObjectMapper.Map<PartReviewDetail,PartReviewDetailDto>(detail);

        }
        

        public async Task<PartReviewDetailDto> UpdateAsync(CreateUpdatePartReviewDetailDto input,Guid id)
        {
            var detail = await _partReviewDetailRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (detail == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            detail = ObjectMapper.Map(input, detail);
            _partReviewDetailRepository.Update(detail);
            
            return ObjectMapper.Map<PartReviewDetail,PartReviewDetailDto>(detail);
        }


        public async Task DeleteAsync(Guid id)
        {
            var detail = await _partReviewDetailRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (detail == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }
            _partReviewDetailRepository.Remove(detail);
            
        }

        public async Task<List<PartReviewDetailDto>> GetListAsync()
        {
            var details = await _partReviewDetailRepository.ToListAsync();
            return ObjectMapper.Map<List<PartReviewDetail>,List<PartReviewDetailDto>>(details);
        }
        

        public async Task<List<PartReviewDetailDto>> GetDetailsByReviewIdAsync(Guid reviewId)
        {
            var details = await _partReviewDetailRepository.GetListWithNavigationPropertiesAsync(reviewId);
            var detailsDto = new List<PartReviewDetailDto>();
            
            var index = 1;
            foreach (var item in details)
            {
                var dto = ObjectMapper.Map<PartReviewDetail,PartReviewDetailDto>(item);
                dto.PartName = item.Part.Name;
                dto.Index = index;
                index++;
                detailsDto.Add(dto);
            }
            return detailsDto;
        }
        
    }
}