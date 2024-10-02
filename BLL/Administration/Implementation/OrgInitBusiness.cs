using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.DataService;
using DAL.UnitOfWork.Control_Panel.Interface;
using DAL.DapperObject.Interface;
using DAL.Repository.Control_Panel;
using Shared.Control_Panel.ViewModels;
using Shared.Control_Panel.Domain;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class OrgInitBusiness : IOrgInitBusiness
    {
        private readonly IControlPanelUnitOfWork _controlPanelDbContext;
        private readonly IMapper _mapper;
        private readonly IDapperData _dapperData;
        private readonly OrganizationRepository _organizationRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly ApplicationRepository _applicationRepository;
        private readonly DivisionRepository _divisionRepository;
        private readonly DistrictRepository _districtRepository;
        private readonly ZoneRepository _zoneRepository;
        private readonly BranchRepository _branchRepository;
        private readonly ISysLogger _sysLogger;
        private string sqlQuery = null;
        public OrgInitBusiness(IControlPanelUnitOfWork controlPanelDbContext, IMapper mapper, IDapperData dapperData, ISysLogger sysLogger)
        {
            _controlPanelDbContext = controlPanelDbContext;
            _sysLogger = sysLogger;
            _mapper = mapper;
            _dapperData = dapperData;
            _organizationRepository = new OrganizationRepository(_controlPanelDbContext);
            _applicationRepository = new ApplicationRepository(_controlPanelDbContext);
            _companyRepository = new CompanyRepository(_controlPanelDbContext);
            _divisionRepository = new DivisionRepository(_controlPanelDbContext);
            _districtRepository = new DistrictRepository(_controlPanelDbContext);
            _zoneRepository = new ZoneRepository(_controlPanelDbContext);
            _branchRepository = new BranchRepository(_controlPanelDbContext);
        }

        // Organization
        public async Task<IEnumerable<OrganizationViewModel>> GetOrganizationsAsync(string OrgName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<OrganizationViewModel> data = new List<OrganizationViewModel>();
            try
            {
                var orgInDb = await _organizationRepository.GetAllAsync(org =>
                    (string.IsNullOrEmpty(OrgName) || org.OrganizationName == OrgName) &&
                    (!IsActive.HasValue || org.IsActive == IsActive) && (OrgId == 0 || org.OrganizationId == OrgId));

                data = _mapper.Map<List<OrganizationViewModel>>(orgInDb);
                foreach (var item in data)
                {
                    if (!Utility.IsNullEmptyOrWhiteSpace(item.OrgLogoPath))
                    {
                        var mimetype = Utility.GetFileMimetype(item.OrgImageFormat);
                        var byt = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, item.OrgLogoPath)) ?? null;

                        var stringByte = byt != null ? Convert.ToBase64String(byt) : "";
                        item.OrgBase64Pic = string.Format(@"data:{0};base64,{1}", mimetype, stringByte);
                        item.OrgLogoPath = item.OrgLogoPath.Substring(item.OrgLogoPath.IndexOf("/") + 1);
                    }
                    if (!Utility.IsNullEmptyOrWhiteSpace(item.ReportLogoPath))
                    {
                        var mimetype = Utility.GetFileMimetype(item.ReportImageFormat);
                        var byt = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, item.ReportLogoPath));

                        var stringByte = byt != null ? Convert.ToBase64String(byt) : "";
                        item.ReportBase64Pic = string.Format(@"data:{0};base64,{1}", mimetype, stringByte);
                        item.ReportLogoPath = item.ReportLogoPath.Substring(item.ReportLogoPath.IndexOf("/") + 1);
                    }
                    item.AppName = (await _applicationRepository.GetSingleAsync(app => app.ApplicationId == item.AppId)).ApplicationName;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetOrganizationsAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<bool> SaveOrganizationAsync(OrganizationViewModel model, long OrgId, long ComId, long BranchId, string UserId)
        {
            string file = null;
            string filePath = null;
            string fileName = null;

            try
            {
                if (model.OrganizationId == 0)
                {
                    Organization organization = new Organization();
                    organization.OrganizationName = model.OrganizationName;
                    organization.OrgCode = model.OrgCode;

                    if (!Utility.IsNullEmptyOrWhiteSpace(model.OrgCode))
                    {
                        Utility.CreateOrgDirectory(model.OrgCode);
                        file = await Utility.SaveFileAsync(model.OrgPicFile, string.Format(@"{0}", model.OrgCode));
                        filePath = file.Substring(0, file.LastIndexOf("/"));
                        fileName = file.Substring(file.LastIndexOf("/") + 1);
                        organization.OrgImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);
                        organization.OrgLogoPath = file;

                        file = await Utility.SaveFileAsync(model.ReportPicFile, string.Format(@"{0}", model.OrgCode));
                        filePath = file.Substring(0, file.LastIndexOf("/"));
                        fileName = file.Substring(file.LastIndexOf("/") + 1);
                        organization.ReportImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);
                        organization.ReportLogoPath = file;
                    }

                    organization.IsActive = model.IsActive;
                    organization.ShortName = model.ShortName;
                    organization.Address = model.Address;
                    organization.Email = model.Email;
                    organization.PhoneNumber = model.PhoneNumber;
                    organization.MobileNumber = model.MobileNumber;
                    organization.Website = model.Website;
                    organization.Fax = model.Fax;
                    organization.ContractStartDate = model.ContractStartDate.Value.Date;
                    organization.ContractExpireDate = model.ContractExpireDate.Value.Date;
                    organization.AppId = model.AppId;
                    organization.Remarks = model.Remarks;

                    organization.OrgUniqueId = Guid.NewGuid().ToString();
                    organization.CreatedBy = UserId;
                    organization.CreatedDate = DateTime.Now;
                    organization.ContractStartDate = organization.ContractStartDate.Value.Date;
                    organization.ContractExpireDate = organization.ContractExpireDate.Value.Date;
                    await _organizationRepository.InsertAsync(organization);
                }
                else
                {
                    var organizationInDb = await _organizationRepository.GetSingleAsync(org => org.OrganizationId == model.OrganizationId);
                    if (organizationInDb != null)
                    {

                        if (model.OrgPicFile != null)
                        {
                            Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, organizationInDb.OrgLogoPath));

                            file = await Utility.SaveFileAsync(model.OrgPicFile, string.Format(@"{0}", organizationInDb.OrgCode));
                            filePath = file.Substring(0, file.LastIndexOf("/"));
                            fileName = file.Substring(file.LastIndexOf("/") + 1);

                            organizationInDb.OrgImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);
                            organizationInDb.OrgLogoPath = file;
                        }
                        if (model.ReportPicFile != null)
                        {
                            Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, organizationInDb.ReportLogoPath));

                            file = await Utility.SaveFileAsync(model.ReportPicFile, string.Format(@"{0}", organizationInDb.OrgCode));
                            filePath = file.Substring(0, file.LastIndexOf("/"));
                            fileName = file.Substring(file.LastIndexOf("/") + 1);
                            organizationInDb.ReportImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);
                            organizationInDb.ReportLogoPath = file;
                        }
                        organizationInDb.OrganizationName = model.OrganizationName;
                        organizationInDb.IsActive = model.IsActive;
                        organizationInDb.ShortName = model.ShortName;
                        organizationInDb.Address = model.Address;
                        organizationInDb.Email = model.Email;
                        organizationInDb.PhoneNumber = model.PhoneNumber;
                        organizationInDb.MobileNumber = model.MobileNumber;
                        organizationInDb.Website = model.Website;
                        organizationInDb.Fax = model.Fax;
                        organizationInDb.ContractStartDate = model.ContractStartDate.Value.Date;
                        organizationInDb.ContractExpireDate = model.ContractExpireDate.Value.Date;
                        organizationInDb.AppId = model.AppId;
                        organizationInDb.Remarks = model.Remarks;

                        if (model.OrgPic != null && model.OrgPic.Length > 0)
                        {
                            organizationInDb.OrgImageFormat = model.OrgImageFormat;
                        }
                        if (model.ReportPic != null && model.ReportPic.Length > 0)
                        {
                            organizationInDb.ReportImageFormat = model.ReportImageFormat;
                        }
                        organizationInDb.UpdatedBy = UserId;
                        organizationInDb.UpdatedDate = DateTime.Now;
                        await _organizationRepository.UpdateAsync(organizationInDb);
                    }
                }
                return await _organizationRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "SaveOrganizationAsync", "", 0, 0, 0);
                return false;
            }
        }
        public async Task<bool> DeleteOrganizationAsync(long OrganizationId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _organizationRepository.DeleteSingleAsync(org => org.OrganizationId == OrganizationId);
                var status = await _organizationRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteOrganizationAsync", "", 0, 0, 0);
                return false;
            }
        }

        public async Task<IEnumerable<Select2Dropdown>> GetOrganizationExtensionAsync()
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                data = (await _organizationRepository.GetAllAsync()).Select(s => new Select2Dropdown
                {
                    Id = s.OrganizationId.ToString(),
                    Text = s.OrganizationName.ToString()
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetOrganizationExtensionAsync", "", 0, 0, 0);
            }
            return data;
        }
        // Company
        public async Task<IEnumerable<CompanyViewModel>> GetCompanyAsync(string CompanyName, long ComOrgId, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<CompanyViewModel> data = new List<CompanyViewModel>();
            try
            {
                var comInDb = await _companyRepository.GetAllAsync(com =>
                    (ComOrgId == 0 || com.OrganizationId == ComOrgId) &&
                    (string.IsNullOrEmpty(CompanyName) || com.CompanyName == CompanyName) &&
                    (!IsActive.HasValue || com.IsActive == IsActive));

                data = _mapper.Map<List<CompanyViewModel>>(comInDb);
                foreach (var item in data)
                {
                    var org = await _organizationRepository.GetSingleAsync(org => org.OrganizationId == item.OrganizationId);
                    if (org != null)
                    {
                        item.OrganizationName = org.OrganizationName;
                    }
                    if (item.CompanyPic != null && !Utility.IsNullEmptyOrWhiteSpace(item.CompanyLogoPath))
                    {
                        var mimetype = Utility.GetFileMimetype(item.CompanyImageFormat);
                        var byt = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, item.CompanyLogoPath));

                        var stringByte = byt != null ? Convert.ToBase64String(byt) : "";

                        item.CompanyBase64Pic = string.Format(@"data:{0};base64,{1}", mimetype, stringByte);
                        item.CompanyLogoPath = item.CompanyLogoPath.Substring(item.CompanyLogoPath.IndexOf("/") + 1);
                    }
                    if (item.ReportPic != null && !Utility.IsNullEmptyOrWhiteSpace(item.ReportLogoPath))
                    {
                        var mimetype = Utility.GetFileMimetype(item.ReportImageFormat);
                        var byt = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, item.ReportLogoPath));

                        var stringByte = byt != null ? Convert.ToBase64String(byt) : "";

                        item.ReportBase64Pic = string.Format(@"data:{0};base64,{1}", mimetype, stringByte);
                        item.ReportLogoPath = item.CompanyLogoPath.Substring(item.ReportLogoPath.IndexOf("/") + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetCompanyAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<bool> SaveCompanyAsync(CompanyViewModel companyVM, long OrgId, long ComId, long BranchId, string UserId)
        {
            string file = null;
            string filePath = null;
            string fileName = null;
            try
            {
                var org = (await GetOrganizationsAsync("", null, companyVM.OrganizationId, 0, 0)).FirstOrDefault();
                if (org != null)
                {
                    if (companyVM.CompanyId == 0)
                    {
                        Company company = new Company();
                        if (!Utility.IsNullEmptyOrWhiteSpace(org.OrgCode))
                        {
                            if (companyVM.CompanyPicFile != null)
                            {
                                file = await Utility.SaveFileAsync(companyVM.CompanyPicFile, string.Format(@"{0}", org.OrgCode));
                                filePath = file.Substring(0, file.LastIndexOf("/"));
                                fileName = file.Substring(file.LastIndexOf("/") + 1);
                                company.CompanyImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                                company.CompanyLogoPath = file;
                            }
                            if (companyVM.ReportPicFile != null)
                            {
                                file = await Utility.SaveFileAsync(companyVM.ReportPicFile, string.Format(@"{0}", org.OrgCode));
                                filePath = file.Substring(0, file.LastIndexOf("/"));
                                fileName = file.Substring(file.LastIndexOf("/") + 1);
                                company.ReportImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                                company.ReportLogoPath = file;
                            }
                        }
                        company.CompanyName = companyVM.CompanyName;
                        company.IsActive = companyVM.IsActive;
                        company.Address = companyVM.Address;
                        company.Email = companyVM.Email;
                        company.PhoneNumber = companyVM.PhoneNumber;
                        company.MobileNumber = companyVM.MobileNumber;
                        company.Website = companyVM.Website;
                        company.Fax = companyVM.Fax;
                        company.ComUniqueId = Guid.NewGuid().ToString();
                        company.CreatedBy = UserId;
                        company.CreatedDate = DateTime.Now;
                        company.ContractExpireDate = company.ContractExpireDate.HasValue ? company.ContractExpireDate.Value.Date : null;
                        company.OrganizationId = companyVM.OrganizationId;
                        await _companyRepository.InsertAsync(company);
                    }
                    else
                    {
                        var companyInDb = await _companyRepository.GetSingleAsync(com => com.CompanyId == companyVM.CompanyId);
                        if (companyInDb != null)
                        {
                            companyInDb.CompanyName = companyVM.CompanyName;
                            companyInDb.IsActive = companyVM.IsActive;
                            companyInDb.Address = companyVM.Address;
                            companyInDb.Email = companyVM.Email;
                            companyInDb.PhoneNumber = companyVM.PhoneNumber;
                            companyInDb.MobileNumber = companyVM.MobileNumber;
                            companyInDb.Website = companyVM.Website;
                            companyInDb.Fax = companyVM.Fax;
                            companyInDb.ContractExpireDate = companyVM.ContractExpireDate.HasValue ? companyVM.ContractExpireDate.Value.Date : null;
                            companyInDb.Remarks = companyVM.Remarks;

                            if (companyVM.CompanyPicFile != null)
                            {
                                Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, companyInDb.CompanyLogoPath));

                                file = await Utility.SaveFileAsync(companyVM.CompanyPicFile, string.Format(@"{0}", org.OrgCode));
                                filePath = file.Substring(0, file.LastIndexOf("/"));
                                fileName = file.Substring(file.LastIndexOf("/") + 1);

                                companyInDb.CompanyImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);
                                companyInDb.CompanyLogoPath = file;
                                companyInDb.CompanyPic = companyVM.CompanyPic;
                            }
                            if (companyVM.ReportPicFile != null)
                            {
                                Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, companyInDb.ReportLogoPath));

                                file = await Utility.SaveFileAsync(companyVM.ReportPicFile, string.Format(@"{0}", org.OrgCode));
                                filePath = file.Substring(0, file.LastIndexOf("/"));
                                fileName = file.Substring(file.LastIndexOf("/") + 1);
                                companyInDb.ReportImageFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);
                                companyInDb.ReportLogoPath = file;
                                companyInDb.ReportPic = companyVM.ReportPic;
                            }

                            companyInDb.UpdatedBy = UserId;
                            companyInDb.UpdatedDate = DateTime.Now;
                            await _companyRepository.UpdateAsync(companyInDb);
                        }
                    }
                    return await _companyRepository.SaveAsync();
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "SaveCompanyAsync", "", 0, 0, 0);
            }
            return false;
        }
        public async Task<bool> DeleteCompanyAsync(long CompanyId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _companyRepository.DeleteSingleAsync(com => com.CompanyId == CompanyId);
                var status = await _companyRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteCompanyAsync", "", 0, 0, 0);
                return false;
            }
        }
        public async Task<IEnumerable<Select2Dropdown>> GetCompanyExtensionAsync(long orgId)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                data = (await _companyRepository.GetAllAsync(c => orgId <= 0 || c.OrganizationId == orgId)).Select(s => new Select2Dropdown
                {
                    Id = s.CompanyId.ToString(),
                    Text = s.CompanyName.ToString()
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetCompanyExtensionAsync", "", 0, 0, 0);
            }
            return data;
        }

        // Division
        public async Task<IEnumerable<DivisionViewModel>> GetDivisionsAsync(string DivisionName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<DivisionViewModel> data = new List<DivisionViewModel>();
            try
            {
                var divisionInDomain = await _divisionRepository.GetAllAsync(div =>
                   (string.IsNullOrEmpty(DivisionName) || div.DivisionName == DivisionName.ToLower()) &&
                   (!IsActive.HasValue || div.IsActive == IsActive));

                data = _mapper.Map<List<DivisionViewModel>>(divisionInDomain);
                foreach (var item in data)
                {
                    item.CompanyName = (await _companyRepository.GetSingleAsync(com => com.CompanyId == item.CompanyId)).CompanyName;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetCompanyExtensionAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<bool> SaveDivisionAsync(Division division, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                var comInDb = await _companyRepository.GetSingleAsync(com => com.CompanyId == division.CompanyId);
                if (division.DivisionId == 0)
                {
                    division.CreatedBy = UserId;
                    division.CreatedDate = DateTime.Now;
                    division.OrganizationId = comInDb.OrganizationId;
                    await _divisionRepository.InsertAsync(division);
                }
                else
                {
                    var divisionInDb = await _divisionRepository.GetSingleAsync(div => div.DivisionId == division.DivisionId && div.CompanyId == division.CompanyId);
                    if (divisionInDb != null)
                    {
                        divisionInDb.DivisionName = division.DivisionName;
                        divisionInDb.ShortName = division.ShortName;
                        divisionInDb.IsActive = division.IsActive;
                        divisionInDb.CompanyId = division.CompanyId;
                        divisionInDb.OrganizationId = comInDb.OrganizationId;
                        divisionInDb.UpdatedBy = UserId;
                        divisionInDb.UpdatedDate = DateTime.Now;
                        await _divisionRepository.UpdateAsync(divisionInDb);
                    }
                }
                return await _divisionRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "SaveDivisionAsync", "", 0, 0, 0);
                return false;
            }
        }
        public async Task<bool> DeleteDivisionAsync(long DivisionId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _divisionRepository.DeleteSingleAsync(div => div.DivisionId == DivisionId);
                var status = await _divisionRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteDivisionAsync", "", 0, 0, 0);
                return false;
            }
        }

        // District
        public async Task<IEnumerable<DistrictViewModel>> GetDistrictsAsync(string DistrictName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<DistrictViewModel> data = new List<DistrictViewModel>();
            try
            {
                var districtInDomain = await _districtRepository.GetAllAsync(dis =>
                   (string.IsNullOrEmpty(DistrictName) || dis.DistrictName == DistrictName.ToLower()) &&
                   (!IsActive.HasValue || dis.IsActive == IsActive));
                data = _mapper.Map<List<DistrictViewModel>>(districtInDomain);
                foreach (var item in data)
                {
                    item.DivisionName = (await _divisionRepository.GetSingleAsync(d => d.DivisionId == item.DivisionId)).DivisionName;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetDistrictsAsync", "", 0, 0, 0);
            }
            return data;
        }

        public async Task<bool> SaveDistrictAsync(District district, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                var divisionInDb = await _divisionRepository.GetSingleAsync(div => div.DivisionId == district.DivisionId);

                if (divisionInDb != null)
                {
                    if (district.DistrictId == 0)
                    {
                        district.CreatedBy = UserId;
                        district.CreatedDate = DateTime.Now;
                        district.CompanyId = divisionInDb.CompanyId;
                        district.OrganizationId = divisionInDb.OrganizationId;
                        await _districtRepository.InsertAsync(district);
                    }
                    else
                    {
                        var districtInDb = await _districtRepository.GetSingleAsync(dis => dis.DistrictId == district.DistrictId && dis.CompanyId == district.CompanyId);
                        if (districtInDb != null)
                        {
                            districtInDb.DistrictName = district.DistrictName;
                            districtInDb.ShortName = district.ShortName;
                            districtInDb.IsActive = district.IsActive;
                            districtInDb.DivisionId = district.DivisionId;
                            districtInDb.CompanyId = divisionInDb.CompanyId;
                            districtInDb.OrganizationId = divisionInDb.OrganizationId;
                            districtInDb.UpdatedBy = UserId;
                            districtInDb.UpdatedDate = DateTime.Now;
                            await _districtRepository.UpdateAsync(districtInDb);
                        }
                    }
                    return await _districtRepository.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "SaveDistrictAsync", "", 0, 0, 0);
            }
            return false;
        }
        public async Task<bool> DeleteDistrictAsync(long DistrictId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _districtRepository.DeleteSingleAsync(dis => dis.DistrictId == DistrictId);
                var status = await _districtRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteDistrictAsync", "", 0, 0, 0);
                return false;
            }
        }

        // Zone
        public async Task<IEnumerable<ZoneViewModel>> GetZonesAsync(string ZoneName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<ZoneViewModel> data = new List<ZoneViewModel>();
            try
            {
                var zoneInDb = await _zoneRepository.GetAllAsync(zn =>
                   (string.IsNullOrEmpty(ZoneName) || zn.ZoneName == ZoneName.ToLower()) &&
                   (!IsActive.HasValue || zn.IsActive == IsActive));

                data = _mapper.Map<List<ZoneViewModel>>(zoneInDb);
                foreach (var item in data)
                {
                    item.DistrictName = (await _districtRepository.GetSingleAsync(d => d.DistrictId == item.DistrictId)).DistrictName;
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteDistrictAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<bool> SaveZoneAsync(Zone zone, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                var districtInDb = await _districtRepository.GetSingleAsync(dis => dis.DistrictId == zone.DistrictId);
                if (districtInDb != null)
                {
                    if (zone.ZoneId == 0)
                    {
                        zone.CreatedBy = UserId;
                        zone.CreatedDate = DateTime.Now;
                        zone.DivisionId = districtInDb.DivisionId;
                        zone.CompanyId = districtInDb.CompanyId;
                        zone.OrganizationId = districtInDb.OrganizationId;
                        await _zoneRepository.InsertAsync(zone);
                    }
                    else
                    {
                        var zoneInDb = await _zoneRepository.GetSingleAsync(zn => zn.ZoneId == zone.ZoneId && zn.DistrictId == zone.DistrictId);
                        if (zoneInDb != null)
                        {
                            zoneInDb.ZoneName = zone.ZoneName;
                            zoneInDb.ShortName = zone.ShortName;
                            zoneInDb.IsActive = zone.IsActive;
                            zoneInDb.DistrictId = zone.DistrictId;
                            zoneInDb.DivisionId = districtInDb.DivisionId;
                            zoneInDb.CompanyId = districtInDb.CompanyId;
                            zoneInDb.OrganizationId = districtInDb.OrganizationId;
                            zoneInDb.UpdatedBy = UserId;
                            zoneInDb.UpdatedDate = DateTime.Now;
                            await _zoneRepository.UpdateAsync(zoneInDb);
                        }
                    }
                }
                return await _zoneRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "SaveZoneAsync", "", 0, 0, 0);
                return false;
            }
        }
        public async Task<bool> DeleteZoneAsync(long ZoneId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _zoneRepository.DeleteSingleAsync(zn => zn.ZoneId == ZoneId);
                var status = await _zoneRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteZoneAsync", "", 0, 0, 0);
                return false;
            }
        }

        // Branch
        public async Task<IEnumerable<BranchViewModel>> GetBranchesAsync(string BranchName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<BranchViewModel> data = new List<BranchViewModel>();
            try
            {
                var branchInDb = await _branchRepository.GetAllAsync(b =>
                   (string.IsNullOrEmpty(BranchName) || b.BranchName == BranchName.ToLower()) &&
                   (!IsActive.HasValue || b.IsActive == IsActive));
                data = _mapper.Map<List<BranchViewModel>>(branchInDb);

                foreach (var item in data)
                {
                    item.ZoneName = (await _zoneRepository.GetSingleAsync(zn => zn.ZoneId == item.ZoneId)).ZoneName;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "GetBranchesAsync", "", 0, 0, 0);
            }
            return data;
        }

        public async Task<bool> SaveBranchAsync(Branch branch, string UserId)
        {
            try
            {
                var divisionInDb = await _divisionRepository.GetSingleAsync(dv => dv.DivisionId == branch.DivisionId);
                if (divisionInDb != null)
                {
                    if (branch.BranchId == 0)
                    {
                        branch.BranchUniqueId = Guid.NewGuid().ToString();
                        branch.CreatedBy = UserId;
                        branch.CreatedDate = DateTime.Now;
                        branch.OrganizationId = divisionInDb.OrganizationId;
                        branch.CompanyId = divisionInDb.CompanyId;
                        branch.DivisionId = branch.DivisionId;
                        await _branchRepository.InsertAsync(branch);
                    }
                    else
                    {
                        var branchInDb = await _branchRepository.GetSingleAsync(b => b.BranchId == branch.BranchId);
                        if (branchInDb != null)
                        {
                            branchInDb.BranchName = branch.BranchName;
                            branchInDb.ShortName = branch.ShortName;
                            branchInDb.MobileNo = branch.MobileNo;
                            branchInDb.Email = branch.Email;
                            branchInDb.PhoneNo = branch.PhoneNo;
                            branchInDb.Fax = branch.Fax;
                            branchInDb.Address = branch.Address;
                            branchInDb.IsActive = branch.IsActive;
                            branchInDb.Remarks = branch.Remarks;
                            branchInDb.DivisionId = branch.DivisionId;
                            branchInDb.CompanyId = divisionInDb.CompanyId;
                            branchInDb.OrganizationId = divisionInDb.OrganizationId;
                            branchInDb.UpdatedBy = UserId;
                            branchInDb.UpdatedDate = DateTime.Now;
                            await _branchRepository.UpdateAsync(branch);
                        }
                    }
                    return await _branchRepository.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "SaveBranchAsync", "", 0, 0, 0);
            }
            return false;
        }
        public async Task<bool> DeleteBranchAsync(long Id, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _branchRepository.DeleteSingleAsync(b => b.BranchId == Id);
                var status = await _branchRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgInitBusiness", "DeleteBranchAsync", "", 0, 0, 0);
                return false;
            }
        }
        public async Task<IEnumerable<Dropdown>> BranchExtension(string flag, long ComId, long OrgId)
        {
            IEnumerable<Dropdown> data = new List<Dropdown>();
            try
            {
                sqlQuery = "sp_BranchExtension";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", ComId);
                parameters.Add("OrgId", OrgId);
                parameters.Add("Flag", flag);
                data = await _dapperData.SqlQueryListAsync<Dropdown>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgIntiBusiness", "BranchExtension", "", OrgId, ComId, 0);
            }
            return data;
        }

        public async Task<Branch> GetBranchById(long id, long companyId, long organizationId)
        {
            Branch branch = null;
            try
            {
                var query = $@"SELECT * FROM tblBranches Where BranchId=@BranchId AND CompanyId= @CompanyId AND OrganizationId=@OrganizationId";
                branch = await _dapperData.SqlQueryFirstAsync<Branch>(Database.ControlPanel, query, new { BranchId = id, CompanyId = companyId, OrganizationId = organizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrgIntiBusiness", "BranchExtension", "", organizationId, companyId, 0);
            }
            return branch;
        }

    }
}
