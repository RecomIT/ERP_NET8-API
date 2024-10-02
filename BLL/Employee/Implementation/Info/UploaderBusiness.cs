using Dapper;
using AutoMapper;
using System.Data;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Administration.Interface;
using Shared.Attendance.Filter.Shift;
using Shared.Employee.DTO.Info;
using Shared.Employee.Domain.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.DTO.Account;
using BLL.Attendance.Interface.WorkShift;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.Filter.Account;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using DAL.Repository.Employee.Interface;
using BLL.Employee.Interface.Info;
using BLL.Employee.Interface.Organizational;
using BLL.Employee.Interface.Account;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Http;
using BLL.Employee.Interface.Setup;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using DAL.Context.Employee;
using Shared.Employee.Domain.Account;
using Shared.Employee.Domain.Termination;
using Shared.Employee.Domain.Stage;
using Shared.OtherModels.DataService;
using Shared.Payroll.Helpers.SalaryProcess;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Filter.Allowance;
using BLL.Salary.Allowance.Interface;
using DAL.Context.Payroll;
using BLL.Employee.Interface.Termination;
using Shared.Employee.DTO.Organizational.InternalDesignation;

namespace BLL.Employee.Implementation.Info
{
    public class UploaderBusiness : IUploaderBusiness
    {
        private readonly IMapper _mapper;
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IModuleConfig _moduleConfig;
        private readonly ICostCenterBusiness _costCenterBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IGradeBusiness _gradeBusiness;
        private readonly IDesignationBusiness _designationBusiness;
        private readonly IDepartmentBusiness _departmentBusiness;
        private readonly ISectionBusiness _sectionBusiness;
        private readonly ISubSectionBusiness _subSectionBusiness;
        private readonly ITableConfigBusiness _tableConfigBusiness;
        private readonly IBankBusiness _bankBusiness;
        private readonly IBankBranchBusiness _bankBranchBusiness;
        private readonly IInfoBusiness _employeeInfoBusiness;
        private readonly IDetailBusiness _employeeDetailBusiness;
        private readonly IWorkShiftBusiness _workShiftBusiness;
        private readonly IOrgInitBusiness _orgInitBusiness;
        private readonly IInternalDesignationBusiness _internalDesignationBusiness;
        private readonly IEmployeeTypeBusiness _employeeTypeBusiness;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IDiscontinuedEmployeeBusiness _discontinuedEmployeeBusiness;
        private readonly IEmployeeHierarchyRepository _employeeHierarchyRepository;
        private readonly IAccountInfoBusiness _accountInfoBusines;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IDocumentRepository _documentRepository;
        private readonly IAccountInfoBusiness _accountInfoBusiness;

