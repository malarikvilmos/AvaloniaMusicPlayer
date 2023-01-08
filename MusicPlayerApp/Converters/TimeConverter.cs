using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MusicPlayerApp.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int? time = value as int?;

            int? hours = time / 3600;
            int? minutes = time % 3600 / 60;
            int? seconds = time % 3600 % 60;

            string? hoursStr = "";
            string? minutesStr = minutes.ToString();
            string? secondsStr = seconds.ToString();

            if (hours != 0)
                hoursStr = hours + ":";

            if (minutes < 10 && hours != 0)
                minutesStr = "0" + minutesStr;

            if (seconds < 10)
                secondsStr = "0" + secondsStr;

            string? final = hoursStr + minutesStr + ":" + secondsStr;
            if (parameter as string == "False")
                return final;
            else return "-" + final;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
