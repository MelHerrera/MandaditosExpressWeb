using System;
using System.Globalization;
using System.Web.Mvc;

namespace MandaditosExpress.Models.Extensions
{
    public class FloatModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object result = null;

            string modelName = bindingContext.ModelName;
            string attemptedValue = bindingContext.ValueProvider.GetValue(modelName)?.AttemptedValue;

            // in decimal? binding attemptedValue can be Null
            if (attemptedValue != null)
            {
                string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                string alternateSeperator = (wantedSeperator == "," ? "." : ",");

                if (attemptedValue.IndexOf(wantedSeperator, StringComparison.Ordinal) == -1
                    && attemptedValue.IndexOf(alternateSeperator, StringComparison.Ordinal) != -1)
                {
                    attemptedValue = attemptedValue.Replace(alternateSeperator, wantedSeperator);
                }

                try
                {
                    if (bindingContext.ModelMetadata.IsNullableValueType && string.IsNullOrWhiteSpace(attemptedValue))
                    {
                        return null;
                    }

                    result = float.Parse(attemptedValue, NumberStyles.Any);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(modelName, e);
                }
            }

            return result;
        }
    }
}