using System;
using System.Globalization;
using Xamarin.Forms;
using System.Diagnostics;

namespace CurtainMonitor
{
    class SliderRelativePositionConverter : IMultiValueConverter
    {

        const int currentValueIndex = 0;
        const int minValueIndex = 1;
        const int maxValueIndex = 2;
        const int widthIndex = 3;
        const int textWidthIndex = 4;
        const int expectedLength = 5;

        /// <summary>
        /// Converts the current, min, and max value of a slider and the width of the field to the offset for the text.
        /// </summary>
        /// <param name="values"> the current, min, and max value of a slider and the width of the field </param>
        /// <param name="targetType"> ignored </param>
        /// <param name="parameter"> ignored </param>
        /// <param name="culture"> ignored </param>
        /// <returns> 0 if invalid or the current value is equal min or max value, the offset for the text otherwise. </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // validate input values length.
            if (values.Length != expectedLength)
            {
                return 0;
            }
            if (!((values[currentValueIndex] is Single currentValue) && (values[minValueIndex] is Single minValue) && (values[maxValueIndex] is Single maxValue) && (values[widthIndex] is Double width) && (values[textWidthIndex] is Double textWidth)))
            {
                return 0;
            }
            // validate input values.
            if ((minValue >= currentValue) || (currentValue >= maxValue) || (width <= 0) || (textWidth <= 0))
            {
                return 0;
            }
            // return converted value.
            return ((currentValue - minValue) * (width - textWidth / 2) / (maxValue - minValue) + (textWidth / 2));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // one-way value converter, convert back is not supported.
            throw new NotSupportedException();
        }
    }
}
