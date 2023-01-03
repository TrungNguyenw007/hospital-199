using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.PartReviewDetails;
using Contract;
using Contract.PartReviewDetails;
using Domain.Parts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/part-review-detail/")]
    public class PartReviewDetailController :ControllerBase, IPartReviewDetailService
    {
        private PartReviewDetailService _partReviewDetailService;
        
        public PartReviewDetailController(PartReviewDetailService partReviewDetailService)
        {
            _partReviewDetailService = partReviewDetailService;
        }
        
        
        [HttpPost]
        public async Task<PartReviewDetailDto> CreateAsync(CreateUpdatePartReviewDetailDto input)
        {
           return  await _partReviewDetailService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<PartReviewDetailDto> UpdateAsync(CreateUpdatePartReviewDetailDto input, Guid id)
        {
            return  await _partReviewDetailService.UpdateAsync(input,id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
             await _partReviewDetailService.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<List<PartReviewDetailDto>> GetListAsync()
        {
            return await _partReviewDetailService.GetListAsync();
        }

        [HttpGet]
        [Route("get-details/{reviewId}")]
        public async Task<List<PartReviewDetailDto>> GetDetailsByReviewIdAsync(Guid reviewId)
        {
            return await _partReviewDetailService.GetDetailsByReviewIdAsync(reviewId);
        }
        
        
    }
}