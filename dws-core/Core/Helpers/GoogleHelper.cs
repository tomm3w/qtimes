using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Core.Models;
using Core.Extensions;
namespace Core.Helpers
{
    public class GoogleHelper
    {
        private string googleUrl = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=true";
        public Location GetLocationFromAddress(string address)
        {
            Location retVal = null;
            string url = String.Format(googleUrl, address);
            WebRequest request = HttpWebRequest.Create(url);            
            
            try
            {
                
                System.IO.Stream stream = request.GetResponse().GetResponseStream();
                string response = new System.IO.StreamReader(stream).ReadToEnd();
                XElement xelement = XElement.Load(new StringReader(response), LoadOptions.PreserveWhitespace);
                if (xelement.Value("status")=="OK")
                {
                    retVal = xelement.Element("result");
                    
                }
                
            }
            catch (Exception)
            {
                
            }
            
            return retVal;
        }
    }
}