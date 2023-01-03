using System;
using System.Collections.Generic;
using Domain.PartReviewDetails;

namespace Domain.PartReviews
{
    public class PartReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Note { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsDeletion { get; set; }
        public IList<PartReviewDetail> PartReviewDetails { get; set; }

    }
}