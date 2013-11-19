using Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Logging.Converters
{
    /// <summary>
    /// Value Converter which will tells whether a given TraceLevel type is indicated in the provided TraceLevel object 
    /// </summary>
    [ValueConversion(typeof(TraceLevel), typeof(Boolean))]
    public class LogLevelToBooleanConverter : IValueConverter
    {
        #region Convert TraceLevel to Boolean
        /// <summary>
        /// Converts a TraceLevel object to a Boolean
        /// </summary>
        /// <param name="value">The TraceLevel object</param>
        /// <param name="targetType">The Type of object being converted</param>
        /// <param name="parameter">The TraceLevel to compare against</param>
        /// <param name="culture">This is not used by convert</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TraceLevel p = (TraceLevel)(TraceLevel.Parse(typeof(TraceLevel), (String)parameter));
            TraceLevel v = (TraceLevel)(value);

            if (v.HasFlag(p))
                return true;
            return false;
        }
        #endregion Convert TraceLevel to Boolean

        #region Convert Boolean to TraceLevel
        /// <summary>
        /// Sets the TraceLevel depending on the provided Boolean value
        /// </summary>
        /// <param name="value">Whether to activate the specified TraceLevel</param>
        /// <param name="targetType">The type of the object being converted</param>
        /// <param name="parameter">The TraceLevel object being analyzed</param>
        /// <param name="culture">This is not used in the conversion</param>
        /// <returns>Sets the visible trace level</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TraceLevel p = (TraceLevel)(TraceLevel.Parse(typeof(TraceLevel), (String)parameter));
            Boolean v = (Boolean)value;

            return v == false ? Log.LogWindow.VisibleTraceLevel ^= p : Log.LogWindow.VisibleTraceLevel |= p;
        }
        #endregion Convert Boolean to TraceLevel
    }
}
