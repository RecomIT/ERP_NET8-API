using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class AuthViewModel
    {
        public long ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public long MMId { get; set; }
        public string MenuName { get; set; }
        public bool HasPermision { get; set; }
    }
    public class AuthApp
    {
        public long AppId { get; set; }
        public string AppName { get; set; }
        public List<AuthModule> Modules { get; set; }
    }
    public class AuthModule
    {
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<AuthMainmenu> MainMenus { get; set; }
    }
    public class AuthMainmenu
    {
        public long MainMenuId { get; set; }
        public string MainMenuName { get; set; }
        public bool HasPermision { get; set; }
    }

    public class Usercomponentprivilege
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Component { get; set; }
        [Range(1, long.MaxValue)]
        public long CompanyId { get; set; }
        [Range(1, long.MaxValue)]
        public long OrganizationId { get; set; }
    }
}
