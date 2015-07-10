using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace PsychicTest.Design
{
    public class TimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var currentDate = value is DateTime ? (DateTime) value : new DateTime();

                return currentDate.ToString("MM/dd/yyyy HH:mm");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
