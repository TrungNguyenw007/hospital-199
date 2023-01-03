using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Contract.PartReviewDetails;
using Core.Enum;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace WebClient.Pages.Admin
{
    public partial class PartReviewDetailManager
    {
        
        [Parameter]
        [SupplyParameterFromQuery(Name = "reviewId")]
        public Guid ReviewID { get; set; }



        public List<PartReviewDetailDto> Details = new List<PartReviewDetailDto>();
        public CreateUpdatePartReviewDetailDto NewDetail = new CreateUpdatePartReviewDetailDto();
        public CreateUpdatePartReviewDetailDto EditDetail = new CreateUpdatePartReviewDetailDto();
        public Guid EditDetailId { get; set;}
        
        private Modal CreateModal;
        private Modal EditModal;
        private string HeaderTitle { get; set; } = "Review Detail ";



        protected override Task OnInitializedAsync()
        {
            
            return base.OnInitializedAsync();
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
               await GetDetails();
               
               StateHasChanged();
            }
        }

        private async Task CreateDetail()
        {
            await InvokeAsync(async () =>
            {
                NewDetail.PartReviewId = ReviewID;
                await _partReviewDetailService.CreateAsync(NewDetail);
                await GetDetails();
                HideNewModal();
            },ActionType.Create,true);
        }
        
        private async Task UpdateDetail()
        {

            await InvokeAsync(async () =>
            {
                await _partReviewDetailService.UpdateAsync(EditDetail, EditDetailId);
                await GetDetails();
                HideEditModal();
            }, ActionType.Update, true);
        }

        public async Task DeleteDetail(Guid id)
        {
            await InvokeAsync( async () =>
            {
                await _partReviewDetailService.DeleteAsync(id);
                await GetDetails();
            }, ActionType.Delete, true);

        }
        
        private async Task GetDetails()
        {
            Details = await _partReviewDetailService.GetDetailsByReviewIdAsync(ReviewID);
        }
        
        private void ShowNewModal()
        {
            NewDetail = new CreateUpdatePartReviewDetailDto();
            CreateModal.Show();
        }
        

        private void HideNewModal()
        {
            NewDetail = new CreateUpdatePartReviewDetailDto();
            CreateModal.Hide();
        }
        
        
        
        private Task ShowEditModal(PartReviewDetailDto input)
        {
            EditDetail = new CreateUpdatePartReviewDetailDto();
            EditDetailId = input.Id;
            EditDetail = ObjectMapper.Map<PartReviewDetailDto,CreateUpdatePartReviewDetailDto>(input);
            return EditModal.Show();
        }

        private  void HideEditModal()
        {
             EditModal.Hide();
        }
        
        

        
    }
}