using System;

namespace Contract.PartReviewDetails
{
    public class PartReviewDetailDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public Guid PartReviewId { get; set; }
        public Guid PartId { get; set; }
        public string PartName { get; set;}
        public int Rate { get; set; }
        
    }



    
}