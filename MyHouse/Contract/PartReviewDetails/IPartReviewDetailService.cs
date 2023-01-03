using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contract.PartReviewDetails
{
    public interface IPartReviewDetailService
    {
        public Task<PartReviewDetailDto> CreateAsync(CreateUpdatePartReviewDetailDto input);
        public Task<PartReviewDetailDto> UpdateAsync(CreateUpdatePartReviewDetailDto input,Guid id);
        public Task DeleteAsync(Guid id);
        public Task<List<PartReviewDetailDto>> GetListAsync();

        public Task<List<PartReviewDetailDto>> GetDetailsByReviewIdAsync(Guid reviewId);
    }
}