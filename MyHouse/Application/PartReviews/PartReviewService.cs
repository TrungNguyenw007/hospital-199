using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contract;
using Contract.PartReviewDetails;
using Contract.PartReviews;
using Contract.Parts;
using Core.Const;
using Core.Enum;
using Core.Exceptions;
using Domain.PartReviewDetails;
using Domain.PartReviews;
using Domain.Parts;
using Microsoft.EntityFrameworkCore;
using SqlServ4r.Repository.PartReviewDetails;
using SqlServ4r.Repository.PartReviews;
using SqlServ4r.Repository.Parts;
using Volo.Abp.DependencyInjection;

namespace Application.PartReviews
{
    public class PartReviewService : ServiceBase, IPartReviewService, ITransientDependency
    {
        public PartReviewRepository _partReviewRepository;

        public PartReviewDetailRepository _partReviewDetailRepository;


        public PartReviewService(PartReviewRepository partReviewRepository,
            PartReviewDetailRepository partReviewDetailRepository)
        {
            _partReviewRepository = partReviewRepository;
            _partReviewDetailRepository = partReviewDetailRepository;
        }

        public async Task<List<PartReviewDto>> GetListWithCalculatingAverageAsync()
        {
            var reviewsWithNav = await _partReviewRepository.GetListWithNavigationPropertiesAsync();
            var reviewsWithNavDto = new List<PartReviewDto>();

            foreach (var item in reviewsWithNav)
            {
                var dto = ObjectMapper.Map<PartReview, PartReviewDto>(item);
                if (item.PartReviewDetails.Count > 0)
                {
                    dto.AveragePoint = item.PartReviewDetails.Sum(x => x.Rate) / item.PartReviewDetails.Count;
                }

                reviewsWithNavDto.Add(dto);
            }

            AttachIndex(reviewsWithNavDto);
            return reviewsWithNavDto;
        }


        public async Task<PartReviewDto> CreateAsync(CreateUpdatePartReviewDto input)
        {
            var review = ObjectMapper.Map<CreateUpdatePartReviewDto, PartReview>(input);
            await _partReviewRepository.AddAsync(review);
            return ObjectMapper.Map<PartReview, PartReviewDto>(review);
        }

        public async Task<PartReviewDto> UpdateAsync(CreateUpdatePartReviewDto input, Guid Id)
        {
            var review = await _partReviewRepository.FirstOrDefaultAsync(x => x.Id == Id);

            if (review == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            ObjectMapper.Map(input, review);
            _partReviewRepository.Update(review);

            return ObjectMapper.Map<PartReview, PartReviewDto>(review);
        }

        public async Task<PartReviewDto> CreateReviewWithDetailsAsync(List<CreateUpdatePartReviewDetailDto> inputs)
        {
            var review = new PartReview();
            await _partReviewRepository.AddAsync(review);
            var details = ObjectMapper.Map<List<CreateUpdatePartReviewDetailDto>, List<PartReviewDetail>>(inputs);

            foreach (var item in details)
            {
                item.PartReviewId = review.Id;
            }

            await _partReviewDetailRepository.AddRangeAsync(details);

            return ObjectMapper.Map<PartReview, PartReviewDto>(review); 
        }

        public async Task DeleteAsync(Guid reviewId)
        {
            var review = await _partReviewRepository.FirstOrDefaultAsync(x => x.Id == reviewId);

            if (review == null)
            {
                throw new GlobalException(HttpMessage.NotFound, HttpStatusCode.BadRequest);
            }

            review.IsDeletion = true;
            _partReviewRepository.Update(review);
        }
    }
}