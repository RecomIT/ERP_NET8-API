using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.ControlPanel.Controllers
{
    [ApiController, Area("ControlPanel"), Route("api/[area]/[controller]"), Authorize]
    public class UserManagementController : Controller
    {
    }
}
