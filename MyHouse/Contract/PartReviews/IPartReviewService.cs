using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.PartReviewDetails;
using Contract.Parts;
using Domain.PartReviewDetails;

namespace Contract.PartReviews
{
    public interface IPartReviewService
    {
        Task<List<PartReviewDto>> GetListWithCalculatingAverageAsync();
        Task<PartReviewDto> CreateAsync(CreateUpdatePartReviewDto input);
        Task<PartReviewDto> UpdateAsync(CreateUpdatePartReviewDto input,Guid Id);
        Task<PartReviewDto> CreateReviewWithDetailsAsync(List<CreateUpdatePartReviewDetailDto> inputs);
        Task DeleteAsync(Guid reviewId);
        


    }
}