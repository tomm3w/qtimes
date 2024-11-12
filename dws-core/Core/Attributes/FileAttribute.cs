using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FileAttribute : DataTypeAttribute
    {
        private double fileSize = double.MaxValue;

        public FileAttribute()
            : base(DataType.Custom)
        {

        }
        public FileAttribute(double FileSize)
            : base(DataType.Custom)
        {
            fileSize = FileSize;
        }



        public FileAttribute(double FileSize, string validExtensions)
            : base(DataType.Custom)
        {
            fileSize = FileSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;//not mandatory
            }
            bool retVal = false;
            HttpPostedFileBase file = (HttpPostedFileBase)value;

            if (file.ContentLength <= fileSize)
            {

                retVal = true;

            }
            return retVal;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FileTypeAttribute : DataTypeAttribute
    {
        private List<string> ValidExtensions { get; set; }

        public FileTypeAttribute()
            : base(DataType.Custom)
        {

        }

        public FileTypeAttribute(string validExtensions)
            : base(DataType.Custom)
        {
            ValidExtensions = validExtensions.Split(',').ToList<string>();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;//not mandatory
            }
            bool retVal = false;
            HttpPostedFileBase file = (HttpPostedFileBase)value;
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (ValidExtensions != null)
            {
                retVal = ValidExtensions.Any(f => extension == "." + f);
            }
            else
            {
                retVal = true;
            }

            return retVal;
        }
    }
}