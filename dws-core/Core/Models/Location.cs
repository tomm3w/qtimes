using System;
using System.Data.Entity.Spatial;
using System.Xml.Linq;
using Core.Extensions;
using Core.Helpers;
namespace Core.Models
{
    public class Money
    {
        public decimal Value { get; set; }
        public string Currency { get; set; }
    }
    public class IPDetailsModel
    {
        public long IP { get; set; }
        public string Code { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryISOCode2 { get; set; }
        public string CountryISOCode3 { get; set; }
        public string CountryISOCountry { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public partial class Location
    {
        public int LocationId { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public DbGeography Geography { get; set; }
        public string Country { get; set; }
        public string SubLocality { get; set; }
        public string Locality { get; set; }
        public string AdministrativeAreaLevel1 { get; set; }
        public string AdministrativeAreaLevel2 { get; set; }
        public Nullable<long> PostalCode { get; set; }
        public string FormattedAddress { get; set; }
    }
    public partial class Location
    {
        public static implicit operator Location(XElement xmlElement)
        {
            Location retVal = new Location();
            retVal.FormattedAddress = xmlElement.Value("formatted_address");
            foreach (var addressComponent in xmlElement.Elements("address_component"))
            {
                if (addressComponent.Value("type") == "sublocality")
                {
                    retVal.SubLocality = addressComponent.Value("long_name");
                }
                else if (addressComponent.Value("type") == "locality")
                {
                    retVal.Locality = addressComponent.Value("long_name");
                }
                else if (addressComponent.Value("type") == "administrative_area_level_1")
                {
                    retVal.AdministrativeAreaLevel1 = addressComponent.Value("long_name");
                }
                else if (addressComponent.Value("type") == "administrative_area_level_2")
                {
                    retVal.AdministrativeAreaLevel2 = addressComponent.Value("long_name");
                }
                else if (addressComponent.Value("type") == "country")
                {
                    retVal.Country = addressComponent.Value("long_name");
                }
                else if (addressComponent.Value("type") == "postal_code")
                {
                    retVal.PostalCode = getLong(addressComponent.Element("long_name"));
                }
            }
            XElement geometry = xmlElement.Element("geometry");
            XElement location = geometry.Element("location");
            retVal.Latitude = getDecimal(location.Element("lat"));
            retVal.Longitude = getDecimal(location.Element("lng"));
            var point = GeoHelper.CreatePoint(retVal.Latitude, retVal.Longitude);

            retVal.Geography = point;
            return retVal;
        }

        private static DateTime? getDateTime(XElement element)
        {
            DateTime tryDate;
            DateTime.TryParse(element.Value, out tryDate);
            if (tryDate == default(DateTime))
            {
                return null;
            }
            return tryDate;
        }

        private static double? getDouble(XElement element)
        {
            double tryDouble;
            double.TryParse(element.Value, out tryDouble);
            if (tryDouble == default(double))
            {
                return null;
            }
            return tryDouble;
        }

        private static long? getLong(XElement element)
        {
            long tryLong;
            long.TryParse(element.Value, out tryLong);
            if (tryLong == default(long))
            {
                return null;
            }
            return tryLong;
        }

        private static decimal? getDecimal(XElement element)
        {
            decimal tryDecimal;
            decimal.TryParse(element.Value, out tryDecimal);
            if (tryDecimal == default(decimal))
            {
                return null;
            }
            return tryDecimal;
        }

        private static Money getMoney(XElement element)
        {
            Money retval = new Money();
            if (element.Value == null)
            {
                return retval;
            }
            String[] array = element.Value.Split(new String[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length != 2)
            {
                return retval;
            }
            decimal value;
            decimal.TryParse(array[0], out value);
            retval = new Money { Value = value, Currency = array[1] };
            return retval;
        }

        private static bool? getBool(XElement element)
        {
            if (element.Value == null)
            {
                return null;
            }
            bool tryBool;
            bool.TryParse(element.Value, out tryBool);
            return tryBool;
        }
    }
}