        public UploaderBusiness(
            IDapperData dapper,
            ISysLogger sysLogger,
            IMapper mapper,
            IOrgInitBusiness orgInitBusiness,
            ICostCenterBusiness costCenterBusiness,
            IUnitBusiness unitBusiness,
            IGradeBusiness gradeBusiness,
            IBankBusiness bankBusiness,
            IBankBranchBusiness bankBranchBusiness,
            IDesignationBusiness designationBusiness,
            IDepartmentBusiness departmentBusiness,
            ISectionBusiness sectionBusiness,
            ISubSectionBusiness subSectionBusiness,
            IInfoBusiness employeeInfoBusiness,
            IDetailBusiness employeeDetailBusiness,
            IWorkShiftBusiness workShiftBusiness,
            IInternalDesignationBusiness internalDesignationBusiness,
            IEmployeeTypeBusiness employeeTypeBusiness,
            IBranchInfoBusiness branchInfoBusiness,
            IEmployeeRepository employeeRepository,
            ITableConfigBusiness tableConfigBusiness,
            EmployeeModuleDbContext employeeModuleDbContext,
            IModuleConfig moduleConfig,
            ISalaryReviewBusiness salaryReviewBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            PayrollDbContext payrollDbContext,
            IDiscontinuedEmployeeBusiness discontinuedEmployeeBusiness,
            IEmployeeHierarchyRepository employeeHierarchyRepository,
            IAccountInfoBusiness accountInfoBusines,
            ISalaryProcessBusiness salaryProcessBusiness,
            IDocumentRepository documentRepository,
            IAccountInfoBusiness accountInfoBusiness
           )
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _orgInitBusiness = orgInitBusiness;
            _costCenterBusiness = costCenterBusiness;
            _gradeBusiness = gradeBusiness;
            _designationBusiness = designationBusiness;
            _unitBusiness = unitBusiness;
            _sectionBusiness = sectionBusiness;
            _bankBusiness = bankBusiness;
            _bankBranchBusiness = bankBranchBusiness;
            _departmentBusiness = departmentBusiness;
            _subSectionBusiness = subSectionBusiness;
            _employeeInfoBusiness = employeeInfoBusiness;
            _employeeDetailBusiness = employeeDetailBusiness;
            _workShiftBusiness = workShiftBusiness;
            _internalDesignationBusiness = internalDesignationBusiness;
            _employeeTypeBusiness = employeeTypeBusiness;
            _branchInfoBusiness = branchInfoBusiness;
            _employeeRepository = employeeRepository;
            _tableConfigBusiness = tableConfigBusiness;
            _employeeModuleDbContext = employeeModuleDbContext;
            _moduleConfig = moduleConfig;
            _salaryReviewBusiness = salaryReviewBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _payrollDbContext = payrollDbContext;
            _discontinuedEmployeeBusiness = discontinuedEmployeeBusiness;
            _employeeHierarchyRepository = employeeHierarchyRepository;
            _accountInfoBusines = accountInfoBusines;
            _salaryProcessBusiness = salaryProcessBusiness;
            _documentRepository = documentRepository;
            _accountInfoBusiness = accountInfoBusiness;
        }
        public async Task<ExecutionStatus> SaveEmployeeInfoAsync(EmployeeUploaderDTO employee, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                #region Cost-Center
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.CostCenterName))
                {
                    var costCenterItem = (await _costCenterBusiness.GetCostCentersAsync(new CostCenter_Filter()
                    {
                        CostCenterName = employee.CostCenterName
                    }, user)).FirstOrDefault();

                    if (costCenterItem == null)
                    {
                        var newCostCenter = await _costCenterBusiness.SaveCostCenterAsync(new CostCenterDTO()
                        {
                            CostCenterName = employee.CostCenterName,
                            CostCenterCode = employee.CostCenterName,
                            IsActive = true,
                        }, user);
                        employee.CostCenterId = newCostCenter.ItemId.ToString();
                    }
                    else
                    {
                        employee.CostCenterId = costCenterItem.CostCenterId.ToString();
                    }
                }
                #endregion

                #region Grade
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.GradeName))
                {
                    var gradeItem = (await _gradeBusiness.GetGradesAsync(new Grade_Filter()
                    {
                        GradeName = employee.GradeName
                    }, user)).FirstOrDefault();

                    if (gradeItem == null)
                    {
                        var newGrade = await _gradeBusiness.SaveGradeAsync(new GradeDTO()
                        {
                            GradeName = employee.GradeName,
                        }, user);
                        employee.GradeId = newGrade.ItemId.ToString();
                    }
                    else
                    {
                        employee.GradeId = gradeItem.GradeId.ToString();
                    }
                }
                #endregion

                #region Designation
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.DesignationName))
                {
                    var designationItem = (await _designationBusiness.GetDesignationsAsync(new Designation_Filter()
                    {
                        DesignationName = employee.DesignationName,
                        GradeId = employee.GradeId
                    }, user)).FirstOrDefault();

                    if (designationItem == null && !Utility.IsNullEmptyOrWhiteSpace(employee.GradeId))
                    {
                        var newDesignation = await _designationBusiness.SaveDesignationAsync(new DesignationDTO()
                        {
                            DesignationName = employee.DesignationName,
                            GradeId = Convert.ToInt32(employee.GradeId)
                        }, user);
                        employee.DesignationId = newDesignation.ItemId.ToString();
                    }
                    else
                    {
                        if (designationItem != null)
                        {
                            employee.DesignationId = designationItem.DesignationId.ToString();
                            employee.GradeId = designationItem.GradeId.ToString();
                        }
                    }
                }
                #endregion

                #region Department
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.DepartmentName))
                {
                    var departmentItem = (await _departmentBusiness.GetDepartmentsAsync(new Department_Filter()
                    {
                        DepartmentName = employee.DepartmentName
                    }, user)).FirstOrDefault();

                    if (departmentItem == null)
                    {
                        var newDepartment = await _departmentBusiness.SaveDepartmentAsync(new DepartmentDTO()
                        {
                            DepartmentName = employee.DepartmentName,
                            IsActive = true,
                        }, user);
                        employee.DepartmentId = newDepartment.ItemId.ToString();
                    }
                    else
                    {
                        employee.DepartmentId = departmentItem.DepartmentId.ToString();
                    }
                }
                #endregion

                #region Section
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.SectionName))
                {
                    var _sectionItem = (await _sectionBusiness.GetSectionsAsync(new Section_Filter()
                    {
                        SectionName = employee.SectionName,
                        DepartmentId = employee.DepartmentId,
                    }, user)).FirstOrDefault();

                    if (_sectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(employee.DepartmentId))
                    {
                        var newSection = await _sectionBusiness.SaveSectionAsync(new SectionDTO()
                        {
                            SectionName = employee.SectionName,
                            DepartmentId = Convert.ToInt32(employee.DepartmentId),
                            IsActive = true,
                        }, user);
                        employee.SectionId = newSection.ItemId.ToString();
                    }
                    else
                    {
                        employee.SectionId = _sectionItem.SectionId.ToString();
                    }
                }
                #endregion

                #region SubSection
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.SubSectionName))
                {
                    var _subSectionItem = (await _subSectionBusiness.GetSubSectionsAsync(new SubSection_Filter()
                    {
                        SubSectionName = employee.SubSectionName,
                        SectionId = employee.SectionId,
                    }, user)).FirstOrDefault();

                    if (_subSectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(employee.SectionId))
                    {
                        var newSubSection = await _subSectionBusiness.SaveSubSectionAsync(new SubSectionDTO()
                        {
                            SubSectionName = employee.SubSectionName,
                            SectionId = Convert.ToInt32(employee.SectionId),
                            IsActive = true,
                        }, user);
                        employee.SubSectionId = newSubSection.ItemId.ToString();
                    }
                    else
                    {
                        employee.SubSectionId = _subSectionItem.SubSectionId.ToString();
                    }
                }
                #endregion

                #region Shift
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.Shift))
                {
                    var _workShiftItem = (await _workShiftBusiness.GetWorkShiftsAsync(new WorkShift_Filter()
                    {
                        WorkshiftName = employee.Shift
                    }, user)).FirstOrDefault();

                    if (_workShiftItem != null)
                    {
                        employee.WorkShiftId = _workShiftItem.WorkShiftId.ToString();
                    }
                }
                else
                {
                    employee.WorkShiftId = "0";
                }
                #endregion

                #region Bank
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.BankName))
                {
                    var bankItem = (await _bankBusiness.GetBanksAsync(new Bank_Filter()
                    {
                        BankName = employee.BankName
                    }, user)).FirstOrDefault();

                    if (bankItem == null)
                    {
                        var newBank = await _bankBusiness.SaveBankAsync(new BankDTO()
                        {
                            BankName = employee.BankName,
                        }, user);
                        employee.BankId = newBank.ItemId.ToString();
                    }
                    else
                    {
                        employee.BankId = bankItem.BankId.ToString();
                    }
                }
                #endregion

                #region BankBranch
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.BankName) && !Utility.IsNullEmptyOrWhiteSpace(employee.BankBranchName))
                {
                    var bankBranchItem = (await _bankBranchBusiness.GetBankBranchesAsync(new BankBranch_Filter()
                    {
                        BankBranchName = employee.BankBranchName,
                        BankId = employee.BankId
                    }, user)).FirstOrDefault();

                    if (bankBranchItem == null)
                    {
                        var newBankBranch = await _bankBranchBusiness.SaveBankBranchAsync(new BankBranchDTO()
                        {
                            BankId = Convert.ToInt32(employee.BankId),
                            BankBranchName = employee.BankBranchName,
                            RoutingNumber = employee.RoutingNumber
                        }, user);
                        employee.BankBranchId = newBankBranch.ItemId.ToString();
                    }
                    else
                    {
                        employee.BankBranchId = bankBranchItem.BankBranchId.ToString();
                    }
                }

                #endregion

                #region Branch
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.BranchName))
                {

                    var branches = await _orgInitBusiness.BranchExtension("1", user.CompanyId, user.OrganizationId);
                    var branchObj = branches.FirstOrDefault(i => i.Text.Trim() == employee.BranchName.Trim());

                    employee.BranchId = branchObj != null ? branchObj.Value : "0";
                }
                #endregion

                #region Internal Designation
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.InternalDesignationName))
                {
                    var internalDesignationItem = (await _internalDesignationBusiness.GetInternalDesignationListAsync(new InternalDesignation_Filter()
                    {
                        InternalDesignationName = employee.InternalDesignationName
                    }, user)).FirstOrDefault();

                    if (internalDesignationItem == null)
                    {
                        var newInternalDesignation = await _internalDesignationBusiness.SaveInternalDesignationAsync(new InternalDesignationDTO()
                        {
                            InternalDesignationName = employee.InternalDesignationName,
                            IsActive = true,
                        }, user);
                        employee.InternalDesignationId = newInternalDesignation.ItemId.ToString();
                    }
                    else
                    {
                        employee.InternalDesignationId = internalDesignationItem.InternalDesignationId.ToString();
                    }
                }
                #endregion

                #region Employee Type

                if (!Utility.IsNullEmptyOrWhiteSpace(employee.EmployeeTypeName))
                {
                    var _employeeTypeItem = (await _employeeTypeBusiness.GetEmployeeTypesAsync(new EmployeeType_Filter()
                    {
                        EmployeeTypeName = employee.EmployeeTypeName
                    }, user)).FirstOrDefault();

                    if (_employeeTypeItem == null)
                    {
                        var newEmployeeType = await _employeeTypeBusiness.SaveEmployeeTypeAsync(new EmployeeTypeDTO()
                        {
                            EmployeeTypeName = employee.EmployeeTypeName,
                            Remarks = ""
                        }, user);
                        employee.EmployeeTypeId = newEmployeeType.ItemId.ToString();
                    }
                    else
                    {
                        employee.EmployeeTypeId = _employeeTypeItem.EmployeeTypeId.ToString();
                    }
                }
                #endregion

                #region Supervisor
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.SupervisorID))
                {
                    var supervisorEmployeeInfo = await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                    {
                        EmployeeCode = employee.SupervisorID
                    }, user);
                    if (supervisorEmployeeInfo.Any())
                    {
                        employee.SupervisorID = supervisorEmployeeInfo.FirstOrDefault().EmployeeId.ToString();
                    }
                }
                #endregion

                #region HOD
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.HODID))
                {
                    var hodEmployeeInfo = await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                    {
                        EmployeeCode = employee.HODID
                    }, user);
                    if (hodEmployeeInfo.Any())
                    {
                        employee.HODID = hodEmployeeInfo.FirstOrDefault().EmployeeId.ToString();
                    }
                }
                #endregion

                var employeeUploadInformation = _mapper.Map<EmployeeUploadInformation>(employee);
                employeeUploadInformation.IsResidential = Utility.IsNullEmptyOrWhiteSpace(employeeUploadInformation.IsResidential) ? "1" : employeeUploadInformation.IsResidential == "Yes" ? "True" : "False";
                //var sp_name = "sp_HR_EmployeeInformation_Insert_From_Uploader";
                //var parameters = Utility.DappperParams(employeeUploadInformation, user, new string[] { "EmployeeId" });
                //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                executionStatus = await SaveAsync(employeeUploadInformation, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeUploaderBusiness", "SaveEmployeeInfoAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateEmployeeInfoAsync(EmployeeUploaderDTO employee, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                #region Cost-Center
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.CostCenterName) || !Utility.IsNullEmptyOrWhiteSpace(employee.CostCenterCode))
                {
                    var costCenterItem = (await _costCenterBusiness.GetCostCentersAsync(new CostCenter_Filter()
                    {
                        CostCenterName = employee.CostCenterName,
                        CostCenterCode = employee.CostCenterCode
                    }, user)).FirstOrDefault();

                    if (costCenterItem == null)
                    {
                        var newCostCenter = await _costCenterBusiness.SaveCostCenterAsync(new CostCenterDTO()
                        {
                            CostCenterName = employee.CostCenterName,
                            CostCenterCode = employee.CostCenterCode,
                            IsActive = true,
                        }, user);
                        employee.CostCenterId = newCostCenter.ItemId.ToString();
                    }
                    else
                    {
                        employee.CostCenterId = costCenterItem.CostCenterId.ToString();
                    }
                }
                #endregion

                #region Grade
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.GradeName))
                {
                    var gradeItem = (await _gradeBusiness.GetGradesAsync(new Grade_Filter()
                    {
                        GradeName = employee.GradeName
                    }, user)).FirstOrDefault();

                    if (gradeItem == null)
                    {
                        var newGrade = await _gradeBusiness.SaveGradeAsync(new GradeDTO()
                        {
                            GradeName = employee.GradeName,
                        }, user);
                        employee.GradeId = newGrade.ItemId.ToString();
                    }
                    else
                    {
                        employee.GradeId = gradeItem.GradeId.ToString();
                    }
                }
                #endregion

                #region Designation
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.DesignationName))
                {
                    var designationItem = (await _designationBusiness.GetDesignationsAsync(new Designation_Filter()
                    {
                        DesignationName = employee.DesignationName,
                        GradeId = employee.GradeId
                    }, user)).FirstOrDefault();

                    if (designationItem == null && !Utility.IsNullEmptyOrWhiteSpace(employee.GradeId))
                    {
                        var newDesignation = await _designationBusiness.SaveDesignationAsync(new DesignationDTO()
                        {
                            DesignationName = employee.DesignationName,
                            GradeId = Convert.ToInt32(employee.GradeId)
                        }, user);
                        employee.DesignationId = newDesignation.ItemId.ToString();
                    }
                    else
                    {
                        if (designationItem != null)
                        {
                            employee.DesignationId = designationItem.DesignationId.ToString();
                            employee.GradeId = designationItem.GradeId.ToString();
                        }
                    }
                }
                #endregion

                #region Department
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.DepartmentName))
                {
                    var departmentItem = (await _departmentBusiness.GetDepartmentsAsync(new Department_Filter()
                    {
                        DepartmentName = employee.DepartmentName
                    }, user)).FirstOrDefault();

                    if (departmentItem == null)
                    {
                        var newDepartment = await _departmentBusiness.SaveDepartmentAsync(new DepartmentDTO()
                        {
                            DepartmentName = employee.DepartmentName,
                            IsActive = true,
                        }, user);
                        employee.DepartmentId = newDepartment.ItemId.ToString();
                    }
                    else
                    {
                        employee.DepartmentId = departmentItem.DepartmentId.ToString();
                    }
                }
                #endregion

                #region Section
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.SectionName))
                {
                    var _sectionItem = (await _sectionBusiness.GetSectionsAsync(new Section_Filter()
                    {
                        SectionName = employee.SectionName,
                        DepartmentId = employee.DepartmentId,
                    }, user)).FirstOrDefault();

                    if (_sectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(employee.DepartmentId))
                    {
                        var newSection = await _sectionBusiness.SaveSectionAsync(new SectionDTO()
                        {
                            SectionName = employee.SectionName,
                            DepartmentId = Convert.ToInt32(employee.DepartmentId),
                            IsActive = true,
                        }, user);
                        employee.SectionId = newSection.ItemId.ToString();
                    }
                    else
                    {
                        if (_sectionItem != null)
                        {
                            employee.SectionId = _sectionItem.SectionId.ToString();
                        }
                    }
                }
                #endregion

                #region SubSection
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.SubSectionName))
                {
                    var _subSectionItem = (await _subSectionBusiness.GetSubSectionsAsync(new SubSection_Filter()
                    {
                        SubSectionName = employee.SubSectionName,
                        SectionId = employee.SectionId,
                    }, user)).FirstOrDefault();

                    if (_subSectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(employee.SectionId))
                    {
                        var newSubSection = await _subSectionBusiness.SaveSubSectionAsync(new SubSectionDTO()
                        {
                            SubSectionName = employee.SubSectionName,
                            SectionId = Convert.ToInt32(employee.SectionId),
                            IsActive = true,
                        }, user);
                        employee.SubSectionId = newSubSection.ItemId.ToString();
                    }
                    else
                    {
                        if (_subSectionItem != null)
                        {
                            employee.SubSectionId = _subSectionItem.SubSectionId.ToString();
                        }
                    }
                }
                #endregion

                #region Shift
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.Shift))
                {
                    var _workShiftItem = (await _workShiftBusiness.GetWorkShiftsAsync(new WorkShift_Filter()
                    {
                        WorkshiftName = employee.Shift
                    }, user)).FirstOrDefault();

                    if (_workShiftItem != null)
                    {
                        employee.WorkShiftId = _workShiftItem.WorkShiftId.ToString();
                    }
                }
                #endregion

                #region Bank
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.BankName))
                {
                    var bankItem = (await _bankBusiness.GetBanksAsync(new Bank_Filter()
                    {
                        BankName = employee.BankName
                    }, user)).FirstOrDefault();

                    if (bankItem == null)
                    {
                        var newBank = await _bankBusiness.SaveBankAsync(new BankDTO()
                        {
                            BankName = employee.BankName,
                        }, user);
                        employee.BankId = newBank.ItemId.ToString();
                    }
                    else
                    {
                        employee.BankId = bankItem.BankId.ToString();
                    }
                }
                #endregion

                #region BankBranch
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.BankName) && !Utility.IsNullEmptyOrWhiteSpace(employee.BankBranchName))
                {
                    var bankBranchItem = (await _bankBranchBusiness.GetBankBranchesAsync(new BankBranch_Filter()
                    {
                        BankBranchName = employee.BankBranchName,
                        BankId = employee.BankId
                    }, user)).FirstOrDefault();

                    if (bankBranchItem == null)
                    {
                        var newBankBranch = await _bankBranchBusiness.SaveBankBranchAsync(new BankBranchDTO()
                        {
                            BankId = Convert.ToInt32(employee.BankId),
                            BankBranchName = employee.BankBranchName,
                            RoutingNumber = employee.RoutingNumber
                        }, user);
                        employee.BankBranchId = newBankBranch.ItemId.ToString();
                    }
                    else
                    {
                        employee.BankBranchId = bankBranchItem.BankBranchId.ToString();
                    }
                }

                #endregion

                #region Branch
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.BranchName))
                {

                    var branches = await _orgInitBusiness.BranchExtension("1", user.CompanyId, user.OrganizationId);
                    var branchObj = branches.FirstOrDefault(i => i.Text.Trim() == employee.BranchName.Trim());

                    employee.BranchId = branchObj != null ? branchObj.Value : "0";
                }
                #endregion

                #region Internal Designation
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.InternalDesignationName))
                {
                    var internalDesignationItem = (await _internalDesignationBusiness.GetInternalDesignationListAsync(new InternalDesignation_Filter()
                    {
                        InternalDesignationName = employee.InternalDesignationName
                    }, user)).FirstOrDefault();

                    if (internalDesignationItem == null)
                    {
                        var newInternalDesignation = await _internalDesignationBusiness.SaveInternalDesignationAsync(new InternalDesignationDTO()
                        {
                            InternalDesignationName = employee.InternalDesignationName,
                            IsActive = true,
                        }, user);
                        employee.InternalDesignationId = newInternalDesignation.ItemId.ToString();
                    }
                    else
                    {
                        employee.InternalDesignationId = internalDesignationItem.InternalDesignationId.ToString();
                    }
                }
                #endregion

                #region Employee Type

                if (!Utility.IsNullEmptyOrWhiteSpace(employee.EmployeeTypeName))
                {
                    var _employeeTypeItem = (await _employeeTypeBusiness.GetEmployeeTypesAsync(new EmployeeType_Filter()
                    {
                        EmployeeTypeName = employee.EmployeeTypeName
                    }, user)).FirstOrDefault();

                    if (_employeeTypeItem == null)
                    {
                        var newEmployeeType = await _employeeTypeBusiness.SaveEmployeeTypeAsync(new EmployeeTypeDTO()
                        {
                            EmployeeTypeName = employee.EmployeeTypeName,
                            Remarks = ""
                        }, user);
                        employee.EmployeeTypeId = newEmployeeType.ItemId.ToString();
                    }
                    else
                    {
                        employee.EmployeeTypeId = _employeeTypeItem.EmployeeTypeId.ToString();
                    }
                }
                #endregion

                #region Supervisor
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.SupervisorID))
                {
                    var supervisorEmployeeInfo = await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                    {
                        EmployeeCode = employee.SupervisorID
                    }, user);
                    if (supervisorEmployeeInfo.Any())
                    {
                        employee.SupervisorID = supervisorEmployeeInfo.FirstOrDefault().EmployeeId.ToString();
                    }
                }
                #endregion

                #region HOD
                if (!Utility.IsNullEmptyOrWhiteSpace(employee.HODID))
                {
                    var hodEmployeeInfo = await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                    {
                        EmployeeCode = employee.HODID
                    }, user);
                    if (hodEmployeeInfo.Any())
                    {
                        employee.HODID = hodEmployeeInfo.FirstOrDefault().EmployeeId.ToString();
                    }
                }
                #endregion

                var employeeUploadInformation = _mapper.Map<EmployeeUploadInformation>(employee);
                employeeUploadInformation.IsResidential = Utility.IsNullEmptyOrWhiteSpace(employeeUploadInformation.IsResidential) ? "1" : employeeUploadInformation.IsResidential == "Yes" ? "True" : "False";
                var sp_name = string.Format(@"sp_HR_EmployeeInformation_Update_From_Uploader");
                var parameters = Utility.DappperParams(employeeUploadInformation, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeUploaderBusiness", "UpdateEmployeeInfoAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<ExecutionStatus>> UploadEmployeeInfoAsync(List<EmployeeUploaderDTO> employees, AppUser user)
        {
            List<ExecutionStatus> executionStatusList = new List<ExecutionStatus>();
            string employeeCode = "";
            try
            {
                foreach (var item in employees)
                {
                    employeeCode = item.EmployeeCode;
                    ExecutionStatus executionStatus = new ExecutionStatus();
                    var employeeDataInDb = await _employeeInfoBusiness.GetEmployeeOfficeInfoByIdAsync(new EmployeeOfficeInfo_Filter()
                    {
                        EmployeeCode = item.EmployeeCode
                    }, user);

                    if (employeeDataInDb != null)
                    {
                        var personalInfo = await _employeeDetailBusiness.GetEmployeePersonalInfoByIdAsync(new EmployeePersonalInfoQuery()
                        {
                            EmployeeCode = employeeDataInDb.EmployeeCode,
                            EmployeeId = employeeDataInDb.EmployeeId.ToString()
                        }, user);

                        item.EmployeeId = employeeDataInDb.EmployeeId.ToString();
                        executionStatus = await UpdateEmployeeInfoAsync(item, user);
                        executionStatusList.Add(executionStatus);
                    }
                    else
                    {
                        executionStatus = await SaveEmployeeInfoAsync(item, user);
                        executionStatusList.Add(executionStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                ExecutionStatus executionStatus = new ExecutionStatus();
                executionStatus = ResponseMessage.Invalid();
                executionStatusList.Add(executionStatus);
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeUploaderBusiness", "UpdateEmployeeInfoAsync", user);
            }
            return executionStatusList;
        }
        public async Task<IEnumerable<DownloadEmployeeInfoViewModel>> DownloadEmployeesDataExcelAsync(AppUser user)
        {
            IEnumerable<DownloadEmployeeInfoViewModel> list = new List<DownloadEmployeeInfoViewModel>();
            try
            {

                var sp_name = $@"SELECT EMP.EmployeeCode,EMP.FullName, [BranchName]='',BranchId=ISNULL(EMP.BranchId,0), GRD.GradeName,DESIG.DesignationName,
	DEPT.DepartmentName, SEC.SectionName,SUBSEC.SubSectionName,EMP.StateStatus,
	[IsActive] = (CASE WHEN EMP.IsActive = 1 THEN 'Yes' ELSE 'No' END),
    CONVERT(VARCHAR, EMP.DateOfJoining, 106) DateOfJoining,EMP.JobType,[WorkShiftName]=(CASE 
	WHEN EMP.WorkShiftId > 0 THEN SFT.[Title] + ' '+ SUBSTRING(CAST(SFT.StartTime AS NVARCHAR(50)),1,5) + '-'+ SUBSTRING(CAST(SFT.EndTime AS NVARCHAR(50)),1,5) ELSE '' END), EMP.OfficeMobile, EMP.OfficeEmail, EMP.ReferenceNo, EMP.FingerID, CONVERT(VARCHAR, DET.DateOfBirth, 106) DateOfBirth, DET.Gender, DET.Religion, DET.MaritalStatus, DET.BloodGroup, DET.IsResidential, DET.PresentAddress, DET.PermanentAddress, BNK.BankName, BR.BankBranchName, BR.RoutingNumber, ACC.AccountNo
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_Grades GRD ON EMP.GradeId = GRD.GradeId
	LEFT JOIN HR_Designations DESIG ON EMP.DesignationId = DESIG.DesignationId
	LEFT JOIN HR_Departments DEPT ON EMP.DepartmentId = DEPT.DepartmentId
	LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
	LEFT JOIN HR_SubSections SUBSEC ON EMP.SubSectionId = SUBSEC.SubSectionId
	LEFT JOIN HR_WorkShifts SFT ON EMP.WorkShiftId = SFT.WorkShiftId	
	INNER JOIN HR_EmployeeDetail DET ON EMP.EmployeeId=DET.EmployeeId
	LEFT JOIN HR_EmployeeAccountInfo ACC ON EMP.EmployeeId = ACC.EmployeeId AND ACC.IsActive=1
	LEFT JOIN HR_Banks BNK ON ACC.BankId = BNK.BankId
	LEFT JOIN HR_BankBranches BR ON BNK.BankId = BR.BankBranchId AND ACC.BankBranchId = BR.BankBranchId
	WHERE 1=1
	AND (ISNULL(@EmployeeId,0) = 0  OR EMP.EmployeeId=@EmployeeId)
	AND (ISNULL(@EmployeeCode,'') = '' OR EMP.EmployeeCode LIKE '%'+@EmployeeCode+'%')
	AND (ISNULL(@BranchId,0) = 0 OR EMP.BranchId=@BranchId)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", 0);
                parameters.Add("EmployeeCode", "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", 0);
                parameters.Add("Flag", Data.Read);

                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    list = await _dapper.SqlQueryListAsync<DownloadEmployeeInfoViewModel>(user.Database, sp_name, parameters);
                    if (list != null)
                    {
                        if (list.Any())
                        {
                            var branches = await _branchInfoBusiness.GetBranchsAsync("", user);
                            if (branches.Any())
                            {
                                foreach (var item in list)
                                {
                                    var branch = branches.Where(i => i.BranchId == item.BranchId).FirstOrDefault();
                                    if (branch != null)
                                    {
                                        item.BranchName = branch.BranchName;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return list;
        }
        public async Task<ExecutionStatus> SaveAsync(EmployeeUploadInformation employees, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        bool isSuccessfullTransaction = false;
                        #region Employee Information
                        EmployeeInformation employeeInformation = null;
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                var query = $@"INSERT INTO HR_EmployeeInformation([EmployeeCode],[GlobalId],[PreviousCode],[Salutation],[FirstName],[MiddleName],[LastName],
                                [NickName],[FullName],[BranchId],[CostCenterId],[GradeId],[DesignationId],[DepartmentId],[SectionId],[SubSectionId],[DateOfJoining],
                                [DateOfConfirmation],[AppointmentDate],[ReferenceNo],[FingerID],[OfficeMobile],[OfficeEmail],[JobType],[WorkShiftId],[IsActive],[IsApproved],[StateStatus],
                                [TerminationDate],[TerminationStatus],[CreatedBy],[CreatedDate],[CompanyId],[OrganizationId],[EmployeeTypeId],[JobCategoryId]) OUTPUT INSERTED.* ";

                                query += $@"VALUES(@EmployeeCode,@GlobalID,@PreviousCode,@Salutation,@FirstName,@MiddleName,@LastName,@NickName,
                                RTRIM((ISNULL(@FirstName,'')+' '+ISNULL(@MiddleName,'')+' '+ISNULL(@LastName,''))),@BranchId,@CostCenterId,@GradeId,@DesignationId,@DepartmentId,@SectionId,@SubSectionId,@JoiningDate,@ConfirmationDate,
                                @AppointmentDate,@ReferenceId,@FingureId,@OfficeMobile,@OfficeEmail,@JobType,@WorkShiftId,
                                (CASE WHEN @JoiningDate IS NULL OR @JoiningDate ='' THEN NULL
                                WHEN CAST(@JoiningDate AS DATE) <= CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END), 0,'Pending',@LastWorkingDate,
                                (CASE WHEN @LastWorkingDate IS NOT NULL OR @LastWorkingDate <> '' THEN 'Approved' ELSE NULL END),@UserId,GETDATE(),@CompanyId,@OrganizationId,@EmployeeTypeId,@JobCategoryId)";

                                var parameters = new
                                {
                                    employees.EmployeeCode,
                                    employees.GlobalID,
                                    employees.PreviousCode,
                                    employees.Salutation,
                                    employees.FirstName,
                                    employees.MiddleName,
                                    employees.LastName,
                                    employees.NickName,
                                    FullName = employees.FirstName.Default() + " " + employees.MiddleName.Default() + " " + employees.LastName,
                                    employees.BranchId,
                                    employees.CostCenterId,
                                    employees.GradeId,
                                    employees.DesignationId,
                                    employees.DepartmentId,
                                    employees.SectionId,
                                    employees.SubSectionId,
                                    employees.EmployeeTypeId,
                                    employees.JobCategoryId,
                                    employees.JoiningDate,
                                    employees.ConfirmationDate,
                                    employees.AppointmentDate,
                                    employees.ReferenceId,
                                    employees.FingureId,
                                    employees.OfficeMobile,
                                    employees.OfficeEmail,
                                    employees.JobType,
                                    employees.WorkShiftId,
                                    employees.LastWorkingDate,
                                    UserId = user.ActionUserId,
                                    user.CompanyId,
                                    user.OrganizationId
                                };

                                employeeInformation = await connection.QueryFirstOrDefaultAsync<EmployeeInformation>(query.Trim(), parameters, transaction);
                                if (employeeInformation != null && employeeInformation.EmployeeId > 0)
                                {
                                    isSuccessfullTransaction = true;
                                    transaction.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                isSuccessfullTransaction = false;
                                await _sysLogger.SaveHRMSException(ex, user.Database, "Employee Module UploaderBusiness -transaction-1", "SaveAsync", user);
                            }
                        }
                        #endregion

                        #region Employee Detail
                        if (isSuccessfullTransaction == true && employeeInformation != null && employeeInformation.EmployeeId > 0)
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {

                                    var query = $@"INSERT INTO HR_EmployeeDetail([EmployeeId],[FatherName],[MotherName],[SpouseName],[PersonalMobileNo],[PersonalEmailAddress],[DateOfBirth],[Gender],[MaritalStatus],[Religion],[BloodGroup],[IsResidential],[PresentAddress],[PresentAddressCity],[PresentAddressContactNo],[PresentAddressZipCode],[PermanentAddress],[PermanentAddressDistrict],[PermanentAddressUpazila],[PermanentAddressContactNumber],[PermanentAddressZipCode],[EmergencyContactPerson],[RelationWithEmergencyContactPerson],[EmergencyContactNo],[EmergencyContactAddress],[EmergencyContactEmailAddress],[EmergencyContactPerson2],[RelationWithEmergencyContactPerson2],[EmergencyContactNo2],[EmergencyContactAddress2],[EmergencyContactEmailAddress2],[CreatedBy],[CreatedDate],[CompanyId],[OrganizationId],[NumberOfChild])";

                                    query += $@"VALUES (@EmployeeId,@FatherName,@MotherName,@SpouseName,@PersonalMobileNumber,@PersonalEmail,@DateOfBirth,@Gender,@MaritalStatus,
        @Religion,@BloodGroup,@IsResidential,@PresentAddress,@PresentAddressCity,@PresentAddressContactNo,@PresentAddressZipCode,@PermanentAddress,@PermanentAddressDistrict,
            @PermanentAddressUpazila,@PermanentAddressContactNumber,@PresentAddressZipCode,@EmergencyContactPerson1,@RelationWithEmergencyContactPerson1,@EmergencyContactNoPerson1,
			            @EmergencyContactAddressPerson1,@EmergencyContactEmailAddressPerson1,@EmergencyContactPerson2,@RelationWithEmergencyContactPerson2,@EmergencyContactNoPerson2,
			            @EmergencyContactAddressPerson2,@EmergencyContactEmailAddressPerson2,@UserId,GETDATE(),@CompanyId,@OrganizationId,@NumberOfChild)";

                                    var parameters = new
                                    {
                                        employeeInformation.EmployeeId,
                                        employees.FatherName,
                                        employees.MotherName,
                                        employees.SpouseName,
                                        employees.PersonalMobileNumber,
                                        employees.PersonalEmail,
                                        employees.DateOfBirth,
                                        employees.Gender,
                                        employees.MaritalStatus,
                                        employees.Religion,
                                        employees.BloodGroup,
                                        employees.IsResidential,
                                        employees.PresentAddress,
                                        employees.PresentAddressCity,
                                        employees.PresentAddressContactNo,
                                        employees.PresentAddressZipCode,
                                        employees.PermanentAddress,
                                        employees.PermanentAddressDistrict,
                                        employees.PermanentAddressUpazila,
                                        employees.PermanentAddressContactNumber,
                                        employees.PermanentAddressZipCode,
                                        employees.EmergencyContactPerson1,
                                        employees.RelationWithEmergencyContactPerson1,
                                        employees.EmergencyContactNoPerson1,
                                        employees.EmergencyContactAddressPerson1,
                                        employees.EmergencyContactEmailAddressPerson1,
                                        employees.EmergencyContactPerson2,
                                        employees.RelationWithEmergencyContactPerson2,
                                        employees.EmergencyContactNoPerson2,
                                        employees.EmergencyContactAddressPerson2,
                                        employees.EmergencyContactEmailAddressPerson2,
                                        UserId = user.ActionUserId,
                                        user.CompanyId,
                                        user.OrganizationId,
                                        employees.NumberOfChild
                                    };

                                    int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                    if (rawAffected > 0)
                                    {
                                        isSuccessfullTransaction = true;
                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        isSuccessfullTransaction = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    isSuccessfullTransaction = false;
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "Employee Module UploaderBusiness-transaction2", "SaveAsync", user);
                                }
                            }
                        }
                        #endregion

                        #region Discontinued Employee
                        if (isSuccessfullTransaction == true)
                        {
                            if (!employees.LastWorkingDate.IsNullEmptyOrWhiteSpace())
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    isSuccessfullTransaction = false;
                                    try
                                    {
                                        if (!employees.LastWorkingDate.IsNullEmptyOrWhiteSpace())
                                        {
                                            var query = $@"INSERT INTO HR_DiscontinuedEmployee([EmployeeId],[LastWorkingDate],[CalculateFestivalBonusTaxProratedBasis],[CalculateProjectionTaxProratedBasis],[StateStatus],[CompanyId],[OrganizationId],[CreatedBy],[CreatedDate],ApprovedBy,[ApprovedDate])";
                                            query += $@"VALUES(@EmployeeId,@LastWorkingDate,0,0,'Approved',@CompanyId,@OrganizationId,@UserId,GETDATE(),@UserId,GETDATE())";

                                            var parameters = new
                                            {
                                                employeeInformation.EmployeeId,
                                                LastWorkingDate = Convert.ToDateTime(employees.LastWorkingDate).ToString("yyyy-MM-dd"),
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            };

                                            int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                            if (rawAffected > 0)
                                            {
                                                isSuccessfullTransaction = true;
                                                transaction.Commit();
                                            }
                                        }
                                        else
                                        {
                                            isSuccessfullTransaction = true;
                                            transaction.Commit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        isSuccessfullTransaction = false;
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "Employee Module UploaderBusiness-transaction3", "SaveAsync", user);
                                    }
                                }
                            }

                        }
                        #endregion

                        #region Employee Account Info
                        if (isSuccessfullTransaction == true && employeeInformation != null && employeeInformation.EmployeeId > 0)
                        {
                            if (employees.BankId.IsStringNumber() && employees.BankBranchId.IsStringNumber()
                                        && employees.AccountNumber.IsNullEmptyOrWhiteSpace() == false)
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        isSuccessfullTransaction = false;
                                        var query = $@"INSERT INTO HR_EmployeeAccountInfo([EmployeeId],[DepartmentId],[GradeId],[DesignationId],[Month],[Year],[PaymentMode],
                                    [BankId],[BankBranchId],[AccountNo],[EffectiveFrom],[IsActive],[IsApproved],[StateStatus],[CreatedBy],[CreatedDate],[BranchId],[CompanyId],[OrganizationId])";

                                        query += $@"Values(@EmployeeId,@DepartmentId,@GradeId,@DesignationId,
                        				MONTH(CAST(@AccountActivationDate AS DATE)),YEAR(CAST(@AccountActivationDate AS DATE)),'Bank',@BankId,@BankBranchId,@AccountNumber,
                                        CAST(@AccountActivationDate AS DATE),1,1,'Approved',@UserId,GETDATE(),@BranchId,@CompanyId,@OrganizationId);";

                                        var parameters = new
                                        {
                                            employeeInformation.EmployeeId,
                                            employeeInformation.DepartmentId,
                                            employeeInformation.GradeId,
                                            employeeInformation.DesignationId,
                                            employees.AccountActivationDate,
                                            employees.BankId,
                                            employees.BankBranchId,
                                            employees.AccountNumber,
                                            UserId = user.ActionUserId,
                                            employees.BranchId,
                                            user.CompanyId,
                                            user.OrganizationId
                                        };

                                        int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                        if (rawAffected > 0)
                                        {
                                            isSuccessfullTransaction = true;
                                            transaction.Commit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        isSuccessfullTransaction = false;
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "Employee Module UploaderBusiness-transaction4", "SaveAsync", user);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Document
                        if (isSuccessfullTransaction == true && employeeInformation != null && employeeInformation.EmployeeId > 0)
                        {
                            if (employees.NID.IsNullEmptyOrWhiteSpace() == false
                                ||
                                employees.DrivingLicense.IsNullEmptyOrWhiteSpace() == false
                                ||
                                employees.TIN.IsNullEmptyOrWhiteSpace() == false
                                ||
                                employees.Passport.IsNullEmptyOrWhiteSpace() == false)
                            {

                                using (var transaction = connection.BeginTransaction())
                                {
                                    isSuccessfullTransaction = false;
                                    try
                                    {
                                        if (employees.NID.IsNullEmptyOrWhiteSpace() == false)
                                        {
                                            var query = $@"INSERT INTO [dbo].[HR_EmployeeDocument]
			                               ([EmployeeId],[DocumentName],[DocumentNumber],[CreatedBy],[CreatedDate],[OrganizationId],[CompanyId],[BranchId])";

                                            query += $@"VALUES(@EmployeeId,@DocumentName,@DocumentNumber,@UserId,GETDATE(),@OrganizationId,@CompanyId,@BranchId)";

                                            var parameters = new
                                            {
                                                employeeInformation.EmployeeId,
                                                DocumentName = "NID",
                                                DocumentNumber = employees.NID,
                                                UserId = user.ActionUserId,
                                                employeeInformation.BranchId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            };

                                            int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                            if (rawAffected > 0)
                                            {
                                                isSuccessfullTransaction = true;
                                            }
                                        }
                                        if (employees.Passport.IsNullEmptyOrWhiteSpace() == false)
                                        {
                                            var query = $@"INSERT INTO [dbo].[HR_EmployeeDocument]
			                               ([EmployeeId],[DocumentName],[DocumentNumber],[CreatedBy],[CreatedDate],[OrganizationId],[CompanyId],[BranchId])";

                                            query += $@"VALUES(@EmployeeId,@DocumentName,@DocumentNumber,@UserId,GETDATE(),@OrganizationId,@CompanyId,@BranchId)";

                                            var parameters = new
                                            {
                                                employeeInformation.EmployeeId,
                                                DocumentName = "Passport",
                                                DocumentNumber = employees.Passport,
                                                UserId = user.ActionUserId,
                                                employeeInformation.BranchId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            };

                                            int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                            if (rawAffected > 0)
                                            {
                                                isSuccessfullTransaction = true;
                                            }
                                        }
                                        if (employees.DrivingLicense.IsNullEmptyOrWhiteSpace() == false)
                                        {
                                            var query = $@"INSERT INTO [dbo].[HR_EmployeeDocument]
			                               ([EmployeeId],[DocumentName],[DocumentNumber],[CreatedBy],[CreatedDate],[OrganizationId],[CompanyId],[BranchId])";

                                            query += $@"VALUES(@EmployeeId,@DocumentName,@DocumentNumber,@UserId,GETDATE(),@OrganizationId,@CompanyId,@BranchId)";
                                            var parameters = new
                                            {
                                                employeeInformation.EmployeeId,
                                                DocumentName = "Driving License",
                                                DocumentNumber = employees.DrivingLicense,
                                                UserId = user.ActionUserId,
                                                employeeInformation.BranchId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            };

                                            int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                            if (rawAffected > 0)
                                            {
                                                isSuccessfullTransaction = true;
                                            }
                                        }
                                        if (employees.TIN.IsNullEmptyOrWhiteSpace() == false)
                                        {
                                            var query = $@"INSERT INTO [dbo].[HR_EmployeeDocument]
			                               ([EmployeeId],[DocumentName],[DocumentNumber],[CreatedBy],[CreatedDate],[OrganizationId],[CompanyId],[BranchId])";

                                            query += $@"VALUES(@EmployeeId,@DocumentName,@DocumentNumber,@UserId,GETDATE(),@OrganizationId,@CompanyId,@BranchId)";

                                            var parameters = new
                                            {
                                                employeeInformation.EmployeeId,
                                                DocumentName = "TIN",
                                                DocumentNumber = employees.TIN,
                                                UserId = user.ActionUserId,
                                                employeeInformation.BranchId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            };

                                            int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);
                                            if (rawAffected > 0)
                                            {
                                                isSuccessfullTransaction = true;
                                            }
                                        }
                                        if (isSuccessfullTransaction == true)
                                        {
                                            transaction.Commit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        isSuccessfullTransaction = false;
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "Employee Module UploaderBusiness-transaction5", "SaveAsync", user);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Employee Hierarchy
                        if (isSuccessfullTransaction == true && employeeInformation != null && employeeInformation.EmployeeId > 0)
                        {
                            if (employees.SupervisorID.IsStringNumber() || employees.HODID.IsStringNumber())
                            {
                                long supervisorId = Utility.TryParseLong(employees.SupervisorID);
                                string supervisorName = "";
                                if (supervisorId > 0)
                                {
                                    var supervisorInfo = await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                                    {
                                        EmployeeId = supervisorId.ToString()
                                    }, user);
                                    if (supervisorInfo.Any())
                                    {
                                        supervisorName = supervisorInfo.FirstOrDefault().EmployeeName;
                                    }
                                }
                                long hodId = Utility.TryParseLong(employees.HODID);
                                string hodName = "";
                                if (hodId > 0)
                                {
                                    var hodInfo = await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                                    {
                                        EmployeeId = hodId.ToString()
                                    }, user);
                                    if (hodInfo.Any())
                                    {
                                        hodName = hodInfo.FirstOrDefault().EmployeeName;
                                    }
                                }

                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        EmployeeHierarchy employeeHierarchy = new EmployeeHierarchy();
                                        employeeHierarchy.EmployeeId = employeeInformation.EmployeeId;
                                        employeeHierarchy.BranchId = employeeInformation.BranchId;
                                        employeeHierarchy.SupervisorId = supervisorId;
                                        employeeHierarchy.SupervisorName = supervisorName;
                                        employeeHierarchy.HeadOfDepartmentId = hodId;
                                        employeeHierarchy.HeadOfDepartmentName = hodName;
                                        employeeHierarchy.ActivationDate = employeeInformation.DateOfJoining;
                                        employeeHierarchy.IsActive = true;
                                        employeeHierarchy.CreatedBy = user.ActionUserId;
                                        employeeHierarchy.CreatedDate = DateTime.Now;
                                        employeeHierarchy.CompanyId = user.CompanyId;
                                        employeeHierarchy.OrganizationId = user.OrganizationId;

                                        var parameters = DapperParam.GetKeyValuePairsDynamic(employeeHierarchy, true);
                                        parameters.Remove("Id");
                                        parameters.Remove("EmployeeInformation");
                                        string query = Utility.GenerateInsertQuery(tableName: "HR_EmployeeHierarchy", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");

                                        var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                        if (rawAffected > 0)
                                        {
                                            isSuccessfullTransaction = true;
                                            transaction.Commit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        isSuccessfullTransaction = false;
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "Employee Module UploaderBusiness-transaction5", "SaveAsync", user);
                                    }
                                }
                            }
                        }
                        #endregion

                        if (isSuccessfullTransaction == true)
                        {
                            executionStatus.Status = true;
                            executionStatus.Msg = "Data has been save by file uploader";
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SaveAsync", "", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateAsync(EmployeeUploadInformation employees, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        var employeeInfoInDb = await _employeeRepository.GetByIdAsync(Utility.TryParseLong(employees.EmployeeId), user);
                    };
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UpdateAsync", "UploaderBusiness", user);
            }
            return executionStatus;
        }
        public async Task<ReadInfoFromExcel> GetEmployeeInfoFromExcelAsync(IFormFile file, AppUser user)
        {
            ReadInfoFromExcel data = new ReadInfoFromExcel();
            Dictionary<string, int> excel_Columns = new Dictionary<string, int>();
            List<KeyValue> new_employee_id_list = new List<KeyValue>();
            List<KeyValue> new_employee_office_email_list = new List<KeyValue>();
            try
            {
                var columns = (await _tableConfigBusiness.GetColumnsAsync("Employee Uploader", "Upload", user)).ToList();
                if (columns.Any())
                {
                    var cellNo = 150;
                    var stream = file.OpenReadStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;

                        #region Find excel column names
                        excel_Columns.Add("#SL", 0);
                        for (var row = 1; row <= 1; row++)
                        {
                            for (var cell = 1; cell <= cellNo; cell++)
                            {
                                var columnName = worksheet.Cells[row, cell].Value?.ToString();
                                if (!Utility.IsNullEmptyOrWhiteSpace(columnName))
                                {
                                    excel_Columns.Add(columnName ?? "", cell);
                                }
                            }
                        }

                        string[] cols = columns.Select(i => i.Column).ToArray();
                        string[] labels = columns.Select(i => i.Label).ToArray();
                        var requiredColumns = columns.Where(i => i.IsMandatory).Select(i => i.Column).ToList();
                        List<string> column_list = new List<string>();

                        #endregion

                        #region Read excel data
                        for (var row = 2; row <= rowCount; row++) // Excel rows
                        {
                            ExcelInfoCollection employee = new ExcelInfoCollection();
                            foreach (var col in cols) // table columns from db
                            {
                                var columnName = "";
                                var displayName = "";
                                var columnInfo = columns.FirstOrDefault(item => item.Column == (col ?? ""));


                                if (columnInfo != null && (col ?? "").IsNullEmptyOrWhiteSpace() == false)
                                {
                                    if (excel_Columns.ContainsKey(col ?? ""))
                                    {
                                        columnName = columnInfo.Column;
                                        displayName = columnInfo.Label;
                                    }
                                    else if (excel_Columns.ContainsKey(columnInfo.Label ?? ""))
                                    {
                                        columnName = columnInfo.Column;
                                        displayName = columnInfo.Label;
                                    }
                                }


                                if (Utility.IsNullEmptyOrWhiteSpace(col ?? "") == false && (excel_Columns.ContainsKey(col ?? "") || excel_Columns.ContainsKey(displayName ?? ""))) // columns from excel
                                {
                                    if (row == 2)
                                    {
                                        column_list.Add(displayName.IsNullEmptyOrWhiteSpace() ? col ?? "" : displayName ?? "");
                                    }
                                    ExcelInfo excelEmployeeInfo = new ExcelInfo();

                                    var key = excel_Columns.First(item => item.Key == (displayName.IsNullEmptyOrWhiteSpace() ? col ?? "" : displayName ?? ""));
                                    var value = worksheet.Cells[row, key.Value].Value?.ToString();
                                    excelEmployeeInfo.Column = columnName;
                                    excelEmployeeInfo.DisplayName = displayName;
                                    excelEmployeeInfo.Value = (value ?? "").Trim();

                                    #region Find employee identity.
                                    if ((columnName ?? "").ToLower() == "employee id") // check existence of the employee.
                                    {
                                        var employeeInDb = await _employeeInfoBusiness.GetEmployeeInformationByCode(value ?? "", user);
                                        if (employeeInDb == null)
                                        {
                                            employee.IsNew = true;
                                            employee.Code = (value ?? "").Trim();
                                            if (value.IsNullEmptyOrWhiteSpace() == false)
                                            {
                                                var employeeExist = new_employee_id_list.FirstOrDefault(i => i.Value == employee.Code);
                                                if (employeeExist != null)
                                                {
                                                    excelEmployeeInfo.IsValid = false;
                                                    excelEmployeeInfo.ErrorMsg = $"Id is already exist at row {employeeExist.Key}";
                                                    employee.IsValid = false;
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                                else
                                                {
                                                    new_employee_id_list.Add(new KeyValue((row - 1).ToString(), employee.Code));
                                                }
                                            }
                                        }
                                        else if (employeeInDb.EmployeeId > 0)
                                        {
                                            employee.Id = employeeInDb.EmployeeId;
                                            employee.Code = employeeInDb.EmployeeCode;
                                            employee.IsNew = false;
                                        }
                                    }
                                    #endregion

                                    if (columnInfo != null)
                                    {
                                        excelEmployeeInfo.Group = columnInfo.Group;
                                        if (Utility.IsNullEmptyOrWhiteSpace(columnInfo.DefaultValue) == false)
                                        {
                                            excelEmployeeInfo.HasDefualtValue = true;
                                        }
                                        if (columnInfo.DataType != null && columnInfo.DataType.ToLower() == "date" && Utility.IsNullEmptyOrWhiteSpace(value) == false)
                                        {
                                            if (excelEmployeeInfo.Value.IsStringNumber() && excelEmployeeInfo.Value.IsNullEmptyOrWhiteSpace() == false)
                                            {
                                                try
                                                {
                                                    var val = Convert.ToDouble(excelEmployeeInfo.Value.RemoveWhitespace());
                                                    excelEmployeeInfo.Value = DateTime.FromOADate(val).ToString("yyyy-MM-dd");
                                                }
                                                catch (Exception ex)
                                                {
                                                    excelEmployeeInfo.ErrorMsg = ex.Message;
                                                    //excelEmployeeInfo.Value = null;
                                                    excelEmployeeInfo.IsValid = false;
                                                    excelEmployeeInfo.ErrorMsg = $"Invalid {columnInfo.Column}";
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                            }
                                            else
                                            {
                                                var date = new DateTime();
                                                if (DateTime.TryParse(excelEmployeeInfo.Value.RemoveWhitespace(), out date))
                                                {
                                                    excelEmployeeInfo.Value = date.ToString("yyyy-MM-dd");
                                                }
                                                else
                                                {
                                                    // excelEmployeeInfo.Value = null;
                                                    excelEmployeeInfo.ErrorMsg = $"Invalid {columnInfo.Column}";
                                                    excelEmployeeInfo.IsValid = false;
                                                    employee.IsValid = false;
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                            }
                                        }

                                        if (!Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value)
                                            && (columnInfo.DataType ?? "").ToLower() == "string"
                                            && excelEmployeeInfo.IsValid)
                                        {
                                            int maxLength = Utility.TryParseInt(columnInfo.MaxLength ?? "");
                                            if (maxLength > 0)
                                            {
                                                if ((excelEmployeeInfo.Value ?? "").Length > maxLength)
                                                {
                                                    excelEmployeeInfo.ErrorMsg = $"{columnInfo.Column} max length is {maxLength.ToString()}";
                                                    excelEmployeeInfo.IsValid = false;
                                                    employee.IsValid = false;
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                            }
                                        }

                                        if (!Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value)
                                            && (columnInfo.DataType ?? "").ToLower() == "email"
                                            && excelEmployeeInfo.IsValid)
                                        {
                                            int maxLength = Utility.TryParseInt(columnInfo.MaxLength ?? "");
                                            if (maxLength > 0)
                                            {
                                                if ((excelEmployeeInfo.Value ?? "").Length > maxLength)
                                                {
                                                    excelEmployeeInfo.ErrorMsg = $"{columnInfo.Column} max length is {maxLength.ToString()}";
                                                    excelEmployeeInfo.IsValid = false;
                                                    employee.IsValid = false;
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                            }

                                            if (!RegexValidator.IsValidEmail(excelEmployeeInfo.Value))
                                            {
                                                excelEmployeeInfo.ErrorMsg = $"{columnInfo.Column} is invalid";
                                                excelEmployeeInfo.IsValid = false;
                                                employee.IsValid = false;
                                                employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                            }
                                            //else
                                            //{

                                            //    // Office Email Checking ...
                                            //    if ((col ?? "").ToLower() == "office email")
                                            //    {
                                            //        var isEmailExist = false;
                                            //        if (employee.Id <= 0)
                                            //        {
                                            //            isEmailExist = (await _employeeInfoBusiness.IsOfficeEmailAvailableAsync((excelEmployeeInfo.Value ?? ""), user));
                                            //        }
                                            //        else
                                            //        {
                                            //            isEmailExist = (await _employeeInfoBusiness.IsOfficeEmailInEditAvailableAsync(employee.Id, (excelEmployeeInfo.Value ?? ""), user));
                                            //        }
                                            //        if (isEmailExist == true)
                                            //        {
                                            //            excelEmployeeInfo.ErrorMsg = $"This email is already exist";
                                            //            excelEmployeeInfo.IsValid = false;
                                            //            employee.IsValid = false;
                                            //            employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                            //        }
                                            //        else
                                            //        {
                                            //            var emailExist = new_employee_office_email_list.FirstOrDefault(i => i.Value == excelEmployeeInfo.Value);
                                            //            if (emailExist != null)
                                            //            {
                                            //                excelEmployeeInfo.IsValid = false;
                                            //                excelEmployeeInfo.ErrorMsg = $"This email is already exist at row {emailExist.Key}";
                                            //                employee.IsValid = false;
                                            //                employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                            //            }
                                            //            else
                                            //            {
                                            //                new_employee_office_email_list.Add(new KeyValue((row - 1).ToString(), (excelEmployeeInfo.Value ?? "")));
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                        }

                                        if (!Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value) && columnInfo.IsConstant)
                                        {
                                            var val = excelEmployeeInfo.Value;
                                            if (!ConstantChecker.IsValid(excelEmployeeInfo.Column, excelEmployeeInfo.Value))
                                            {
                                                excelEmployeeInfo.IsValid = false;
                                                excelEmployeeInfo.ErrorMsg = $"{val} invalid type of {columnInfo.Column} ";
                                                excelEmployeeInfo.Value = val;
                                                employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                            }
                                        }

                                        if ((columnInfo.DataType ?? "").ToLower() == "bool")
                                        {
                                            if (!Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value))
                                            {
                                                excelEmployeeInfo.Value = BoolExtension.ParseBool(excelEmployeeInfo.Value);
                                                if (Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value))
                                                {
                                                    excelEmployeeInfo.Value = (columnInfo.DefaultValue ?? "");
                                                }
                                                else
                                                {
                                                    excelEmployeeInfo.Value = excelEmployeeInfo.Value.ToProperCase();
                                                }
                                            }
                                            else
                                            {
                                                excelEmployeeInfo.Value = (columnInfo.DefaultValue ?? "");
                                            }
                                        }

                                        if (columnInfo.IsMandatory == true && excelEmployeeInfo.IsValid)
                                        {
                                            if (employee.Id == 0)
                                            {
                                                if (Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value))
                                                {
                                                    excelEmployeeInfo.ErrorMsg = $"{columnInfo.Column} is required";
                                                    excelEmployeeInfo.IsValid = false;
                                                    employee.IsValid = false;
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                            }
                                        }

                                        if (columnInfo.ParentId > 0 && Utility.IsNullEmptyOrWhiteSpace(excelEmployeeInfo.Value) == false)
                                        {
                                            var parentColumn = columns.FirstOrDefault(item => item.Id == columnInfo.ParentId);
                                            if (parentColumn != null)
                                            {
                                                var parentValue = employee.ExcelInfos.FirstOrDefault(item => item.Column == parentColumn.Column);
                                                if (parentValue == null || Utility.IsNullEmptyOrWhiteSpace(parentValue.Value))
                                                {
                                                    excelEmployeeInfo.IsValid = false;
                                                    excelEmployeeInfo.ErrorMsg = $"{columnInfo.Column} depands on {parentColumn?.Column} which is missing";
                                                    employee.IsValid = false;

                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                }
                                            }
                                        }

                                        if (columnInfo.IsUnique && excelEmployeeInfo.Value.IsNullEmptyOrWhiteSpace() == false)
                                        {
                                            if (await IsDuplicate(employee.Id, columnInfo.Column, (excelEmployeeInfo.Value ?? ""), data.Collections, user))
                                            {
                                                excelEmployeeInfo.ErrorMsg = $"{columnName} is already exist";
                                                excelEmployeeInfo.IsValid = false;
                                                employee.IsValid = false;
                                                employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                            }
                                        }
                                    }
                                    employee.ExcelInfos.Add(excelEmployeeInfo);
                                }
                                else
                                {
                                    if (requiredColumns.Contains(col ?? "") && columnInfo != null)
                                    {
                                        ExcelInfo excelEmployeeInfo = new ExcelInfo();
                                        excelEmployeeInfo.Column = columnName.IsNullEmptyOrWhiteSpace() ? col : columnName;
                                        excelEmployeeInfo.DisplayName = displayName.IsNullEmptyOrWhiteSpace() ? col : displayName;
                                        excelEmployeeInfo.Value = columnInfo.DefaultValue;

                                        if (columnInfo != null)
                                        {
                                            excelEmployeeInfo.IsValid = !columnInfo.DefaultValue.IsNullEmptyOrWhiteSpace();
                                            if (columnInfo.IsMandatory == true && excelEmployeeInfo.IsValid == false) //
                                            {
                                                if (employee.Id == 0)
                                                {
                                                    excelEmployeeInfo.ErrorMsg = $"{columnInfo.Column} is required";
                                                    excelEmployeeInfo.IsValid = false;
                                                    excelEmployeeInfo.Group = columnInfo.Group;
                                                    employee.IsValid = false;
                                                    employee.ErrorMsg = employee.ErrorMsg + "," + excelEmployeeInfo.ErrorMsg;
                                                    employee.ExcelInfos.Add(excelEmployeeInfo);
                                                    if (row == 2)
                                                    {
                                                        column_list.Add(col ?? "");
                                                    }
                                                }

                                                else
                                                {
                                                    
                                                    if(employee.Id > 0)
                                                    {
                                                        if(columnInfo.DefaultValue.IsNullEmptyOrWhiteSpace() == false)
                                                        {
                                                            excelEmployeeInfo.Value = "";
                                                        }
                                                    }
                                                    excelEmployeeInfo.IsValid = true;
                                                    employee.ExcelInfos.Add(excelEmployeeInfo);
                                                    if(column_list.Contains(col) == false)
                                                    {
                                                        column_list.Add(col ?? "");
                                                    }
                                                    
                                                }
                                            }
                                            else
                                            {
                                                if (employee.Id > 0)
                                                {
                                                    if (columnInfo.DefaultValue.IsNullEmptyOrWhiteSpace() == false)
                                                    {
                                                        excelEmployeeInfo.Value = "";
                                                    }
                                                }
                                                employee.ExcelInfos.Add(excelEmployeeInfo);
                                                if (row == 2)
                                                {
                                                    if (column_list.Contains(col) == false)
                                                    {
                                                        column_list.Add(col ?? "");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                   
                                }
                            }

                            var values = employee.ExcelInfos.Where(i => Utility.IsNullEmptyOrWhiteSpace(i.Value) == false && i.HasDefualtValue == false).ToList();

                            if (values.Any())
                            {
                                var employeeCodeInCollection = employee.ExcelInfos.FirstOrDefault(item => item.Column == "Employee ID");
                                if (employeeCodeInCollection != null)
                                {
                                    if (employee.ExcelInfos.Any())
                                    {
                                        data.Collections.Add(employee);
                                    }
                                }
                            }
                        }
                        #endregion

                        column_list.Insert(0, "#SL");
                        data.Columns = column_list;
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploaderBusiness", "GetEmployeeInfoFromExcelAsync", user);
            }
            return data;
        }
        public async Task<List<ExecutionStatus>> SaveExcelData(List<ExcelInfoCollection> models, AppUser user)
        {
            List<ExecutionStatus> statuses = new List<ExecutionStatus>();
            try
            {
                foreach (var model in models)
                {
                    if (model.Id > 0)
                    {
                        // Update
                        var status = await UpdateAsync(model, user);
                        statuses.Add(status);
                    }
                    else
                    {
                        // Save
                        var status = await InsertAsync(model, user);
                        statuses.Add(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return statuses;
        }
        public async Task<ExecutionStatus> InsertAsync(ExcelInfoCollection model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            string line = "";
            try
            {
                CostCenterDTO costCenter = null;
                EmployeeInformation information = null;
                DiscontinuedEmployee discontinuedEmployee = null;
                EmployeeDetail detail = null;
                EmployeeHierarchy hierarchy = null;
                List<EmployeeDocument> employeeDocuments = null;
                EmployeeAccountInfo accountInfo = null;
                EmployeePFActivation employeePFActivation = null;
                SalaryReviewFromEmployeeUploader salaryReview = null;
                // Salary Review

                var column_list = (await _tableConfigBusiness.GetColumnsAsync("Employee Uploader", "Upload", user)).ToList();

                #region Cost Center
                var costCenterColumns = model.ExcelInfos.Where(i => i.Group == "Cost Center").ToList();
                if (costCenterColumns.Any())
                {
                    costCenter = new CostCenterDTO();
                    foreach (var item in costCenterColumns)
                    {
                        if (item.Column == "Cost Center ID")
                        {
                            costCenter.CostCenterCode = item.Column;
                        }
                        else if (item.Column == "Cost Center")
                        {
                            costCenter.CostCenterName = item.Column;
                        }
                    }
                }
                #endregion

                line = "Start reading of Employee Information";
                #region Employee Information
                var employeeInfoColumns = model.ExcelInfos.Where(i => i.Group == "Employee Information").ToList(); //

                if (employeeInfoColumns.Any() && employeeInfoColumns != null)
                {
                    information = new EmployeeInformation();
                    foreach (var item in employeeInfoColumns)
                    {
                        if (item.Value.IsNullEmptyOrWhiteSpace() == false)
                        {
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Employee ID")
                                {
                                    information.EmployeeCode = item.Value;
                                }
                                else if (item.Column == "Global ID")
                                {
                                    information.GlobalID = item.Value;
                                }
                                else if (item.Column == "Previous ID")
                                {
                                    information.PreviousCode = item.Value;
                                }
                                else if (item.Column == "Mr./Ms./Mrs.")
                                {
                                    information.Salutation = item.Value;
                                }
                                else if (item.Column == "First Name")
                                {
                                    information.FirstName = item.Value;
                                }
                                else if (item.Column == "Middle Name")
                                {
                                    information.MiddleName = item.Value;
                                }
                                else if (item.Column == "Last Name")
                                {
                                    information.LastName = item.Value;
                                }
                                else if (item.Column == "Joining Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        information.DateOfJoining = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        information.DateOfJoining = null;
                                    }
                                }
                                else if (item.Column == "Confirmation Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        information.DateOfConfirmation = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        information.DateOfConfirmation = null;
                                    }
                                }
                                else if (item.Column == "Grade")
                                {
                                    information.GradeId = 0;
                                    if (!Utility.IsNullEmptyOrWhiteSpace(item.Value))
                                    {
                                        var gradeItem = (await _gradeBusiness.GetGradesAsync(new Grade_Filter()
                                        {
                                            GradeName = item.Value
                                        }, user)).FirstOrDefault();

                                        if (gradeItem == null)
                                        {
                                            var newGrade = await _gradeBusiness.SaveGradeAsync(new GradeDTO()
                                            {
                                                GradeName = item.Value,
                                            }, user);
                                            information.GradeId = (int)newGrade.ItemId;
                                        }
                                        else
                                        {
                                            information.GradeId = (int)gradeItem.GradeId;
                                        }
                                    }
                                }
                                else if (item.Column == "Designation")
                                {
                                    var designationItem = (await _designationBusiness.GetDesignationsAsync(new Designation_Filter()
                                    {
                                        DesignationName = item.Value,
                                        GradeId = information.GradeId.ToString()
                                    }, user)).FirstOrDefault();

                                    if (designationItem == null && !Utility.IsNullEmptyOrWhiteSpace(information.GradeId.ToString()))
                                    {
                                        var newDesignation = await _designationBusiness.SaveDesignationAsync(new DesignationDTO()
                                        {
                                            DesignationName = item.Value,
                                            GradeId = Convert.ToInt32(information.GradeId.ToString())
                                        }, user);
                                        information.DesignationId = (int)newDesignation.ItemId;
                                    }
                                    else
                                    {
                                        if (designationItem != null)
                                        {
                                            information.DesignationId = designationItem.DesignationId;
                                            information.GradeId = designationItem.GradeId;
                                        }
                                    }
                                }
                                else if (item.Column == "Internal Designation")
                                {

                                }
                                else if (item.Column == "Department")
                                {
                                    var departmentItem = (await _departmentBusiness.GetDepartmentsAsync(new Department_Filter()
                                    {
                                        DepartmentName = item.Value
                                    }, user)).FirstOrDefault();

                                    if (departmentItem == null)
                                    {
                                        var newDepartment = await _departmentBusiness.SaveDepartmentAsync(new DepartmentDTO()
                                        {
                                            DepartmentName = item.Value,
                                            IsActive = true,
                                        }, user);
                                        information.DepartmentId = (int)newDepartment.ItemId;
                                    }
                                    else
                                    {
                                        information.DepartmentId = (int)departmentItem.DepartmentId;
                                    }
                                }
                                else if (item.Column == "Section")
                                {
                                    var _sectionItem = (await _sectionBusiness.GetSectionsAsync(new Section_Filter()
                                    {
                                        SectionName = item.Value,
                                        DepartmentId = information.DepartmentId.ToString(),
                                    }, user)).FirstOrDefault();

                                    if (_sectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(information.DepartmentId.ToString()))
                                    {
                                        var newSection = await _sectionBusiness.SaveSectionAsync(new SectionDTO()
                                        {
                                            SectionName = item.Value,
                                            DepartmentId = Convert.ToInt32(information.DepartmentId.ToString()),
                                            IsActive = true,
                                        }, user);
                                        information.SectionId = (int)newSection.ItemId;
                                    }
                                    else
                                    {
                                        information.SectionId = _sectionItem.SectionId;
                                    }
                                }
                                else if (item.Column == "Subsection")
                                {
                                    var _subSectionItem = (await _subSectionBusiness.GetSubSectionsAsync(new SubSection_Filter()
                                    {
                                        SubSectionName = item.Value,
                                        SectionId = information.SectionId.ToString(),
                                    }, user)).FirstOrDefault();

                                    if (_subSectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(information.SectionId.ToString()))
                                    {
                                        var newSubSection = await _subSectionBusiness.SaveSubSectionAsync(new SubSectionDTO()
                                        {
                                            SubSectionName = item.Value,
                                            SectionId = Convert.ToInt32(information.SectionId.ToString()),
                                            IsActive = true,
                                        }, user);
                                        information.SubSectionId = (int)newSubSection.ItemId;
                                    }
                                    else
                                    {
                                        information.SubSectionId = _subSectionItem.SubSectionId;
                                    }
                                }
                                else if (item.Column == "Office Email")
                                {
                                    information.OfficeEmail = item.Value;
                                }
                                else if (item.Column == "Location/Branch")
                                {
                                    var branches = await _orgInitBusiness.BranchExtension("1", user.CompanyId, user.OrganizationId);
                                    var branchObj = branches.FirstOrDefault(i => i.Text.Trim() == item.Value.Trim());
                                    if (branchObj != null)
                                    {
                                        information.BranchId = Utility.TryParseLong(branchObj.Value);
                                    }
                                }
                                else if (item.Column == "Unit")
                                {
                                    information.UnitId = 0;
                                }
                                else if (item.Column == "Appointment Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        information.AppointmentDate = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        information.AppointmentDate = null;
                                    }
                                }
                                else if (item.Column == "Probation End Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        information.ProbationEndDate = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        information.ProbationEndDate = null;
                                    }
                                }
                                else if (item.Column == "Contract End Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        information.ContractEndDate = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        information.ContractEndDate = null;
                                    }
                                }
                                else if (item.Column == "PF Activation Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        information.PFActivationDate = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        information.PFActivationDate = null;
                                    }
                                }
                                else if (item.Column == "Office Mobile")
                                {
                                    information.OfficeMobile = item.Value;
                                }
                                else if (item.Column == "Fingure Id")
                                {
                                    information.FingerID = item.Value;
                                }
                                else if (item.Column == "Job Type")
                                {
                                    information.JobType = item.Value;
                                }
                                else if (item.Column == "Job Category")
                                {
                                }
                                else if (item.Column == "Employee Type")
                                {
                                    var _employeeTypeItem = (await _employeeTypeBusiness.GetEmployeeTypesAsync(new EmployeeType_Filter()
                                    {
                                        EmployeeTypeName = item.Value
                                    }, user)).FirstOrDefault();

                                    if (_employeeTypeItem == null)
                                    {
                                        var newEmployeeType = await _employeeTypeBusiness.SaveEmployeeTypeAsync(new EmployeeTypeDTO()
                                        {
                                            EmployeeTypeName = item.Value,
                                            Remarks = ""
                                        }, user);
                                        information.EmployeeTypeId = newEmployeeType.ItemId;
                                    }
                                    else
                                    {
                                        information.EmployeeTypeId = _employeeTypeItem.EmployeeTypeId;
                                    }
                                }
                                else if (item.Column == "Shift")
                                {
                                    if (!Utility.IsNullEmptyOrWhiteSpace(item.Value))
                                    {
                                        var _workShiftItem = (await _workShiftBusiness.GetWorkShiftsAsync(new WorkShift_Filter()
                                        {
                                            WorkshiftName = item.Value
                                        }, user)).FirstOrDefault();

                                        if (_workShiftItem != null)
                                        {
                                            information.WorkShiftId = _workShiftItem.WorkShiftId;
                                        }
                                    }
                                    else
                                    {
                                        information.WorkShiftId = 0;
                                    }
                                }
                            }

                        }
                    }
                    if ((information.BranchId ?? 0) == 0)
                    {
                        var branches = await _branchInfoBusiness.GetBranchsAsync("", user);
                        if (branches != null && branches.Any())
                        {
                            information.BranchId = branches.First().BranchId;
                        }
                    }
                    if (information.WorkShiftId == 0)
                    {
                        var shifts = await _workShiftBusiness.GetWorkShiftDropdownAsync(user);
                        if (shifts != null && shifts.Any())
                        {
                            information.WorkShiftId = Convert.ToInt64(shifts.First().Id);
                        }
                    }
                    if (information.JobType.IsNullEmptyOrWhiteSpace())
                    {
                        information.JobType = Jobtype.Permanent;
                    }
                }
                line = "End reading of Employee Information";
                #endregion

                line = "Start reading of Employee Detail";
                #region Employee Detail
                var employeeDetailColumns = model.ExcelInfos.Where(i => i.Group == "Employee Detail").ToList();
                if (employeeDetailColumns.Any() && employeeDetailColumns != null)
                {
                    detail = new EmployeeDetail();
                    foreach (var item in employeeDetailColumns)
                    {
                        if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                        {
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Date of birth")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        detail.DateOfBirth = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                    else
                                    {
                                        detail.DateOfBirth = null;
                                    }
                                }
                                else if (item.Column == "Gender")
                                {
                                    detail.Gender = item.Value;
                                }
                                else if (item.Column == "Religion")
                                {
                                    detail.Religion = item.Value;
                                }
                                else if (item.Column == "Legal Name")
                                {
                                    detail.LegalName = item.Value;
                                }
                                else if (item.Column == "Father Name")
                                {
                                    detail.FatherName = item.Value;
                                }
                                else if (item.Column == "Mother Name")
                                {
                                    detail.MotherName = item.Value;
                                }
                                else if (item.Column == "Marital Status")
                                {
                                    detail.MaritalStatus = item.Value;
                                }
                                else if (item.Column == "Spouse Name")
                                {
                                    detail.SpouseName = item.Value;
                                }
                                else if (item.Column == "No Of Child")
                                {
                                    detail.NumberOfChild = item.Value;
                                }
                                else if (item.Column == "Blood Group")
                                {
                                    detail.BloodGroup = item.Value;
                                }
                                else if (item.Column == "Is Residential")
                                {
                                    detail.IsResidential = item.Value.IsNullEmptyOrWhiteSpace() == false ? BoolExtension.TryParseBool(item.Value) : null;
                                }
                                else if (item.Column == "Personal Mobile Number")
                                {
                                    detail.PersonalMobileNo = item.Value;
                                }
                                else if (item.Column == "Personal Email")
                                {
                                    detail.PersonalEmailAddress = item.Value;
                                }
                                else if (item.Column == "Alternative Email Address")
                                {
                                    detail.AlternativeEmailAddress = item.Value;
                                }
                                else if (item.Column == "Present Address")
                                {
                                    detail.PresentAddress = item.Value;
                                }
                                else if (item.Column == "Present Address City")
                                {
                                    detail.PresentAddressCity = item.Value;
                                }
                                else if (item.Column == "Present Address Contact No")
                                {
                                    detail.PresentAddressContactNo = item.Value;
                                }
                                else if (item.Column == "Present Address Zip Code")
                                {
                                    detail.PresentAddressZipCode = item.Value;
                                }
                                else if (item.Column == "Permanent Address")
                                {
                                    detail.PermanentAddress = item.Value;
                                }
                                else if (item.Column == "Permanent Address District")
                                {
                                    detail.PermanentAddressDistrict = item.Value;
                                }
                                else if (item.Column == "Permanent Address Upazila")
                                {
                                    detail.PermanentAddressUpazila = item.Value;
                                }
                                else if (item.Column == "Permanent Address Zip Code")
                                {
                                    detail.PermanentAddressZipCode = item.Value;
                                }
                                else if (item.Column == "Emergency Contact Person1")
                                {
                                    detail.EmergencyContactNo = item.Value;
                                }
                                else if (item.Column == "Relation With Emergency Contact Person1")
                                {
                                    detail.RelationWithEmergencyContactPerson = item.Value;
                                }
                                else if (item.Column == "Emergency Contact No Person1")
                                {
                                    detail.EmergencyContactNo = item.Value;
                                }
                                else if (item.Column == "Emergency Contact Address Person1")
                                {
                                    detail.EmergencyContactAddress = item.Value;
                                }
                                else if (item.Column == "Emergency Contact Email Address Person1")
                                {
                                    detail.EmergencyContactEmailAddress = item.Value;
                                }
                                else if (item.Column == "Emergency Contact Person2")
                                {
                                    detail.EmergencyContactNo2 = item.Value;
                                }
                                else if (item.Column == "Relation With Emergency Contact Person2")
                                {
                                    detail.RelationWithEmergencyContactPerson2 = item.Value;
                                }
                                else if (item.Column == "Emergency Contact No Person2")
                                {
                                    detail.EmergencyContactNo2 = item.Value;
                                }
                                else if (item.Column == "Emergency Contact Address Person2")
                                {
                                    detail.EmergencyContactAddress2 = item.Value;
                                }
                                else if (item.Column == "Emergency Contact Email Address Person2")
                                {
                                    detail.EmergencyContactEmailAddress2 = item.Value;
                                }
                            }
                        }
                    }

                    if (detail.IsResidential == null)
                    {
                        detail.IsResidential = false;
                    }

                }
                line = "End reading of Employee Detail";
                #endregion

                #region Discontinued
                line = "Start reading of Discontinued";
                var discontinuedEmployeeColumns = model.ExcelInfos.Where(i => i.Group == "Discontinued Employee").ToList();
                if (discontinuedEmployeeColumns.Any() && discontinuedEmployeeColumns != null)
                {
                    var lastWorkingDate = discontinuedEmployeeColumns.FirstOrDefault(i => i.Column == "Last Working Date");
                    if (lastWorkingDate != null && lastWorkingDate.Value.IsNullEmptyOrWhiteSpace() == false)
                    {
                        discontinuedEmployee = new DiscontinuedEmployee();
                        foreach (var item in discontinuedEmployeeColumns)
                        {
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Last Working Date")
                                {
                                    if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                    {
                                        discontinuedEmployee.LastWorkingDate = DateTimeExtension.TryParseDate(item.Value);
                                    }
                                }
                            }
                        }
                        discontinuedEmployee.CompanyId = user.CompanyId;
                        discontinuedEmployee.OrganizationId = user.OrganizationId;
                        discontinuedEmployee.CreatedBy = user.ActionUserId;
                        discontinuedEmployee.CreatedDate = DateTime.Now;
                    }
                }
                line = "End reading of Discontinued";
                #endregion

                #region Hierarchy
                line = "Start reading of Hierarchy";
                var hierarchyColumns = model.ExcelInfos.Where(i => i.Group == "Employee Hierarchy").ToList();
                if (hierarchyColumns.Any() && hierarchyColumns != null)
                {
                    foreach (var item in hierarchyColumns)
                    {
                        if (item.Value.IsNullEmptyOrWhiteSpace() == false)
                        {
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Supervisor")
                                {
                                    if (hierarchy == null)
                                    { hierarchy = new EmployeeHierarchy(); }

                                    if (hierarchy != null)
                                    {
                                        var supervisorInfo = await _employeeInfoBusiness.GetEmployeeInformationByCode(item.Value, user);
                                        if (supervisorInfo != null)
                                        {
                                            hierarchy.SupervisorId = supervisorInfo.EmployeeId;
                                        }

                                    }
                                }
                                if (item.Column == "HOD")
                                {
                                    if (hierarchy == null)
                                    { hierarchy = new EmployeeHierarchy(); }
                                    if (hierarchy != null)
                                    {
                                        var hodInfo = await _employeeInfoBusiness.GetEmployeeInformationByCode(item.Value, user);
                                        if (hodInfo != null)
                                        {
                                            hierarchy.HeadOfDepartmentId = hodInfo.EmployeeId;
                                        }
                                    }
                                }
                            }
                        }

                        if (hierarchy != null && (hierarchy.SupervisorId ?? 0) == 0 && (hierarchy.HeadOfDepartmentId ?? 0) == 0)
                        {
                            hierarchy = null;
                        }

                    }
                }
                line = "End reading of Discontinued";
                #endregion

                #region Employee Documents
                line = "Start reading of Employee Documents";
                var documentColumns = model.ExcelInfos.Where(i => i.Group == "Employee Document").ToList();
                if (documentColumns.Any() && documentColumns != null)
                {
                    employeeDocuments = new List<EmployeeDocument>();
                    foreach (var item in documentColumns)
                    {
                        if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                        {
                            // ["Birth Certificate", "NID", "Passport", "TIN","Driving License"]
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Birth Certificate")
                                {
                                    var employeeDocument = new EmployeeDocument();
                                    employeeDocument.DocumentName = "Birth Certificate";
                                    employeeDocument.DocumentNumber = item.Value;
                                    employeeDocument.CompanyId = user.CompanyId;
                                    employeeDocument.OrganizationId = user.OrganizationId;
                                    employeeDocument.CreatedBy = user.ActionUserId;
                                    employeeDocument.CreatedDate = DateTime.Now;
                                    employeeDocuments.Add(employeeDocument);
                                }
                                else if (item.Column == "NID")
                                {
                                    var employeeDocument = new EmployeeDocument();
                                    employeeDocument.DocumentName = "NID";
                                    employeeDocument.DocumentNumber = item.Value;
                                    employeeDocument.CompanyId = user.CompanyId;
                                    employeeDocument.OrganizationId = user.OrganizationId;
                                    employeeDocument.CreatedBy = user.ActionUserId;
                                    employeeDocument.CreatedDate = DateTime.Now;
                                    employeeDocuments.Add(employeeDocument);
                                }
                                else if (item.Column == "Passport")
                                {
                                    var employeeDocument = new EmployeeDocument();
                                    employeeDocument.DocumentName = "Passport";
                                    employeeDocument.DocumentNumber = item.Value;
                                    employeeDocument.CompanyId = user.CompanyId;
                                    employeeDocument.OrganizationId = user.OrganizationId;
                                    employeeDocument.CreatedBy = user.ActionUserId;
                                    employeeDocument.CreatedDate = DateTime.Now;
                                    employeeDocuments.Add(employeeDocument);
                                }
                                else if (item.Column == "TIN")
                                {
                                    var employeeDocument = new EmployeeDocument();
                                    employeeDocument.DocumentName = "TIN";
                                    employeeDocument.DocumentNumber = item.Value;
                                    employeeDocument.CompanyId = user.CompanyId;
                                    employeeDocument.OrganizationId = user.OrganizationId;
                                    employeeDocument.CreatedBy = user.ActionUserId;
                                    employeeDocument.CreatedDate = DateTime.Now;
                                    employeeDocuments.Add(employeeDocument);
                                }
                                else if (item.Column == "Driving License")
                                {
                                    var employeeDocument = new EmployeeDocument();
                                    employeeDocument.DocumentName = "Driving License";
                                    employeeDocument.DocumentNumber = item.Value;
                                    employeeDocument.CompanyId = user.CompanyId;
                                    employeeDocument.OrganizationId = user.OrganizationId;
                                    employeeDocument.CreatedBy = user.ActionUserId;
                                    employeeDocument.CreatedDate = DateTime.Now;
                                    employeeDocuments.Add(employeeDocument);
                                }
                            }
                        }
                    }
                }
                line = "End reading of Employee Documents";
                #endregion

                #region Employee Account Info
                line = "Start reading of Account Info";
                var employeeAccountColumns = model.ExcelInfos.Where(i => i.Group == "Account Information").ToList();
                if (employeeAccountColumns.Any() && employeeAccountColumns != null)
                {
                    accountInfo = new EmployeeAccountInfo();
                    foreach (var item in employeeAccountColumns)
                    {
                        if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                        {
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Bank Name")
                                {
                                    var bankItem = (await _bankBusiness.GetBanksAsync(new Bank_Filter()
                                    {
                                        BankName = item.Value
                                    }, user)).FirstOrDefault();
                                    if (bankItem == null)
                                    {
                                        var newBank = await _bankBusiness.SaveBankAsync(new BankDTO()
                                        {
                                            BankName = item.Value,
                                        }, user);

                                        accountInfo.BankId = (int)newBank.ItemId;
                                    }
                                    else
                                    {
                                        accountInfo.BankId = (int)bankItem.BankId;
                                    }
                                }
                                else if (item.Column == "Bank Branch Name")
                                {
                                    var bankBranchItem = (await _bankBranchBusiness.GetBankBranchesAsync(new BankBranch_Filter()
                                    {
                                        BankBranchName = item.Value,
                                        BankId = accountInfo.BankId.ToString()
                                    }, user)).FirstOrDefault();

                                    if (bankBranchItem == null)
                                    {
                                        var routingNumber = employeeAccountColumns.FirstOrDefault(i => i.Column == "Routing Number");
                                        var newBankBranch = await _bankBranchBusiness.SaveBankBranchAsync(new BankBranchDTO()
                                        {
                                            BankId = accountInfo.BankId ?? 0,
                                            BankBranchName = item.Value,
                                            RoutingNumber = routingNumber != null ? routingNumber.Value : ""
                                        }, user);
                                        accountInfo.BankBranchId = (int)newBankBranch.ItemId;
                                    }
                                    else
                                    {
                                        accountInfo.BankBranchId = (int)bankBranchItem.BankBranchId;
                                    }
                                }
                                else if (item.Column == "Account Number")
                                {
                                    accountInfo.AccountNo = item.Value;
                                }
                            }
                        }
                    }
                    if ((accountInfo.BankId ?? 0) > 0 && (accountInfo.BankBranchId ?? 0) > 0)
                    {
                        accountInfo.DepartmentId = information != null ? information.DepartmentId : 0;
                        accountInfo.DesignationId = information != null ? information.DesignationId : 0;
                        accountInfo.ActivationReason = "New Joiner";
                        accountInfo.CompanyId = user.CompanyId;
                        accountInfo.OrganizationId = user.OrganizationId;
                        accountInfo.CreatedBy = user.ActionUserId;
                        accountInfo.CreatedDate = DateTime.Now;
                    }
                }
                line = "End reading of Account Info";
                #endregion

                #region PF Activation
                line = "Start reading of PF Activation";
                var employeePFColumns = model.ExcelInfos.Where(i => i.Group == "PF Activation").AsList().ToList();
                if (employeePFColumns.Any() && employeePFColumns != null)
                {
                    employeePFActivation = new EmployeePFActivation();
                    foreach (ExcelInfo item in employeePFColumns)
                    {
                        if (item != null)
                        {
                            if (item.Value.IsNullEmptyOrWhiteSpace() == false)
                            {
                                var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                if (column_info != null)
                                {
                                    if (item.Column == "PF Confirmation")
                                    {
                                        if (information != null)
                                        {
                                            information.IsPFMember = BoolExtension.TryParseBool(item.Value);
                                        }
                                    }
                                    if (item.Column == "PF Confirmation Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            employeePFActivation.PFEffectiveDate = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            employeePFActivation.PFEffectiveDate = null;
                                        }
                                    }
                                    if (item.Column == "PF Activation Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            employeePFActivation.PFActivationDate = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            employeePFActivation.PFActivationDate = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (employeePFActivation != null && (employeePFActivation.PFActivationDate.HasValue || employeePFActivation.PFEffectiveDate.HasValue))
                    {
                        var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);
                        if (payrollModuleConfig != null && payrollModuleConfig.BaseOfProvidentFund.IsNullEmptyOrWhiteSpace() == false && Utility.TryParseDecimal(payrollModuleConfig.PercentageOfProvidentFund) > 0)
                        {
                            employeePFActivation.PFBasedAmount = payrollModuleConfig.BaseOfProvidentFund;
                            employeePFActivation.StateStatus = StateStatus.Pending;
                            employeePFActivation.PFPercentage = Utility.TryParseDecimal(payrollModuleConfig.PercentageOfProvidentFund);

                            if (employeePFActivation.PFEffectiveDate.HasValue && !employeePFActivation.PFActivationDate.HasValue)
                            {
                                employeePFActivation.PFActivationDate = employeePFActivation.PFEffectiveDate.Value;
                            }
                            if (employeePFActivation.PFActivationDate.HasValue && !employeePFActivation.PFEffectiveDate.HasValue)
                            {
                                employeePFActivation.PFEffectiveDate = employeePFActivation.PFActivationDate.Value;
                            }
                        }
                        else
                        {
                            employeePFActivation = null;
                        }
                    }
                    else
                    {
                        employeePFActivation = null;
                    }

                }
                line = "End reading of PF Activation";
                #endregion

                #region Salary Review
                line = "Start reading of Salary Review";
                var salaryReviewColumns = model.ExcelInfos.Where(i => i.Group == "Salary Review").ToList();
                if (salaryReviewColumns != null && salaryReviewColumns.Any() && information != null)
                {
                    salaryReview = new SalaryReviewFromEmployeeUploader();
                    foreach (var item in salaryReviewColumns)
                    {
                        if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                        {
                            var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                            if (column_info != null)
                            {
                                if (item.Column == "Gross Salary")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        salaryReview.GrossSalary = Convert.ToDecimal(item.Value);
                                    }
                                }
                                else if (item.Column == "Basic Salary")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        salaryReview.BasicSalary = Convert.ToDecimal(item.Value);
                                    }
                                }
                                else if (item.Column == "House Rent")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        salaryReview.HouseRent = Convert.ToDecimal(item.Value);
                                    }
                                }
                                else if (item.Column == "Medical")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        salaryReview.Medical = Convert.ToDecimal(item.Value);
                                    }
                                }
                                else if (item.Column == "Conveyance")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        salaryReview.Conveyance = Convert.ToDecimal(item.Value);
                                    }
                                }
                                else if (item.Column == "LFA")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        salaryReview.LFA = Convert.ToDecimal(item.Value);
                                    }
                                }
                                else if (item.Column == "Salary Effective Date")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        try
                                        {
                                            var val = Convert.ToDouble(item.Value.RemoveWhitespace());
                                            salaryReview.SalaryEffectiveDate = information != null ? information.DateOfJoining : DateTime.FromOADate(val);
                                        }
                                        catch (Exception ex)
                                        {
                                            salaryReview.SalaryEffectiveDate = information != null ? information.DateOfJoining : null;
                                            Console.WriteLine(ex);
                                        }
                                    }
                                    else
                                    {
                                        var date = new DateTime();
                                        if (DateTime.TryParse(item.Value.RemoveWhitespace(), out date))
                                        {
                                            salaryReview.SalaryEffectiveDate = information != null ? information.DateOfJoining : date;
                                        }
                                        else
                                        {
                                            salaryReview.SalaryEffectiveDate = information != null ? information.DateOfJoining : null;
                                        }
                                    }
                                }
                                else if (item.Column == "Salary Activation Date")
                                {
                                    if (item.Value.IsStringNumber())
                                    {
                                        try
                                        {
                                            if (information != null)
                                            {
                                                var val = Convert.ToDouble(item.Value.RemoveWhitespace());
                                                var date = DateTime.FromOADate(val);
                                                if (date >= information.DateOfJoining)
                                                {
                                                    if (information.DateOfJoining.Value.Month != date.Month)
                                                    {
                                                        date = new DateTime(date.Year, date.Month, 1);
                                                    }
                                                    salaryReview.SalaryActivationDate = date;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            salaryReview.SalaryActivationDate = information != null ? information.DateOfJoining : null;
                                            Console.WriteLine(ex);
                                        }
                                    }
                                    else
                                    {
                                        var date = new DateTime();
                                        if (DateTime.TryParse(item.Value.RemoveWhitespace(), out date))
                                        {
                                            if (information != null)
                                            {
                                                if (date >= information.DateOfJoining)
                                                {
                                                    if (information.DateOfJoining.Value.Month != date.Month)
                                                    {
                                                        date = new DateTime(date.Year, date.Month, 1);
                                                    }
                                                    salaryReview.SalaryActivationDate = date;
                                                }
                                                else
                                                {
                                                    salaryReview.SalaryActivationDate = information.DateOfJoining;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            salaryReview.SalaryActivationDate = information != null ? information.DateOfJoining : null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (salaryReview != null)
                    {
                        // Find Salary Review Type
                        if (salaryReview.BasicSalary > 0 && (salaryReview.HouseRent > 0 || salaryReview.Medical > 0 || salaryReview.Conveyance > 0 || salaryReview.LFA > 0))
                        {
                            salaryReview.ConfigType = "Flat";
                        }
                        else if (salaryReview.GrossSalary == 0 && salaryReview.BasicSalary > 0)
                        {
                            salaryReview.ConfigType = "Basic";
                        }
                        else if (salaryReview.GrossSalary > 0 && salaryReview.BasicSalary == 0)
                        {
                            salaryReview.ConfigType = "Gross";
                        }
                        if(salaryReview.SalaryEffectiveDate == null)
                        {
                            salaryReview.SalaryEffectiveDate = information.DateOfJoining;
                        }
                        if (salaryReview.SalaryActivationDate == null)
                        {
                            salaryReview.SalaryEffectiveDate = information.DateOfJoining;
                        }

                    }
                }
                line = "End reading of Salary Review";
                #endregion

                //--------------
                #region Save Cost Center
                line = "Start reading of Cost Center";
                if (costCenter != null && information != null)
                {
                    if (!Utility.IsNullEmptyOrWhiteSpace(costCenter.CostCenterName) || !Utility.IsNullEmptyOrWhiteSpace(costCenter.CostCenterCode))
                    {
                        var costCenterItem = (await _costCenterBusiness.GetCostCentersAsync(new CostCenter_Filter()
                        {
                            CostCenterName = costCenter.CostCenterName,
                            CostCenterCode = costCenter.CostCenterCode
                        }, user)).FirstOrDefault();

                        if (costCenterItem == null)
                        {
                            var newCostCenter = await _costCenterBusiness.SaveCostCenterAsync(new CostCenterDTO()
                            {
                                CostCenterName = costCenter.CostCenterName,
                                CostCenterCode = costCenter.CostCenterCode,
                                IsActive = true,
                            }, user);
                            information.CostCenterId = (int)newCostCenter.ItemId;
                        }
                        else
                        {
                            information.CostCenterId = (int)costCenterItem.CostCenterId;
                        }
                    }
                }
                line = "End reading of Cost Center";
                #endregion

                #region Save Employee Info & Detail
                line = "Start Save Employee Info & Detail";
                if (information != null)
                {
                    using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            information.FullName = information.FirstName + " " + information.LastName;
                            information.StateStatus = StateStatus.Pending;
                            information.CompanyId = user.CompanyId;
                            information.OrganizationId = user.OrganizationId;
                            information.CreatedBy = user.ActionUserId;
                            information.CreatedDate = DateTime.Now;
                            await _employeeModuleDbContext.HR_EmployeeInformation.AddAsync(information);
                            if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                            {
                                if (detail != null)
                                {
                                    detail.EmployeeId = information.EmployeeId;
                                    detail.CompanyId = user.CompanyId;
                                    detail.OrganizationId = user.OrganizationId;
                                    detail.CreatedBy = user.ActionUserId;
                                    detail.CreatedDate = DateTime.Now;
                                    await _employeeModuleDbContext.HR_EmployeeDetail.AddAsync(detail);
                                    if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                    {
                                        executionStatus.Msg = "Detail";
                                        executionStatus.Status = true;
                                        executionStatus.ItemId = information.EmployeeId;
                                        await transaction.CommitAsync();
                                    }
                                    else
                                    {
                                        executionStatus.Msg = "Detail";
                                        executionStatus.Status = false;
                                        await transaction.RollbackAsync();
                                    }
                                }
                                else
                                {
                                    executionStatus.Msg = "information";
                                    executionStatus.Status = true;
                                    executionStatus.ItemId = information.EmployeeId;
                                    await transaction.CommitAsync();
                                }
                            }
                            else
                            {
                                executionStatus.Msg = "information";
                                executionStatus.Status = false;
                                await transaction.RollbackAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            executionStatus.Status = false;
                            await transaction.RollbackAsync();
                        }
                    }
                }
                line = "End Save Employee Info & Detail";
                #endregion

                #region Save Others
                if (information != null)
                {
                    #region Hierarchy
                    line = "Start Save Hierarchy";
                    if (information.EmployeeId > 0 && hierarchy != null)
                    {
                        using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                hierarchy.IsActive = true;
                                hierarchy.EmployeeId = information.EmployeeId;
                                hierarchy.CompanyId = information.CompanyId;
                                hierarchy.OrganizationId = information.OrganizationId;
                                hierarchy.CreatedBy = user.ActionUserId;
                                hierarchy.CreatedDate = DateTime.Now;
                                await _employeeModuleDbContext.HR_EmployeeHierarchy.AddAsync(hierarchy);
                                if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                {
                                    executionStatus.Msg = "Hierarchy";
                                    await transaction.CommitAsync();
                                }
                                else
                                {
                                    executionStatus.Msg = "Hierarchy";
                                    await transaction.RollbackAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                await transaction.RollbackAsync();
                            }
                        }
                    }
                    line = "End Save Hierarchy";
                    #endregion

                    #region Discontinued Employee
                    line = "Start Save Discontinued";
                    if (information.EmployeeId > 0 && discontinuedEmployee != null && discontinuedEmployee.LastWorkingDate.HasValue)
                    {
                        using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                discontinuedEmployee.EmployeeId = information.EmployeeId;
                                discontinuedEmployee.Releasetype = "Resigned";
                                discontinuedEmployee.CalculateFestivalBonusTaxProratedBasis = false;
                                discontinuedEmployee.CalculateProjectionTaxProratedBasis = false;
                                discontinuedEmployee.StateStatus = StateStatus.Approved;
                                discontinuedEmployee.CompanyId = user.CompanyId;
                                discontinuedEmployee.OrganizationId = user.OrganizationId;
                                discontinuedEmployee.CreatedBy = user.ActionUserId;
                                discontinuedEmployee.CreatedDate = DateTime.Now;

                                await _employeeModuleDbContext.HR_DiscontinuedEmployee.AddAsync(discontinuedEmployee);
                                if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                {
                                    executionStatus.Msg = "Discontinued";
                                    information.TerminationDate = discontinuedEmployee.LastWorkingDate;
                                    information.TerminationStatus = StateStatus.Approved;
                                    _employeeModuleDbContext.HR_EmployeeInformation.Update(information);
                                    await _employeeModuleDbContext.SaveChangesAsync();
                                    await transaction.CommitAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                executionStatus.Msg = "Discontinued";
                                Console.WriteLine(ex.ToString());
                                await transaction.RollbackAsync();
                            }
                        }
                    }
                    line = "End Save Discontinued";
                    #endregion

                    #region Documents
                    line = "Start Save Documents";
                    if (information.EmployeeId > 0 && employeeDocuments != null && employeeDocuments.Any())
                    {
                        using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                        {
                            foreach (var item in employeeDocuments)
                            {
                                item.EmployeeId = information.EmployeeId;
                            }
                            await _employeeModuleDbContext.HR_EmployeeDocument.AddRangeAsync(employeeDocuments);
                            if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                            {
                                executionStatus.Msg = "Documents";
                                await transaction.CommitAsync();
                            }
                            else
                            {
                                executionStatus.Msg = "Documents";
                                await transaction.RollbackAsync();
                            }
                        }
                    }
                    line = "End Save Documents";
                    #endregion

                    #region Account Info
                    line = "Start Save Account Info";
                    if (information.EmployeeId > 0 && accountInfo != null && accountInfo.BankId > 0
                        && accountInfo.BankBranchId > 0 && accountInfo.AccountNo.IsNullEmptyOrWhiteSpace() == false)
                    {
                        using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                accountInfo.EmployeeId = information.EmployeeId;
                                accountInfo.CompanyId = information.CompanyId;
                                accountInfo.OrganizationId = information.OrganizationId;
                                accountInfo.BranchId = information.BranchId;
                                accountInfo.PaymentMode = PaymentMode.Bank;
                                accountInfo.StateStatus = StateStatus.Pending;
                                await _employeeModuleDbContext.HR_EmployeeAccountInfo.AddAsync(accountInfo);
                                if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                {
                                    executionStatus.Msg = "Account";
                                    await transaction.CommitAsync();
                                }
                                else
                                {
                                    executionStatus.Msg = "Account";
                                    await transaction.RollbackAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                await transaction.RollbackAsync();
                            }
                        }
                    }
                    line = "End Save Account Info";
                    #endregion

                    #region PF Activation
                    line = "Start Save PF Activation";
                    if (information.EmployeeId > 0 && employeePFActivation != null && employeePFActivation.PFEffectiveDate.HasValue && employeePFActivation.PFActivationDate.HasValue)
                    {
                        using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                employeePFActivation.EmployeeId = information.EmployeeId;
                                employeePFActivation.CompanyId = user.CompanyId;
                                employeePFActivation.OrganizationId = user.OrganizationId;
                                employeePFActivation.CreatedBy = user.ActionUserId;
                                employeePFActivation.CreatedDate = DateTime.Now;
                                employeePFActivation.StateStatus = StateStatus.Pending;
                                await _employeeModuleDbContext.AddAsync(employeePFActivation);
                                if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                {
                                    if (information.DateOfJoining.HasValue)
                                    {
                                        if (employeePFActivation.PFActivationDate.Value.Date >= information.DateOfJoining.Value.Date)
                                        {
                                            information.IsPFMember = true;
                                            information.PFActivationDate = employeePFActivation.PFActivationDate.Value.Date;
                                            _employeeModuleDbContext.HR_EmployeeInformation.Update(information);
                                            await _employeeModuleDbContext.SaveChangesAsync();
                                            await transaction.CommitAsync();
                                            executionStatus.Msg = "PF Activation";
                                        }
                                    }
                                    else
                                    {
                                        executionStatus.Msg = "PF Activation";
                                        await transaction.CommitAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                executionStatus.Msg = "PF Activation";
                                Console.WriteLine(ex);
                                await transaction.RollbackAsync();
                            }
                        }
                    }
                    line = "End Save PF Activation";
                    #endregion

                    #region Salary Review
                    line = "Start Save Salary Review";
                    if (information != null && information.EmployeeId > 0 && salaryReview != null)
                    {
                        List<SalaryReviewDetail> salaryReviewDetails = new List<SalaryReviewDetail>();
                        if (salaryReview.ConfigType == "Flat")
                        {
                            if (salaryReview.BasicSalary > 0)
                            {
                                var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                {
                                    AllowanceFlag = "BASIC"
                                }, user)).FirstOrDefault();
                                if (allowance != null)
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        AllowanceName = allowance.Name,
                                        AllowanceBase = salaryReview.ConfigType,
                                        AllowanceAmount = salaryReview.BasicSalary,
                                        AllowancePercentage = 0,
                                        CurrentAmount = salaryReview.BasicSalary,
                                        AdditionalAmount = 0,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = 0,
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                            if (salaryReview.HouseRent > 0)
                            {
                                var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                {
                                    AllowanceFlag = "HR"
                                }, user)).FirstOrDefault();
                                if (allowance != null)
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        AllowanceName = allowance.Name,
                                        AllowanceBase = salaryReview.ConfigType,
                                        AllowanceAmount = salaryReview.HouseRent,
                                        AllowancePercentage = 0,
                                        CurrentAmount = salaryReview.HouseRent,
                                        AdditionalAmount = 0,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = 0
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                            if (salaryReview.Medical > 0)
                            {
                                var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                {
                                    AllowanceFlag = "MEDICAL"
                                }, user)).FirstOrDefault();
                                if (allowance != null)
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        AllowanceName = allowance.Name,
                                        AllowanceBase = salaryReview.ConfigType,
                                        AllowanceAmount = salaryReview.Medical,
                                        AllowancePercentage = 0,
                                        CurrentAmount = salaryReview.Medical,
                                        AdditionalAmount = 0,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = 0
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                            if (salaryReview.Conveyance > 0)
                            {
                                var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                {
                                    AllowanceFlag = "CONVEYANCE"
                                }, user)).FirstOrDefault();
                                if (allowance != null)
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        AllowanceName = allowance.Name,
                                        AllowanceBase = salaryReview.ConfigType,
                                        AllowanceAmount = salaryReview.Conveyance,
                                        AllowancePercentage = 0,
                                        CurrentAmount = salaryReview.Conveyance,
                                        AdditionalAmount = 0,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = 0
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                            if (salaryReview.LFA > 0)
                            {
                                var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                {
                                    AllowanceFlag = "LFA"
                                }, user)).FirstOrDefault();
                                if (allowance != null)
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        AllowanceName = allowance.Name,
                                        AllowanceBase = salaryReview.ConfigType,
                                        AllowanceAmount = salaryReview.LFA,
                                        AllowancePercentage = 0,
                                        CurrentAmount = salaryReview.LFA,
                                        AdditionalAmount = 0,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = 0
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                        }
                        if (salaryReview.ConfigType == "Gross")
                        {
                            var grossAmount = salaryReview.GrossSalary;
                            var allowances = await _salaryReviewBusiness.GetSalaryAllowanceForReviewAsync(information.EmployeeId, user);
                            var flatAmount = allowances.Where(i => i.AllowanceBase == "Flat").Sum(i => i.AllowanceAmount);
                            var actualGross = grossAmount - flatAmount ?? 0;
                            var remainAmount = actualGross;
                            foreach (var allowance in allowances)
                            {
                                if (allowance.AllowanceBase == "Gross")
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AdditionalAmount = 0,
                                        AllowanceBase = "Gross",
                                        AllowanceAmount = allowance.AllowanceAmount,
                                        AllowancePercentage = allowance.AllowancePercentage,
                                        AllowanceName = allowance.AllowanceName,
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        CurrentAmount = Math.Round(((actualGross) / 100 * allowance.AllowancePercentage), MidpointRounding.AwayFromZero),
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                    };
                                    remainAmount = remainAmount - salaryReviewDetail.CurrentAmount;
                                    if (remainAmount < 0 && Math.Abs(remainAmount) > 0)
                                    {
                                        salaryReviewDetail.CurrentAmount = salaryReviewDetail.CurrentAmount - remainAmount;
                                    }
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                                if (allowance.AllowanceBase == "Flat")
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AdditionalAmount = 0,
                                        AllowanceBase = "Flat",
                                        AllowanceAmount = allowance.AllowanceAmount,
                                        AllowancePercentage = allowance.AllowancePercentage,
                                        AllowanceName = allowance.AllowanceName,
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        CurrentAmount = allowance.AllowanceAmount ?? 0,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                        }
                        if (salaryReview.ConfigType == "Basic")
                        {
                            var basicAmount = salaryReview.BasicSalary;
                            var allowances = await _salaryReviewBusiness.GetSalaryAllowanceForReviewAsync(information.EmployeeId, user);
                            foreach (var allowance in allowances)
                            {
                                if (allowance.AllowanceBase == "Basic")
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AdditionalAmount = 0,
                                        AllowanceBase = "Gross",
                                        AllowanceAmount = allowance.AllowanceAmount,
                                        AllowancePercentage = allowance.AllowancePercentage,
                                        AllowanceName = allowance.AllowanceName,
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        CurrentAmount = Math.Round(((basicAmount) / 100 * allowance.AllowancePercentage), MidpointRounding.AwayFromZero),
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                                if (allowance.AllowanceBase == "Flat")
                                {
                                    SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                    {
                                        AdditionalAmount = 0,
                                        AllowanceBase = "Flat",
                                        AllowanceAmount = allowance.AllowanceAmount,
                                        AllowancePercentage = allowance.AllowancePercentage,
                                        AllowanceName = allowance.AllowanceName,
                                        AllowanceNameId = allowance.AllowanceNameId,
                                        BranchId = information.BranchId,
                                        CompanyId = information.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        CurrentAmount = allowance.AllowanceAmount ?? 0,
                                        EmployeeId = information.EmployeeId,
                                        OrganizationId = information.OrganizationId,
                                        PreviousAmount = 0,
                                        SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                    };
                                    salaryReviewDetails.Add(salaryReviewDetail);
                                }
                            }
                        }
                        if (salaryReviewDetails.Any())
                        {
                            SalaryReviewInfo salaryReviewInfo = new SalaryReviewInfo()
                            {
                                ActivationDate = salaryReview.SalaryActivationDate,
                                ArrearCalculatedDate = salaryReview.SalaryActivationDate,
                                BaseType = salaryReview.ConfigType,
                                BranchId = information.BranchId,
                                CompanyId = information.CompanyId,
                                CreatedBy = user.ActionUserId,
                                CreatedDate = DateTime.Now,
                                DepartmentId = information.DepartmentId,
                                DesignationId = information.DesignationId,
                                EffectiveFrom = salaryReview.SalaryEffectiveDate,
                                EmployeeId = information.EmployeeId,
                                IncrementReason = "Joining",
                                PreviousSalaryAmount = 0,
                                SalaryAllowanceConfigId = 0,
                                SalaryBaseAmount = salaryReviewDetails.Sum(i => i.CurrentAmount),
                                SalaryReviewDetails = salaryReviewDetails,
                                IsAutoCalculate = false,
                                SectionId = information.SectionId,
                                StateStatus = StateStatus.Pending,
                                CurrentSalaryAmount = salaryReviewDetails.Sum(i => i.CurrentAmount),
                                IsArrearCalculated = false,
                                IsActive = false,
                                IsApproved = false,
                                OrganizationId = information.OrganizationId,
                                SalaryConfigCategory = salaryReview.ConfigType,
                                InternalDesignationId = information.InternalDesignationId,
                                PreviousReviewId = 0
                            };
                            using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    await _payrollDbContext.AddAsync(salaryReviewInfo);
                                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                                    {
                                        await transaction.CommitAsync();
                                    }
                                    else
                                    {
                                        await transaction.RollbackAsync();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    await transaction.RollbackAsync();
                                }
                            }
                        }
                    }
                    line = "End Save Salary Review";
                    #endregion
                }
                #endregion

            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ex.Message + "" + ex.StackTrace + " " + line);
                Console.WriteLine(ex.ToString());
            }
            executionStatus.Code = model.Code;
            executionStatus.Action = line;
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateAsync(ExcelInfoCollection model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            string line = "";
            try
            {
                CostCenterDTO costCenter = null;
                EmployeeInformation information = null;
                EmployeeDetail detail = null;
                DiscontinuedEmployee discontinuedEmployee = null;
                EmployeeHierarchy hierarchy = null;
                List<EmployeeTransferProposal> employeeTransferProposals = new List<EmployeeTransferProposal>();
                List<EmployeePromotionProposal> employeePromotionProposals = new List<EmployeePromotionProposal>();
                List<EmployeeConfirmationProposal> employeeConfirmationProposals = new List<EmployeeConfirmationProposal>();
                List<EmployeeDocument> employeeDocuments = null;
                EmployeeAccountInfo accountInfo = null;
                EmployeePFActivation employeePFActivation = null;
                SalaryReviewFromEmployeeUploader salaryReview = null;

                var column_list = (await _tableConfigBusiness.GetColumnsAsync("Employee Uploader", "Upload", user)).ToList();

                var employeeInformationInDb = await _employeeInfoBusiness.GetEmployeeInformationById(model.Id, user);
                var employeeInformationInDb2 = employeeInformationInDb;

                if (employeeInformationInDb != null && employeeInformationInDb.EmployeeId > 0)
                {
                    var employeeDetailInDb = await _employeeInfoBusiness.GetEmployeeDetailById(model.Id, user);
                    var employeeDetailInDb2 = employeeDetailInDb;

                    #region Employee Information

                    var employeeInfoColumns = model.ExcelInfos.Where(i => i.Group == "Employee Information").ToList();
                    if (employeeInfoColumns.Any() && employeeInfoColumns != null)
                    {
                        line = "Start reading of Employee Information";
                        information = new EmployeeInformation();
                        foreach (var item in employeeInfoColumns)
                        {
                            if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                            {
                                var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                if (column_info != null)
                                {
                                    if (item.Column == "Employee ID")
                                    {
                                        information.EmployeeCode = item.Value;
                                    }
                                    else if (item.Column == "Global ID")
                                    {
                                        information.GlobalID = item.Value;
                                    }
                                    else if (item.Column == "Previous ID")
                                    {
                                        information.PreviousCode = item.Value;
                                    }
                                    else if (item.Column == "Mr./Ms./Mrs.")
                                    {
                                        information.Salutation = item.Value;
                                    }
                                    else if (item.Column == "First Name")
                                    {
                                        information.FirstName = item.Value;
                                    }
                                    else if (item.Column == "Middle Name")
                                    {
                                        information.MiddleName = item.Value;
                                    }
                                    else if (item.Column == "Last Name")
                                    {
                                        information.LastName = item.Value;
                                    }
                                    else if (item.Column == "Joining Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            information.DateOfJoining = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            information.DateOfJoining = null;
                                        }
                                    }
                                    else if (item.Column == "Confirmation Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            information.DateOfConfirmation = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            information.DateOfConfirmation = null;
                                        }
                                    }
                                    else if (item.Column == "Grade")
                                    {
                                        information.GradeId = 0;
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.Value))
                                        {
                                            var gradeItem = (await _gradeBusiness.GetGradesAsync(new Grade_Filter()
                                            {
                                                GradeName = item.Value
                                            }, user)).FirstOrDefault();

                                            if (gradeItem == null)
                                            {
                                                var newGrade = await _gradeBusiness.SaveGradeAsync(new GradeDTO()
                                                {
                                                    GradeName = item.Value,
                                                }, user);
                                                information.GradeId = (int)newGrade.ItemId;
                                            }
                                            else
                                            {
                                                information.GradeId = (int)gradeItem.GradeId;
                                            }
                                        }
                                    }
                                    else if (item.Column == "Designation")
                                    {
                                        var designationItem = (await _designationBusiness.GetDesignationsAsync(new Designation_Filter()
                                        {
                                            DesignationName = item.Value,
                                            GradeId = information.GradeId.ToString()
                                        }, user)).FirstOrDefault();

                                        if (designationItem == null && !Utility.IsNullEmptyOrWhiteSpace(information.GradeId.ToString()))
                                        {
                                            var newDesignation = await _designationBusiness.SaveDesignationAsync(new DesignationDTO()
                                            {
                                                DesignationName = item.Value,
                                                GradeId = Convert.ToInt32(information.GradeId.ToString())
                                            }, user);
                                            information.DesignationId = (int)newDesignation.ItemId;
                                        }
                                        else
                                        {
                                            if (designationItem != null)
                                            {
                                                information.DesignationId = designationItem.DesignationId;
                                                information.GradeId = designationItem.GradeId;
                                            }
                                        }
                                    }
                                    else if (item.Column == "Internal Designation")
                                    {

                                    }
                                    else if (item.Column == "Department")
                                    {
                                        var departmentItem = (await _departmentBusiness.GetDepartmentsAsync(new Department_Filter()
                                        {
                                            DepartmentName = item.Value
                                        }, user)).FirstOrDefault();

                                        if (departmentItem == null)
                                        {
                                            var newDepartment = await _departmentBusiness.SaveDepartmentAsync(new DepartmentDTO()
                                            {
                                                DepartmentName = item.Value,
                                                IsActive = true,
                                            }, user);
                                            information.DepartmentId = (int)newDepartment.ItemId;
                                        }
                                        else
                                        {
                                            information.DepartmentId = (int)departmentItem.DepartmentId;
                                        }
                                    }
                                    else if (item.Column == "Section")
                                    {
                                        var _sectionItem = (await _sectionBusiness.GetSectionsAsync(new Section_Filter()
                                        {
                                            SectionName = item.Value,
                                            DepartmentId = information.DepartmentId.ToString(),
                                        }, user)).FirstOrDefault();

                                        if (_sectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(information.DepartmentId.ToString()))
                                        {
                                            var newSection = await _sectionBusiness.SaveSectionAsync(new SectionDTO()
                                            {
                                                SectionName = item.Value,
                                                DepartmentId = Convert.ToInt32(information.DepartmentId.ToString()),
                                                IsActive = true,
                                            }, user);
                                            information.SectionId = (int)newSection.ItemId;
                                        }
                                        else
                                        {
                                            information.SectionId = _sectionItem.SectionId;
                                        }
                                    }
                                    else if (item.Column == "Subsection")
                                    {
                                        var _subSectionItem = (await _subSectionBusiness.GetSubSectionsAsync(new SubSection_Filter()
                                        {
                                            SubSectionName = item.Value,
                                            SectionId = information.SectionId.ToString(),
                                        }, user)).FirstOrDefault();

                                        if (_subSectionItem == null && !Utility.IsNullEmptyOrWhiteSpace(information.SectionId.ToString()))
                                        {
                                            var newSubSection = await _subSectionBusiness.SaveSubSectionAsync(new SubSectionDTO()
                                            {
                                                SubSectionName = item.Value,
                                                SectionId = Convert.ToInt32(information.SectionId.ToString()),
                                                IsActive = true,
                                            }, user);
                                            information.SubSectionId = (int)newSubSection.ItemId;
                                        }
                                        else
                                        {
                                            information.SubSectionId = _subSectionItem.SubSectionId;
                                        }
                                    }
                                    else if (item.Column == "Office Email")
                                    {
                                        information.OfficeEmail = item.Value;
                                    }
                                    else if (item.Column == "Location/Branch")
                                    {
                                        var branches = await _orgInitBusiness.BranchExtension("1", user.CompanyId, user.OrganizationId);
                                        var branchObj = branches.FirstOrDefault(i => i.Text.Trim() == item.Value.Trim());
                                        if (branchObj != null)
                                        {
                                            information.BranchId = Utility.TryParseLong(branchObj.Value);
                                        }
                                    }
                                    else if (item.Column == "Unit")
                                    {
                                        information.UnitId = 0;
                                    }
                                    else if (item.Column == "Appointment Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            information.AppointmentDate = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            information.AppointmentDate = null;
                                        }
                                    }
                                    else if (item.Column == "Probation End Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            information.ProbationEndDate = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            information.ProbationEndDate = null;
                                        }
                                    }
                                    else if (item.Column == "Contract End Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            information.ContractEndDate = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            information.ContractEndDate = null;
                                        }
                                    }
                                    else if (item.Column == "PF Activation Date")
                                    {
                                        if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                        {
                                            information.PFActivationDate = DateTimeExtension.TryParseDate(item.Value);
                                        }
                                        else
                                        {
                                            information.PFActivationDate = null;
                                        }
                                    }
                                    else if (item.Column == "Office Mobile")
                                    {
                                        information.OfficeMobile = item.Value;
                                    }
                                    else if (item.Column == "Fingure Id")
                                    {
                                        information.FingerID = item.Value;
                                    }
                                    else if (item.Column == "Job Type")
                                    {
                                        information.JobType = item.Value;
                                    }
                                    else if (item.Column == "Job Category")
                                    {
                                    }
                                    else if (item.Column == "Employee Type")
                                    {
                                        var _employeeTypeItem = (await _employeeTypeBusiness.GetEmployeeTypesAsync(new EmployeeType_Filter()
                                        {
                                            EmployeeTypeName = item.Value
                                        }, user)).FirstOrDefault();

                                        if (_employeeTypeItem == null)
                                        {
                                            var newEmployeeType = await _employeeTypeBusiness.SaveEmployeeTypeAsync(new EmployeeTypeDTO()
                                            {
                                                EmployeeTypeName = item.Value,
                                                Remarks = ""
                                            }, user);
                                            information.EmployeeTypeId = newEmployeeType.ItemId;
                                        }
                                        else
                                        {
                                            information.EmployeeTypeId = _employeeTypeItem.EmployeeTypeId;
                                        }
                                    }
                                    else if (item.Column == "Shift")
                                    {
                                        if (!Utility.IsNullEmptyOrWhiteSpace(item.Value))
                                        {
                                            var _workShiftItem = (await _workShiftBusiness.GetWorkShiftsAsync(new WorkShift_Filter()
                                            {
                                                WorkshiftName = item.Value
                                            }, user)).FirstOrDefault();

                                            if (_workShiftItem != null)
                                            {
                                                information.WorkShiftId = _workShiftItem.WorkShiftId;
                                            }
                                        }
                                        else
                                        {
                                            information.WorkShiftId = 0;
                                        }
                                    }
                                }

                            }
                        }

                        #region Update Employee Information
                        if (!information.Salutation.IsNullEmptyOrWhiteSpace() && information.Salutation != employeeInformationInDb.Salutation)
                        {
                            employeeInformationInDb.Salutation = information.Salutation;
                        }
                        if (!information.FirstName.IsNullEmptyOrWhiteSpace() && information.FirstName != employeeInformationInDb.FirstName)
                        {
                            employeeInformationInDb.FirstName = information.FirstName;
                        }
                        if (!information.MiddleName.IsNullEmptyOrWhiteSpace() && information.MiddleName != employeeInformationInDb.MiddleName)
                        {
                            employeeInformationInDb.MiddleName = information.MiddleName;
                        }
                        if (!information.LastName.IsNullEmptyOrWhiteSpace() && information.LastName != employeeInformationInDb.LastName)
                        {
                            employeeInformationInDb.LastName = information.LastName;
                        }
                        if (!information.NickName.IsNullEmptyOrWhiteSpace() && information.NickName != employeeInformationInDb.NickName)
                        {
                            employeeInformationInDb.NickName = information.NickName;
                        }
                        if ((information.BranchId ?? 0) > 0 && (information.BranchId ?? 0) != (employeeInformationInDb.BranchId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeeTransferProposal transferProposal = new EmployeeTransferProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "Branch",
                                    Flag = "Transfer",
                                    ExistingValue = employeeInformationInDb.BranchId.ToString(),
                                    ProposalValue = information.BranchId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    OrganizationId = employeeInformationInDb.OrganizationId
                                };
                                employeeTransferProposals.Add(transferProposal);
                            }
                            employeeInformationInDb.BranchId = information.BranchId;
                        }
                        if ((information.GradeId ?? 0) > 0 && (information.GradeId ?? 0) != (employeeInformationInDb.GradeId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeePromotionProposal promotionProposal = new EmployeePromotionProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "Grade",
                                    Flag = "Promotion",
                                    ExistingValue = employeeInformationInDb.GradeId.ToString(),
                                    ProposalValue = information.GradeId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    OrganizationId = employeeInformationInDb.OrganizationId

                                };
                                employeePromotionProposals.Add(promotionProposal);
                            }
                            employeeInformationInDb.GradeId = information.GradeId;
                        }
                        if ((information.DesignationId ?? 0) > 0 && (information.DesignationId ?? 0) != (employeeInformationInDb.DesignationId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeePromotionProposal promotionProposal = new EmployeePromotionProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "Desgination",
                                    Flag = "Promotion",
                                    ExistingValue = employeeInformationInDb.GradeId.ToString(),
                                    ProposalValue = information.GradeId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    OrganizationId = employeeInformationInDb.OrganizationId

                                };
                                employeePromotionProposals.Add(promotionProposal);
                            }
                            employeeInformationInDb.DesignationId = information.DesignationId;
                        }
                        if ((information.DepartmentId ?? 0) > 0 && (information.DepartmentId ?? 0) != (employeeInformationInDb.DepartmentId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeeTransferProposal transferProposal = new EmployeeTransferProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "Department",
                                    Flag = "Transfer",
                                    ExistingValue = employeeInformationInDb.DepartmentId.ToString(),
                                    ProposalValue = information.DepartmentId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    OrganizationId = employeeInformationInDb.OrganizationId

                                };
                                employeeTransferProposals.Add(transferProposal);
                            }
                            employeeInformationInDb.DepartmentId = information.DepartmentId;
                        }
                        if ((information.SectionId ?? 0) > 0 && (information.SectionId ?? 0) != (employeeInformationInDb.SectionId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeeTransferProposal transferProposal = new EmployeeTransferProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "Section",
                                    Flag = "Transfer",
                                    ExistingValue = employeeInformationInDb.SectionId.ToString(),
                                    ProposalValue = information.SectionId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    OrganizationId = employeeInformationInDb.OrganizationId

                                };
                                employeeTransferProposals.Add(transferProposal);
                            }
                            employeeInformationInDb.SectionId = information.SectionId;
                        }
                        if ((information.SubSectionId ?? 0) > 0 && (information.SubSectionId ?? 0) != (employeeInformationInDb.SubSectionId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeeTransferProposal transferProposal = new EmployeeTransferProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "SubSection",
                                    Flag = "Transfer",
                                    ExistingValue = employeeInformationInDb.SubSectionId.ToString(),
                                    ProposalValue = information.SubSectionId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    OrganizationId = employeeInformationInDb.OrganizationId

                                };
                                employeeTransferProposals.Add(transferProposal);
                            }
                            employeeInformationInDb.SubSectionId = information.SubSectionId;
                        }
                        if ((information.CostCenterId ?? 0) > 0 && (information.CostCenterId ?? 0) != (employeeInformationInDb.CostCenterId ?? 0))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeeTransferProposal transferProposal = new EmployeeTransferProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    Head = "CostCenter",
                                    Flag = "Transfer",
                                    ExistingValue = employeeInformationInDb.CostCenterId.ToString(),
                                    ProposalValue = information.CostCenterId.ToString(),
                                    IsActive = true,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = DateTime.Now.Date,
                                    ActiveDate = DateTime.Now.Date,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    OrganizationId = employeeInformationInDb.OrganizationId

                                };
                                employeeTransferProposals.Add(transferProposal);
                            }
                            employeeInformationInDb.CostCenterId = information.CostCenterId;
                        }
                        if (information.WorkShiftId > 0 && information.WorkShiftId != employeeInformationInDb.WorkShiftId)
                        {
                            employeeInformationInDb.WorkShiftId = information.WorkShiftId;
                        }
                        if (information.DateOfJoining.HasValue && information.DateOfJoining.Value.Date != employeeInformationInDb.DateOfJoining && employeeInformationInDb.StateStatus == StateStatus.Pending)
                        {
                            employeeInformationInDb.DateOfJoining = information.DateOfJoining;
                        }
                        if (information.DateOfConfirmation.HasValue && (information.DateOfConfirmation.Value.Date != employeeInformationInDb.DateOfConfirmation))
                        {
                            if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                            {
                                EmployeeConfirmationProposal confirmationProposal = new EmployeeConfirmationProposal()
                                {
                                    EmployeeId = employeeInformationInDb.EmployeeId,
                                    StateStatus = StateStatus.Approved,
                                    EffectiveDate = information.DateOfConfirmation,
                                    ConfirmationDate = information.DateOfConfirmation,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = employeeInformationInDb.CompanyId,
                                    OrganizationId = employeeInformationInDb.OrganizationId,
                                    ApprovedBy = user.ActionUserId,
                                    ApprovedDate = DateTime.Now,

                                };
                                employeeConfirmationProposals.Add(confirmationProposal);
                            }
                            employeeInformationInDb.DateOfConfirmation = information.DateOfConfirmation;
                            employeeInformationInDb.IsConfirmed = true;
                        }
                        if (information.AppointmentDate.HasValue && (information.AppointmentDate.Value.Date != employeeInformationInDb.AppointmentDate))
                        {
                            employeeInformationInDb.AppointmentDate = information.AppointmentDate;
                        }
                        if (!information.OfficeMobile.IsNullEmptyOrWhiteSpace() && (information.OfficeMobile != employeeInformationInDb.OfficeMobile))
                        {
                            employeeInformationInDb.OfficeMobile = information.OfficeMobile;
                        }
                        if (!information.OfficeEmail.IsNullEmptyOrWhiteSpace() && (information.OfficeEmail != employeeInformationInDb.OfficeEmail))
                        {
                            employeeInformationInDb.OfficeEmail = information.OfficeEmail;
                        }
                        if (!information.ReferenceNo.IsNullEmptyOrWhiteSpace() && (information.ReferenceNo != employeeInformationInDb.ReferenceNo))
                        {
                            employeeInformationInDb.ReferenceNo = information.ReferenceNo;
                        }
                        if (!information.FingerID.IsNullEmptyOrWhiteSpace() && (information.FingerID != employeeInformationInDb.FingerID))
                        {
                            employeeInformationInDb.FingerID = information.FingerID;
                        }
                        if (!information.JobType.IsNullEmptyOrWhiteSpace() && information.JobType != employeeInformationInDb.JobType
                            && employeeInformationInDb.StateStatus == StateStatus.Pending)
                        {
                            employeeInformationInDb.JobType = information.JobType;
                        }
                        employeeInformationInDb.UpdatedBy = user.ActionUserId;
                        employeeInformationInDb.UpdatedDate = DateTime.Now;
                        line = "End reading of Employee Information";
                        #endregion

                        #region Cost Center
                        line = "Start reading of Cost Center";
                        var costCenterColumns = model.ExcelInfos.Where(i => i.Group == "Cost Center").ToList();
                        if (costCenterColumns.Any() && costCenterColumns != null)
                        {
                            costCenter = new CostCenterDTO();
                            foreach (var item in costCenterColumns)
                            {
                                if (item.Column == "Cost Center ID")
                                {
                                    costCenter.CostCenterCode = item.Value;
                                }
                                else if (item.Column == "Cost Center")
                                {
                                    costCenter.CostCenterName = item.Value;
                                }
                            }

                            if (costCenter != null)
                            {
                                if (!Utility.IsNullEmptyOrWhiteSpace(costCenter.CostCenterName) || !Utility.IsNullEmptyOrWhiteSpace(costCenter.CostCenterCode))
                                {
                                    var costCenterItem = (await _costCenterBusiness.GetCostCentersAsync(new CostCenter_Filter()
                                    {
                                        CostCenterName = costCenter.CostCenterName,
                                        CostCenterCode = costCenter.CostCenterCode
                                    }, user)).FirstOrDefault();

                                    if (costCenterItem == null)
                                    {
                                        var newCostCenter = await _costCenterBusiness.SaveCostCenterAsync(new CostCenterDTO()
                                        {
                                            CostCenterName = costCenter.CostCenterName,
                                            CostCenterCode = costCenter.CostCenterCode,
                                            IsActive = true,
                                        }, user);
                                        information.CostCenterId = (int)newCostCenter.ItemId;
                                    }
                                    else
                                    {
                                        information.CostCenterId = (int)costCenterItem.CostCenterId;
                                    }
                                }

                                if(information.CostCenterId > 0 && information.CostCenterId != (employeeInformationInDb.CostCenterId ?? 0))
                                {
                                    employeeInformationInDb.CostCenterId = information.CostCenterId;
                                }
                            }
                        }
                        line = "End reading of Cost Center";
                        #endregion

                        #region Update Employee Detail
                        line = "Start reading of Employee Detail";
                        var employeeDetailColumns = model.ExcelInfos.Where(i => i.Group == "Employee Detail").ToList();
                        if (employeeDetailColumns.Any() && employeeDetailColumns != null)
                        {
                            detail = new EmployeeDetail();
                            foreach (var item in employeeDetailColumns)
                            {
                                if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                                {
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "Date of birth")
                                        {
                                            if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                            {
                                                detail.DateOfBirth = DateTimeExtension.TryParseDate(item.Value);
                                            }
                                            else
                                            {
                                                detail.DateOfBirth = null;
                                            }
                                        }
                                        else if (item.Column == "Gender")
                                        {
                                            detail.Gender = item.Value;
                                        }
                                        else if (item.Column == "Religion")
                                        {
                                            detail.Religion = item.Value;
                                        }
                                        else if (item.Column == "Legal Name")
                                        {
                                            detail.LegalName = item.Value;
                                        }
                                        else if (item.Column == "Father Name")
                                        {
                                            detail.FatherName = item.Value;
                                        }
                                        else if (item.Column == "Mother Name")
                                        {
                                            detail.MotherName = item.Value;
                                        }
                                        else if (item.Column == "Marital Status")
                                        {
                                            detail.MaritalStatus = item.Value;
                                        }
                                        else if (item.Column == "Spouse Name")
                                        {
                                            detail.SpouseName = item.Value;
                                        }
                                        else if (item.Column == "No Of Child")
                                        {
                                            detail.NumberOfChild = item.Value;
                                        }
                                        else if (item.Column == "Blood Group")
                                        {
                                            detail.BloodGroup = item.Value;
                                        }
                                        else if (item.Column == "Is Residential")
                                        {
                                            detail.IsResidential = BoolExtension.TryParseBool(item.Value);
                                        }
                                        else if (item.Column == "Personal Mobile Number")
                                        {
                                            detail.PersonalMobileNo = item.Value;
                                        }
                                        else if (item.Column == "Personal Email")
                                        {
                                            detail.PersonalEmailAddress = item.Value;
                                        }
                                        else if (item.Column == "Alternative Email Address")
                                        {
                                            detail.AlternativeEmailAddress = item.Value;
                                        }
                                        else if (item.Column == "Present Address")
                                        {
                                            detail.PresentAddress = item.Value;
                                        }
                                        else if (item.Column == "Present Address City")
                                        {
                                            detail.PresentAddressCity = item.Value;
                                        }
                                        else if (item.Column == "Present Address Contact No")
                                        {
                                            detail.PresentAddressContactNo = item.Value;
                                        }
                                        else if (item.Column == "Present Address Zip Code")
                                        {
                                            detail.PresentAddressZipCode = item.Value;
                                        }
                                        else if (item.Column == "Permanent Address")
                                        {
                                            detail.PermanentAddress = item.Value;
                                        }
                                        else if (item.Column == "Permanent Address District")
                                        {
                                            detail.PermanentAddressDistrict = item.Value;
                                        }
                                        else if (item.Column == "Permanent Address Upazila")
                                        {
                                            detail.PermanentAddressUpazila = item.Value;
                                        }
                                        else if (item.Column == "Permanent Address Zip Code")
                                        {
                                            detail.PermanentAddressZipCode = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact Person1")
                                        {
                                            detail.EmergencyContactNo = item.Value;
                                        }
                                        else if (item.Column == "Relation With Emergency Contact Person1")
                                        {
                                            detail.RelationWithEmergencyContactPerson = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact No Person1")
                                        {
                                            detail.EmergencyContactNo = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact Address Person1")
                                        {
                                            detail.EmergencyContactAddress = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact Email Address Person1")
                                        {
                                            detail.EmergencyContactEmailAddress = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact Person2")
                                        {
                                            detail.EmergencyContactNo2 = item.Value;
                                        }
                                        else if (item.Column == "Relation With Emergency Contact Person2")
                                        {
                                            detail.RelationWithEmergencyContactPerson2 = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact No Person2")
                                        {
                                            detail.EmergencyContactNo2 = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact Address Person2")
                                        {
                                            detail.EmergencyContactAddress2 = item.Value;
                                        }
                                        else if (item.Column == "Emergency Contact Email Address Person2")
                                        {
                                            detail.EmergencyContactEmailAddress2 = item.Value;
                                        }
                                    }
                                }
                            }

                            if (!detail.LegalName.IsNullEmptyOrWhiteSpace() && detail.LegalName != employeeDetailInDb.LegalName)
                            {
                                employeeDetailInDb.LegalName = detail.LegalName;
                            }
                            if (!detail.FatherName.IsNullEmptyOrWhiteSpace() && detail.FatherName != employeeDetailInDb.FatherName)
                            {
                                employeeDetailInDb.FatherName = detail.FatherName;
                            }
                            if (!detail.MotherName.IsNullEmptyOrWhiteSpace() && detail.MotherName != employeeDetailInDb.MotherName)
                            {
                                employeeDetailInDb.MotherName = detail.MotherName;
                            }
                            if (!detail.SpouseName.IsNullEmptyOrWhiteSpace() && detail.SpouseName != employeeDetailInDb.SpouseName)
                            {
                                employeeDetailInDb.SpouseName = detail.SpouseName;
                            }
                            if (!detail.PersonalMobileNo.IsNullEmptyOrWhiteSpace() && detail.PersonalMobileNo != employeeDetailInDb.PersonalMobileNo)
                            {
                                employeeDetailInDb.PersonalMobileNo = detail.PersonalMobileNo;
                            }
                            if (!detail.PersonalEmailAddress.IsNullEmptyOrWhiteSpace() && detail.PersonalEmailAddress != employeeDetailInDb.PersonalEmailAddress)
                            {
                                employeeDetailInDb.PersonalEmailAddress = detail.PersonalEmailAddress;
                            }
                            if (!detail.PersonalEmailAddress.IsNullEmptyOrWhiteSpace() && detail.PersonalEmailAddress != employeeDetailInDb.PersonalEmailAddress)
                            {
                                employeeDetailInDb.PersonalEmailAddress = detail.PersonalEmailAddress;
                            }
                            if (detail.DateOfBirth.HasValue && detail.DateOfBirth.Value.Date != employeeDetailInDb.DateOfBirth)
                            {
                                employeeDetailInDb.DateOfBirth = detail.DateOfBirth;
                            }
                            if (!detail.Gender.IsNullEmptyOrWhiteSpace() && detail.Gender != employeeDetailInDb.Gender)
                            {
                                employeeDetailInDb.Gender = detail.Gender;
                            }
                            if (!detail.BloodGroup.IsNullEmptyOrWhiteSpace() && detail.BloodGroup != employeeDetailInDb.BloodGroup)
                            {
                                employeeDetailInDb.BloodGroup = detail.BloodGroup;
                            }
                            if (!detail.MaritalStatus.IsNullEmptyOrWhiteSpace() && detail.MaritalStatus != employeeDetailInDb.MaritalStatus)
                            {
                                employeeDetailInDb.MaritalStatus = detail.MaritalStatus;
                            }
                            if (detail.IsResidential.HasValue && detail.IsResidential != employeeDetailInDb.IsResidential)
                            {
                                employeeDetailInDb.IsResidential = detail.IsResidential;
                            }
                            if (!detail.PresentAddress.IsNullEmptyOrWhiteSpace() && detail.PresentAddress != employeeDetailInDb.PresentAddress)
                            {
                                employeeDetailInDb.PresentAddress = detail.PresentAddress;
                            }
                            if (!detail.NumberOfChild.IsNullEmptyOrWhiteSpace() && detail.NumberOfChild != employeeDetailInDb.NumberOfChild)
                            {
                                employeeDetailInDb.NumberOfChild = detail.NumberOfChild;
                            }
                            if (!detail.PresentAddressCity.IsNullEmptyOrWhiteSpace() && detail.PresentAddressCity != employeeDetailInDb.PresentAddressCity)
                            {
                                employeeDetailInDb.PresentAddressCity = detail.PresentAddressCity;
                            }
                            if (!detail.PresentAddressContactNo.IsNullEmptyOrWhiteSpace() && detail.PresentAddressContactNo != employeeDetailInDb.PresentAddressContactNo)
                            {
                                employeeDetailInDb.PresentAddressContactNo = detail.PresentAddressContactNo;
                            }
                            if (!detail.PresentAddressZipCode.IsNullEmptyOrWhiteSpace() && detail.PresentAddressZipCode != employeeDetailInDb.PresentAddressZipCode)
                            {
                                employeeDetailInDb.PresentAddressZipCode = detail.PresentAddressZipCode;
                            }
                            if (!detail.PermanentAddress.IsNullEmptyOrWhiteSpace() && detail.PermanentAddress != employeeDetailInDb.PermanentAddress)
                            {
                                employeeDetailInDb.PermanentAddress = detail.PermanentAddress;
                            }
                            if (!detail.PermanentAddressDistrict.IsNullEmptyOrWhiteSpace() && detail.PermanentAddressDistrict != employeeDetailInDb.PermanentAddressDistrict)
                            {
                                employeeDetailInDb.PermanentAddressDistrict = detail.PermanentAddressDistrict;
                            }
                            if (!detail.PermanentAddressUpazila.IsNullEmptyOrWhiteSpace() && detail.PermanentAddressUpazila != employeeDetailInDb.PermanentAddressUpazila)
                            {
                                employeeDetailInDb.PermanentAddressUpazila = detail.PermanentAddressUpazila;
                            }
                            if (!detail.PermanentAddressContactNumber.IsNullEmptyOrWhiteSpace() && detail.PermanentAddressContactNumber != employeeDetailInDb.PermanentAddressContactNumber)
                            {
                                employeeDetailInDb.PermanentAddressContactNumber = detail.PermanentAddressContactNumber;
                            }
                            if (!detail.PermanentAddressZipCode.IsNullEmptyOrWhiteSpace() && detail.PermanentAddressZipCode != employeeDetailInDb.PermanentAddressZipCode)
                            {
                                employeeDetailInDb.PermanentAddressZipCode = detail.PermanentAddressZipCode;
                            }
                            if (!detail.EmergencyContactPerson.IsNullEmptyOrWhiteSpace() && detail.EmergencyContactPerson != employeeDetailInDb.EmergencyContactPerson)
                            {
                                employeeDetailInDb.EmergencyContactPerson = detail.EmergencyContactPerson;
                            }
                            if (!detail.RelationWithEmergencyContactPerson.IsNullEmptyOrWhiteSpace() && detail.RelationWithEmergencyContactPerson != employeeDetailInDb.RelationWithEmergencyContactPerson)
                            {
                                employeeDetailInDb.RelationWithEmergencyContactPerson = detail.RelationWithEmergencyContactPerson;
                            }
                            if (!detail.EmergencyContactNo.IsNullEmptyOrWhiteSpace() && detail.EmergencyContactNo != employeeDetailInDb.EmergencyContactNo)
                            {
                                employeeDetailInDb.EmergencyContactNo = detail.EmergencyContactNo;
                            }
                            if (!detail.EmergencyContactAddress.IsNullEmptyOrWhiteSpace() && detail.EmergencyContactAddress != employeeDetailInDb.EmergencyContactAddress)
                            {
                                employeeDetailInDb.EmergencyContactAddress = detail.EmergencyContactAddress;
                            }
                            if (!detail.EmergencyContactEmailAddress.IsNullEmptyOrWhiteSpace() && detail.EmergencyContactEmailAddress != employeeDetailInDb.EmergencyContactEmailAddress)
                            {
                                employeeDetailInDb.EmergencyContactEmailAddress = detail.EmergencyContactEmailAddress;
                            }
                            if (!detail.EmergencyContactPerson2.IsNullEmptyOrWhiteSpace() && detail.EmergencyContactPerson2 != employeeDetailInDb.EmergencyContactPerson2)
                            {
                                employeeDetailInDb.EmergencyContactPerson2 = detail.EmergencyContactPerson2;
                            }
                            if (!detail.EmergencyContactEmailAddress2.IsNullEmptyOrWhiteSpace() && detail.EmergencyContactEmailAddress2 != employeeDetailInDb.EmergencyContactEmailAddress2)
                            {
                                employeeDetailInDb.EmergencyContactEmailAddress2 = detail.EmergencyContactEmailAddress2;
                            }
                            employeeDetailInDb.UpdatedBy = user.ActionUserId;
                            employeeDetailInDb.UpdatedDate = DateTime.Now;
                        }
                        line = "End reading of Employee Detail";
                        #endregion

                        #region Discontinued Employee
                        line = "Start reading of Discontinued Employee";
                        var discontinuedEmployeeColumns = model.ExcelInfos.Where(i => i.Group == "Discontinued Employee").ToList();
                        if (discontinuedEmployeeColumns.Any() && discontinuedEmployeeColumns != null)
                        {
                            var lastWorkingDate = discontinuedEmployeeColumns.FirstOrDefault(i => i.Column == "Last Working Date");
                            if (lastWorkingDate != null && lastWorkingDate.Value.IsNullEmptyOrWhiteSpace() == false)
                            {
                                discontinuedEmployee = new DiscontinuedEmployee();
                                foreach (var item in discontinuedEmployeeColumns)
                                {
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "Last Working Date")
                                        {
                                            if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                            {
                                                discontinuedEmployee.LastWorkingDate = DateTimeExtension.TryParseDate(item.Value);
                                            }
                                        }
                                    }
                                }

                                discontinuedEmployee.EmployeeId = employeeInformationInDb.EmployeeId;
                                discontinuedEmployee.CalculateFestivalBonusTaxProratedBasis = false;
                                discontinuedEmployee.CalculateProjectionTaxProratedBasis = false;
                                discontinuedEmployee.CompanyId = user.CompanyId;
                                discontinuedEmployee.OrganizationId = user.OrganizationId;
                                discontinuedEmployee.CreatedBy = user.ActionUserId;
                                discontinuedEmployee.CreatedDate = DateTime.Now;
                            }
                        }

                        var discontinuedEmployeeInDb = await _discontinuedEmployeeBusiness.GetDiscontinuedEmployeeById(employeeInformationInDb.EmployeeId, user);
                        if (discontinuedEmployeeInDb != null && discontinuedEmployeeInDb.DiscontinuedId > 0 && discontinuedEmployee != null)
                        {
                            if (discontinuedEmployeeInDb.LastWorkingDate.HasValue && discontinuedEmployeeInDb.LastWorkingDate.Value.Date != discontinuedEmployee.LastWorkingDate)
                            {
                                discontinuedEmployeeInDb.LastWorkingDate = discontinuedEmployee.LastWorkingDate;
                            }
                        }
                        line = "End reading of Discontinued Employee";
                        #endregion

                        #region Employee Hierarchy
                        line = "Start reading of Employee Hierarchy";
                        var hierarchyColumns = model.ExcelInfos.Where(i => i.Group == "Employee Hierarchy").ToList();
                        if (hierarchyColumns.Any() && hierarchyColumns != null)
                        {
                            foreach (var item in hierarchyColumns)
                            {
                                if (item.Value.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "Supervisor")
                                        {
                                            if (hierarchy == null)
                                            { hierarchy = new EmployeeHierarchy(); }

                                            if (hierarchy != null)
                                            {
                                                var supervisorInfo = await _employeeInfoBusiness.GetEmployeeInformationByCode(item.Value, user);
                                                hierarchy.SupervisorId = supervisorInfo.EmployeeId;
                                            }
                                        }
                                        if (item.Column == "HOD")
                                        {
                                            if (hierarchy == null)
                                            { hierarchy = new EmployeeHierarchy(); }
                                            if (hierarchy != null)
                                            {
                                                var hodInfo = await _employeeInfoBusiness.GetEmployeeInformationByCode(item.Value, user);
                                                hierarchy.HeadOfDepartmentId = hodInfo.EmployeeId;
                                            }
                                        }
                                    }
                                }

                            }
                            if (hierarchy != null)
                            {
                                hierarchy.EmployeeId = employeeInformationInDb.EmployeeId;
                                hierarchy.IsActive = true;
                                hierarchy.CompanyId = user.CompanyId;
                                hierarchy.OrganizationId = user.OrganizationId;
                                hierarchy.CreatedBy = user.ActionUserId;
                                hierarchy.CreatedDate = DateTime.Now;
                            }
                        }

                        var hierarchyInDb = await _employeeHierarchyRepository.GetEmployeeActiveHierarchy(employeeInformationInDb.EmployeeId, user);
                        if (hierarchy != null && hierarchyInDb != null && hierarchyInDb.Id > 0)
                        {
                            hierarchyInDb.IsActive = false;
                            hierarchyInDb.UpdatedBy = user.ActionUserId;
                            hierarchyInDb.UpdatedDate = DateTime.Now;
                        }
                        line = "End reading of Employee Hierarchy";
                        #endregion

                        #region Employee Documents
                        line = "Start reading of Employee Documents";
                        var documentColumns = model.ExcelInfos.Where(i => i.Group == "Employee Document").ToList();
                        if (documentColumns.Any() && documentColumns != null)
                        {
                            employeeDocuments = new List<EmployeeDocument>();
                            foreach (var item in documentColumns)
                            {
                                if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                                {
                                    // ["Birth Certificate", "NID", "Passport", "TIN","Driving License"]
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "Birth Certificate")
                                        {
                                            var employeeDocument = new EmployeeDocument();
                                            employeeDocument.DocumentName = "Birth Certificate";
                                            employeeDocument.DocumentNumber = item.Value;
                                            employeeDocument.CompanyId = user.CompanyId;
                                            employeeDocument.OrganizationId = user.OrganizationId;
                                            employeeDocument.CreatedBy = user.ActionUserId;
                                            employeeDocument.CreatedDate = DateTime.Now;
                                            employeeDocuments.Add(employeeDocument);
                                        }
                                        else if (item.Column == "NID")
                                        {
                                            var employeeDocument = new EmployeeDocument();
                                            employeeDocument.DocumentName = "NID";
                                            employeeDocument.DocumentNumber = item.Value;
                                            employeeDocument.CompanyId = user.CompanyId;
                                            employeeDocument.OrganizationId = user.OrganizationId;
                                            employeeDocument.CreatedBy = user.ActionUserId;
                                            employeeDocument.CreatedDate = DateTime.Now;
                                            employeeDocuments.Add(employeeDocument);
                                        }
                                        else if (item.Column == "Passport")
                                        {
                                            var employeeDocument = new EmployeeDocument();
                                            employeeDocument.DocumentName = "Passport";
                                            employeeDocument.DocumentNumber = item.Value;
                                            employeeDocument.CompanyId = user.CompanyId;
                                            employeeDocument.OrganizationId = user.OrganizationId;
                                            employeeDocument.CreatedBy = user.ActionUserId;
                                            employeeDocument.CreatedDate = DateTime.Now;
                                            employeeDocuments.Add(employeeDocument);
                                        }
                                        else if (item.Column == "TIN")
                                        {
                                            var employeeDocument = new EmployeeDocument();
                                            employeeDocument.DocumentName = "TIN";
                                            employeeDocument.DocumentNumber = item.Value;
                                            employeeDocument.CompanyId = user.CompanyId;
                                            employeeDocument.OrganizationId = user.OrganizationId;
                                            employeeDocument.CreatedBy = user.ActionUserId;
                                            employeeDocument.CreatedDate = DateTime.Now;
                                            employeeDocuments.Add(employeeDocument);
                                        }
                                        else if (item.Column == "Driving License")
                                        {
                                            var employeeDocument = new EmployeeDocument();
                                            employeeDocument.DocumentName = "Driving License";
                                            employeeDocument.DocumentNumber = item.Value;
                                            employeeDocument.CompanyId = user.CompanyId;
                                            employeeDocument.OrganizationId = user.OrganizationId;
                                            employeeDocument.CreatedBy = user.ActionUserId;
                                            employeeDocument.CreatedDate = DateTime.Now;
                                            employeeDocuments.Add(employeeDocument);
                                        }
                                    }
                                }
                            }
                        }
                        line = "End reading of Employee Documents";
                        #endregion

                        #region Employee Account Info
                        line = "Start reading of Account Info";
                        var employeeAccountColumns = model.ExcelInfos.Where(i => i.Group == "Account Information").ToList();
                        var employeeAccountInDb = employeeInformationInDb.StateStatus == StateStatus.Approved ?
                            await _accountInfoBusines.GetActiveAccountInfoByEmployeeId(employeeInformationInDb.EmployeeId, user) :
                            await _accountInfoBusines.GetPendingAccountInfoByEmployeeId(employeeInformationInDb.EmployeeId, user);
                        if (employeeAccountColumns.Any())
                        {
                            accountInfo = new EmployeeAccountInfo();
                            foreach (var item in employeeAccountColumns)
                            {
                                if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                                {
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "Bank Name")
                                        {
                                            var bankItem = (await _bankBusiness.GetBanksAsync(new Bank_Filter()
                                            {
                                                BankName = item.Value
                                            }, user)).FirstOrDefault();
                                            if (bankItem == null)
                                            {
                                                var newBank = await _bankBusiness.SaveBankAsync(new BankDTO()
                                                {
                                                    BankName = item.Value,
                                                }, user);

                                                accountInfo.BankId = (int)newBank.ItemId;
                                            }
                                            else
                                            {
                                                accountInfo.BankId = (int)bankItem.BankId;
                                            }
                                        }
                                        else if (item.Column == "Bank Branch Name")
                                        {
                                            var bankBranchItem = (await _bankBranchBusiness.GetBankBranchesAsync(new BankBranch_Filter()
                                            {
                                                BankBranchName = item.Value,
                                                BankId = accountInfo.BankId.ToString()
                                            }, user)).FirstOrDefault();

                                            if (bankBranchItem == null)
                                            {
                                                var routingNumber = employeeAccountColumns.FirstOrDefault(i => i.Column == "Routing Number");
                                                var newBankBranch = await _bankBranchBusiness.SaveBankBranchAsync(new BankBranchDTO()
                                                {
                                                    BankId = accountInfo.BankId ?? 0,
                                                    BankBranchName = item.Value,
                                                    RoutingNumber = routingNumber != null ? routingNumber.Value : ""
                                                }, user);
                                                accountInfo.BankBranchId = (int)newBankBranch.ItemId;
                                            }
                                            else
                                            {
                                                accountInfo.BankBranchId = (int)bankBranchItem.BankBranchId;
                                            }
                                        }
                                        else if (item.Column == "Account Number")
                                        {
                                            accountInfo.AccountNo = item.Value;
                                        }
                                    }
                                }
                            }
                            if ((accountInfo.BankId ?? 0) > 0 && (accountInfo.BankBranchId ?? 0) > 0)
                            {
                                if (employeeAccountInDb != null && employeeAccountInDb.StateStatus == StateStatus.Approved)
                                {
                                    accountInfo.EmployeeId = employeeInformationInDb.EmployeeId;
                                    accountInfo.DepartmentId = employeeInformationInDb.DepartmentId;
                                    accountInfo.DesignationId = employeeInformationInDb.DesignationId;
                                    accountInfo.ActivationReason = "New Joiner";
                                    accountInfo.CompanyId = employeeInformationInDb.CompanyId;
                                    accountInfo.OrganizationId = employeeInformationInDb.OrganizationId;
                                    accountInfo.CreatedBy = user.ActionUserId;
                                    accountInfo.CreatedDate = DateTime.Now;
                                    if (employeeInformationInDb.StateStatus == StateStatus.Approved)
                                    {
                                        accountInfo.StateStatus = StateStatus.Approved;
                                        accountInfo.IsActive = true;
                                        accountInfo.IsApproved = true;
                                        accountInfo.ApprovedBy = user.ActionUserId;
                                        accountInfo.ApprovedDate = DateTime.Now;
                                    }

                                    employeeAccountInDb.IsActive = false;
                                    employeeAccountInDb.UpdatedBy = user.ActionUserId;
                                    employeeAccountInDb.UpdatedDate = DateTime.Now;
                                }
                                if (employeeAccountInDb != null && employeeAccountInDb.StateStatus == StateStatus.Pending)
                                {
                                    employeeAccountInDb.DepartmentId = employeeInformationInDb.DepartmentId;
                                    employeeAccountInDb.DesignationId = employeeInformationInDb.DesignationId;
                                    employeeAccountInDb.BankId = accountInfo.BankId;
                                    employeeAccountInDb.BankBranchId = accountInfo.BankBranchId;
                                    employeeAccountInDb.AccountNo = accountInfo.AccountNo;
                                    employeeAccountInDb.ActivationReason = "Others";
                                    employeeAccountInDb.IsActive = false;
                                    employeeAccountInDb.UpdatedBy = user.ActionUserId;
                                    employeeAccountInDb.UpdatedDate = DateTime.Now;
                                }
                            }
                        }
                        line = "End reading of Account Info";
                        #endregion

                        #region PF Activation
                        line = "Start reading of PF Activation";
                        var employeePFColumns = model.ExcelInfos.Where(i => i.Group == "PF Activation").ToList();
                        if (employeePFColumns.Any() && employeePFColumns != null)
                        {
                            employeePFActivation = new EmployeePFActivation();
                            foreach (var item in employeePFColumns)
                            {
                                if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                                {
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "PF Confirmation")
                                        {
                                            if (information != null)
                                            {
                                                information.IsPFMember = BoolExtension.TryParseBool(item.Value);
                                            }
                                        }
                                        if (item.Column == "PF Confirmation Date")
                                        {
                                            if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                            {
                                                employeePFActivation.PFEffectiveDate = DateTimeExtension.TryParseDate(item.Value);
                                            }
                                            else
                                            {
                                                employeePFActivation.PFEffectiveDate = null;
                                            }
                                        }
                                        if (item.Column == "PF Activation Date")
                                        {
                                            if (column_info.DataType.IsNullEmptyOrWhiteSpace() == false && column_info.DataType == "Date")
                                            {
                                                employeePFActivation.PFActivationDate = DateTimeExtension.TryParseDate(item.Value);
                                            }
                                            else
                                            {
                                                employeePFActivation.PFActivationDate = null;
                                            }
                                        }
                                    }
                                }
                            }
                            if (employeePFActivation != null && (employeePFActivation.PFActivationDate.HasValue || employeePFActivation.PFEffectiveDate.HasValue))
                            {
                                var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);
                                if (payrollModuleConfig != null && payrollModuleConfig.BaseOfProvidentFund.IsNullEmptyOrWhiteSpace() == false && Utility.TryParseDecimal(payrollModuleConfig.PercentageOfProvidentFund) > 0)
                                {
                                    employeePFActivation.PFBasedAmount = payrollModuleConfig.BaseOfProvidentFund;
                                    employeePFActivation.StateStatus = StateStatus.Pending;
                                    employeePFActivation.PFPercentage = Utility.TryParseDecimal(payrollModuleConfig.PercentageOfProvidentFund);

                                    if (employeePFActivation.PFEffectiveDate.HasValue && !employeePFActivation.PFActivationDate.HasValue)
                                    {
                                        employeePFActivation.PFActivationDate = employeePFActivation.PFEffectiveDate.Value;
                                    }
                                    if (employeePFActivation.PFActivationDate.HasValue && !employeePFActivation.PFEffectiveDate.HasValue)
                                    {
                                        employeePFActivation.PFEffectiveDate = employeePFActivation.PFActivationDate.Value;
                                    }
                                }
                                else
                                {
                                    employeePFActivation = null;
                                }
                            }
                            else
                            {
                                employeePFActivation = null;
                            }

                        }
                        line = "End reading of PF Activation";
                        #endregion

                        #region Salary Review
                        line = "Start reading of Salary Review";
                        var salaryReviewColumns = model.ExcelInfos.Where(i => i.Group == "Salary Review").ToList();
                        if (salaryReviewColumns.Any() && salaryReviewColumns != null)
                        {
                            salaryReview = new SalaryReviewFromEmployeeUploader();
                            foreach (var item in salaryReviewColumns)
                            {
                                if (Utility.IsNullEmptyOrWhiteSpace(item.Value) == false)
                                {
                                    var column_info = column_list.FirstOrDefault(i => i.Column == item.Column);
                                    if (column_info != null)
                                    {
                                        if (item.Column == "Gross Salary")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                salaryReview.GrossSalary = Convert.ToDecimal(item.Value);
                                            }
                                        }
                                        else if (item.Column == "Basic Salary")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                salaryReview.BasicSalary = Convert.ToDecimal(item.Value);
                                            }
                                        }
                                        else if (item.Column == "House Rent")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                salaryReview.HouseRent = Convert.ToDecimal(item.Value);
                                            }
                                        }
                                        else if (item.Column == "Medical")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                salaryReview.Medical = Convert.ToDecimal(item.Value);
                                            }
                                        }
                                        else if (item.Column == "Conveyance")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                salaryReview.Conveyance = Convert.ToDecimal(item.Value);
                                            }
                                        }
                                        else if (item.Column == "LFA")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                salaryReview.LFA = Convert.ToDecimal(item.Value);
                                            }
                                        }
                                        else if (item.Column == "Salary Effective Date")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                try
                                                {
                                                    var val = Convert.ToDouble(item.Value.RemoveWhitespace());
                                                    salaryReview.SalaryEffectiveDate = DateTime.FromOADate(val);
                                                }
                                                catch (Exception ex)
                                                {
                                                    salaryReview.SalaryEffectiveDate = information != null ? information.DateOfJoining : null;
                                                    Console.WriteLine(ex);
                                                }
                                            }
                                            else
                                            {
                                                var date = new DateTime();
                                                if (DateTime.TryParse(item.Value.RemoveWhitespace(), out date))
                                                {
                                                    salaryReview.SalaryEffectiveDate = date;
                                                }
                                                else
                                                {
                                                    salaryReview.SalaryEffectiveDate = information != null ? information.DateOfJoining : null;
                                                }
                                            }
                                        }
                                        else if (item.Column == "Salary Activation Date")
                                        {
                                            if (item.Value.IsStringNumber())
                                            {
                                                try
                                                {
                                                    var val = Convert.ToDouble(item.Value.RemoveWhitespace());
                                                    salaryReview.SalaryActivationDate = DateTime.FromOADate(val);
                                                }
                                                catch (Exception ex)
                                                {
                                                    salaryReview.SalaryActivationDate = information != null ? information.DateOfJoining : null;
                                                    Console.WriteLine(ex);
                                                }
                                            }
                                            else
                                            {
                                                var date = new DateTime();
                                                if (DateTime.TryParse(item.Value.RemoveWhitespace(), out date))
                                                {
                                                    salaryReview.SalaryActivationDate = date;
                                                }
                                                else
                                                {
                                                    salaryReview.SalaryActivationDate = information != null ? information.DateOfJoining : null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (salaryReview != null)
                            {
                                // Find Salary Review Type
                                if (salaryReview.BasicSalary > 0 && (salaryReview.HouseRent > 0 || salaryReview.Medical > 0 || salaryReview.Conveyance > 0 || salaryReview.LFA > 0))
                                {
                                    salaryReview.ConfigType = "Flat";
                                }
                                else if (salaryReview.GrossSalary == 0 && salaryReview.BasicSalary > 0)
                                {
                                    salaryReview.ConfigType = "Basic";
                                }
                                else if (salaryReview.GrossSalary > 0 && salaryReview.BasicSalary == 0)
                                {
                                    salaryReview.ConfigType = "Gross";
                                }
                            }
                        }
                        line = "End reading of Salary Review";
                        #endregion

                        var totalSalaryCount = await _salaryProcessBusiness.GetSalaryReceiptInTotalAsync(employeeInformationInDb.EmployeeId, user);

                        try
                        {
                            #region Save Employee Information & Detail
                            line = "Save of Employee Information & Detail";
                            using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    employeeInformationInDb.UpdatedBy = user.ActionUserId;
                                    employeeInformationInDb.UpdatedDate = DateTime.Now;
                                    employeeInformationInDb.FullName = employeeInformationInDb.FirstName + " " + employeeInformationInDb.LastName;
                                    if (totalSalaryCount > 0)
                                    {
                                        employeeDetailInDb.Gender = employeeDetailInDb2.Gender;
                                        employeeDetailInDb.IsResidential = employeeDetailInDb2.IsResidential;
                                    }
                                    _employeeModuleDbContext.HR_EmployeeInformation.Update(employeeInformationInDb);
                                    _employeeModuleDbContext.HR_EmployeeDetail.Update(employeeDetailInDb);

                                    if (employeeTransferProposals.Any())
                                    {
                                        await _employeeModuleDbContext.HR_EmployeeTransferProposal.AddRangeAsync(employeeTransferProposals);
                                    }
                                    if (employeePromotionProposals.Any())
                                    {
                                        await _employeeModuleDbContext.HR_EmployeePromotionProposal.AddRangeAsync(employeePromotionProposals);
                                    }
                                    if (employeeConfirmationProposals.Any())
                                    {
                                        await _employeeModuleDbContext.HR_EmployeeConfirmationProposal.AddRangeAsync(employeeConfirmationProposals);
                                    }
                                    if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                    {
                                        await transaction.CommitAsync();
                                    }
                                    else
                                    {
                                        await transaction.RollbackAsync();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    await transaction.RollbackAsync();
                                }
                            }
                            #endregion

                            #region Save Discontinued Employee 
                            line = "Save of Discontinued Employee ";
                            if (discontinuedEmployee != null)
                            {
                                using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                                {
                                    try
                                    {
                                        var employeeInDb = await _employeeInfoBusiness.GetEmployeeInformationById(model.Id, user);
                                        if (discontinuedEmployeeInDb != null)
                                        {
                                            discontinuedEmployeeInDb.UpdatedBy = user.ActionUserId;
                                            discontinuedEmployeeInDb.UpdatedDate = DateTime.Now;
                                            if (discontinuedEmployeeInDb.StateStatus == StateStatus.Approved)
                                            {
                                                employeeInDb.TerminationDate = discontinuedEmployeeInDb.LastWorkingDate;
                                                employeeInDb.TerminationStatus = StateStatus.Approved;
                                                employeeInDb.UpdatedBy = user.ActionUserId;
                                                employeeInDb.UpdatedDate = DateTime.Now;
                                                _employeeModuleDbContext.HR_EmployeeInformation.Update(employeeInDb);
                                            }
                                            _employeeModuleDbContext.HR_DiscontinuedEmployee.Update(discontinuedEmployeeInDb);
                                            if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                            {
                                                await transaction.CommitAsync();
                                            }
                                            else
                                            {
                                                await transaction.RollbackAsync();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        await transaction.RollbackAsync();
                                        Console.WriteLine(ex);
                                    }
                                }
                            }
                            #endregion

                            #region Save Employee Hierarchy
                            line = "Save of Employee Hierarchy";
                            if (hierarchy != null)
                            {
                                using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                                {
                                    try
                                    {
                                        hierarchy.IsActive = true;
                                        if (hierarchyInDb != null)
                                        {
                                            _employeeModuleDbContext.HR_EmployeeHierarchy.Update(hierarchyInDb);
                                        }
                                        await _employeeModuleDbContext.HR_EmployeeHierarchy.AddAsync(hierarchy);
                                        if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                        {
                                            await transaction.CommitAsync();
                                        }
                                        else
                                        {
                                            await transaction.RollbackAsync();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        await transaction.RollbackAsync();
                                        Console.WriteLine(ex);
                                    }
                                }
                            }
                            #endregion

                            #region Save Employee Document
                            line = "Save of Employee Document";
                            if (employeeDocuments != null && employeeDocuments.Any())
                            {
                                foreach (var item in employeeDocuments)
                                {
                                    var documentInDb = await _documentRepository.GetDocumentByEmployeeIdAsync(item.EmployeeId, item.DocumentName, user);
                                    using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                                    {
                                        try
                                        {
                                            if (documentInDb != null && documentInDb.EmployeeId > 0)
                                            {
                                                documentInDb.DocumentNumber = item.DocumentName;
                                                documentInDb.UpdatedBy = user.ActionUserId;
                                                documentInDb.UpdatedDate = DateTime.Now;
                                                _employeeModuleDbContext.HR_EmployeeDocument.Update(documentInDb);
                                            }
                                            else
                                            {
                                                await _employeeModuleDbContext.HR_EmployeeDocument.AddAsync(item);
                                            }

                                            if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                            {
                                                await transaction.CommitAsync();
                                            }
                                            else
                                            {
                                                await transaction.RollbackAsync();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await transaction.RollbackAsync();
                                            Console.WriteLine(ex);
                                        }
                                    }

                                }
                            }
                            #endregion

                            #region Save Employee Account
                            line = "Save of Employee Account";
                            if (accountInfo != null)
                            {
                                using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                                {
                                    try
                                    {
                                        if (employeeAccountInDb != null && employeeAccountInDb.StateStatus == StateStatus.Pending)
                                        {
                                            _employeeModuleDbContext.HR_EmployeeAccountInfo.Update(employeeAccountInDb);
                                        }
                                        else if (employeeAccountInDb != null && employeeAccountInDb.StateStatus == StateStatus.Approved)
                                        {
                                            await _employeeModuleDbContext.HR_EmployeeAccountInfo.AddAsync(accountInfo);
                                            _employeeModuleDbContext.HR_EmployeeAccountInfo.Update(employeeAccountInDb);
                                        }
                                        else if (employeeAccountInDb == null)
                                        {
                                            await _employeeModuleDbContext.HR_EmployeeAccountInfo.AddAsync(accountInfo);
                                        }
                                        if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                        {
                                            await transaction.CommitAsync();
                                        }
                                        else
                                        {
                                            await transaction.RollbackAsync();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        await transaction.RollbackAsync();
                                        Console.WriteLine(ex);
                                    }
                                }
                            }
                            #endregion

                            #region Save PF Activation
                            line = "Save of PF Activation";
                            if (employeePFActivation != null)
                            {
                                using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                                {
                                    try
                                    {
                                        var employeeInDb = await _employeeInfoBusiness.GetEmployeeInformationById(model.Id, user);
                                        if (employeeInDb != null && employeeInDb.PFActivationDate == null && employeePFActivation.PFActivationDate.HasValue)
                                        {
                                            employeeInDb.PFActivationDate = employeePFActivation.PFActivationDate;
                                            employeeInDb.IsPFMember = employeePFActivation.PFActivationDate.Value.Date <= DateTimeExtension.LastDateOfThisMonth();
                                            employeeInDb.UpdatedBy = user.ActionUserId;
                                            employeeInDb.UpdatedDate = DateTime.Now;

                                            _employeeModuleDbContext.HR_EmployeeInformation.Update(employeeInDb);
                                            await _employeeModuleDbContext.HR_EmployeePFActivation.AddAsync(employeePFActivation);
                                            if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                                            {
                                                await transaction.CommitAsync();
                                            }
                                            else
                                            {
                                                await transaction.RollbackAsync();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        await transaction.RollbackAsync();
                                        Console.WriteLine(ex);
                                    }
                                }
                            }
                            #endregion

                            #region Save Salary Review
                            line = "Save of Salary Review";
                            if (employeeInformationInDb != null && employeeInformationInDb.EmployeeId > 0 && salaryReview != null)
                            {
                                var lastSalaryReviewInfo =
                                    await _salaryReviewBusiness.GetLastSalaryReviewInfoByEmployeeAsync(employeeInformationInDb.EmployeeId, user);
                                List<SalaryReviewDetail> salaryReviewDetails = new List<SalaryReviewDetail>();
                                if (salaryReview.ConfigType == "Flat")
                                {
                                    if (salaryReview.BasicSalary > 0)
                                    {
                                        var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                        {
                                            AllowanceFlag = "BASIC"
                                        }, user)).FirstOrDefault();
                                        if (allowance != null)
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                AllowanceName = allowance.Name,
                                                AllowanceBase = salaryReview.ConfigType,
                                                AllowanceAmount = salaryReview.BasicSalary,
                                                AllowancePercentage = 0,
                                                CurrentAmount = salaryReview.BasicSalary,
                                                AdditionalAmount = 0,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = 0
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                    if (salaryReview.HouseRent > 0)
                                    {
                                        var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                        {
                                            AllowanceFlag = "HR"
                                        }, user)).FirstOrDefault();
                                        if (allowance != null)
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                AllowanceName = allowance.Name,
                                                AllowanceBase = salaryReview.ConfigType,
                                                AllowanceAmount = salaryReview.HouseRent,
                                                AllowancePercentage = 0,
                                                CurrentAmount = salaryReview.HouseRent,
                                                AdditionalAmount = 0,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = 0
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                    if (salaryReview.Medical > 0)
                                    {
                                        var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                        {
                                            AllowanceFlag = "MEDICAL"
                                        }, user)).FirstOrDefault();
                                        if (allowance != null)
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                AllowanceName = allowance.Name,
                                                AllowanceBase = salaryReview.ConfigType,
                                                AllowanceAmount = salaryReview.Medical,
                                                AllowancePercentage = 0,
                                                CurrentAmount = salaryReview.Medical,
                                                AdditionalAmount = 0,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = 0
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                    if (salaryReview.Conveyance > 0)
                                    {
                                        var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                        {
                                            AllowanceFlag = "CONVEYANCE"
                                        }, user)).FirstOrDefault();
                                        if (allowance != null)
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                AllowanceName = allowance.Name,
                                                AllowanceBase = salaryReview.ConfigType,
                                                AllowanceAmount = salaryReview.Conveyance,
                                                AllowancePercentage = 0,
                                                CurrentAmount = salaryReview.Conveyance,
                                                AdditionalAmount = 0,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = 0
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                    if (salaryReview.LFA > 0)
                                    {
                                        var allowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                                        {
                                            AllowanceFlag = "LFA"
                                        }, user)).FirstOrDefault();
                                        if (allowance != null)
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                AllowanceName = allowance.Name,
                                                AllowanceBase = salaryReview.ConfigType,
                                                AllowanceAmount = salaryReview.LFA,
                                                AllowancePercentage = 0,
                                                CurrentAmount = salaryReview.LFA,
                                                AdditionalAmount = 0,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = 0
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                }
                                if (salaryReview.ConfigType == "Gross")
                                {
                                    var grossAmount = salaryReview.GrossSalary;
                                    var allowances = await _salaryReviewBusiness.GetSalaryAllowanceForReviewAsync(employeeInformationInDb.EmployeeId, user);
                                    var flatAmount = allowances.Where(i => i.AllowanceBase == "Flat").Sum(i => i.AllowanceAmount);
                                    var actualGross = grossAmount - flatAmount ?? 0;
                                    var remainAmount = actualGross;
                                    foreach (var allowance in allowances)
                                    {
                                        if (allowance.AllowanceBase == "Gross")
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AdditionalAmount = 0,
                                                AllowanceBase = "Gross",
                                                AllowanceAmount = allowance.AllowanceAmount,
                                                AllowancePercentage = allowance.AllowancePercentage,
                                                AllowanceName = allowance.AllowanceName,
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                CurrentAmount = Math.Round(((actualGross) / 100 * allowance.AllowancePercentage), MidpointRounding.AwayFromZero),
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                            };
                                            remainAmount = remainAmount - salaryReviewDetail.CurrentAmount;
                                            if (remainAmount < 0 && Math.Abs(remainAmount) > 0)
                                            {
                                                salaryReviewDetail.CurrentAmount = salaryReviewDetail.CurrentAmount - remainAmount;
                                            }
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                        if (allowance.AllowanceBase == "Flat")
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AdditionalAmount = 0,
                                                AllowanceBase = "Flat",
                                                AllowanceAmount = allowance.AllowanceAmount,
                                                AllowancePercentage = allowance.AllowancePercentage,
                                                AllowanceName = allowance.AllowanceName,
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                BranchId = employeeInformationInDb.BranchId,
                                                CompanyId = employeeInformationInDb.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                CurrentAmount = allowance.AllowanceAmount ?? 0,
                                                EmployeeId = employeeInformationInDb.EmployeeId,
                                                OrganizationId = employeeInformationInDb.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                }
                                if (salaryReview.ConfigType == "Basic")
                                {
                                    var basicAmount = salaryReview.BasicSalary;
                                    var allowances = await _salaryReviewBusiness.GetSalaryAllowanceForReviewAsync(information.EmployeeId, user);
                                    foreach (var allowance in allowances)
                                    {
                                        if (allowance.AllowanceBase == "Basic")
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AdditionalAmount = 0,
                                                AllowanceBase = "Gross",
                                                AllowanceAmount = allowance.AllowanceAmount,
                                                AllowancePercentage = allowance.AllowancePercentage,
                                                AllowanceName = allowance.AllowanceName,
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                BranchId = information.BranchId,
                                                CompanyId = information.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                CurrentAmount = Math.Round(((basicAmount) / 100 * allowance.AllowancePercentage), MidpointRounding.AwayFromZero),
                                                EmployeeId = information.EmployeeId,
                                                OrganizationId = information.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                        if (allowance.AllowanceBase == "Flat")
                                        {
                                            SalaryReviewDetail salaryReviewDetail = new SalaryReviewDetail()
                                            {
                                                AdditionalAmount = 0,
                                                AllowanceBase = "Flat",
                                                AllowanceAmount = allowance.AllowanceAmount,
                                                AllowancePercentage = allowance.AllowancePercentage,
                                                AllowanceName = allowance.AllowanceName,
                                                AllowanceNameId = allowance.AllowanceNameId,
                                                BranchId = information.BranchId,
                                                CompanyId = information.CompanyId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                CurrentAmount = allowance.AllowanceAmount ?? 0,
                                                EmployeeId = information.EmployeeId,
                                                OrganizationId = information.OrganizationId,
                                                PreviousAmount = 0,
                                                SalaryAllowanceConfigDetailId = allowance.SalaryAllowanceConfigDetailId
                                            };
                                            salaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                }

                                decimal previousSalaryAmount = 0;
                                if (lastSalaryReviewInfo != null && lastSalaryReviewInfo.EmployeeLastApprovedSalaryReviewDetails.Any())
                                {
                                    previousSalaryAmount = lastSalaryReviewInfo.EmployeeLastApprovedSalaryReviewDetails.Sum(i => i.CurrentAmount);
                                    foreach (var item in salaryReviewDetails)
                                    {
                                        var currentAllowance = lastSalaryReviewInfo.EmployeeLastApprovedSalaryReviewDetails.FirstOrDefault(i => i.AllowanceNameId == item.AllowanceNameId);
                                        if (currentAllowance != null)
                                        {
                                            item.PreviousAmount = (item.CurrentAmount - currentAllowance.CurrentAmount) > 0 ? (item.CurrentAmount - currentAllowance.CurrentAmount) : 0;
                                        }
                                    }
                                }
                                if (salaryReviewDetails.Any())
                                {
                                    SalaryReviewInfo salaryReviewInfo = new SalaryReviewInfo()
                                    {
                                        ActivationDate = salaryReview.SalaryActivationDate,
                                        ArrearCalculatedDate = salaryReview.SalaryActivationDate,
                                        BaseType = salaryReview.ConfigType,
                                        BranchId = employeeInformationInDb.BranchId,
                                        CompanyId = employeeInformationInDb.CompanyId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        DepartmentId = employeeInformationInDb.DepartmentId,
                                        DesignationId = employeeInformationInDb.DesignationId,
                                        EffectiveFrom = salaryReview.SalaryEffectiveDate,
                                        EmployeeId = employeeInformationInDb.EmployeeId,
                                        IncrementReason = "Others",
                                        PreviousSalaryAmount = previousSalaryAmount,
                                        SalaryAllowanceConfigId = 0,
                                        SalaryBaseAmount = salaryReviewDetails.Sum(i => i.CurrentAmount),
                                        SalaryReviewDetails = salaryReviewDetails,
                                        IsAutoCalculate = false,
                                        SectionId = employeeInformationInDb.SectionId,
                                        StateStatus = StateStatus.Pending,
                                        CurrentSalaryAmount = salaryReviewDetails.Sum(i => i.CurrentAmount),
                                        IsArrearCalculated = false,
                                        IsActive = false,
                                        IsApproved = false,
                                        OrganizationId = employeeInformationInDb.OrganizationId,
                                        SalaryConfigCategory = salaryReview.ConfigType,
                                        InternalDesignationId = employeeInformationInDb.InternalDesignationId,
                                        PreviousReviewId = lastSalaryReviewInfo != null ? lastSalaryReviewInfo.SalaryReviewInfoId : 0
                                    };
                                    using (var transaction = await _employeeModuleDbContext.Database.BeginTransactionAsync())
                                    {
                                        try
                                        {
                                            await _payrollDbContext.AddAsync(salaryReviewInfo);
                                            if (await _payrollDbContext.SaveChangesAsync() > 0)
                                            {
                                                await transaction.CommitAsync();
                                            }
                                            else
                                            {
                                                await transaction.RollbackAsync();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex);
                                            await transaction.RollbackAsync();
                                        }
                                    }
                                }
                            }
                            #endregion

                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Message(false, ex.Message + "" + ex.StackTrace + " " + line);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ex.Message + "" + ex.StackTrace + " " + line);
                Console.WriteLine(ex);
            }
            executionStatus.Code = model.Code;
            executionStatus.Action = line;

            return executionStatus;
        }
        public async Task<bool> IsDuplicate(long employeeId, string column, string value, List<ExcelInfoCollection> employees, AppUser user)
        {
            bool IsValidate = false;
            try
            {
                if (employeeId == 0)
                {
                    foreach (var employee in employees)
                    {
                        foreach (var col in employee.ExcelInfos)
                        {
                            if (col.Column == column)
                            {
                                if (col.Value == value)
                                {
                                    IsValidate = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                // Check in list 
                if (IsValidate == false)
                {
                    if (column == "Account Number")
                    {
                        IsValidate = await _accountInfoBusines.IsAccountNumberAlreadyActive(value, user);
                    }
                    if (column == "TIN" || column == "NID" || column == "Driving License" || column == "Passport" || column == "Birth Certificate")
                    {
                        IsValidate = await _documentRepository.IsDocumentAlreadyExistInAnotherEmployeeAsync(employeeId, column, value, user);
                    }
                    if (column == "Office Email")
                    {
                        IsValidate = await _employeeInfoBusiness.IsOfficeEmailAvailableAsync(employeeId, value, user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return IsValidate;
        }
    }
}
