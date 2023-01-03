using System;

namespace Contract.PartReviews
{
    public class PartReviewDto
    {
        public Guid Id { get; set;}
        public int Index { get; set; }
        public string Note { get; set; }
        public int AveragePoint { get; set;}
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}