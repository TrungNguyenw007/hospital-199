using System;
using Domain.PartReviews;
using Domain.Parts;

namespace Domain.PartReviewDetails
{
    public class PartReviewDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PartReviewId { get; set; }
        public Guid PartId { get; set; }
        public int Rate { get; set; }

        public PartReview PartReview { get; set; }
        public Part Part { get; set; }
    }
}