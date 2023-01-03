using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.PartReviews;
using Domain.PartReviewDetails;
using Domain.PartReviews;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SqlServ4r.EntityFramework;
using SqlServ4r.RepGenerationPatten;
using Volo.Abp.DependencyInjection;

namespace SqlServ4r.Repository.PartReviewDetails
{
    public class PartReviewDetailRepository : GenericRepository<PartReviewDetail,Guid>,ITransientDependency,IPartReviewDetailRepository
    {
        public PartReviewDetailRepository([NotNull] DreamContext context) : base(context)
        {
        }

        public async Task<List<PartReviewDetail>> GetListWithNavigationPropertiesAsync(Guid reviewId)
        {
          var details = await  _context.PartReviewDetails.Where(x=>x.PartReviewId == reviewId).
              Include(x => x.Part).ToListAsync();
          return details;
        }
    }
}