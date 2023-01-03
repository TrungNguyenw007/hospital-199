using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.PartReviews;
using Domain.PartReviews;

namespace SqlServ4r.Repository.PartReviews
{
    public interface IPartReviewRepository
    {
        Task<List<PartReview>> GetListWithNavigationPropertiesAsync();
    }
}