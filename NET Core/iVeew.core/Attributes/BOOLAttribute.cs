using System;
using System.ComponentModel.DataAnnotations;

namespace iVeew.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class BOOLAttribute : DataTypeAttribute
    {

        public BOOLAttribute()
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