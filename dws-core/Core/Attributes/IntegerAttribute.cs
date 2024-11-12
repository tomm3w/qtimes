using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IntegerAttribute : DataTypeAttribute
    {

        public IntegerAttribute()
            : base(DataType.Custom)
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;//not mandatory
            }
            bool retVal = false;
            int temp = 0;
            retVal = Int32.TryParse(value.ToString(), out temp);
            if (temp == 0)
            {
                retVal = false;
            }
            return retVal;
        }
    }
}