using System;
using System.Data;
using System.Linq;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Report;
using Shared.Attendance.Report;
using Shared.Attendance.Filter.Report;

namespace BLL.Attendance.Implementation.Report
{
    public class AttendanceReportBusiness : IAttendanceReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public AttendanceReportBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<DailyAttendance>> DailyAttendancesReportAsync(DailyAttendanceReport_Filter filter, AppUser user)
        {
            IEnumerable<DailyAttendance> list = new List<DailyAttendance>();
            try
            {
                var sp_name = "sp_HR_RptAttendance";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                parameters.Add("ExecutionFlag", "DailyAttendances");
                list = await _dapper.SqlQueryListAsync<DailyAttendance>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceReportBusiness", "DailyAttendancesReportAsync", user);
            }
            return list;
        }
        public async Task<EmployeeAttendanceReport> EmployeeAttendanceReportAsync(EmployeeAttendanceReport_Filter filter, AppUser user)
        {
            EmployeeAttendanceReport report = new EmployeeAttendanceReport();
            try
            {
                var sp_name = "sp_HR_RptAttendance";
                var parameters1 = DapperParam.AddParams(filter, user, addUserId: false);
                parameters1.Add("ExecutionFlag", "DailyAttendances");
                report.DailyAttendances = await _dapper.SqlQueryListAsync<DailyAttendance>(user.Database, sp_name, parameters1, CommandType.StoredProcedure);

                var parameters2 = DapperParam.AddParams(filter, user, addUserId: false);
                parameters2.Add("ExecutionFlag", "AttendanceSummeries");
                report.AttendanceSummeries = await _dapper.SqlQueryListAsync<AttendanceSummery>(user.Database, sp_name, parameters2, CommandType.StoredProcedure);

                // Employee Shift
                if (report.DailyAttendances.Count() >= 0)
                {
                    var shifts = report.DailyAttendances.Select(item => new { item.WorkShiftId }).Distinct().ToList();
                    foreach (var item in shifts)
                    {
                        AttendanceWorkShift workShift = new AttendanceWorkShift();
                        workShift.WorkShiftId = item.WorkShiftId;
                        var shiftInfo = report.DailyAttendances.FirstOrDefault(i => i.WorkShiftId == item.WorkShiftId);
                        if (shiftInfo != null)
                        {
                            workShift.WorkShiftName = shiftInfo.WorkShiftName;
                            workShift.TotalDaysInShift = report.DailyAttendances.Where(i => i.WorkShiftId == item.WorkShiftId).ToList().Count;
                            workShift.ShiftStart = shiftInfo.ShiftStart;
                            workShift.ShiftEnd = shiftInfo.ShiftEnd;
                            workShift.ShiftTime = shiftInfo.ShiftStart + "-" + shiftInfo.ShiftEnd;
                            report.AttendanceWorkShifts.Add(workShift);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceReportBusiness", "EmployeeAttendanceReportAsync", user);
            }
            return report;
        }
    }
}
