//using System;
//using System.IO;
//using System.Net;
//using System.Xml.Linq;
//using iVeew.Core.Extensions;
//using iVeew.Core.Models;

//namespace iVeew.Core.Helpers
//{
//    public class GoogleHelper
//    {
//        private string googleUrl = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=true";
//        public Location GetLocationFromAddress(string address)
//        {
//            Location retVal = null;
//            string url = String.Format(googleUrl, address);
//            WebRequest request = HttpWebRequest.Create(url);            
            
//            try
//            {

//                Stream stream = request.GetResponse().GetResponseStream();
//                string response = new System.IO.StreamReader(stream).ReadToEnd();
//                XElement xelement = XElement.Load(new StringReader(response), LoadOptions.PreserveWhitespace);
//                if (xelement.Value("status")=="OK")
//                {
//                    retVal = xelement.Element("result");
                    
//                }
                
//            }
//            catch (Exception)
//            {
                
//            }
            
//            return retVal;
//        }
//    }
//}