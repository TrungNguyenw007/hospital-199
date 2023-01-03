using System;

namespace Contract.PartReviews
{
    public class CreateUpdatePartReviewDto
    {
        public string? Note { get; set; } 
        public int Rating { get; set;}
        public Guid PartId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}