using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Globalization;

namespace iVeew.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class URLExAttribute : DataTypeAttribute
    {
        private readonly Regex regex = new Regex(@"^\s*((?:https?://)?(?:[\w-]+\.)+[\w-]+)(/[\w ./?%&=-]*)?\s*$", RegexOptions.Compiled);

        public URLExAttribute()
            : base(DataType.Url)
        {

        }

        public override bool IsValid(object value)
        {

            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(str))
                return true;
            Match match = regex.Match(str);
            return ((match.Success && (match.Index == 0)) && (match.Length == str.Length));
        }
    }
}