using Shared.Helpers;
using Shared.Leave.DTO.Setup;

namespace Shared.Services
{
    public static class DateTimeExtension
    {
        public static short[] FirstSixMonthsOfFiscalYear = new short[] { 7, 8, 9, 10, 11, 12 };
        public static short[] LastSixMonthsOfFiscalYear = new short[] { 1, 2, 3, 4, 5, 6 };
        public static int GetMonthDiffExcludingThisMonth(this DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public static int GetMonthDiffIncludingThisMonth(this DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart) + 1;
        }
        public static double GetDateDiff(this DateTime datetime, DateTime startDate, DateTime endDate)
        {
            double days = Math.Round((endDate - startDate).TotalDays + 1);
            return days;
        }
        public static double GetHoursDiff(this DateTime datetime, TimeSpan startTime, TimeSpan endTime)
        {
            double hours = (endTime - startTime).TotalHours;
            return hours;
        }
        public static string GetMonthName(short monthNo)
        {
            switch (monthNo)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";
            }
        }
        public static string GetMonthName(this DateTime datetime)
        {
            var month = datetime.Month;
            return GetMonthName((short)month);
        }
        public static DateTime FirstDateOfAMonth(int year, int month)
        {
            var firstDate = new DateTime(year, month, 1);
            return firstDate;
        }
        public static DateTime LastDateOfAMonth(int year, int month)
        {
            var dayInMonth = DateTime.DaysInMonth(year, month);
            var lastDate = new DateTime(year, month, dayInMonth);
            return lastDate;
        }
        public static int DaysInAMonth(this DateTime date)
        {
            var dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return dayInMonth;
        }
        public static DateTime FirstDateOfAMonth(this DateTime date)
        {
            var firstDate = new DateTime(date.Year, date.Month, 1);
            return firstDate;
        }

        public static DateTime FirstDateOfThisMonth()
        {
            var firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return firstDate;
        }

