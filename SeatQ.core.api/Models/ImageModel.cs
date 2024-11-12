using common.api.MultipartDataMediaFormatter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Models
{
    public class ImageModel
    {
        public int RestaurantChainId { get; set; }
        public HttpFile Image { get; set; }

        public string ImagePath { get; set; }

        private Stream _inputStream = null;
        private object _locker = new object();
        public Stream InputStream
        {
            get
            {
                if (_inputStream == null)
                {
                    lock (_locker)
                    {
                        if (_inputStream == null)
                            _inputStream = new MemoryStream(Image.Buffer);
                    }
                }

                return _inputStream;
            }
        }
    }
}