using System;
using System.Globalization;

namespace Application.Helpers
{
    public static class DateHelper
    {
        public static string ToShamsi(DateOnly date)
        {
            PersianCalendar pc = new PersianCalendar();

            var miladi = new DateTime(date.Year, date.Month, date.Day);
            int year = pc.GetYear(miladi);
            int month = pc.GetMonth(miladi);
            int day = pc.GetDayOfMonth(miladi);

            return $"{year:0000}/{month:00}/{day:00}";
        }

        public static string ToShamsi(DateOnly date, string separator)
        {
            PersianCalendar pc = new PersianCalendar();
            var miladi = new DateTime(date.Year, date.Month, date.Day);
            int year = pc.GetYear(miladi);
            int month = pc.GetMonth(miladi);
            int day = pc.GetDayOfMonth(miladi);

            return $"{year:0000}{separator}{month:00}{separator}{day:00}";
        }
    }
}