using Shared.Overtime.Domain;
using System;

namespace Shared.Overtime
{
    public static class OvertimeCalculationHelper
    {
        public static decimal CalculateIndividualOvertimeAmount(OvertimePolicy overtimePolicy, OvertimeRequest request, decimal dailyBasic, decimal hourlyBasic)
        {
            decimal amount;

            // Flat Based
            if (overtimePolicy.IsFlatAmountType)
            {

                amount = overtimePolicy.Amount * overtimePolicy.AmountRate;
                //amount = overtimePolicy.LimitationOfAmount ? CheckLimitationOfAmount(overtimePolicy, amount) : amount;

            }

            // Basic Based
            else if (overtimePolicy.IsPercentageAmountType)
            {

                if (overtimePolicy.Unit == "Daily")
                {

                    amount = dailyBasic * overtimePolicy.AmountRate;

                }
                else if (overtimePolicy.Unit == "Hourly")
                {


                    //TimeSpan totalHours = request.EndTime.Value - request.StartTime.Value;

                    decimal totalHours = (decimal)(request.EndTime.Value - request.StartTime.Value).TotalHours;
                    if (totalHours < 0)
                    {
                        totalHours += 12;
                    }

                    amount = Math.Abs(totalHours) * hourlyBasic * overtimePolicy.AmountRate;
                }

                else
                {
                    amount = dailyBasic * overtimePolicy.AmountRate;
                }

                //amount = overtimePolicy.LimitationOfAmount ? CheckLimitationOfAmount(overtimePolicy, amount) : amount;

            }

            else { amount = 0; }

            amount = overtimePolicy.LimitationOfAmount ? CheckLimitationOfAmount(overtimePolicy, amount) : amount;

            return amount;
        }
        public static decimal CalculateUploadedOvertimeAmount(OvertimePolicy overtimePolicy, UploadOvertimeAllowances request, decimal dailyBasic, decimal hourlyBasic)
        {
            decimal amount;

            request.Unit = overtimePolicy.LimitationOfUnit ? CheckLimitationOfUnit(overtimePolicy, request.Unit) : request.Unit;

            // Flat Based
            if (overtimePolicy.IsFlatAmountType)
            {

                amount = overtimePolicy.Amount * overtimePolicy.AmountRate * request.Unit;
            }

            // Basic Based
            else if (overtimePolicy.IsPercentageAmountType)
            {

                if (overtimePolicy.Unit == "Daily")
                {

                    amount = dailyBasic * overtimePolicy.AmountRate * request.Unit;

                }
                else if (overtimePolicy.Unit == "Hourly")
                {

                    amount = hourlyBasic * overtimePolicy.AmountRate * request.Unit;
                }

                else
                {
                    amount = dailyBasic * overtimePolicy.AmountRate * request.Unit;
                }

            }

            else { amount = 0; }

            return amount;
        }
        private static decimal CheckLimitationOfAmount(OvertimePolicy overtimePolicy, decimal amount)
        {
            if (amount > overtimePolicy.MaxAmount)
            {
                amount = overtimePolicy.MaxAmount;
            }

            if (amount < overtimePolicy.MinAmount)
            {
                amount = overtimePolicy.MinAmount;
            }

            return amount;
        }
        private static decimal CheckLimitationOfUnit(OvertimePolicy overtimePolicy, decimal unit)
        {
            if (unit > overtimePolicy.MaxUnit)
            {
                unit = overtimePolicy.MaxUnit;
            }

            if (unit < overtimePolicy.MinUnit)
            {
                unit = overtimePolicy.MinUnit;
            }

            return unit;
        }

    }
}
