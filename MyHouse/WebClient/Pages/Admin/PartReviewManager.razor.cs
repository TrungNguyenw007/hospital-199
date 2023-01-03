using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal.Mappers;
using Blazorise;
using Contract.Identity.RoleManager;
using Contract.Identity.UserManager;
using Contract.PartReviews;
using Contract.Parts;
using Core.Enum;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace WebClient.Pages.Admin
{
    public partial class PartReviewManager
    {
        [Inject] IMessageService _messageService { get; set; }
        private List<PartReviewDto> PartReviews { get; set; } =
            new List<PartReviewDto>();

        private CreateUpdatePartReviewDto NewReview { get; set; } = new CreateUpdatePartReviewDto();
        private CreateUpdatePartReviewDto EditReview { get; set; } = new CreateUpdatePartReviewDto();
        private List<PartDto> Parts = new List<PartDto>();
       


        private Guid SelectedNewPartId = new Guid();
        private Guid EditPartId { get; set; }
        private Guid EditReviewId { get; set;}

        private Modal CreateModal = new Modal();
        
        private Modal EditModal = new Modal();
        private string HeaderTitle { get; set; } = "Part Review";


        public PartReviewManager()
        {
            
        }


        protected override async Task OnInitializedAsync()
        {
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            { 
                await GetPartReviews();
                StateHasChanged();
            }
        }

        private async Task GetPartReviews()
        {
            PartReviews = await _partReviewService.GetListWithCalculatingAverageAsync();
        }
        private async Task NavigateToDetails(Guid reviewId)
        {
            _navigationManager.NavigateTo($"part-review-detail?reviewId={reviewId}");
        }

        private async Task ShowConfirmMessage(Guid reviewId)
        {
            if ( await _messageService.Confirm( "Are you sure you want to confirm?", "Confirmation" ) )
            {
                await InvokeAsync(async () =>
                {
                    await DeletePartReview(reviewId);
                }, ActionType.Delete, true);
            }
            
        }
        private async Task DeletePartReview(Guid reviewId)
        {
            await _partReviewService.DeleteAsync(reviewId);
            await GetPartReviews();
        }
        
        private void ShowNewModal()
        {
            NewReview = new CreateUpdatePartReviewDto();
            CreateModal.Show();
        }
        
        
        private void ShowEditModal(PartReviewDto input)
        {
            EditPartId = input.Id;
            EditReviewId = input.Id;
            EditModal.Show();
        }
    }
}