        public static DateTime LastDateOfThisMonth()
        {
            var lastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.DaysInAMonth());
            return lastDate;
        }
        public static DateTime LastDateOfAMonth(this DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            var dayInMonth = DateTime.DaysInMonth(year, month);
            var firstDate = new DateTime(date.Year, date.Month, dayInMonth);
            return firstDate;
        }
        public static int DaysBetweenDateRangeIncludingStartDate(this DateTime startDate, DateTime endDate)
        {
            TimeSpan span = endDate - startDate;
            int daysDiff = span.Days + 1;
            return daysDiff;
        }
        public static int DaysBetweenDateRangeExcludingStartDate(this DateTime startDate, DateTime endDate)
        {
            TimeSpan span = endDate - startDate;
            int daysDiff = span.Days;
            return daysDiff;
        }
        public static List<DateTime> DatesBetweenTwoDate(this DateTime startDate, DateTime endDate)
        {
            List<DateTime> dates = new List<DateTime>();
            var totalDays = startDate.DaysBetweenDateRangeIncludingStartDate(endDate);
            for (int i = 0; i < totalDays; i++)
            {
                DateTime date = startDate.AddDays(i);
                dates.Add(date);
            }
            return dates;
        }

        public static bool IsThisDateExistInThisMonth(this DateTime date)
        {
            bool isExist = date.IsDateBetweenTwoDates(FirstDateOfThisMonth(), LastDateOfThisMonth());
            return isExist;
        }
        public static bool IsDateBetweenTwoDates(this DateTime toBeChecked, DateTime fromDate, DateTime toDate)
        {
            if (toBeChecked >= fromDate && toBeChecked <= toDate)
                return true;
            return false;
        }

        public static int CalculateAgeInMonths(this DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date should be before end date.");
            }
            int monthsApart = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
            if (endDate.Day < startDate.Day)
            {
                monthsApart--;
            }
            return monthsApart;
        }
        public static DateTime? TryParseDate(string date)
        {
            if (date.IsNullEmptyOrWhiteSpace() == false)
            {
                if (date.IsStringNumber())
                {
                    return DateTime.FromOADate(Convert.ToDouble(date));
                }
                else
                {
                    if (DateTime.TryParse(date, out DateTime result))
                    {
                        return Convert.ToDateTime(date);
                    }
                }
            };
            return null;
        }
        public static DateTime GetNextServiceYearCompleteDate(DateTime date, int year)
        {
            bool isFound = false;
            for (int i = 0; i < FirstSixMonthsOfFiscalYear.Length; i++)
            {
                if (FirstSixMonthsOfFiscalYear[i] == date.Month)
                {
                    isFound = true;
                    break;
                }
            }
            if (isFound)
            {
                return new DateTime(DateTime.Now.Year, date.Month, date.Day);
            }
            else
            {
                for (int i = 0; i < LastSixMonthsOfFiscalYear.Length; i++)
                {
                    if (LastSixMonthsOfFiscalYear[i] == date.Month)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                {
                    if (year != date.Year)
                    {
                        return new DateTime(year, date.Month, date.Day);
                    }
                    else
                    {
                        return new DateTime(year + 2, date.Month, date.Day);
                    }

                }
            }
            return new DateTime(year + 2, date.Month, date.Day);
        }
        public static LeavePeriodRange GetLeavePeriodDateRange(DateTime? joiningDate, int startMonth, int endMonth, int year = 0)
        {
            DateTime contractStartDate = DateTime.Now;
            if (startMonth <= 0)
            {
                throw new InvalidOperationException("Start month cannot be <=0 ");
            }
            if (endMonth <= 0)
            {
                throw new InvalidOperationException("End month cannot be <=0 ");
            }
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            DateTime? startDate = null;
            DateTime? endDate = null;



            // Calculate endDate
            if (endMonth == 12 || endMonth == 0)
            {
                endDate = new DateTime(year, 12, 31);
            }
            else if (endMonth == 6)
            {
                if (DateTime.Now.Month <= 6)
                    endDate = new DateTime(year, 6, 30);
                else
                    endDate = new DateTime(year + 1, 6, 30);
            }
            else
            {
                DateTime lastDateOfLeaveEndMonth = LastDateOfAMonth(year, endMonth);

                if (lastDateOfLeaveEndMonth > startDate)
                    endDate = lastDateOfLeaveEndMonth;
                else
                    endDate = lastDateOfLeaveEndMonth.AddYears(1);
            }

            if (endDate != null)
            {
                var possibleStartDate = endDate.Value.AddMonths(-11);
                contractStartDate = new DateTime(possibleStartDate.Year, possibleStartDate.Month, 1);
            }

            // Calculate startDate
            if (startMonth == 1)
            {
                if (contractStartDate >= new DateTime(year, 1, 1))
                    startDate = contractStartDate;
                else
                    startDate = new DateTime(year, 1, 1);
            }
            else if (startMonth == 7)
            {
                DateTime julyFirstCurrentYear = new DateTime(year, 7, 1);
                DateTime julyFirstPreviousYear = julyFirstCurrentYear.AddYears(-1);

                if (contractStartDate < julyFirstCurrentYear && contractStartDate < julyFirstPreviousYear)
                    startDate = julyFirstPreviousYear;
                else if (contractStartDate < julyFirstCurrentYear && contractStartDate >= julyFirstPreviousYear)
                    startDate = contractStartDate;
            }
            else if (startMonth > 0 && startMonth != 1 && startMonth != 7)
            {
                DateTime leaveStartDate = new DateTime(year, startMonth, 1);
                DateTime leaveStartDatePreviousYear = leaveStartDate.AddYears(-1);

                if (contractStartDate < leaveStartDate && contractStartDate < leaveStartDatePreviousYear)
                    startDate = leaveStartDatePreviousYear;
                else if (contractStartDate < leaveStartDate && contractStartDate >= leaveStartDatePreviousYear)
                    startDate = contractStartDate;
                else
                {
                    startDate = leaveStartDate;
                }
            }
            if (joiningDate != null)
            {
                if (joiningDate > startDate)
                    startDate = joiningDate;
            }


            if (startDate != null && endDate != null)
            {
                LeavePeriodRange leavePeriodRange = new LeavePeriodRange();
                leavePeriodRange.StartDate = startDate;
                leavePeriodRange.EndDate = endDate;
                return leavePeriodRange;
            }
            return null;
        }
    }
}
