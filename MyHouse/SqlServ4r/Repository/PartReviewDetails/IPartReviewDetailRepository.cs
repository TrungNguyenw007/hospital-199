using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.PartReviewDetails;

namespace SqlServ4r.Repository.PartReviewDetails
{
    public interface IPartReviewDetailRepository
    {
        Task<List<PartReviewDetail>> GetListWithNavigationPropertiesAsync(Guid reviewId);

    }
}