using Dapper;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared.Services
{
    public static class DapperParam
    {
        public static DynamicParameters AddParams<T>(T Object, AppUser appUser, bool addBaseProperty = false, bool addUserSession = false, bool addRoleId = false, bool addRoleName = false, bool addUserId = true, bool addUsername = false, bool addEmployee = false, bool addDesignation = false, bool addDepartment = false, bool addBranch = false, bool addDivision = false, bool addCompany = true, bool addOrganization = true)
        {
            var parameters = new DynamicParameters();
            var keyValuePairs = GetKeyValuePairs(Object, addBaseProperty);
            foreach (var item in keyValuePairs) {
                parameters.Add(item.Key, item.Value);
            }
            if (appUser != null) {
                if (addUserSession) {
                    parameters.Add("UserSession", appUser.UserSession);
                }
                if (addRoleId) {
                    parameters.Add("RoleId", appUser.RoleId);
                }
                if (addRoleName) {
                    parameters.Add("RoleName", appUser.RoleName);
                }
                if (addUserId) {
                    parameters.Add("UserId", appUser.ActionUserId);
                }
                if (addUsername) {
                    parameters.Add("Username", appUser.Username);
                }
                if (addEmployee) {
                    parameters.Add("EmployeeId", appUser.EmployeeId);
                }
                if (addDesignation) {
                    parameters.Add("DesignationId", appUser.DesignationId);
                }
                if (addDepartment) {
                    parameters.Add("DepartmentId", appUser.DepartmentId);
                }
                if (addBranch) {
                    parameters.Add("UserBranchId", appUser.BranchId);
                }
                if (addDivision) {
                    parameters.Add("UserDivisionId", appUser.DivisionId);
                }
                if (addCompany) {
                    parameters.Add("CompanyId", appUser.CompanyId);
                }
                if (addOrganization) {
                    parameters.Add("OrganizationId", appUser.OrganizationId);
                }
            }
            return parameters;
        }

        public static DynamicParameters AddParams<T>(T Object, AppUser appUser, string[] excludeProps, bool addBaseProperty = false, bool addUserSession = false, bool addRoleId = false, bool addRoleName = false, bool addUserId = true, bool addUsername = false, bool addEmployee = false, bool addDesignation = false, bool addDepartment = false, bool addBranch = false, bool addDivision = false, bool addCompany = true, bool addOrganization = true)
        {
            var parameters = new DynamicParameters();
            var keyValuePairs = GetKeyValuePairs(Object, excludeProps, addBaseProperty);
            foreach (var item in keyValuePairs) {
                parameters.Add(item.Key, item.Value);
            }
            if (appUser != null) {
                if (addUserSession) {
                    parameters.Add("UserSession", appUser.UserSession);
                }
                if (addRoleId) {
                    parameters.Add("RoleId", appUser.RoleId);
                }
                if (addRoleName) {
                    parameters.Add("RoleName", appUser.RoleName);
                }
                if (addUserId) {
                    parameters.Add("UserId", appUser.ActionUserId);
                }
                if (addUsername) {
                    parameters.Add("Username", appUser.Username);
                }
                if (addEmployee) {
                    parameters.Add("EmployeeId", appUser.EmployeeId);
                }
                if (addDesignation) {
                    parameters.Add("DesignationId", appUser.DesignationId);
                }
                if (addDepartment) {
                    parameters.Add("DepartmentId", appUser.DepartmentId);
                }
                if (addBranch) {
                    parameters.Add("UserBranchId", appUser.BranchId);
                }
                if (addDivision) {
                    parameters.Add("UserDivisionId", appUser.DivisionId);
                }
                if (addCompany) {
                    parameters.Add("CompanyId", appUser.CompanyId);
                }
                if (addOrganization) {
                    parameters.Add("OrganizationId", appUser.OrganizationId);
                }
            }
            return parameters;
        }

        public static Dictionary<string, string> GetKeyValuePairs<T>(T obj, bool addBaseProperty = false)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var p1 = obj.GetType().GetProperties();
            PropertyInfo[] infos = addBaseProperty == false ?
                obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) : obj.GetType().GetProperties();
            foreach (PropertyInfo info in infos) {
                var value = info.GetValue(obj, null) == null ? null : (info.GetValue(obj, null).ToString() == "null" ? null : info.GetValue(obj, null).ToString().Trim());
                keyValuePairs.Add(info.Name, value);
            }
            return keyValuePairs;
        }
        public static Dictionary<string, string> GetKeyValuePairs<T>(T obj, string[] excludeProps, bool addBaseProperty = false)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            PropertyInfo[] infos = addBaseProperty == false ?
                obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) : obj.GetType().GetProperties();
            foreach (PropertyInfo info in infos) {
                var isInExcludeList = false;
                if (excludeProps != null) {
                    isInExcludeList = excludeProps.Contains(info.Name);
                }
                if (!isInExcludeList) {
                    keyValuePairs.Add(info.Name, info.GetValue(obj, null) == null ? null : (info.GetValue(obj, null).ToString() == "null" ? null : info.GetValue(obj, null).ToString().Trim()));
                }
            }
            return keyValuePairs;
        }

        public static Dictionary<string, string> GetKeyValuePairs<T>(T obj, AppUser appUser, string[] excludeProps, bool addBaseProperty = false, bool addUserSession = false, bool addRoleId = false, bool addRoleName = false, bool addUserId = true, bool addUsername = false, bool addEmployee = false, bool addDesignation = false, bool addDepartment = false, bool addBranch = false, bool addDivision = false, bool addCompany = true, bool addOrganization = true)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            PropertyInfo[] infos = addBaseProperty == false ?
                obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) : obj.GetType().GetProperties();
            foreach (PropertyInfo info in infos) {
                var isInExcludeList = excludeProps.Contains(info.Name);
                if (!isInExcludeList) {
                    keyValuePairs.Add(info.Name, info.GetValue(obj, null) == null ? null : info.GetValue(obj, null).ToString());
                }
            }
            if (appUser != null) {
                if (addUserSession) {
                    keyValuePairs.Add("UserSession", appUser.UserSession);
                }
                if (addRoleId) {
                    keyValuePairs.Add("RoleId", appUser.RoleId);
                }
                if (addRoleName) {
                    keyValuePairs.Add("RoleName", appUser.RoleName);
                }
                if (addUserId) {
                    keyValuePairs.Add("UserId", appUser.ActionUserId);
                }
                if (addUsername) {
                    keyValuePairs.Add("Username", appUser.Username);
                }
                if (addEmployee) {
                    keyValuePairs.Add("EmployeeId", appUser.EmployeeId.ToString());
                }
                if (addDesignation) {
                    keyValuePairs.Add("DesignationId", appUser.DesignationId.ToString());
                }
                if (addDepartment) {
                    keyValuePairs.Add("DepartmentId", appUser.DepartmentId.ToString());
                }
                if (addBranch) {
                    keyValuePairs.Add("UserBranchId", appUser.BranchId.ToString());
                }
                if (addDivision) {
                    keyValuePairs.Add("UserDivisionId", appUser.DivisionId.ToString());
                }
                if (addCompany) {
                    keyValuePairs.Add("CompanyId", appUser.CompanyId.ToString());
                }
                if (addOrganization) {
                    keyValuePairs.Add("OrganizationId", appUser.OrganizationId.ToString());
                }
            }
            return keyValuePairs;
        }

        public static DynamicParameters AddParams(
        AppUser appUser,
        bool addBaseProperty = false,
        bool addUserSession = false,
        bool addRoleId = false,
        bool addRoleName = false,
        bool addUserId = true,
        bool addUsername = false,
        bool addEmployee = true,
        bool addDesignation = false,
        bool addDepartment = false,
        bool addBranch = false,
        bool addDivision = false,
        bool addCompany = true,
        bool addOrganization = true
        )
        {
            var parameters = new DynamicParameters();
            var keyValuePairs = GetKeyValuePairs(addBaseProperty);
            foreach (var item in keyValuePairs) {
                parameters.Add(item.Key, item.Value);
            }
            if (appUser != null) {
                if (addUserSession) {
                    parameters.Add("UserSession", appUser.UserSession);
                }
                if (addRoleId) {
                    parameters.Add("RoleId", appUser.RoleId);
                }
                if (addRoleName) {
                    parameters.Add("RoleName", appUser.RoleName);
                }
                if (addUserId) {
                    parameters.Add("UserId", appUser.ActionUserId);
                }
                if (addUsername) {
                    parameters.Add("Username", appUser.Username);
                }
                if (addEmployee) {
                    parameters.Add("EmployeeId", appUser.EmployeeId);
                }
                if (addDesignation) {
                    parameters.Add("DesignationId", appUser.DesignationId);
                }
                if (addDepartment) {
                    parameters.Add("DepartmentId", appUser.DepartmentId);
                }
                if (addBranch) {
                    parameters.Add("UserBranchId", appUser.BranchId);
                }
                if (addDivision) {
                    parameters.Add("UserDivisionId", appUser.DivisionId);
                }
                if (addCompany) {
                    parameters.Add("CompanyId", appUser.CompanyId);
                }
                if (addOrganization) {
                    parameters.Add("OrganizationId", appUser.OrganizationId);
                }
            }
            return parameters;
        }

        public static Dictionary<string, dynamic> AddParamsInKeyValuePairs<T>(T Object, AppUser appUser, bool addBaseProperty = false, bool addUserSession = false,
           bool addRoleId = false, bool addRoleName = false, bool addUserId = true, bool addUsername = false, bool addEmployee = false, bool addDesignation = false,
           bool addDepartment = false, bool addBranch = false, bool addDivision = false, bool addCompany = true, bool addOrganization = true)
        {

            var keyValuePairs = GetKeyValuePairsDynamic(Object, addBaseProperty);

            if (appUser != null) {
                if (addUserSession) {
                    keyValuePairs.Add("UserSession", appUser.UserSession);
                }
                if (addRoleId) {
                    keyValuePairs.Add("RoleId", appUser.RoleId);
                }
                if (addRoleName) {
                    keyValuePairs.Add("RoleName", appUser.RoleName);
                }
                if (addUserId) {
                    keyValuePairs.Add("UserId", appUser.UserId);
                }
                if (addUsername) {
                    keyValuePairs.Add("Username", appUser.Username);
                }
                if (addEmployee) {
                    keyValuePairs.Add("EmployeeId", appUser.EmployeeId);
                }
                if (addDesignation) {
                    keyValuePairs.Add("DesignationId", appUser.DesignationId);
                }
                if (addDepartment) {
                    keyValuePairs.Add("DepartmentId", appUser.DepartmentId);
                }
                if (addBranch) {
                    keyValuePairs.Add("BranchId", appUser.BranchId);
                }
                if (addDivision) {
                    keyValuePairs.Add("UserDivisionId", appUser.DivisionId);
                }
                if (addCompany) {
                    keyValuePairs.Add("CompanyId", appUser.CompanyId);
                }
                if (addOrganization) {
                    keyValuePairs.Add("OrganizationId", appUser.OrganizationId);
                }
            }

            return keyValuePairs;
        }
        public static Dictionary<string, dynamic> GetKeyValuePairsDynamic<T>(T obj, bool addBaseProperty = false)
        {
            Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
            PropertyInfo[] infos = addBaseProperty == false ?
                obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) : obj.GetType().GetProperties();
            foreach (PropertyInfo info in infos) {
                //var value = info.GetValue(obj, null) == null ? null : info.GetValue(obj, null).ToString();
                var value = info.GetValue(obj, null) == null ? null : (info.GetValue(obj, null).ToString() == "null" ? null : info.GetValue(obj, null).ToString().Trim());
                keyValuePairs.Add(info.Name, value);
            }
            return keyValuePairs;
        }

        public static Dictionary<string, dynamic> GetKeyValuePairsDynamic<T>(T obj, string[] excludeProps, bool addBaseProperty = false)
        {
            Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
            PropertyInfo[] infos = addBaseProperty == false ?
                obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) : obj.GetType().GetProperties();
            foreach (PropertyInfo info in infos) {
                var isInExcludeList = excludeProps.Contains(info.Name);
                if (!isInExcludeList) {
                    var value = info.GetValue(obj, null) == null ? null : (info.GetValue(obj, null).ToString() == "null" ? null : info.GetValue(obj, null).ToString().Trim());
                    keyValuePairs.Add(info.Name, value);
                }
            }
            return keyValuePairs;
        }

        public static Dictionary<string, dynamic> GetKeyValuePairsDynamic<T>(T obj, string[] excludeProps, AppUser user, bool addBranch = false, bool addCompany = true, bool addOrganization = true, bool addBaseProperty = false)
        {
            Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
            PropertyInfo[] infos = addBaseProperty == false ?
                obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public) : obj.GetType().GetProperties();
            foreach (PropertyInfo info in infos) {
                var isInExcludeList = excludeProps.Contains(info.Name);
                if (!isInExcludeList) {
                    var value = info.GetValue(obj, null) == null ? null : (info.GetValue(obj, null).ToString() == "null" ? null : info.GetValue(obj, null).ToString().Trim());
                    keyValuePairs.Add(info.Name, value);
                }
            }

            if (user != null) {
                if (addBranch) {
                    keyValuePairs.Add("BranchId", user.BranchId);
                }
                if (addCompany) {
                    keyValuePairs.Add("CompanyId", user.CompanyId);
                }
                if (addOrganization) {
                    keyValuePairs.Add("OrganizationId", user.OrganizationId);
                }
            }
            return keyValuePairs;
        }
        public static Pageparam GetPageparam(ref DynamicParameters parameters)
        {
            Pageparam pageparam = new Pageparam();
            pageparam.PageNumber = parameters.Get<short>("PageNumber");
            pageparam.PageSize = parameters.Get<short>("PageSize");
            pageparam.TotalPages = parameters.Get<int>("TotalPages");
            pageparam.TotalRows = parameters.Get<int>("TotalRows");
            return pageparam;
        }
        public static void AddDappperParams<T>(T Object, ref DynamicParameters parameters)
        {
            var keyValuePairs = GetKeyValuePairs(Object);
            foreach (var item in keyValuePairs) {
                parameters.Add(item.Key, item.Value);
            }
        }
        public static void AddDappperParams<T>(T Object, string[] excludeProps, ref DynamicParameters parameters)
        {
            var keyValuePairs = GetKeyValuePairs(Object);
            foreach (var item in keyValuePairs) {
                if (!excludeProps.Contains(item.Key)) {

                    parameters.Add(item.Key, item.Value);
                }
            }
        }

    }
}
