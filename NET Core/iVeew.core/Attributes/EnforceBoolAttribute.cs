using System;
using System.ComponentModel.DataAnnotations;

namespace iVeew.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EnforceBoolAttribute : ValidationAttribute
    {
        public bool Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value == Value;
        }
    }
}
