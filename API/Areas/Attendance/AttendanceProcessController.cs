using System;
using System.Linq;
using API.Services;
using OfficeOpenXml;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using API.Base;
using Shared.Attendance.ViewModel.Attendance;

namespace API.Areas.Attendance
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]
    public class AttendanceProcessController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IAttendanceProcessBusiness _attendanceProcessBusiness;
        public AttendanceProcessController(IAttendanceProcessBusiness attendanceProcessBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _attendanceProcessBusiness = attendanceProcessBusiness;
        }

        [HttpPost, Route("Process")]
        public async Task<IActionResult> AttendanceProcessAsync(AttendanceProcessViewModel model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var status = await _attendanceProcessBusiness.AttendanceProcessAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessController", "GetEmployeeManualAttendancesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAttendanceProcessInfos")]
        public async Task<IActionResult> GetAttendanceProcessInfosAsync(short? month, short? year)
        {
            var user = AppUser();
            try
            {
                if (month.HasValue && year.HasValue)
                {
                    var data = await _attendanceProcessBusiness.GetAttendanceProcessInfosAsync(month, year, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessController", "GetAttendanceProcessInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("LockAttendanceProcess")]
        public async Task<IActionResult> LockAttendanceProcessAsync(AttendanceProcessLockUnlock model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _attendanceProcessBusiness.LockAttendanceProcessAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessController", "LockAttendanceProcess", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UnLockAttendanceProcess")]
        public async Task<IActionResult> UnLockAttendanceProcessAsync(AttendanceProcessLockUnlock model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _attendanceProcessBusiness.UnLockAttendanceProcessAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessController", "UnLockAttendanceProcessAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadRowAttendanceData")]
        public async Task<IActionResult> UploadRowAttendanceData([FromForm] UploadAttendanceRowData model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.ExcelFile?.Length > 0)
                    {
                        var stream = model.ExcelFile.OpenReadStream();
                        List<UploadAttendanceViewModel> attendances = new List<UploadAttendanceViewModel>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            if (worksheet != null)
                            {
                                var rowCount = worksheet.Dimension.Rows;
                                if (worksheet != null)
                                {

                                    for (var row = 2; row <= rowCount; row++)
                                    {
                                        var attendanceDate = worksheet.Cells[row, 2].Value.ToString();
                                        if (attendanceDate != null)
                                        {
                                            DateTime result = DateTime.FromOADate(double.Parse(attendanceDate));
                                            UploadAttendanceViewModel attendance = new UploadAttendanceViewModel();
                                            attendance.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                            attendance.AttendanceDate = result;
                                            attendance.MachineId = "";
                                            attendances.Add(attendance);
                                        }
                                    }
                                }
                            }
                        }
                        var data = await _attendanceProcessBusiness.UploadRowAttendanceData(attendances, user);

                        if (model.FromDate != null && model.ToDate != null)
                        {
                            AttendanceProcessViewModel attendanceProcess = new AttendanceProcessViewModel();
                            attendanceProcess.Month = (short)model.FromDate.Value.Month;
                            attendanceProcess.Year = (short)model.FromDate.Value.Year;
                            attendanceProcess.FromDate = model.FromDate;
                            attendanceProcess.ToDate = model.ToDate;
                            var validate = await _attendanceProcessBusiness.ValidateAttendanceProcessAsync(attendanceProcess.Month, attendanceProcess.Year, user);
                            if (validate == null || validate.Status == true)
                            {
                                var process = await _attendanceProcessBusiness.AttendanceProcessAsync(attendanceProcess, user);
                                return Ok(process);
                            }
                            return Ok(validate);
                        }
                        return Ok(data);
                    }
                    return Ok("No file found to import data");
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessController", "UploadRowAttendanceData", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
