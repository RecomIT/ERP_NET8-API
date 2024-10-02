using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        [Required, StringLength(150)]
        public string FullName { get; set; }
        //Required,
        [StringLength(50)]
        public string EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsRoleActive { get; set; }
        [Required, StringLength(50)]
        public string RoleId { get; set; }
        [StringLength(500)]
        public string AccessToken { get; set; }
        [StringLength(500)]
        public string IdToken { get; set; }
        [StringLength(300)]
        public string ProfilePicPath { get; set; }
        public byte[] ProfilePic { get; set; }
        [StringLength(50)]
        public string CurrentState { get; set; } // Login, LogOut
        [StringLength(150)]
        public string Address { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public long? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public long? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        // 
        [Required, StringLength(256)]
        public string UserName { get; set; }
        [DataType(DataType.PhoneNumber), StringLength(50)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress), Required, StringLength(256)]
        public string Email { get; set; }
        //[Required,DataType(DataType.Password),StringLength(20)]
        //[DataType(DataType.Password), StringLength(20)]
        public string Password { get; set; }
        //[Required, DataType(DataType.Password), StringLength(20), Compare("Password")]
        //[DataType(DataType.Password), StringLength(20), Compare("Password")]
        public string ConfirmPassword { get; set; }
        public bool? IsDefaultPassword { get; set; }
        public bool? IsPasswordExpired { get; set; }
        public short RemainExpiredDate { get; set; }
        [StringLength(100)]
        public string DefaultCode { get; set; }

        // 
        public string RoleName { get; set; }
    }
    public class UserInfoData
    {
        public ApplicationUserViewModel AppUserInfo { get; set; }
        public List<AppMainMenuForPermission> AppUserMenuPermission { get; set; }
    }

    public class RoleAuthData
    {
        [Range(1, long.MaxValue)]
        public long OrganizationId { get; set; }
        [Range(1, long.MaxValue)]
        public long CompanyId { get; set; }
        [Range(1, long.MaxValue)]
        public long RoleId { get; set; }
        public List<AppMainMenuForPermission> AppUserMenuPermission { get; set; }
    }
    /// <summary>
    /// User Customer Authorization Model
    /// </summary>
    public class UserAuthorizationViewModel : BaseModel
    {
        public long TaskId { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public long SubmenuId { get; set; }
        public long ParentSubmenuId { get; set; }
        public bool IsSubmenuPermission { get; set; }
        public bool IsPageTabPermission { get; set; }
        public bool HasTab { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Approval { get; set; }
        public bool Report { get; set; }
        public bool Check { get; set; }
        public bool Accept { get; set; }
        public bool Upload { get; set; }
        public long DivisionId { get; set; }
        public long BranchId { get; set; }
    }
    /// <summary>
    /// User Customer-Tab Authorization Model
    /// </summary>
    public class UserAuthTabViewModel : BaseModel
    {
        public long UATId { get; set; }
        public long TaskId { get; set; }
        public string UserId { get; set; }
        public long SubmenuId { get; set; }
        public long TabId { get; set; }
        [StringLength(100)]
        public string TabName { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Approval { get; set; }
        public bool Report { get; set; }
        public bool Check { get; set; }
        public bool Accept { get; set; }
        public bool Upload { get; set; }
        public long BranchId { get; set; }
    }
    /// <summary>
    /// User Role Authorization Model
    /// </summary>
    public class RoleAuthorizationViewModel : BaseModel
    {
        public long TaskId { get; set; }
        public string RoleId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public long SubmenuId { get; set; }
        public long ParentSubmenuId { get; set; }
        public bool HasTab { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Approval { get; set; }
        public bool Report { get; set; }
        public bool Check { get; set; }
        public bool Accept { get; set; }
        public bool Upload { get; set; }
        public long BranchId { get; set; }
    }
    /// <summary>
    /// User Role-Tab Authorization Model
    /// </summary>
    public class RoleAuthTabViewModel : BaseModel
    {
        public long RATId { get; set; }
        public long TaskId { get; set; }
        [StringLength(256)]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public long SubmenuId { get; set; }
        public long TabId { get; set; }
        [StringLength(100)]
        public string TabName { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Approval { get; set; }
        public bool Report { get; set; }
        public bool Check { get; set; }
        public bool Accept { get; set; }
        public bool Upload { get; set; }
        public long BranchId { get; set; }
    }
}
