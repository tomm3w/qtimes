using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attributes
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
