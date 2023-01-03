using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.PartReviews;
using Contract;
using Contract.PartReviewDetails;
using Contract.PartReviews;
using Contract.Parts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/part-review/")]
    public class PartReviewController : IPartReviewService
    {
        public PartReviewService _partReviewService;
        
        public PartReviewController(PartReviewService partReviewService)
        {
            _partReviewService = partReviewService;
        }
        
        [HttpGet]
        [Route("get-list-with-navigation")]
        public async Task<List<PartReviewDto>> GetListWithCalculatingAverageAsync()
        {
           return await _partReviewService.GetListWithCalculatingAverageAsync();
        }
        

        [HttpPost]
        public async Task<PartReviewDto> CreateAsync(CreateUpdatePartReviewDto input)
        {
            return await _partReviewService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<PartReviewDto> UpdateAsync(CreateUpdatePartReviewDto input, Guid Id)
        {
            return await _partReviewService.UpdateAsync(input, Id);
        }

        [HttpPost]
        [Route("create-review-details")]
        public async Task<PartReviewDto> CreateReviewWithDetailsAsync(List<CreateUpdatePartReviewDetailDto> inputs)
        {
            return await _partReviewService.CreateReviewWithDetailsAsync(inputs);
        }


        [HttpDelete]
        [Route("reviewId={reviewId}")]
        public async Task DeleteAsync(Guid reviewId)
        {
             await _partReviewService.DeleteAsync(reviewId);
        }

      


       
    }
    
    
}