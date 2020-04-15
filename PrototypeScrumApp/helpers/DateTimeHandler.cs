using System;

namespace PrototypeScrumApp.helpers
{
    public class DateTimeHandler
    {
        /// <summary>
        /// Function to convert DateTime type to string
        /// </summary>
        /// <param name="value">object value that is of type DateTime</param>
        /// <returns>string with the converted date</returns>
        public object Convert(object value)
        {
            var item = (DateTime)value;

            var a = item.ToString("yyyy-MM-dd");
            return a;
        }
    }
}