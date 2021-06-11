using System;
using System.Globalization;

namespace Gym
{
    public static class Utility
    {
        public static string date(this DateTime value)     ///////// تبدیل تاریخ میلادی به شمسی
        {

            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }
        public static string date2(this DateTime Date2)   ///////// افزودن یک ماه به تاریخ کنونی
        {

            PersianCalendar pc = new PersianCalendar();
            DateTime date = DateTime.Now;
            DateTime date2 = date.AddMonths(1);
            return pc.GetYear(date2) + "/" + pc.GetMonth(date2).ToString("00") + "/" +
                   pc.GetDayOfMonth(date2).ToString("00");
        }

    }
}
