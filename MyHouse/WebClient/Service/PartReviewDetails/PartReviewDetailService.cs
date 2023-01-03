using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract;
using Contract.PartReviewDetails;
using WebClient.RequestHttp;

namespace WebClient.Service.PartReviewDetails
{
    public class PartReviewDetailService : IPartReviewDetailService
    {

        public PartReviewDetailService()
        {
            
        }
        
        public async Task<PartReviewDetailDto> CreateAsync(CreateUpdatePartReviewDetailDto input)
        {
            return await RequestClient.PostAPIAsync<PartReviewDetailDto>($"part-review-detail",input);
        }
        
        public async Task<PartReviewDetailDto> UpdateAsync(CreateUpdatePartReviewDetailDto input, Guid id)
        {
            return await RequestClient.PutAPIAsync<PartReviewDetailDto>($"part-review-detail/{id}",input);
        }
        
        public async Task DeleteAsync(Guid id)
        {
             await RequestClient.DeleteAPIAsync<Task>($"part-review-detail/{id}");
        }
        
        public async Task<List<PartReviewDetailDto>> GetListAsync()
        {
            return await RequestClient.GetAPIAsync<List<PartReviewDetailDto>>($"part-review-detail");

        }
        
        public async Task<List<PartReviewDetailDto>> GetDetailsByReviewIdAsync(Guid reviewId)
        {
            return await RequestClient.GetAPIAsync<List<PartReviewDetailDto>>($"part-review-detail/get-details/{reviewId}");
        }
    }
}