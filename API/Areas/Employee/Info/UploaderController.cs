using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Shared.Employee.DTO.Info;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using BLL.Employee.Interface.Setup;
using Shared.Employee.ViewModel.Info;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class UploaderController : ApiBaseController
    {
        private readonly IUploaderBusiness _employeeUploaderBusiness;
        private readonly ITableConfigBusiness _tableConfigBusiness;
        private readonly ISysLogger _sysLogger;
        private ExcelGenerator _excelGenerator;
        public UploaderController(
            IUploaderBusiness employeeUploaderBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase,
            ITableConfigBusiness tableConfigBusiness) : base(clientDatabase)
        {
            _employeeUploaderBusiness = employeeUploaderBusiness;
            _sysLogger = sysLogger;
            _tableConfigBusiness = tableConfigBusiness;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpPost, Route("UploadEmployeeInfo")]
        public async Task<IActionResult> UploadEmployeeInfoAsync([FromForm] EmployeeUploaderFileDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        List<EmployeeUploaderDTO> data = new List<EmployeeUploaderDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                EmployeeUploaderDTO employee = new EmployeeUploaderDTO();
                                employee.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                employee.EmployeeCode = employee.EmployeeCode != null ? employee.EmployeeCode.Trim() : employee.EmployeeCode;

                                if (employee.EmployeeCode != null)
                                {
                                    employee.GlobalID = worksheet.Cells[row, 2].Value?.ToString();
                                    employee.GlobalID = employee.GlobalID != null ? employee.GlobalID.Trim() : employee.GlobalID;

                                    employee.PreviousCode = worksheet.Cells[row, 3].Value?.ToString();
                                    employee.PreviousCode = employee.PreviousCode != null ? employee.PreviousCode.RemoveWhitespace() : employee.PreviousCode;

                                    employee.Salutation = worksheet.Cells[row, 4].Value?.ToString();
                                    employee.Salutation = employee.Salutation != null ? employee.Salutation.RemoveWhitespace() : employee.Salutation;

                                    employee.FirstName = worksheet.Cells[row, 5].Value?.ToString();
                                    employee.FirstName = employee.FirstName != null ? employee.FirstName.RemoveWhitespace() : employee.FirstName;

                                    employee.MiddleName = worksheet.Cells[row, 6].Value?.ToString();
                                    employee.MiddleName = employee.MiddleName != null ? employee.MiddleName.RemoveWhitespace() : employee.MiddleName;

                                    employee.LastName = worksheet.Cells[row, 7].Value?.ToString();
                                    employee.LastName = employee.LastName != null ? employee.LastName.RemoveWhitespace() : employee.LastName;

                                    employee.NickName = worksheet.Cells[row, 8].Value?.ToString();
                                    employee.NickName = employee.NickName != null ? employee.NickName.RemoveWhitespace() : employee.NickName;

                                    employee.BranchName = worksheet.Cells[row, 9].Value?.ToString();
                                    employee.BranchName = employee.BranchName != null ? employee.BranchName.RemoveWhitespace() : employee.BranchName;

                                    employee.DivisionName = worksheet.Cells[row, 10].Value?.ToString();
                                    employee.DivisionName = employee.DivisionName != null ? employee.DivisionName.RemoveWhitespace() : employee.DivisionName;

                                    employee.UnitName = worksheet.Cells[row, 11].Value?.ToString();
                                    employee.UnitName = employee.UnitName != null ? employee.UnitName.RemoveWhitespace() : employee.UnitName;

                                    employee.CostCenterCode = worksheet.Cells[row, 12].Value?.ToString();
                                    employee.CostCenterCode = employee.CostCenterCode != null ? employee.CostCenterCode.RemoveWhitespace() : employee.CostCenterCode;

                                    employee.CostCenterName = worksheet.Cells[row, 13].Value?.ToString();
                                    employee.CostCenterName = employee.CostCenterName != null ? employee.CostCenterName.RemoveWhitespace() : employee.CostCenterName;

                                    employee.GradeName = worksheet.Cells[row, 14].Value?.ToString();
                                    employee.GradeName = employee.GradeName != null ? employee.GradeName.RemoveWhitespace() : employee.GradeName;

                                    employee.DesignationName = worksheet.Cells[row, 15].Value?.ToString();
                                    employee.DesignationName = employee.DesignationName != null ? employee.DesignationName.RemoveWhitespace() : employee.DesignationName;

                                    employee.DepartmentName = worksheet.Cells[row, 16].Value?.ToString();
                                    employee.DepartmentName = employee.DepartmentName != null ? employee.DepartmentName.RemoveWhitespace() : employee.DepartmentName;

                                    employee.SectionName = worksheet.Cells[row, 17].Value?.ToString();
                                    employee.SectionName = employee.SectionName != null ? employee.SectionName.RemoveWhitespace() : employee.SectionName;

                                    employee.SubSectionName = worksheet.Cells[row, 18].Value?.ToString();
                                    employee.SubSectionName = employee.SubSectionName != null ? employee.SubSectionName.RemoveWhitespace() : employee.SubSectionName;

                                    employee.JoiningDate = worksheet.Cells[row, 19].Value?.ToString();
                                    if (employee.JoiningDate.IsStringNumber() && employee.JoiningDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.JoiningDate = DateTime.FromOADate(Convert.ToDouble(employee.JoiningDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.JoiningDate = employee.JoiningDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.JoiningDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.ConfirmationDate = worksheet.Cells[row, 20].Value?.ToString();
                                    if (employee.ConfirmationDate.IsStringNumber() && employee.ConfirmationDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.ConfirmationDate = DateTime.FromOADate(Convert.ToDouble(employee.ConfirmationDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.ConfirmationDate = employee.ConfirmationDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.ConfirmationDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.AppointmentDate = worksheet.Cells[row, 21].Value?.ToString();
                                    if (employee.AppointmentDate.IsStringNumber() && employee.AppointmentDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.AppointmentDate = DateTime.FromOADate(Convert.ToDouble(employee.AppointmentDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.AppointmentDate = employee.AppointmentDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.AppointmentDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.ProbationEndDate = worksheet.Cells[row, 22].Value?.ToString();
                                    if (employee.ProbationEndDate.IsStringNumber() && employee.ProbationEndDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.ProbationEndDate = DateTime.FromOADate(Convert.ToDouble(employee.ProbationEndDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.ProbationEndDate = employee.ProbationEndDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.ProbationEndDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.ContractEndDate = worksheet.Cells[row, 23].Value?.ToString();
                                    if (employee.ContractEndDate.IsStringNumber() && employee.ContractEndDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.ContractEndDate = DateTime.FromOADate(Convert.ToDouble(employee.ContractEndDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.ContractEndDate = employee.ContractEndDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.ContractEndDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.PFActivationDate = worksheet.Cells[row, 24].Value?.ToString();
                                    if (employee.PFActivationDate.IsStringNumber() && employee.PFActivationDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.PFActivationDate = DateTime.FromOADate(Convert.ToDouble(employee.PFActivationDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.PFActivationDate = employee.PFActivationDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.PFActivationDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.LastWorkingDate = worksheet.Cells[row, 25].Value?.ToString();
                                    if (employee.LastWorkingDate.IsStringNumber() && employee.LastWorkingDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.LastWorkingDate = DateTime.FromOADate(Convert.ToDouble(employee.LastWorkingDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.LastWorkingDate = employee.LastWorkingDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.LastWorkingDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.OfficeMobile = worksheet.Cells[row, 26].Value?.ToString();
                                    employee.OfficeMobile = employee.OfficeMobile != null ? employee.OfficeMobile.RemoveWhitespace() : employee.OfficeMobile;

                                    employee.OfficeEmail = worksheet.Cells[row, 27].Value?.ToString();
                                    employee.OfficeEmail = employee.OfficeEmail != null ? employee.OfficeEmail.RemoveWhitespace() : employee.OfficeEmail;

                                    employee.ReferenceId = worksheet.Cells[row, 28].Value?.ToString();
                                    employee.ReferenceId = employee.ReferenceId != null ? employee.ReferenceId.RemoveWhitespace() : employee.ReferenceId;

                                    employee.FingureId = worksheet.Cells[row, 29].Value?.ToString();
                                    employee.FingureId = employee.FingureId != null ? employee.FingureId.RemoveWhitespace() : employee.FingureId;

                                    employee.JobType = worksheet.Cells[row, 30].Value?.ToString();
                                    employee.JobType = employee.JobType != null ? employee.JobType.RemoveWhitespace() : employee.JobType;

                                    employee.JobCategoryName = worksheet.Cells[row, 31].Value?.ToString();
                                    employee.JobCategoryName = employee.JobCategoryName != null ? employee.JobCategoryName.RemoveWhitespace() : employee.JobCategoryName;

                                    employee.EmployeeTypeName = worksheet.Cells[row, 32].Value?.ToString();
                                    employee.EmployeeTypeName = employee.EmployeeTypeName != null ? employee.EmployeeTypeName.RemoveWhitespace() : employee.EmployeeTypeName;

                                    employee.Shift = worksheet.Cells[row, 33].Value?.ToString();
                                    employee.Shift = employee.Shift != null ? employee.Shift.RemoveWhitespace() : employee.Shift;

                                    // Supervisor & HOD
                                    employee.SupervisorID = worksheet.Cells[row, 34].Value?.ToString();
                                    employee.SupervisorID = employee.SupervisorID != null ? employee.SupervisorID.RemoveWhitespace() : employee.SupervisorID;
                                    employee.HODID = worksheet.Cells[row, 35].Value?.ToString();
                                    employee.HODID = employee.HODID != null ? employee.HODID.RemoveWhitespace() : employee.HODID;

                                    employee.NID = worksheet.Cells[row, 36].Value?.ToString();
                                    employee.NID = employee.NID != null ? employee.NID.RemoveWhitespace() : employee.NID;

                                    employee.TIN = worksheet.Cells[row, 37].Value?.ToString();
                                    employee.TIN = employee.TIN != null ? employee.TIN.RemoveWhitespace() : employee.TIN;

                                    employee.DrivingLicense = worksheet.Cells[row, 38].Value?.ToString();
                                    employee.DrivingLicense = employee.DrivingLicense != null ? employee.DrivingLicense.RemoveWhitespace() : employee.DrivingLicense;

                                    employee.Passport = worksheet.Cells[row, 39].Value?.ToString();
                                    employee.Passport = employee.Passport != null ? employee.Passport.RemoveWhitespace() : employee.Passport;

                                    employee.FatherName = worksheet.Cells[row, 40].Value?.ToString();
                                    employee.FatherName = employee.FatherName != null ? employee.FatherName.RemoveWhitespace() : employee.FatherName;

                                    employee.MotherName = worksheet.Cells[row, 41].Value?.ToString();
                                    employee.MotherName = employee.MotherName != null ? employee.MotherName.RemoveWhitespace() : employee.MotherName;

                                    employee.DateOfBirth = worksheet.Cells[row, 42].Value?.ToString();
                                    if (employee.DateOfBirth.IsStringNumber() && employee.DateOfBirth.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.DateOfBirth = DateTime.FromOADate(Convert.ToDouble(employee.DateOfBirth.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.DateOfBirth = employee.DateOfBirth.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.DateOfBirth.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.Gender = worksheet.Cells[row, 43].Value?.ToString();
                                    employee.Gender = employee.Gender != null ? employee.Gender.RemoveWhitespace() : employee.Gender;

                                    employee.Religion = worksheet.Cells[row, 44].Value?.ToString();
                                    employee.Religion = employee.Religion != null ? employee.Religion.RemoveWhitespace() : employee.Religion;

                                    employee.MaritalStatus = worksheet.Cells[row, 45].Value?.ToString();
                                    employee.MaritalStatus = employee.MaritalStatus != null ? employee.MaritalStatus.RemoveWhitespace() : employee.MaritalStatus;

                                    employee.SpouseName = worksheet.Cells[row, 46].Value?.ToString();
                                    employee.SpouseName = employee.SpouseName != null ? employee.SpouseName.RemoveWhitespace() : employee.SpouseName;

                                    employee.NumberOfChild = worksheet.Cells[row, 47].Value?.ToString();
                                    employee.NumberOfChild = employee.NumberOfChild != null ? employee.NumberOfChild.RemoveWhitespace() : employee.NumberOfChild;

                                    employee.BloodGroup = worksheet.Cells[row, 48].Value?.ToString();
                                    employee.BloodGroup = employee.BloodGroup != null ? employee.BloodGroup.RemoveWhitespace() : employee.BloodGroup;

                                    employee.IsResidential = worksheet.Cells[row, 49].Value?.ToString();
                                    employee.IsResidential = employee.IsResidential != null ? employee.IsResidential.RemoveWhitespace() : employee.IsResidential;

                                    employee.PersonalMobileNumber = worksheet.Cells[row, 50].Value?.ToString();
                                    employee.PersonalMobileNumber = employee.PersonalMobileNumber != null ? employee.PersonalMobileNumber.RemoveWhitespace() : employee.PersonalMobileNumber;

                                    employee.PersonalEmail = worksheet.Cells[row, 51].Value?.ToString();
                                    employee.PersonalEmail = employee.PersonalEmail != null ? employee.PersonalEmail.RemoveWhitespace() : employee.PersonalEmail;

                                    employee.AlternativeEmailAddress = worksheet.Cells[row, 52].Value?.ToString();
                                    employee.AlternativeEmailAddress = employee.AlternativeEmailAddress != null ? employee.AlternativeEmailAddress.RemoveWhitespace() : employee.AlternativeEmailAddress;

                                    employee.PresentAddress = worksheet.Cells[row, 53].Value?.ToString();
                                    employee.PresentAddress = employee.PresentAddress != null ? employee.PresentAddress.RemoveWhitespace() : employee.PresentAddress;

                                    employee.PresentAddressCity = worksheet.Cells[row, 54].Value?.ToString();
                                    employee.PresentAddressCity = employee.PresentAddressCity != null ? employee.PresentAddressCity.RemoveWhitespace() : employee.PresentAddressCity;

                                    employee.PresentAddressContactNo = worksheet.Cells[row, 55].Value?.ToString();
                                    employee.PresentAddressContactNo = employee.PresentAddressContactNo != null ? employee.PresentAddressContactNo.RemoveWhitespace() : employee.PresentAddressContactNo;

                                    employee.PresentAddressZipCode = worksheet.Cells[row, 56].Value?.ToString();
                                    employee.PresentAddressZipCode = employee.PresentAddressZipCode != null ? employee.PresentAddressZipCode.RemoveWhitespace() : employee.PresentAddressZipCode;

                                    employee.PermanentAddress = worksheet.Cells[row, 57].Value?.ToString();
                                    employee.PermanentAddress = employee.PermanentAddress != null ? employee.PermanentAddress.Trim() : employee.PermanentAddress;

                                    employee.PermanentAddressDistrict = worksheet.Cells[row, 58].Value?.ToString();
                                    employee.PermanentAddressDistrict = employee.PermanentAddressDistrict != null ? employee.PermanentAddressDistrict.RemoveWhitespace() : employee.PermanentAddressDistrict;

                                    employee.PermanentAddressUpazila = worksheet.Cells[row, 59].Value?.ToString();
                                    employee.PermanentAddressUpazila = employee.PermanentAddressUpazila != null ? employee.PermanentAddressUpazila.RemoveWhitespace() : employee.PermanentAddressUpazila;

                                    employee.PermanentAddressZipCode = worksheet.Cells[row, 60].Value?.ToString();
                                    employee.PermanentAddressZipCode = employee.PermanentAddressZipCode != null ? employee.PermanentAddressZipCode.RemoveWhitespace() : employee.PermanentAddressZipCode;

                                    employee.EmergencyContactPerson1 = worksheet.Cells[row, 61].Value?.ToString();
                                    employee.EmergencyContactPerson1 = employee.EmergencyContactPerson1 != null ? employee.EmergencyContactPerson1.RemoveWhitespace() : employee.EmergencyContactPerson1;

                                    employee.RelationWithEmergencyContactPerson1 = worksheet.Cells[row, 62].Value?.ToString();
                                    employee.RelationWithEmergencyContactPerson1 = employee.RelationWithEmergencyContactPerson1 != null ? employee.RelationWithEmergencyContactPerson1.RemoveWhitespace() : employee.RelationWithEmergencyContactPerson1;

                                    employee.EmergencyContactNoPerson1 = worksheet.Cells[row, 63].Value?.ToString();
                                    employee.EmergencyContactNoPerson1 = employee.EmergencyContactNoPerson1 != null ? employee.EmergencyContactNoPerson1.RemoveWhitespace() : employee.EmergencyContactNoPerson1;

                                    employee.EmergencyContactAddressPerson1 = worksheet.Cells[row, 64].Value?.ToString();
                                    employee.EmergencyContactAddressPerson1 = employee.EmergencyContactAddressPerson1 != null ? employee.EmergencyContactAddressPerson1.RemoveWhitespace() : employee.EmergencyContactAddressPerson1;

                                    employee.EmergencyContactEmailAddressPerson1 = worksheet.Cells[row, 65].Value?.ToString();
                                    employee.EmergencyContactEmailAddressPerson1 = employee.EmergencyContactEmailAddressPerson1 != null ? employee.EmergencyContactEmailAddressPerson1.RemoveWhitespace() : employee.EmergencyContactEmailAddressPerson1;

                                    employee.EmergencyContactPerson2 = worksheet.Cells[row, 66].Value?.ToString();
                                    employee.EmergencyContactPerson2 = employee.EmergencyContactPerson2 != null ? employee.EmergencyContactPerson2.RemoveWhitespace() : employee.EmergencyContactPerson2;

                                    employee.RelationWithEmergencyContactPerson2 = worksheet.Cells[row, 67].Value?.ToString();
                                    employee.RelationWithEmergencyContactPerson2 = employee.RelationWithEmergencyContactPerson2 != null ? employee.RelationWithEmergencyContactPerson2.RemoveWhitespace() : employee.RelationWithEmergencyContactPerson2;

                                    employee.EmergencyContactNoPerson2 = worksheet.Cells[row, 68].Value?.ToString();
                                    employee.EmergencyContactNoPerson2 = employee.EmergencyContactNoPerson2 != null ? employee.EmergencyContactNoPerson2.RemoveWhitespace() : employee.EmergencyContactNoPerson2;

                                    employee.EmergencyContactAddressPerson2 = worksheet.Cells[row, 69].Value?.ToString();
                                    employee.EmergencyContactAddressPerson2 = employee.EmergencyContactAddressPerson2 != null ? employee.EmergencyContactAddressPerson2.RemoveWhitespace() : employee.EmergencyContactAddressPerson2;

                                    employee.EmergencyContactEmailAddressPerson2 = worksheet.Cells[row, 70].Value?.ToString();
                                    employee.EmergencyContactEmailAddressPerson2 = employee.EmergencyContactEmailAddressPerson2 != null ? employee.EmergencyContactEmailAddressPerson2.RemoveWhitespace() : employee.EmergencyContactEmailAddressPerson2;

                                    employee.BankCode = worksheet.Cells[row, 71].Value?.ToString();
                                    employee.BankCode = employee.BankCode != null ? employee.BankCode.RemoveWhitespace() : employee.BankCode;

                                    employee.BankName = worksheet.Cells[row, 72].Value?.ToString();
                                    employee.BankName = employee.BankName != null ? employee.BankName.RemoveWhitespace() : employee.BankName;

                                    employee.BankBranchName = worksheet.Cells[row, 73].Value?.ToString();
                                    employee.BankBranchName = employee.BankBranchName != null ? employee.BankBranchName.RemoveWhitespace() : employee.BankBranchName;

                                    employee.RoutingNumber = worksheet.Cells[row, 74].Value?.ToString();
                                    employee.RoutingNumber = employee.RoutingNumber != null ? employee.RoutingNumber.RemoveWhitespace() : employee.RoutingNumber;

                                    employee.AccountNumber = worksheet.Cells[row, 75].Value?.ToString();
                                    employee.AccountNumber = employee.AccountNumber != null ? employee.AccountNumber.RemoveWhitespace() : employee.AccountNumber;

                                    employee.AccountActivationDate = worksheet.Cells[row, 76].Value?.ToString();
                                    if (employee.AccountActivationDate.IsStringNumber() && employee.AccountActivationDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.AccountActivationDate = DateTime.FromOADate(Convert.ToDouble(employee.AccountActivationDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.AccountActivationDate = employee.AccountActivationDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.AccountActivationDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.GrossSalary = worksheet.Cells[row, 77].Value?.ToString();
                                    employee.GrossSalary = employee.GrossSalary != null ? employee.GrossSalary.RemoveWhitespace() : employee.GrossSalary;

                                    employee.BasicSalary = worksheet.Cells[row, 78].Value?.ToString();
                                    employee.BasicSalary = employee.BasicSalary != null ? employee.BasicSalary.RemoveWhitespace() : employee.BasicSalary;

                                    employee.SalaryEffectiveDate = worksheet.Cells[row, 79].Value?.ToString();
                                    if (employee.SalaryEffectiveDate.IsStringNumber() && employee.SalaryEffectiveDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.SalaryEffectiveDate = DateTime.FromOADate(Convert.ToDouble(employee.SalaryEffectiveDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.SalaryEffectiveDate = employee.SalaryEffectiveDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.SalaryEffectiveDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.SalaryActivationDate = worksheet.Cells[row, 80].Value?.ToString();
                                    if (employee.SalaryActivationDate.IsStringNumber() && employee.SalaryActivationDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        employee.SalaryActivationDate = DateTime.FromOADate(Convert.ToDouble(employee.SalaryActivationDate.RemoveWhitespace())).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        employee.SalaryActivationDate = employee.SalaryActivationDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(employee.SalaryActivationDate.RemoveWhitespace()).ToString("yyyy-MM-dd") : null;
                                    }

                                    employee.InternalDesignationName = worksheet.Cells[row, 81].Value?.ToString();
                                    employee.InternalDesignationName = employee.InternalDesignationName != null ? employee.InternalDesignationName.RemoveWhitespace() : employee.InternalDesignationName;

                                    data.Add(employee);
                                }
                            }
                        }
                        if (data.Count > 0)
                        {
                            var status = await _employeeUploaderBusiness.UploadEmployeeInfoAsync(data, user);
                            var successfull = status.Where(item => item.Status == true);
                            var successfullCount = successfull.Count();
                            var unsuccessfull = status.Where(item => item.Status == false);
                            var unsuccessfullCount = unsuccessfull.Count();
                            return Ok(new { successfull = successfull, totalSuccessfull = successfullCount, unsuccessfull = unsuccessfull, totalUnsuccessfull = unsuccessfullCount });
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeUploaderController", "UploadEmployeeInfoAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadEmployeesInfo")]
        public async Task<IActionResult> DownloadEmployeesInfoAsync()
        {
            var user = AppUser();
            try
            {
                var data = await _employeeUploaderBusiness.DownloadEmployeesDataExcelAsync(user);
                var datatable = ListToDatatable.ToDataTable(data);
                var fileBytes = _excelGenerator.GenerateExcel(datatable, "Employee Sheet");
                string fileName = "data.xlsx";
                string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                using (var package = new ExcelPackage(new MemoryStream(fileBytes)))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    fileBytes = package.GetAsByteArray();
                }
                HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
                HttpContext.Response.ContentType = contentType;
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploaderController", "DownloadEmployeesInfoAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }

        }

        [HttpGet, Route("DownloadEmployeeInfoExcelFile")]
        public async Task<IActionResult> DownloadEmployeeInfoExcelFileAsync(string fileName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel", fileName);
            var provider = new FileExtensionContentTypeProvider();
            string contenttype = "";
            if (System.IO.File.Exists(filepath))
            {
                contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, fileName);
        }

        [HttpPost, Route("ReadEmployeeInfoExcelFile")]
        public async Task<IActionResult> ReadEmployeeInfoExcelFileAsync([FromForm] EmployeeUploaderFileDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        var data = await _employeeUploaderBusiness.GetEmployeeInfoFromExcelAsync(model.File, user);
                        return Ok(data);
                    }
                    return BadRequest("File length is 0");
                }
                else
                {
                    return BadRequest(ModelStateErrorMsg(ModelState));
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploaderController", "ReadEmployeeInfoExcelFileAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("SaveEmployeeInfo")]
        public async Task<IActionResult> SaveEmployeeInfoAsync(List<ExcelInfoCollection> models)
        {
            var user = AppUser();
            try
            {
                if (models.Any() && user.HasBoth)
                {
                    var data = await _employeeUploaderBusiness.SaveExcelData(models, user);
                    if (data != null && data.Any())
                    {
                        var successful = data.Where(i => i.Status == true).ToList();
                        var unsuccessful = data.Where(i => i.Status == false).ToList();
                        return Ok(new { successful = successful, successfulCount = successful.Count, unsuccessful = unsuccessful, unsuccessfulCount = unsuccessful.Count });
                    }
                    return BadRequest(ResponseMessage.ServerResponsedWithError);
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploaderController", "SaveEmployeeInfoAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
