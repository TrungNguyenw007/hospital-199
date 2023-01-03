using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract;
using Contract.PartReviewDetails;
using Contract.PartReviews;
using Contract.Parts;
using Microsoft.AspNetCore.Components;
using WebClient.RequestHttp;

namespace WebClient.Service.PartReviews
{
    public class PartReviewService : IPartReviewService
    {
        
        
        public async Task<List<PartDto>> LookUpPartsAsync()
        {
            return await RequestClient.GetAPIAsync<List<PartDto>>($"part-review/look-up-parts");
        }

        public async Task<List<PartReviewDto>> GetListWithCalculatingAverageAsync()
        {
            return await RequestClient.GetAPIAsync<List<PartReviewDto>>($"part-review/get-list-with-navigation");
        }

        public async Task<List<PartReviewDetailDto>> GetDetailsAsync(Guid reviewId)
        {
            return await RequestClient.GetAPIAsync<List<PartReviewDetailDto>>($"part-review/get-list-with-navigation");
        }

        public async  Task<PartReviewDto> CreateAsync(CreateUpdatePartReviewDto input)
        {
            return await RequestClient.PostAPIAsync<PartReviewDto>($"part-review",input,false);
        }

        public async Task<PartReviewDto> UpdateAsync(CreateUpdatePartReviewDto input, Guid Id)
        {
            return await RequestClient.PutAPIAsync<PartReviewDto>($"part-review/{Id}",input);
        }

        public async Task<PartReviewDto> CreateReviewWithDetailsAsync(List<CreateUpdatePartReviewDetailDto> inputs)
        {
            return await RequestClient.PostAPIAsync<PartReviewDto>($"part-review/create-review-details",inputs);
        }
        
        public async Task DeleteAsync(Guid reviewId)
        {
             await RequestClient.DeleteAPIAsync<Task>($"part-review/reviewId={reviewId}");
        }
        

    
    }
}