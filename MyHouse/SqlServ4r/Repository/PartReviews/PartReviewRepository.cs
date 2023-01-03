using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Contract.PartReviews;
using Domain.PartReviews;
using Domain.Parts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SqlServ4r.EntityFramework;
using SqlServ4r.RepGenerationPatten;
using Volo.Abp.DependencyInjection;

namespace SqlServ4r.Repository.PartReviews
{
    public class PartReviewRepository : GenericRepository<PartReview,Guid>,ITransientDependency,IPartReviewRepository
    {
        public PartReviewRepository([NotNull] DreamContext context) : base(context)
        {
        }

        
        public async Task<List<PartReview>> GetListWithNavigationPropertiesAsync()
        {
            var items = await _context.PartReviews.Where(x=>!x.IsDeletion).
                Include(x => x.PartReviewDetails).ToListAsync();

            return items;
        }
    }
}