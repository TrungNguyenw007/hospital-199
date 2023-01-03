using System;

namespace Contract.PartReviewDetails
{
    public class CreateUpdatePartReviewDetailDto
    {
        public Guid? PartReviewId { get; set; }
        public Guid? PartId { get; set; }
        public int Rate { get; set; }
    }
}