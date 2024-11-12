//using System;
//using System.Globalization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace Core.Models.Binder
//{
//    public class DateTimeBinder : IModelBinderProvider
//    {
//        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
            
//        }

//        public IModelBinder GetBinder(ModelBinderProviderContext context)
//        {
//            var displayFormat = context.Metadata.DisplayFormatString;
//            var value = context.va.GetValue(bindingContext.ModelName);
//            if (string.IsNullOrEmpty(displayFormat))
//            {
//                displayFormat = "yyyy-MM-dd HH:mm:ss";
//            }
//            if (value != null)
//            {
//                string attemptedValue = HttpContext.Current.Server.UrlDecode(value.AttemptedValue);
//                DateTime date;
//                displayFormat = displayFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
//                // use the format specified in the DisplayFormat attribute to parse the date
//                if (DateTime.TryParseExact(attemptedValue, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
//                {
//                    return date;
//                }
//                else
//                {
//                    bindingContext.ModelState.AddModelError(
//                        bindingContext.ModelName,
//                        string.Format("{0} is an invalid date format", value.AttemptedValue)
//                    );
//                }
//            }

//            return base.BindModel(controllerContext, bindingContext);
//        }
//    }
//}
