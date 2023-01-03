using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Contract.PartReviewDetails;
using Contract.PartReviews;
using Contract.Parts;
using Core.Enum;
using Radzen;

namespace WebClient.Pages.Client
{
    public partial class PartReview
    {
        private CreateUpdatePartReviewDto NewReview { get; set; } = new CreateUpdatePartReviewDto();
        private Guid EditReviewId { get; set;}
        private List<PartDto> Parts = new List<PartDto>();
        private Modal CreateModal = new Modal();
        private List<ReviewWithNavPropertiesModel> ReviewsWithNav = new List<ReviewWithNavPropertiesModel>();
        private string ReviewNote { get; set; }
        private List<PartReviewDto> Result { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            
            if (firstRender)
            {
                await GetParts();
                Init();
                StateHasChanged();
                
            }
        }

        public void Init()
        {
            ReviewsWithNav = ObjectMapper.Map<List<PartDto>, List<ReviewWithNavPropertiesModel>>(Parts);
        }

      

        private async Task CreatePartReviewsWithDetail()
        {

            await InvokeAsync(async () =>
            {
                var details = new List<CreateUpdatePartReviewDetailDto>();
                foreach (var item in ReviewsWithNav)
                {
                    details.Add(new CreateUpdatePartReviewDetailDto() {PartId = item.PartId, Rate = item.Rating});
                }

                var reviewDto = await _partReviewService.CreateReviewWithDetailsAsync(details);
                Init();
                EditReviewId = reviewDto.Id;
                await ShowInlineDialog();
            },ActionType.Create,true);

        }

        private async Task UpdatePartReviews()
        {
            if (!ReviewNote.IsNullOrWhiteSpace())
            {
                await InvokeAsync(async () =>
                {
                    NewReview.Note = ReviewNote;
                    await _partReviewService.UpdateAsync(NewReview, EditReviewId);
                    NewReview = new CreateUpdatePartReviewDto();
                    EditReviewId = new Guid();
                    ReviewNote = "";
                },ActionType.Update,true);
            }
        }
        
    
        
        private async Task GetParts()
        {
            Parts = await _partService.GetListAsync(new PartFilter(){IsActive = true});
        }


        private async Task HideDiaLog(DialogService ds)
        {
            await UpdatePartReviews();
            ds.Close(true);
        }


        public class ReviewWithNavPropertiesModel
        {
            public Guid PartId { get; set; } = Guid.NewGuid(); 
            public string Name { get; set; }
            public int Rating { get; set;}
            public string ImageUrl { get; set;}
        }
        
    }
}