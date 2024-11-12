using AutoMapper;
using SeatQ.core.api.Queries.Reservation.GetBusinessProfile;
using SeatQ.core.api.Queries.Reservation.GetConcertById;
using SeatQ.core.api.Queries.Reservation.GetConcertEvents;
using SeatQ.core.api.Queries.Reservation.GetConcertMessageRule;
using SeatQ.core.api.Queries.Reservation.GetConcertReservations;
using SeatQ.core.api.Queries.Reservation.GetMessages;
using SeatQ.core.api.Queries.Reservation.GetReservationBusinessById;
using SeatQ.core.api.Queries.Reservation.GetReservationMessageRule;
using SeatQ.core.api.Queries.Reservation.GetReservations;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SeatQ.core.api.Infrastructure
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

            Mapper.CreateMap<Reservation, ReservationData>()
                .ForMember(dest => dest.GuestName, opt => opt.MapFrom(src => src.ReservationGuest.Name))
                .ForMember(dest => dest.GuestMobileNumber, opt => opt.MapFrom(src => src.ReservationGuest.MobileNumber))
                .ForMember(dest => dest.GuestType, opt => opt.MapFrom(src => src.GuestType.GuestType1))
                .ReverseMap();

            Mapper.CreateMap<ReservationMessageRule, MessageRules>().ReverseMap();
            Mapper.CreateMap<BusinessDetail, GetBusinessProfileResponse>()
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.ReservationBusiness.BusinessName))
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
            Mapper.CreateMap<ReservationMessage, Messages>()
                .ForMember(dest => dest.GuestName, opt => opt.MapFrom(src => src.Reservation.ReservationGuest.Name))
                .ReverseMap();
            Mapper.CreateMap<ConcertSeating, ConcertSpot>().ReverseMap();
            Mapper.CreateMap<Concert, ConcertEvent>().ReverseMap();
            Mapper.CreateMap<ConcertMessageRule, ConcertMessageRules>().ReverseMap();
            Mapper.CreateMap<ConcertReservation, ConcertReservationData>().ReverseMap();
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