using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.OtherModels.Response;
using Shared.Services;
using Shared.Employee.Domain.Termination;
using Shared.Employee.Domain.Account;
using System.Data;
using BLL.Employee.Interface.Info;
using BLL.Employee.Interface.Account;
using DAL.DapperObject.Interface;
using Shared.Employee.Domain.Info;
using Shared.Employee.ViewModel.Info;
using Shared.Helpers;


namespace BLL.Employee.Implementation.Info
{
    public class EmployeeLoggerBusiness : IEmployeeLoggerBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IInfoBusiness _infoBusiness;
        private readonly IAccountInfoBusiness _accountInfoBusiness;

        public EmployeeLoggerBusiness(IDapperData dapper, ISysLogger sysLogger, IInfoBusiness infoBusiness, IAccountInfoBusiness accountInfoBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _infoBusiness = infoBusiness;
            _accountInfoBusiness = accountInfoBusiness;
        }
        public async Task<List<EmployeeLoggerDTO>> GetEmployeeLogReportAsync(EmployeeLogReport_Filter filter, AppUser user)
        {
            List<EmployeeLoggerDTO> list = new List<EmployeeLoggerDTO>();
            try
            {
                var dateRange = filter.DateRange.Split("~");
                var startDate = dateRange[0];
                var endDate = dateRange[1];

                var sqlQuery = $"sp_employee_info_activity_log";
                IEnumerable<HRActivityLoggerDTO> logOfEmployeeInformation = new List<HRActivityLoggerDTO>();
                logOfEmployeeInformation = await _dapper.SqlQueryListAsync<HRActivityLoggerDTO>(user.Database, sqlQuery, new
                {
                    filter.UserId,
                    StartDate = startDate,
                    filter.BranchId,
                    EndDate = endDate
                }, CommandType.StoredProcedure);

                var userIds = logOfEmployeeInformation.Select(item => Utility.TryParseLong(item.UserEmployeeId)).Distinct().ToList();
                foreach (var userId in userIds)
                {
                    var userInfo = logOfEmployeeInformation.FirstOrDefault(log => Utility.TryParseLong(log.UserId) == userId);
                    var useremployeecode = "";
                    var useremployeeid = userId.ToString();
                    var username = "";
                    var useremail = "";
                    var userdesignation = "";

                    if (userInfo != null)
                    {
                        useremployeecode = userInfo.UserEmployeeCode;
                        username = userInfo.Username;
                        useremail = userInfo.Useremail;
                        userdesignation = userInfo.UserDesignation;
                    }

                    var employeeIds = logOfEmployeeInformation.Where(emp => Utility.TryParseLong(emp.UserEmployeeId) == userId).Select(item => Utility.TryParseLong(item.EmployeeId)).Distinct().ToList();
                    foreach (var id in employeeIds)
                    {
                        // Employee Information //
                        var employeeInformations = logOfEmployeeInformation.Where(item => Utility.TryParseLong(item.EmployeeId) == id && item.ImpactTables == "HR_EmployeeInformation" && Utility.TryParseLong(item.UserEmployeeId) == userId).ToList();

                        var employeeDetails = logOfEmployeeInformation.Where(item => Utility.TryParseLong(item.EmployeeId) == id
                        && item.ImpactTables == "HR_EmployeeDetail" && Utility.TryParseLong(item.UserEmployeeId) == userId).ToList();

                        var employeeDocuments = logOfEmployeeInformation.Where(item => Utility.TryParseLong(item.EmployeeId) == id
                        && item.ImpactTables == "HR_EmployeeDocument" && Utility.TryParseLong(item.UserEmployeeId) == userId).ToList();

                        var employeeDiscontinued = logOfEmployeeInformation.Where(item => Utility.TryParseLong(item.EmployeeId) == id
                        && item.ImpactTables == "HR_DiscontinuedEmployee" && Utility.TryParseLong(item.UserEmployeeId) == userId).ToList();

                        var employeeAccountInfo = logOfEmployeeInformation.Where(item => Utility.TryParseLong(item.EmployeeId) == id
                        && item.ImpactTables == "HR_EmployeeAccountInfo" && Utility.TryParseLong(item.UserEmployeeId) == userId).ToList();

                        List<EmployeeDocument> findTinDataInLogs = new List<EmployeeDocument>();
                        foreach (var item in employeeDocuments)
                        {
                            if (!Utility.IsNullEmptyOrWhiteSpace(item.PresentValue))
                            {
                                var docments = JsonReverseConverter.JsonToObject<IEnumerable<EmployeeDocument>>(item.PresentValue);
                                if (docments.Any())
                                {
                                    if (docments.FirstOrDefault().DocumentNumber == "TIN")
                                    {
                                        findTinDataInLogs.Add(docments.FirstOrDefault());
                                    }
                                }
                            }
                        }

                        if (employeeInformations.Any() || employeeDetails.Any() || findTinDataInLogs.Any() || employeeDiscontinued.Any() || employeeAccountInfo.Any())
                        {
                            EmployeeLoggerDTO firstRow = new EmployeeLoggerDTO();
                            firstRow.UserEmployeeCode = useremployeecode;
                            firstRow.Username = username;
                            firstRow.Useremail = useremail;
                            firstRow.UserDesignation = userdesignation;
                            firstRow.DateRange = Convert.ToDateTime(startDate).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(endDate).ToString("dd MMM yyyy");

                            bool hasEmployeeJoinInThisDate = false;
                            if (employeeInformations.Any())
                            {
                                var employeeInfoInLog = employeeInformations.FirstOrDefault();
                                if (employeeInfoInLog != null)
                                {
                                    var employeeInfos = JsonReverseConverter.JsonToObject2<IEnumerable<EmployeeInformation>>(employeeInfoInLog.PresentValue);
                                    if (employeeInfos.Any())
                                    {
                                        var employeeInfo = employeeInfos.FirstOrDefault();
                                        if (employeeInfo != null)
                                        {
                                            //hasEmployeeJoinInThisDate = true;
                                            hasEmployeeJoinInThisDate = employeeInfo.DateOfJoining.Value.IsDateBetweenTwoDates(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                                            if (hasEmployeeJoinInThisDate)
                                            {
                                                firstRow.Date = employeeInfo.DateOfJoining.Value.ToString("dd-MMM-yyyy");
                                            }
                                            else
                                            {
                                                firstRow.Date = "";
                                            }

                                            firstRow.EmployeeId = id;
                                            firstRow.EmployeeCode = employeeInfo.EmployeeCode;

                                            firstRow.FullName = employeeInfo.FullName;
                                            firstRow.Store_FullName = employeeInfo.FullName;

                                            firstRow.OfficeEmail = employeeInfo.OfficeEmail;
                                            firstRow.Store_OfficeEmail = employeeInfo.OfficeEmail;

                                            firstRow.OfficeMobileNo = employeeInfo.OfficeMobile;
                                            firstRow.Store_OfficeMobileNo = employeeInfo.OfficeMobile;

                                            firstRow.Designation = "";
                                            firstRow.Store_Designation = "";

                                            firstRow.Department = "";
                                            firstRow.Store_Department = "";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Find Employee Info when not found in the log
                                var employeeInfos = await _infoBusiness.GetEmployeeDataAsync(new EmployeeService_Filter()
                                {
                                    EmployeeId = id.ToString(),
                                }, user);

                                if (employeeInfos.Any())
                                {
                                    var employeeInfo = employeeInfos.FirstOrDefault();
                                    if (employeeInfo != null)
                                    {

                                        firstRow.Date = "";
                                        firstRow.EmployeeId = id;
                                        firstRow.EmployeeCode = employeeInfo.EmployeeCode;

                                        firstRow.FullName = employeeInfo.EmployeeName;
                                        firstRow.Store_FullName = employeeInfo.EmployeeName;

                                        firstRow.OfficeEmail = employeeInfo.OfficeEmail;
                                        firstRow.Store_OfficeEmail = employeeInfo.OfficeEmail;

                                        firstRow.OfficeMobileNo = employeeInfo.OfficeMobile;
                                        firstRow.Store_OfficeMobileNo = employeeInfo.OfficeMobile;

                                        firstRow.Designation = employeeInfo.DesignationName;
                                        firstRow.Store_Designation = employeeInfo.DesignationName;

                                        firstRow.Department = employeeInfo.DepartmentName;
                                        firstRow.Store_Department = employeeInfo.DepartmentName;

                                        var bankAccountInfo = await _accountInfoBusiness.GetAccountActivationInfoBeforeDate(id, startDate, user);
                                        if (bankAccountInfo != null)
                                        {
                                            firstRow.AccountNo = bankAccountInfo.AccountNo;
                                        }
                                    }
                                }
                            }

                            var firstDetail = employeeDetails.FirstOrDefault();
                            if (firstDetail != null)
                            {
                                if (!Utility.IsNullEmptyOrWhiteSpace(firstDetail.PreviousValue))
                                {
                                    var employeeDeatilsFromLog = JsonReverseConverter.JsonToObject2<IEnumerable<EmployeeDetail>>(firstDetail.PreviousValue);
                                    if (employeeDeatilsFromLog.Any())
                                    {
                                        var firstEmployeeDeatil = employeeDeatilsFromLog.FirstOrDefault();
                                        if (firstEmployeeDeatil != null)
                                        {
                                            firstRow.PresentAddress = firstEmployeeDeatil.PresentAddress;
                                            firstRow.Store_PresentAddress = firstEmployeeDeatil.PresentAddress;

                                            firstRow.PermanentAddress = firstEmployeeDeatil.PermanentAddress;
                                            firstRow.Store_PermanentAddress = firstEmployeeDeatil.PermanentAddress;

                                            firstRow.PersonalMobileNo = firstEmployeeDeatil.PersonalMobileNo;
                                            firstRow.Store_PersonalMobileNo = firstEmployeeDeatil.PersonalMobileNo;

                                            firstRow.Nationality = firstEmployeeDeatil.IsResidential == true ? "Bangladeshi" : "";
                                            firstRow.Store_Nationality = firstRow.Nationality;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var employeeDetailInDbNow = await _infoBusiness.GetEmployeePersonalInfoByIdAsync(new EmployeePersonalInfoQuery()
                                {
                                    EmployeeId = id.ToString(),
                                }, user);

                                if (employeeDetailInDbNow != null)
                                {
                                    firstRow.PresentAddress = employeeDetailInDbNow.PresentAddress;
                                    firstRow.Store_PresentAddress = employeeDetailInDbNow.PresentAddress;

                                    firstRow.PermanentAddress = employeeDetailInDbNow.PermanentAddress;
                                    firstRow.Store_PermanentAddress = employeeDetailInDbNow.PermanentAddress;

                                    firstRow.PersonalMobileNo = employeeDetailInDbNow.PersonalMobileNo;
                                    firstRow.Store_PersonalMobileNo = employeeDetailInDbNow.PersonalMobileNo;

                                    firstRow.Nationality = employeeDetailInDbNow.IsResidential == true ? "Bangladeshi" : "";
                                    firstRow.Store_Nationality = firstRow.Nationality;
                                }
                            }

                            if (employeeDocuments.Any())
                            {
                                // find tin number //
                                List<EmployeeDocument> documents = new List<EmployeeDocument>();
                                foreach (var item in employeeDocuments)
                                {
                                    if (!Utility.IsNullEmptyOrWhiteSpace(item.PreviousValue))
                                    {
                                        var document = JsonReverseConverter.JsonToObject<IEnumerable<EmployeeDocument>>(item.PreviousValue);
                                        if (document.Any())
                                        {
                                            var isfileTin = document.Where(i => i.DocumentName == "TIN").FirstOrDefault() != null;
                                            if (isfileTin)
                                            {
                                                var tinFile = document.FirstOrDefault();
                                                tinFile.CreatedDate = item.CreatedDate;
                                                documents.Add(tinFile);
                                            }
                                        }
                                    }
                                }

                                var tin = documents.OrderBy(i => i.CreatedDate).FirstOrDefault();
                                if (tin != null)
                                {
                                    firstRow.TIN = tin.DocumentNumber;
                                    firstRow.Store_TIN = firstRow.TIN;
                                }
                            }

                            list.Add(firstRow);
                            List<EmployeeLoggerDTO> innerList = new List<EmployeeLoggerDTO>();
                            // Start logging //
                            var distinctDates = logOfEmployeeInformation.Where(item => Utility.TryParseLong(item.EmployeeId) == id).Select(item => item.CreatedDate.Value.ToString("yyyy-MM-dd")).Distinct();

                            int count = 1;
                            foreach (var date in distinctDates)
                            {

                                //var itemToCompare = list[count - 1];
                                var itemToCompare = list[list.Count - 1];
                                EmployeeLoggerDTO row = new EmployeeLoggerDTO();
                                row.EmployeeId = firstRow.EmployeeId;
                                row.EmployeeCode = firstRow.EmployeeCode;

                                row.UserEmployeeCode = useremployeecode;
                                row.Username = username;
                                row.Useremail = useremail;
                                row.UserDesignation = userdesignation;

                                var parsedDate = Convert.ToDateTime(date);
                                row.Date = parsedDate.ToString("dd-MMM-yyyy");

                                // Employee Info
                                bool hasEmployeeChange = false;
                                var employeeInfo_under_this_date = employeeInformations.Where(item => Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd"))
                                >= Convert.ToDateTime(date).Date && Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")).Date == Convert.ToDateTime(date).Date);
                                if (employeeInfo_under_this_date.Any())
                                {
                                    foreach (var item in employeeInfo_under_this_date)
                                    {
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.PresentValue))
                                        {
                                            var jsonObj = JsonReverseConverter.JsonToObject<IEnumerable<EmployeeInformation>>(item.PresentValue);
                                            var jsonItem = jsonObj.FirstOrDefault();

                                            if (jsonItem != null)
                                            {
                                                row.OfficeEmail = itemToCompare.Store_OfficeEmail == jsonItem.OfficeEmail ? "-" : jsonItem.OfficeEmail;
                                                row.Store_OfficeEmail = jsonItem.OfficeEmail;

                                                row.OfficeMobileNo = itemToCompare.Store_OfficeMobileNo == jsonItem.OfficeMobile ? "-" : jsonItem.OfficeMobile;
                                                row.Store_OfficeMobileNo = jsonItem.OfficeMobile;

                                                row.FullName = itemToCompare.Store_FullName == jsonItem.FullName ? "-" : jsonItem.FullName;
                                                row.Store_FullName = jsonItem.FullName;

                                                row.Store_Designation = itemToCompare.Store_Designation;
                                                row.Store_Department = itemToCompare.Store_Department;

                                                if (row.OfficeEmail == "-" && row.OfficeMobileNo == "-" && row.FullName == "-")
                                                {
                                                    hasEmployeeChange = false;
                                                }
                                                else
                                                {
                                                    //hasEmployeeChange = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            row.FullName = "-";
                                            row.Store_FullName = itemToCompare.Store_FullName;

                                            row.OfficeEmail = "-";
                                            row.Store_OfficeEmail = itemToCompare.Store_OfficeEmail;

                                            row.OfficeMobileNo = "-";
                                            row.Store_OfficeMobileNo = itemToCompare.Store_OfficeMobileNo;

                                            row.Designation = "-";
                                            row.Store_Designation = itemToCompare.Store_Designation;

                                            row.Department = "-";
                                            row.Store_Department = itemToCompare.Store_Department;
                                            hasEmployeeChange = false;

                                        }
                                    }
                                }
                                else
                                {
                                    row.FullName = "-";
                                    row.Store_FullName = itemToCompare.Store_FullName;
                                    row.OfficeEmail = "-";
                                    row.Store_OfficeEmail = itemToCompare.Store_OfficeEmail;
                                    row.OfficeMobileNo = "-";
                                    row.Store_OfficeMobileNo = itemToCompare.Store_OfficeMobileNo;
                                    row.Designation = "-";
                                    row.Store_Designation = itemToCompare.Store_Designation;
                                    row.Department = "-";
                                    row.Store_Department = itemToCompare.Store_Department;
                                    hasEmployeeChange = false;
                                }

                                // Employee Detail
                                bool hasEmployeeDetailChange = false;
                                var employeeDetail_under_this_date = employeeDetails.Where(item => Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd"))
                                >= Convert.ToDateTime(date).Date && Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")).Date == Convert.ToDateTime(date).Date);

                                if (employeeDetail_under_this_date.Any())
                                {
                                    foreach (var item in employeeDetail_under_this_date)
                                    {
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.PresentValue))
                                        {
                                            var jsonObj = JsonReverseConverter.JsonToObject<IEnumerable<EmployeeDetail>>(item.PresentValue);
                                            var jsonItem = jsonObj.FirstOrDefault();

                                            if (jsonItem != null)
                                            {
                                                row.PresentAddress = itemToCompare.Store_PresentAddress == jsonItem.PresentAddress ? "-" : jsonItem.PresentAddress;
                                                row.Store_PresentAddress = jsonItem.PresentAddress;

                                                row.PermanentAddress = itemToCompare.Store_PermanentAddress == jsonItem.PermanentAddress ? "-" : jsonItem.PermanentAddress;
                                                row.Store_PermanentAddress = jsonItem.PermanentAddress;

                                                row.PersonalMobileNo = itemToCompare.Store_PersonalMobileNo == jsonItem.PersonalMobileNo ? "-" : jsonItem.PersonalMobileNo;
                                                row.Store_PersonalMobileNo = jsonItem.PersonalMobileNo;

                                                row.Nationality = itemToCompare.Store_Nationality == (jsonItem.IsResidential ?? false ? "Bangladeshi" : "") ? "-" : jsonItem.IsResidential ?? false ? "Bangladeshi" : "";

                                                row.Store_Nationality = jsonItem.IsResidential ?? false ? "Bangladeshi" : "";


                                                if (row.PresentAddress == "-" && row.PermanentAddress == "-" && row.PersonalMobileNo == "-" && row.Nationality == "-")
                                                {
                                                    hasEmployeeDetailChange = false;
                                                }
                                                else
                                                {
                                                    hasEmployeeDetailChange = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            row.PresentAddress = "-";
                                            row.Store_PresentAddress = itemToCompare.Store_PresentAddress;
                                            row.PermanentAddress = "-";
                                            row.Store_PermanentAddress = itemToCompare.Store_PermanentAddress;
                                            row.Nationality = "-";
                                            row.Store_Nationality = itemToCompare.Store_Nationality;
                                            row.PersonalMobileNo = "-";
                                            row.Store_PersonalMobileNo = itemToCompare.Store_PersonalMobileNo;
                                            hasEmployeeDetailChange = false;

                                        }
                                    }
                                }
                                else
                                {
                                    row.PresentAddress = "-";
                                    row.Store_PresentAddress = itemToCompare.Store_PresentAddress;
                                    row.PermanentAddress = "-";
                                    row.Store_PermanentAddress = itemToCompare.Store_PermanentAddress;
                                    row.Nationality = "-";
                                    row.Store_Nationality = itemToCompare.Store_Nationality;
                                    row.PersonalMobileNo = "-";
                                    row.Store_PersonalMobileNo = itemToCompare.Store_PersonalMobileNo;
                                    hasEmployeeDetailChange = false;
                                }

                                // Employee Document //
                                bool hasDocumentChange = false;
                                var employeeDocument_under_this_date = employeeDocuments.Where(item => Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd"))
                               >= Convert.ToDateTime(date) && Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")).Date == Convert.ToDateTime(date).Date);
                                bool hasTinInfo = false;
                                if (employeeDocument_under_this_date.Any())
                                {
                                    foreach (var item in employeeDocument_under_this_date)
                                    {
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.PresentValue))
                                        {
                                            var jsonObj = JsonReverseConverter.JsonToObject<IEnumerable<EmployeeDocument>>(item.PresentValue);
                                            var jsonItem = jsonObj.FirstOrDefault();
                                            if (jsonItem.DocumentName == "TIN")
                                            {
                                                hasTinInfo = true;
                                                row.TIN = itemToCompare.Store_TIN == jsonItem.DocumentNumber ? "-" : jsonItem.DocumentNumber;
                                                row.Store_TIN = jsonItem.DocumentNumber;
                                            }
                                        }
                                        else
                                        {
                                            row.TIN = "-";
                                            row.Store_TIN = itemToCompare.Store_TIN;
                                        }
                                    }
                                }
                                else
                                {
                                    row.TIN = "-";
                                    row.Store_TIN = itemToCompare.Store_TIN;
                                }

                                if (row.TIN == "-")
                                {
                                    hasDocumentChange = false;
                                }
                                else
                                {
                                    hasDocumentChange = true;
                                }


                                // Employee Discontinued
                                bool hasDiscontinuedChange = false;
                                var employeeDiscontinued_under_this_date = employeeDiscontinued.Where(item => Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(date) && Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")).Date == Convert.ToDateTime(date).Date);
                                if (employeeDiscontinued_under_this_date.Any())
                                {
                                    foreach (var item in employeeDiscontinued_under_this_date)
                                    {
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.PresentValue))
                                        {
                                            var jsonObj = JsonReverseConverter.JsonToObject<IEnumerable<DiscontinuedEmployee>>(item.PresentValue);
                                            var jsonItem = jsonObj.FirstOrDefault();
                                            if (jsonItem.LastWorkingDate != null && jsonItem.StateStatus == "Approved")
                                            {

                                                if (DateTimeExtension.TryParseDate(jsonItem.LastWorkingDate.Value.ToString()) != null)
                                                {
                                                    row.TerminationDate = Convert.ToDateTime(itemToCompare.Store_TerminationDate).Date == jsonItem.LastWorkingDate.Value.Date ? "-" : jsonItem.LastWorkingDate.Value.Date.ToString("dd MMM yyyy");
                                                }
                                                else
                                                {
                                                    row.TerminationDate = "-";
                                                }

                                                row.ResignType = itemToCompare.Store_ResignType == jsonItem.Releasetype ? "-" : jsonItem.Releasetype;

                                                row.Store_TerminationDate = jsonItem.LastWorkingDate.Value.Date.ToString("dd MMM yyyy");
                                                row.Store_ResignType = jsonItem.Releasetype;
                                            }
                                            else
                                            {
                                                row.TerminationDate = "-";
                                                row.Store_TerminationDate = itemToCompare.Store_TerminationDate;
                                                row.ResignType = "-";
                                                row.Store_ResignType = itemToCompare.Store_ResignType;
                                            }
                                        }
                                        else
                                        {
                                            row.TerminationDate = "-";
                                            row.Store_TerminationDate = itemToCompare.Store_TerminationDate;
                                            row.ResignType = "-";
                                            row.Store_ResignType = itemToCompare.Store_ResignType;
                                        }
                                    }
                                }
                                else
                                {
                                    row.TerminationDate = "-";
                                    row.Store_TerminationDate = itemToCompare.Store_TerminationDate;
                                    row.ResignType = "-";
                                    row.Store_ResignType = itemToCompare.Store_ResignType;
                                }

                                if (row.TerminationDate == "-")
                                {
                                    hasDocumentChange = false;
                                }
                                else
                                {
                                    hasDocumentChange = true;
                                }


                                // Employee Bank Account
                                bool hasBankChange = false;
                                var employeeAccountInfo_under_this_date = employeeAccountInfo.Where(item => Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(date) && Convert.ToDateTime(item.CreatedDate.Value.ToString("yyyy-MM-dd")).Date == Convert.ToDateTime(date).Date);

                                if (employeeAccountInfo_under_this_date.Any())
                                {
                                    foreach (var item in employeeAccountInfo_under_this_date)
                                    {
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.PresentValue))
                                        {
                                            var jsonObj = JsonReverseConverter.JsonToObject<IEnumerable<EmployeeAccountInfo>>(item.PresentValue);
                                            var jsonItem = jsonObj.FirstOrDefault();
                                            if (jsonItem.AccountNo != null)
                                            {
                                                row.AccountNo = itemToCompare.Store_AccountNo == jsonItem.AccountNo ? "-" : jsonItem.AccountNo;
                                                row.Store_AccountNo = jsonItem.AccountNo;
                                            }
                                        }
                                        else
                                        {
                                            row.AccountNo = "-";
                                            row.Store_AccountNo = itemToCompare.AccountNo;
                                        }
                                    }
                                }
                                else
                                {
                                    row.AccountNo = "-";
                                    row.Store_AccountNo = itemToCompare.AccountNo;
                                }

                                if (row.AccountNo == "-")
                                {
                                    hasBankChange = false;
                                }
                                else
                                {
                                    hasBankChange = true;
                                }

                                if (employeeInfo_under_this_date.Any() || employeeDetail_under_this_date.Any() || employeeDiscontinued_under_this_date.Any() ||
                                    employeeAccountInfo_under_this_date.Any())
                                {
                                    // hasTinInfo == true
                                    if (hasEmployeeJoinInThisDate || hasEmployeeChange || hasEmployeeDetailChange || hasDiscontinuedChange || hasDocumentChange || hasBankChange)
                                    {
                                        if (hasEmployeeJoinInThisDate == true &&
                                            hasEmployeeChange == false &&
                                            hasEmployeeDetailChange == false &&
                                            hasDiscontinuedChange == false &&
                                            hasDocumentChange == false &&
                                            hasBankChange == false)
                                        {
                                            //innerList.Add(row);
                                        }
                                        else
                                        {
                                            innerList.Add(row);
                                        }

                                    }
                                    count++;
                                }
                            }

                            if (innerList.Any())
                            {
                                foreach (var inner in innerList)
                                {
                                    list.Add(inner);
                                }
                            }
                            else
                            {
                                if (hasEmployeeJoinInThisDate == false)
                                {
                                    if (list.Any())
                                    {
                                        list.RemoveAt(list.Count - 1);
                                    }
                                }
                            }
                        }
                    }
                    var datatable = list.ToDataTable();
                    //if (!list.Any()) {
                    //    list.Add(new EmployeeLoggerDTO());
                    //}
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public async Task<DataTable> GetUserLogInfoAysnc(string userId, AppUser user)
        {
            DataTable userInfo = new DataTable();
            try
            {
                EmployeeOfficeInfoViewModel editedByEmployeeInfo = null;
                if (userId != "0" && userId != "" && userId != null && userId.IsNullEmptyOrWhiteSpace() == false)
                {
                    editedByEmployeeInfo = await _infoBusiness.GetEmployeeOfficeInfoByIdAsync(new EmployeeOfficeInfo_Filter()
                    {
                        EmployeeId = userId
                    }, user);
                }

                if (editedByEmployeeInfo != null)
                {
                    var list = new List<EmployeeOfficeInfoViewModel>(); list.Add(editedByEmployeeInfo);
                    var userInfo_list = list.Select(item => new
                    {
                        UserEmployeeCode = editedByEmployeeInfo.EmployeeCode,
                        Username = editedByEmployeeInfo.FullName,
                        Useremail = editedByEmployeeInfo.OfficeEmail,
                        UserDesignation = editedByEmployeeInfo.DesignationName,
                        DateRange = ""
                    });
                    if (userInfo_list.Any())
                    {
                        userInfo = userInfo_list.ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userInfo;
        }

        public async Task<bool> IsEmployeeApprovedBeforeStartDate(long employeeId, string startDate, AppUser user)
        {
            bool isApproved = false;
            try
            {
                var sqlQuery = @"SELECT * FROM HR_EmployeeInformation Where EmployeeId=@EmployeeId AND CAST(ApprovedDate AS DATE) < CAST(@StartDate AS DATE)";
                var data = await _dapper.SqlQueryFirstAsync<EmployeeInformation>(user.Database, sqlQuery, new { EmployeeId = employeeId, StartDate = startDate });
                if (data != null && data.EmployeeId > 0 && data.IsApproved == true)
                {
                    isApproved = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isApproved;
        }
        public async Task<ExecutionStatus> IsEmployeeApprovedBetweenStartAndEndDate(long employeeId, string startDate, string endDate, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            bool isApproved = false;
            var _startDate = Convert.ToDateTime(startDate);
            try
            {
                var sqlQuery = @"SELECT * FROM HR_EmployeeInformation Where EmployeeId=@EmployeeId AND CAST(ApprovedDate AS DATE) BETWEEN @StartDate AND @EndDate";
                var data = await _dapper.SqlQueryFirstAsync<EmployeeInformation>(user.Database, sqlQuery, new { EmployeeId = employeeId, StartDate = startDate, EndDate = endDate });
                if (data != null && data.EmployeeId > 0 && data.IsApproved == true)
                {
                    executionStatus.Status = true;
                    executionStatus.Msg = data.ApprovedDate.Value.ToString("yyyy-MM-dd hh:mm tt");
                }
            }
            catch (Exception ex)
            {

            }
            return executionStatus;
        }
    }
}
