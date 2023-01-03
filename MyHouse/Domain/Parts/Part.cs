using System;
using System.Collections.Generic;
using Domain.PartReviewDetails;

namespace Domain.Parts
{
    public class Part
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Note { get; set; }
        public int Odx { get; set; }
        public bool IsActive { get; set; }
        public string? FileName { get; set; }
        public string? Path { get; set;}
        public string? ImageUrl { get; set;}
        public bool IsDeletion { get; set; }

        public IList<PartReviewDetail> PartReviewDetails { get; set; }
    }
}