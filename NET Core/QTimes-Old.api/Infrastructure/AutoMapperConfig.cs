using AutoMapper;
using QTimes.api.Queries.Reservation.GetBusinessProfile;
using QTimes.api.Queries.Reservation.GetConcertById;
using QTimes.api.Queries.Reservation.GetReservationBusinessById;
using QTimes.core.dal.Models;
using QTimes.api.Queries.Reservation.GetConcertEvents;
using QTimes.api.Queries.Reservation.GetReservationMessageRule;
using QTimes.api.Queries.Reservation.GetReservations;
using System;
using System.Collections.Generic;
using System.Configuration;
using QTimes.api.Queries.Reservation.GetConcertEventMessageRule;
using QTimes.api.Queries.Reservation.GetConcertEventReservations;
using QTimes.api.Queries.Reservation.GetConcertEventById;

namespace QTimes.api.Infrastructure
{
    /// <summary>
    /// Auto mapper configuration
    /// </summary>
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.CreateMap<IEnumerable<string>, IEnumerable<string>>().ConvertUsing(x => x);
            Mapper.CreateMap<IEnumerable<KeyValuePair<DateTime, int>>, IEnumerable<KeyValuePair<DateTime, int>>>().ConvertUsing(x => x);
            Mapper.CreateMap<IEnumerable<KeyValuePair<string, int>>, IEnumerable<KeyValuePair<string, int>>>().ConvertUsing(x => x);

            Mapper.CreateMap<ConcertEventSeating, Queries.Reservation.GetConcertEventById.ConcertSpot>().ReverseMap();
            Mapper.CreateMap<Reservation, ReservationData>()
                .ForMember(dest => dest.GuestName, opt => opt.MapFrom(src => src.ReservationGuest.Name))
                .ForMember(dest => dest.GuestMobileNumber, opt => opt.MapFrom(src => src.ReservationGuest.MobileNumber))
                .ForMember(dest => dest.GuestType, opt => opt.MapFrom(src => src.GuestType.GuestType1))
                .ReverseMap();

            Mapper.CreateMap<ReservationMessageRule, MessageRules>().ReverseMap();
            Mapper.CreateMap<BusinessDetail, GetBusinessProfileResponse>()
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.ReservationBusiness.BusinessName))
                .ForMember(dest => dest.BusinessDetailName, opt => opt.MapFrom(src => src.BusinessName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ReservationBusiness.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ReservationBusiness.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ReservationBusiness.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ReservationBusiness.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ReservationBusiness.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.ReservationBusiness.Zip))
                .ForMember(dest => dest.TimezoneOffset, opt => opt.MapFrom(src => src.ReservationBusiness.TimezoneOffset))
                .ForMember(dest => dest.TimezoneOffsetValue, opt => opt.MapFrom(src => src.ReservationBusiness.TimezoneOffsetValue))
                .ReverseMap();
            Mapper.CreateMap<BusinessDetail, GetReservationBusinessByIdResponse>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ReservationBusiness.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ReservationBusiness.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ReservationBusiness.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.ReservationBusiness.Zip))
                .ReverseMap();
            Mapper.CreateMap<ReservationMessage, Queries.Reservation.GetMessages.Messages>()
                .ForMember(dest => dest.GuestName, opt => opt.MapFrom(src => src.Reservation.ReservationGuest.Name))
                .ReverseMap();

            Mapper.CreateMap<ConcertEvent, ConcertEventItem>().ReverseMap();
            Mapper.CreateMap<ConcertEventMessageRule, ConcertMessageRules>().ReverseMap();
            Mapper.CreateMap<ConcertEventReservation, ConcertEventReservationData>().ReverseMap();
            Mapper.CreateMap<Concert, GetConcertByIdResponse>()
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.ReservationBusiness.BusinessName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ReservationBusiness.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ReservationBusiness.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ReservationBusiness.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ReservationBusiness.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ReservationBusiness.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.ReservationBusiness.Zip))
                .ForMember(dest => dest.TimezoneOffset, opt => opt.MapFrom(src => src.ReservationBusiness.TimezoneOffset))
                .ForMember(dest => dest.TimezoneOffsetValue, opt => opt.MapFrom(src => src.ReservationBusiness.TimezoneOffsetValue))
                //.ForMember(dest => dest.ConcertSeatings, opt => opt.MapFrom(src => src.ConcertEventSeating))
                .ReverseMap();

            Mapper.CreateMap<ConcertEvent, GetConcertEventByIdResponse>()
                .ForMember(dest => dest.ConcertSeatings, opt => opt.MapFrom(src => src.ConcertEventSeating))
                .ReverseMap();

        }

        private static string CreateImagePath(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string cleared = imagePath.Replace("~", string.Empty);
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["CoreApiEndpoint"], cleared);
            }
            return string.Empty;
        }

    }

}