using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEventById
{
    public class GetConcertEventByIdQuery : IQuery<GetConcertEventByIdResponse, GetConcertEventByIdRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _eventRepository;
        private IBlobService _blobService;

        public GetConcertEventByIdQuery(IGenericRepository<QTimesContext, ConcertEvent> eventRepository, IBlobService blobService)
        {
            _eventRepository = eventRepository;
            _blobService = blobService;
        }

        public GetConcertEventByIdResponse Handle(GetConcertEventByIdRequest request)
        {

            var @event = _eventRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (@event == null)
                throw new NotFoundException("Event not found");

            var result = Mapper.Map<GetConcertEventByIdResponse>(@event);
            result.ConcertEventId = @event.Id;
            result.ImageFullPath = GetFilePath(result.ImagePath, @event.Concert.ImagePath);
            if (!result.EnablePrivacyPolicy.HasValue)
            {
                result.EnablePrivacyPolicy = @event.Concert.EnablePrivacyPolicy;
            }
            if (!result.EnableCommunityGuidelines.HasValue)
            {
                result.EnableCommunityGuidelines = @event.Concert.EnableCommunityGuidelines;
            }
            if (!result.EnableServiceTerms.HasValue)
            {
                result.EnableServiceTerms = @event.Concert.EnableServiceTerms;
            }
            if (string.IsNullOrWhiteSpace(result.Description))
            {
                result.Description = @event.Concert.Description;
            }
            if (string.IsNullOrWhiteSpace(result.VirtualNo))
            {
                result.VirtualNo = @event.Concert.VirtualNo;
            }
            if (string.IsNullOrWhiteSpace(result.MobileNo))
            {
                result.MobileNo = @event.Concert.MobileNo;
            }
            if (string.IsNullOrWhiteSpace(result.PhoneNo))
            {
                result.PhoneNo = @event.Concert.PhoneNo;
            }
            result.PrivacyPolicyFileFullPath = GetFilePath(result.PrivacyPolicyFilePath, @event.Concert.PrivacyPolicyFilePath);
            result.ServiceTermsFileFullPath = GetFilePath(result.ServiceTermsFilePath, @event.Concert.ServiceTermsFilePath);
            result.CummunityGuidelinesFileFullPath = GetFilePath(result.CummunityGuidelinesFilePath, @event.Concert.CummunityGuidelinesFilePath);
            result.SeatMapFullPath = _blobService.GetFileUrl(result.SeatMapPath) ?? "";
            result.TimeFrom = result.TimeFrom;
            result.TimeTo = result.TimeTo;
            result.TotalSeatingsAdded = result.ConcertSeatings.Sum(x => (x.SeatsPerSpot ?? 0) * (x.Spots ?? 0));
            return result;
        }

        private string GetFilePath(string eventFilePath, string concertFilePath)
        {
            if (string.IsNullOrWhiteSpace(eventFilePath))
            {
                return _blobService.GetFileUrl(concertFilePath) ?? "";
            }
            else
            {
                return _blobService.GetFileUrl(eventFilePath) ?? "";
            }
        }
    }
}