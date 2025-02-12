﻿namespace iVeew.common.api.MultipartDataMediaFormatter
{
    public class HttpFile
    {
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public byte[] Buffer { get; set; }

        public HttpFile() { }

        public HttpFile(string fileName, string mediaType, byte[] buffer)
        {
            FileName = fileName;
            MediaType = mediaType;
            Buffer = buffer;
        }
    }
}
