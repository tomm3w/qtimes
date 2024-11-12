using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
namespace Core.Extensions
{
    public static class Extensions
    {
        public static string Join<T>(this List<T> collection, Func<T, string> toStringMethod)
        {
            return collection.Join(toStringMethod, ",");
        }

        public static string Join<T>(this List<T> collection,string joinstring, Func<T, string> toStringMethod)
        {
            return collection.Join(toStringMethod, joinstring);
        }

        public static string Join<T>(this List<T> collection, Func<T, string> toStringMethod, string joinString)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T thing in collection)
                sb.Append(toStringMethod(thing)).Append(joinString);

            return sb.ToString(0, sb.Length - 1); //remove trailing ,
        }



        public static DateTime? ToUniversalTimeEx(this DateTime? date)
        {
            if (date == null)
            {
                return null;
            }
            else
            {
                DateTime d = (DateTime)date;
                return d.ToUniversalTime();
            }
        }

        public static string ToUniversalTimeString(this DateTime? date)
        {
            if (date == null)
            {
                return null;
            }
            else
            {
                DateTime d = (DateTime)date;
                return d
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
        }

        public static string ToUniversalTimeString(this DateTime date)
        {
            return date
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        public static string Value(this XElement xmlelement, string xName)
        {
            if (xmlelement == null || xmlelement.Element(xName) == null)
            {
                return null;
            }
            return xmlelement.Element(xName).Value;
        }

        public static string ToStringBits(this List<int> data, int length)
        {
            string retVal = null;
            if (data == null)
            {
                retVal = new string('0', length);
            }
            else
            {
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    bool match = data.Contains(i + 1);// data[i] == (i + 1);
                    strBuilder.Append((match) ? "1" : "0");
                }
                retVal = strBuilder.ToString();
            }
            return retVal;
        }

        public static List<int> ToDataBits(this string data)
        {
            if (data == null)
            {
                return null;
            }
            List<int> retVal = new List<int>();
            char trueBit = '1';
            for (int i = 0; i < data.Length; i++)
            {
                char chr = data[i];
                if (chr == trueBit)
                {
                    retVal.Add(i);
                }
            }
            return retVal;
        }

        public static string ContentAbsUrl(this UrlHelper url, string relativeContentPath)
        {
            Uri contextUri = HttpContext.Current.Request.Url;

            var baseUri = string.Format("{0}://{1}{2}", contextUri.Scheme,
               contextUri.Host, contextUri.Port == 80 ? string.Empty : ":" + contextUri.Port);
            if (relativeContentPath == null)
            {
                return baseUri;
            }
            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }
    }
}