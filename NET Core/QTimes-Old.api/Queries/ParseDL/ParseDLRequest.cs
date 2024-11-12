using iVeew.common.api.Queries;
using System;
using System.Net.Http;

namespace QTimes.api.Queries
{
    public class ParseDLRequest : IQueryRequest
    {
        public string Base64FrontImage { get; set; }
        public string Base64BackImage { get; set; }
        public ParseDLRequest()
        {

        }
        public ParseDLRequest(string base64FrontImage, string base64BackImage)
        {
            Base64FrontImage = base64FrontImage;
            Base64BackImage = base64BackImage;
        }

        public ByteArrayContent FrontByteArrayContent => ToByteArrayContent(Base64FrontImage);
        public ByteArrayContent BackByteArrayContent => ToByteArrayContent(Base64BackImage);

        public ByteArrayContent ToByteArrayContent(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            return new ByteArrayContent(imageBytes);
        }
    }
}