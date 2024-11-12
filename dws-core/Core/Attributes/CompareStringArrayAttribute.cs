using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CompareStringArrayAttribute : DataTypeAttribute
    {
        private string[] strs;
        public CompareStringArrayAttribute()
            : base(DataType.Custom)
        {

        }
        public CompareStringArrayAttribute(string[] strings)
            : base(DataType.Custom)
        {
            strs = strings;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;//not mandatory
            }
            string str = value.ToString();
            bool retVal = false;
            foreach (var item in strs)
            {
                if (str == item)
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }
    }
}