using Shared.OtherModels.User;

namespace Shared.Services
{
    public static class ReportingHelper
    {
        public static string ReportPath(string defaultReportPath, AppUser user)
        {
            if (user.ReportConfig != null & !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
            {
                return user.ReportConfig.ReportPath;
            }
            return defaultReportPath;
        }
        public static string ReportProcess(string defaultProcessName, AppUser user)
        {
            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ProcessName))
            {
                return user.ReportConfig.ProcessName;
            }
            return defaultProcessName;
        }
        public static string SubReport1Path(string defaultReportPath, AppUser user)
        {
            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.SubReport1ReportPath))
            {
                return user.ReportConfig.SubReport1ReportPath;
            }
            return defaultReportPath;
        }
        public static string SubReport1Process(string defaultProcessName, AppUser user)
        {
            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.SubReport1Process))
            {
                return user.ReportConfig.SubReport1Process;
            }
            return defaultProcessName;
        }
        public static string SubReport2Path(string defaultReportPath, AppUser user)
        {
            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.SubReport2ReportPath))
            {
                return user.ReportConfig.SubReport2ReportPath;
            }
            return defaultReportPath;
        }
        public static string SubReport2Process(string defaultProcessName, AppUser user)
        {
            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.SubReport2Process))
            {
                return user.ReportConfig.SubReport2Process;
            }
            return defaultProcessName;
        }
        public static string MinifiedReportPath(string defaultReportPath, AppUser user)
        {
            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.MinifiedReportPath))
            {
                return user.ReportConfig.MinifiedReportPath;
            }
            return defaultReportPath;
        }
        public static string FiscalYearRange(short month, short year)
        {
            string fiscalYearRange = "";
            if (month >= 7 && month <= 12)
            {
                fiscalYearRange = year.ToString() + "-" + (year + 1).ToString();
            }
            else
            {
                fiscalYearRange = (year - 1).ToString() + "-" + year.ToString();
            }
            return fiscalYearRange;
        }
        public static string FindTaxReportPath(string defaultReportPath, short month, AppUser user, short terminateMonth = 0)
        {
            if (user.ReportConfig != null & !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
            {
                if (user.ReportConfig.Month != null && user.ReportConfig.Month > 0 && user.ReportConfig.Month == month)
                {
                    if (!Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.FinalReportPath))
                    {
                        return user.ReportConfig.FinalReportPath;
                    }
                }
                if (user.ReportConfig.Month != null && user.ReportConfig.Month > 0 && terminateMonth > 0 && month == terminateMonth)
                {
                    if (!Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.FinalReportPath))
                    {
                        return user.ReportConfig.FinalReportPath;
                    }
                }
                return user.ReportConfig.ReportPath;
            }
            return defaultReportPath;
        }
    }
}
