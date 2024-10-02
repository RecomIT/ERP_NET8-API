using API.Base;
using AutoMapper;
using BLL.Overtime.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using Shared.Attendance.DTO.Attendance;
using Shared.Overtime.Domain;
using Shared.Overtime.DTO;
using Shared.Overtime.ViewModel;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace API.Areas.Overtime
{
    //[SysAuthorize]
    [ApiController, Area("Payroll"), Route("api/[area]/[controller]"), Authorize]
    public class OvertimeController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IOvertimeBusiness _overtimeBusiness;
        public OvertimeController(IMapper mapper, IOvertimeBusiness overtimeBusiness, IClientDatabase _clientDatabase) : base(_clientDatabase)
        {
            _mapper = mapper;
            _overtimeBusiness = overtimeBusiness;
        }

        #region Overtime Approval Level Assignment

        [HttpGet, Route("GetAllApprovalLevel")]
        public async Task<ActionResult> GetAllOvertimeApprovalLevel()
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);
                if (!result.Any())
                {
                    return NotFound(new { message = "No Approval Level Data Found." });
                }
                return Ok(_mapper.Map<List<GetOvertimeApprovalLevelDTO>>(result.ToList()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetApprovalLevelById/{id:long}")]
        public async Task<ActionResult> GetOvertimeApprovalLevelById(long id)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetOvertimeApprovalLevelById(id, appUser);
                if (result == null)
                {
                    return NotFound(new { message = "Approval Level Data Not Found." });
                }
                return Ok(_mapper.Map<GetOvertimeApprovalLevelDTO>(result));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreateApprovalLevel")]
        public ActionResult CreateOvertimeApprovalLevel()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveApprovalLevel")]
        public async Task<ActionResult> SaveOvertimeApprovalLevel(CreateOvertimeApprovalLevelDTO model)
        {
            var appUser = AppUser();
            try
            {

                var overtimeApprovalLevel = _mapper.Map<OvertimeApprovalLevel>(model);

                var alreadyExist = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);
                if (alreadyExist.Any())
                {
                    return BadRequest(new { message = $" Overtime Approval Level Already Exist." });
                }


                var result = await _overtimeBusiness.SaveOvertimeApprovalLevel(overtimeApprovalLevel, appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });

                }
                return Ok(new { message = "Overtime Approval Level Successfully Created." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("EditApprovalLevel/{id:long}")]
        public async Task<ActionResult> EditOvertimeApprovalLevel(long id)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetOvertimeApprovalLevelById(id, appUser);
                if (result == null)
                {
                    return NotFound(new { message = "Approval Level Data Not Found." });
                }
                return Ok(_mapper.Map<GetOvertimeApprovalLevelDTO>(result));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPut, Route("UpdateApprovalLevel/{id:long}")]
        public async Task<ActionResult> UpdateOvertimeApprovalLevel(long id, CreateOvertimeApprovalLevelDTO model)
        {
            var appUser = AppUser();
            try
            {
                var approvalLevelToUpdate = await _overtimeBusiness.GetOvertimeApprovalLevelById(id, appUser);

                if (approvalLevelToUpdate == null)
                {
                    return NotFound(new { message = "Approval Level Data Not Found." });
                }

                var overtimeApprovalLevel = _mapper.Map(model, approvalLevelToUpdate);

                var result = await _overtimeBusiness.UpdateOvertimeApprovalLevel(overtimeApprovalLevel, appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Update Data." });

                }
                return Ok(new { message = "Overtime Approval Level Successfully Updated." });

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }

        }

        [HttpDelete, Route("DeleteApprovalLevel/{id:long}")]
        public async Task<ActionResult> DeleteOvertimeApprovalLevel(long id)
        {
            var appUser = AppUser();
            try
            {
                var overtimeApprovalLevel = await _overtimeBusiness.GetOvertimeApprovalLevelById(id, appUser);

                if (overtimeApprovalLevel == null)
                {
                    return NotFound(new { message = "Approval Level Data Not Found." });
                }

                var result = await _overtimeBusiness.DeleteOvertimeApprovalLevel(overtimeApprovalLevel.OvertimeApprovalLevelId, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Deleted Data." });

                }
                return Ok(new { message = "Overtime Approval Level Successfully Deleted." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region Overtime Approver Assignment

        [HttpGet, Route("GetAllApprover")]
        public async Task<ActionResult> GetAllOvertimeApprover()
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetAllOvertimeApprover(appUser);
                if (!result.Any())
                {
                    return NotFound(new { message = "No Approver Data Found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetApproverById/{id:long}")]
        public async Task<ActionResult> GetOvertimeApproverById(long id)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetOvertimeApproverByApproverId(id, appUser);
                if (result == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreateApprover")]
        public async Task<ActionResult> CreateOvertimeApprover()
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetEmployeesForOvertimeApprover(appUser);
                if (!result.Any())
                {
                    return NotFound(new { message = "No Data Found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveApprover")]
        public async Task<ActionResult> SaveOvertimeApprover(List<OvertimeApproverDTO> model)
        {
            if (!model.Any())
            {
                return BadRequest(new { message = "Please Select Employee(s) to Add in Approver List " });
            }

            var appUser = AppUser();
            try
            {
                List<OvertimeApproverDTO> alreadyExist = new();
                //List<OvertimeApproverDTO> newOvertimeApproverList = new();

                var allApproverList = await _overtimeBusiness.GetAllOvertimeApprover(appUser);

                //foreach (var item in model) {
                //    var approver = allApproverList.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                //    if (approver != null) {
                //        alreadyExist.Add(approver);
                //    }
                //    else {
                //        newOvertimeApproverList.Add(item);
                //    }

                //}

                alreadyExist = model.Where(x => allApproverList.Select(e => e.EmployeeId).Contains(x.EmployeeId)).ToList();

                if (alreadyExist.Any())
                {
                    return BadRequest(new { message = $" Overtime Approver(s) Already Exist.", overtimeApprovers = alreadyExist });
                }


                var overtimeApproverList = _mapper.Map<List<OvertimeApprover>>(model);
                var result = await _overtimeBusiness.SaveOvertimeApprover(overtimeApproverList, appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });

                }


                return Ok(new { message = "Overtime Approver Successfully Created." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("EditApprover/{id:long}")]
        public async Task<ActionResult> EditOvertimeApprover(long id)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetOvertimeApproverByApproverId(id, appUser);
                if (result == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPut, Route("UpdateApprover/{id:long}")]
        public async Task<ActionResult> UpdateOvertimeApprover(long id, OvertimeApproverDTO model)
        {
            var appUser = AppUser();
            try
            {
                var approverToUpdate = await _overtimeBusiness.GetOvertimeApproverByApproverId(id, appUser);

                if (approverToUpdate == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var overtimeApprover = _mapper.Map<OvertimeApprover>(approverToUpdate);

                var result = await _overtimeBusiness.UpdateOvertimeApprover(_mapper.Map(model, overtimeApprover), appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Update Data." });

                }
                return Ok(new { message = "Overtime Approver Successfully Updated." });

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }

        }

        [HttpDelete, Route("DeleteApprover/{id:long}")]
        public async Task<ActionResult> DeleteOvertimeApprover(long id)
        {
            var appUser = AppUser();
            try
            {
                var overtimeApprover = await _overtimeBusiness.GetOvertimeApproverByApproverId(id, appUser);

                if (overtimeApprover == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var teamMembers = await _overtimeBusiness.GetOvertimeTeamMembersByApproverId(overtimeApprover.OvertimeApproverId, appUser);

                if (teamMembers.Any())
                {

                    var dto = new OvertimeTeamApprovalMappingDTO() { Approver = overtimeApprover, TeamMembers = teamMembers.ToList() };

                    return BadRequest(new { message = "Approver Having Associate Team Member Approval Mapping(s). Remove Team Members from Approval Mapping First.", dto.Approver, dto.TeamMembers });
                }

                var result = await _overtimeBusiness.DeleteOvertimeApprover(overtimeApprover.OvertimeApproverId, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Deleted Data." });

                }
                return Ok(new { message = "Overtime Approver Successfully Deleted." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region Overtime Team Approval Mapping

        [HttpGet, Route("GetAllTeamApprovalMapping")]
        public async Task<ActionResult> GetAllOvertimeTeamApprovalMapping()
        {
            var appUser = AppUser();
            try
            {
                var teamMemberList = await _overtimeBusiness.GetAllOvertimeTeamMembersApprovalMapping(appUser);
                if (!teamMemberList.Any())
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var result = new List<OvertimeTeamApprovalMappingDTO>();

                foreach (var approverWithTeam in teamMemberList.GroupBy(g => g.OvertimeApproverId))
                {

                    var dto = new OvertimeTeamApprovalMappingDTO();

                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(approverWithTeam.Key, appUser);
                    if (approver != null)
                    {
                        dto.Approver = approver;
                    }

                    if (approverWithTeam.Any())
                    {
                        dto.TeamMembers = approverWithTeam.ToList();
                    }


                    result.Add(dto);
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                int maxApprovalLevel = 0;
                int minApprovalLevel = 0;

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).FirstOrDefault();
                    maxApprovalLevel = first!.MaximumLevel;
                    minApprovalLevel = first!.MinimumLevel;
                }



                return Ok(result.Select(x => new { x.Approver, x.TeamMembers, minApprovalLevel, maxApprovalLevel }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetTeamApprovalMappingById/{id:long}")]
        public async Task<ActionResult> GetOvertimeTeamApprovalMappingById(long id)
        {
            var appUser = AppUser();
            try
            {
                var member = await _overtimeBusiness.GetOvertimeTeamMemberApprovalMappingById(id, appUser);
                if (member == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(member.OvertimeApproverId, appUser);
                if (approver == null)
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                return Ok(new { approver, teamMember = member });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetApproverByTeamMemberId/{id:long}")]
        public async Task<ActionResult> GetOvertimeApproverByTeamMemberId(long id)
        {
            var appUser = AppUser();
            try
            {
                var teamMemberList = await _overtimeBusiness.GetOvertimeTeamMembersApprovalMappingByTeamMemberId(id, appUser);

                List<OvertimeApproverDTO> teamApprover = new();

                foreach (var member in teamMemberList)
                {
                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(member.OvertimeApproverId, appUser);

                    if (approver != null)
                    {
                        teamApprover.Add(approver);
                    }

                }

                return Ok(new { approver = teamApprover });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetTeamApprovalMappingByApproverId/{id:long}")]
        public async Task<ActionResult> GetOvertimeTeamApprovalMappingByApproverId(long id)
        {
            var appUser = AppUser();
            try
            {

                var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(id, appUser);
                if (approver == null)
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                var teamMembers = await _overtimeBusiness.GetOvertimeTeamMembersByApproverId(approver.OvertimeApproverId, appUser);
                if (!teamMembers.Any())
                {
                    return NotFound(new { message = "No Team Members Found." });
                }

                var result = new OvertimeTeamApprovalMappingDTO() { Approver = approver, TeamMembers = teamMembers.ToList() };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreateTeamApprovalMapping")]
        public async Task<ActionResult> CreateOvertimeTeamApprovalMapping(long overtimeApproverId)
        {
            var appUser = AppUser();
            try
            {

                var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(overtimeApproverId, appUser);
                if (approver == null)
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                var employeeList = await _overtimeBusiness.GetEmployeesForOvertimeTeamApprovalMapping(appUser);

                if (!employeeList.Any())
                {
                    return NotFound(new { message = "Team Members Data Not Found." });
                }


                var teamMembers = await _overtimeBusiness.GetOvertimeTeamMembersByApproverId(approver.OvertimeApproverId, appUser);

                if (teamMembers.Any())
                {

                    employeeList = employeeList.Where(x => !teamMembers.Select(e => e.EmployeeId).Contains(x.EmployeeId));
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                int maxApprovalLevel = 0;
                int minApprovalLevel = 0;

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).FirstOrDefault();
                    maxApprovalLevel = first!.MaximumLevel;
                    minApprovalLevel = first!.MinimumLevel;
                }

                var result = new { approver, teamMembers = employeeList.ToList(), minApprovalLevel, maxApprovalLevel, approvalLevel = 1 };
                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveTeamApprovalMapping")]
        public async Task<ActionResult> SaveOvertimeTeamApprovalMapping(OvertimeTeamApprovalMappingDTO model)
        {
            var appUser = AppUser();

            try
            {

                var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(model.Approver.OvertimeApproverId, appUser);

                if (approver == null)
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                if (!model.TeamMembers.Any())
                {
                    return BadRequest(new { message = "Please Select Team Member(s) to Add in Approval mapping List " });
                }


                List<OvertimeEmployeeDTO> alreadyExist = new();
                List<OvertimeTeamApprovalMapping> newTeamMemberList = new();

                var allTeamMembersList = await _overtimeBusiness.GetOvertimeTeamMembersByApproverId(approver.OvertimeApproverId, appUser);

                //alreadyExist = model.TeamMembers.Where(x => allTeamMembersList.Select(e => e.EmployeeId).Contains(x.EmployeeId)).ToList();
                alreadyExist = allTeamMembersList.Where(x => model.TeamMembers.Select(e => e.EmployeeId).Contains(x.EmployeeId)).ToList();

                if (alreadyExist.Any())
                {

                    return BadRequest(new { message = $"Overtime Team Member(s) Already Exist.", TeamMembers = alreadyExist });
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                int maxApprovalLevel = 0;

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).FirstOrDefault();
                    maxApprovalLevel = first!.MaximumLevel;
                }

                newTeamMemberList = model.TeamMembers.Select(x => new OvertimeTeamApprovalMapping()
                {
                    OvertimeApproverId = approver.OvertimeApproverId,
                    EmployeeId = x.EmployeeId,
                    ApprovalLevel = model.ApprovalLevel > maxApprovalLevel ? maxApprovalLevel : model.ApprovalLevel
                }).ToList();

                var result = await _overtimeBusiness.SaveOvertimeTeamMembersToApprover(newTeamMemberList, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });

                }

                return Ok(new { message = "Overtime Team Member(s) Successfully Added." });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpDelete, Route("DeleteTeamMemberByApprovalMappingId/{id:long}")]
        public async Task<ActionResult> DeleteOvertimeTeamMemberByApprovalMappingId(long id)
        {
            var appUser = AppUser();
            try
            {
                var teamMember = await _overtimeBusiness.GetOvertimeTeamMemberByApprovalMappingId(id, appUser);

                if (teamMember == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var result = await _overtimeBusiness.DeleteOvertimeTeamMemberByApprovalMappingId(teamMember.OvertimeTeamApprovalMappingId, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Deleted Data." });

                }
                return Ok(new { message = "Overtime Team Approval Mapping Successfully Deleted." });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpDelete, Route("DeleteAllTeamMembersByApproverId/{id:long}")]
        public async Task<ActionResult> DeleteOvertimeAllTeamMembersByApproverId(long id)
        {

            var appUser = AppUser();
            try
            {

                var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(id, appUser);
                if (approver == null)
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                var result = await _overtimeBusiness.DeleteOvertimeTeamMemberByApproverId(approver.OvertimeApproverId, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Deleted Data." });

                }
                return Ok(new { message = "Overtime Team Approval Mapping(s) Successfully Deleted." });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("EditTeamApprovalMappingLevel/{id:long}")]
        public async Task<ActionResult> EditOvertimeTeamApprovalMappingLevel(long id)
        {
            var appUser = AppUser();
            try
            {
                var member = await _overtimeBusiness.GetOvertimeTeamMemberApprovalMappingById(id, appUser);
                if (member == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(member.OvertimeApproverId, appUser);
                if (approver == null)
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                int maxApprovalLevel = 0;
                int minApprovalLevel = 0;

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).FirstOrDefault();
                    maxApprovalLevel = first!.MaximumLevel;
                    minApprovalLevel = first!.MinimumLevel;
                }

                var result = new { approver, teamMember = member, minApprovalLevel, maxApprovalLevel };
                return Ok(result);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPut, Route("UpdateTeamApprovalMappingLevel/{id:long}")]
        public async Task<ActionResult> UpdateOvertimeTeamApprovalMappingLevel(long id, OvertimeEmployeeDTO model) //OvertimeEmployeeDTO
        {
            var appUser = AppUser();
            try
            {

                var mappingToUpdate = await _overtimeBusiness.GetOvertimeTeamMemberApprovalMappingById(id, appUser);
                if (mappingToUpdate == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                int level = model.ApprovalLevel;
                if (level < 1)
                {
                    return BadRequest(new { message = "Approval Level Must Be Greater then Zero." });
                }


                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                int maxApprovalLevel = 0;

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).FirstOrDefault();
                    maxApprovalLevel = first!.MaximumLevel;
                }

                level = level > maxApprovalLevel ? maxApprovalLevel : level;
                var result = await _overtimeBusiness.UpdateOvertimeTeamApprovalMappingLevel(mappingToUpdate.OvertimeTeamApprovalMappingId, level, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Update Data." });

                }
                return Ok(new { message = "Overtime Team Approval Mapping Level Successfully Updated." });

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }

        }

        #endregion

        #region Overtime Policy

        [HttpGet, Route("GetAllPolicy")]
        public async Task<ActionResult> GetAllOvertimePolicy()
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetAllOvertimePolicy(appUser);
                if (!result.Any())
                {
                    return NotFound(new { message = "No Data Found." });
                }

                return Ok(_mapper.Map<List<GetOvertimePolicyDTO>>(result.ToList()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetPolicyById/{id:long}")]
        public async Task<ActionResult> GetOvertimePolicyById(long id)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetOvertimePolicyById(id, appUser);
                if (result == null)
                {
                    return NotFound(new { message = "No Data Found." });

                }
                return Ok(_mapper.Map<GetOvertimePolicyDTO>(result));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreatePolicy")]
        public ActionResult CreateOvertimePolicy()
        {
            try
            {
                return Ok(new CreateOvertimeDTO());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SavePolicy")]
        public async Task<ActionResult> SaveOvertimePolicy(CreateOvertimePolicyDTO model)
        {
            var appUser = AppUser();
            try
            {

                var overtimePolicy = _mapper.Map<OvertimePolicy>(model);

                var alreadyExist = await _overtimeBusiness.GetOvertimePolicyBySpecification(nameof(overtimePolicy.OvertimeName), model.OvertimeName, appUser);
                if (alreadyExist.Any())
                {
                    return BadRequest(new { message = $" \'{model.OvertimeName}\' Already Exist. Try Something Different." });
                }


                var result = await _overtimeBusiness.SaveOvertimePolicy(overtimePolicy, appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });

                }
                return Ok(new { message = "Overtime Policy Successfully Created." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("EditPolicy/{id:long}")]
        public async Task<ActionResult> EditOvertimePolicy(long id)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetOvertimePolicyById(id, appUser);
                if (result == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                var createOvertimeDTO = new CreateOvertimeDTO();
                return Ok(new { OvertimePolicy = _mapper.Map<GetOvertimePolicyDTO>(result), createOvertimeDTO.Units, createOvertimeDTO.AmountTypes });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPut, Route("UpdatePolicy/{id:long}")]
        public async Task<ActionResult> UpdateOvertimePolicy(long id, CreateOvertimePolicyDTO model)
        {
            var appUser = AppUser();
            try
            {
                var policyToUpdate = await _overtimeBusiness.GetOvertimePolicyById(id, appUser);

                if (policyToUpdate == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var alreadyExist = await _overtimeBusiness.GetOvertimePolicyBySpecification(nameof(policyToUpdate.OvertimeName), model.OvertimeName, appUser);
                if (alreadyExist.Count() > 1)
                {
                    return BadRequest(new { message = $" \'{policyToUpdate.OvertimeName}\' Already Exist. Try Something Different." });
                }

                var overtimePolicy = _mapper.Map(model, policyToUpdate);

                var result = await _overtimeBusiness.UpdateOvertimePolicy(overtimePolicy, appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Update Data." });

                }
                return Ok(new { message = "Overtime Policy Successfully Updated." });

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }

        }

        [HttpDelete, Route("DeletePolicy/{id:long}")]
        public async Task<ActionResult> DeleteOvertimePolicy(long id)
        {
            var appUser = AppUser();
            try
            {
                var policy = await _overtimeBusiness.GetOvertimePolicyById(id, appUser);

                if (policy == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var result = await _overtimeBusiness.DeleteOvertimePolicy(policy.OvertimeId, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Deleted Data." });

                }
                return Ok(new { message = "Overtime Policy Successfully Deleted." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region Apply For Overtime Request 

        [HttpGet, Route("GetRequestById/{id:long}")]
        public async Task<ActionResult> GetOvertimeRequestById(long id)
        {
            var appUser = AppUser();
            try
            {
                var request = await _overtimeBusiness.GetOvertimeRequestById(id, appUser);
                if (request == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var result = _mapper.Map<GetOvertimeRequestDTO>(request);
                var employee = await _overtimeBusiness.GetEmployeeDetailsById(request.EmployeeId, appUser);

                if (employee != null)
                {
                    result.Employee = employee;
                }

                var overtimeType = await _overtimeBusiness.GetOvertimePolicyById(request.OvertimeId, appUser);

                if (overtimeType != null)
                {

                    result.OvertimeType = _mapper.Map<GetOvertimePolicyDTO>(overtimeType);
                }

                if (request.OvertimeRequestDetails.Any())
                {

                    foreach (var reqDetails in result.OvertimeRequestDetails)
                    {

                        var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(reqDetails.OvertimeApproverId, appUser);
                        if (approver != null)
                        {
                            reqDetails.Approver = approver;
                        }
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetAllRequest")]
        public async Task<ActionResult> GetAllOvertimeRequest(string status = "")
        {
            var appUser = AppUser();
            try
            {
                var allRequest = await _overtimeBusiness.GetAllOvertimeRequest(status, appUser);

                if (!allRequest.Any())
                {
                    return NotFound(new { message = "No Data Found..." });
                }

                var resultDTO = new List<GetOvertimeRequestDTO>();

                foreach (var request in allRequest)
                {

                    var result = _mapper.Map<GetOvertimeRequestDTO>(request);
                    var employee = await _overtimeBusiness.GetEmployeeDetailsById(request.EmployeeId, appUser);

                    if (employee != null)
                    {
                        result.Employee = employee;
                    }

                    var overtimeType = await _overtimeBusiness.GetOvertimePolicyById(request.OvertimeId, appUser);

                    if (overtimeType != null)
                    {

                        result.OvertimeType = _mapper.Map<GetOvertimePolicyDTO>(overtimeType);
                    }

                    if (request.OvertimeRequestDetails.Any())
                    {

                        foreach (var reqDetails in result.OvertimeRequestDetails)
                        {

                            var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(reqDetails.OvertimeApproverId, appUser);
                            if (approver != null)
                            {
                                reqDetails.Approver = approver;
                            }
                        }
                    }

                    resultDTO.Add(result);
                }

                //if (!string.IsNullOrWhiteSpace(status)) {

                //    var isStatusExist = resultDTO.Select(x => x.Status.ToLower() == status).FirstOrDefault();
                //    if (isStatusExist) {
                //        resultDTO = resultDTO.Where(x => x.Status.ToLower() == status).ToList();
                //    }
                //}

                return Ok(resultDTO.OrderByDescending(o => o.RequestDate));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetAllRequestForEmployee")]
        public async Task<ActionResult> GetAllOvertimeRequestForEmployee(string status = "")
        {
            var appUser = AppUser();
            try
            {
                var allRequest = await _overtimeBusiness.GetAllOvertimeRequestForEmployee(appUser.EmployeeId, status, appUser);

                if (!allRequest.Any())
                {
                    return NotFound(new { message = "No Data Found." });
                }

                var resultDTO = new List<GetOvertimeRequestDTO>();

                foreach (var request in allRequest)
                {

                    var result = _mapper.Map<GetOvertimeRequestDTO>(request);
                    var employee = await _overtimeBusiness.GetEmployeeDetailsById(request.EmployeeId, appUser);

                    if (employee != null)
                    {
                        result.Employee = employee;
                    }

                    var overtimeType = await _overtimeBusiness.GetOvertimePolicyById(request.OvertimeId, appUser);

                    if (overtimeType != null)
                    {

                        result.OvertimeType = _mapper.Map<GetOvertimePolicyDTO>(overtimeType);
                    }

                    if (request.OvertimeRequestDetails.Any())
                    {

                        foreach (var reqDetails in result.OvertimeRequestDetails)
                        {

                            var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(reqDetails.OvertimeApproverId, appUser);
                            if (approver != null)
                            {
                                reqDetails.Approver = approver;
                            }
                        }
                    }

                    resultDTO.Add(result);

                    //if (!string.IsNullOrWhiteSpace(status)) {

                    //    var isStatusExist = resultDTO.Select(x => x.Status.ToLower() == status).FirstOrDefault();
                    //    if (isStatusExist) {
                    //        resultDTO = resultDTO.Where(x => x.Status.ToLower() == status).ToList();
                    //    }
                    //}
                }

                return Ok(resultDTO.OrderByDescending(o => o.RequestDate));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetAllRequestForApprover")]
        public async Task<ActionResult> GetAllOvertimeRequestForApprover(string status = "")
        {
            var appUser = AppUser();
            try
            {

                List<GetOvertimeRequestDTO> resultDTO = new List<GetOvertimeRequestDTO>();

                var _approver = await _overtimeBusiness.GetOvertimeApproverByEmployeeId(appUser.EmployeeId, appUser); //8 K , 9 S, 10 P , 12 R, 13 M => appUser.EmployeeId 

                if (_approver == null)
                {

                    return NotFound(new { message = "No Data Found." });
                }

                var allRequest = await _overtimeBusiness.GetAllOvertimeRequestForApprover(_approver.OvertimeApproverId, status, appUser);

                if (!allRequest.Any())
                {
                    return NotFound(new { message = "No Data Found." });
                }



                foreach (var request in allRequest)
                {

                    var result = _mapper.Map<GetOvertimeRequestDTO>(request);
                    var employee = await _overtimeBusiness.GetEmployeeDetailsById(request.EmployeeId, appUser);

                    if (employee != null)
                    {
                        result.Employee = employee;
                    }

                    var overtimeType = await _overtimeBusiness.GetOvertimePolicyById(request.OvertimeId, appUser);

                    if (overtimeType != null)
                    {

                        result.OvertimeType = _mapper.Map<GetOvertimePolicyDTO>(overtimeType);
                    }

                    if (request.OvertimeRequestDetails.Any())
                    {

                        foreach (var reqDetails in result.OvertimeRequestDetails)
                        {

                            var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(reqDetails.OvertimeApproverId, appUser);
                            if (approver != null)
                            {
                                reqDetails.Approver = approver;
                            }
                        }
                    }

                    resultDTO.Add(result);

                    //if (!string.IsNullOrWhiteSpace(status)) {

                    //    var isStatusExist = resultDTO.Select(x => x.Status.ToLower() == status).FirstOrDefault();
                    //    if (isStatusExist) {
                    //        resultDTO = resultDTO.Where(x => x.Status.ToLower() == status).ToList();
                    //    }
                    //}
                }

                return Ok(resultDTO.OrderByDescending(o => o.RequestDate));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreateRequestFromEmployee")]
        public async Task<ActionResult> CreateOvertimeRequestFromEmployee()
        {
            var appUser = AppUser();
            try
            {
                CreateOvertimeRequestDTO result = new();

                var polices = await _overtimeBusiness.GetAllOvertimePolicy(appUser);
                if (polices.Any())
                {
                    result.OvertimeTypes = _mapper.Map<List<GetOvertimePolicyDTO>>(polices.ToList());
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).First();
                    result.MaxApprovalLevel = first.MaximumLevel;
                    result.MinApprovalLevel = first.MinimumLevel;
                }

                var teamMemberList = await _overtimeBusiness.GetOvertimeTeamMembersApprovalMappingByTeamMemberId(appUser.EmployeeId, appUser);

                foreach (var member in teamMemberList)
                {
                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(member.OvertimeApproverId, appUser);

                    if (approver != null)
                    {
                        result.Approvers.Add(approver);
                        result.TeamMembers.Add(member);
                    }

                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreateRequestFromApprover")]
        public async Task<ActionResult> CreateOvertimeRequestFromApprover()
        {
            var appUser = AppUser();
            try
            {
                CreateOvertimeRequestDTO result = new();

                var polices = await _overtimeBusiness.GetAllOvertimePolicy(appUser);
                if (polices.Any())
                {
                    result.OvertimeTypes = _mapper.Map<List<GetOvertimePolicyDTO>>(polices.ToList());
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).First();
                    result.MaxApprovalLevel = first.MaximumLevel;
                    result.MinApprovalLevel = first.MinimumLevel;
                }

                var approver = await _overtimeBusiness.GetOvertimeApproverByEmployeeId(appUser.EmployeeId, appUser);  //8 K , 9 S, 10 P , 12 R, 13 M => appUser.EmployeeId 
                if (approver != null)
                {
                    result.Approvers.Add(approver);

                    var teamMembers = await _overtimeBusiness.GetOvertimeTeamMembersByApproverId(approver.OvertimeApproverId, appUser);
                    if (teamMembers.Any())
                    {
                        result.TeamMembers = teamMembers.ToList();
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("CreateRequestFromAdmin")]
        public async Task<ActionResult> CreateOvertimeRequestFromAdmin()
        {
            var appUser = AppUser();
            try
            {
                CreateOvertimeRequestDTO result = new();

                var polices = await _overtimeBusiness.GetAllOvertimePolicy(appUser);
                if (polices.Any())
                {
                    result.OvertimeTypes = _mapper.Map<List<GetOvertimePolicyDTO>>(polices.ToList());
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).First();
                    result.MaxApprovalLevel = first.MaximumLevel;
                    result.MinApprovalLevel = first.MinimumLevel;
                }

                var teamMemberList = await _overtimeBusiness.GetAllOvertimeTeamMembersApprovalMapping(appUser);
                if (teamMemberList.Any())
                {

                    foreach (var approverWithTeam in teamMemberList.GroupBy(g => g.OvertimeApproverId))
                    {

                        var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(approverWithTeam.Key, appUser);
                        if (approver != null)
                        {
                            result.Approvers.Add(approver);
                        }

                        if (approverWithTeam.Any())
                        {
                            result.TeamMembers.AddRange(approverWithTeam.ToList());
                        }
                    }
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveRequest")]
        public async Task<ActionResult> SaveOvertimeRequest(SaveOvertimeRequestDTO model)
        {
            var appUser = AppUser();

            try
            {

                if (!model.Approvers.Any())
                {
                    return NotFound(new { message = "Approver Data Not Found." });
                }

                var alreadyExist = await _overtimeBusiness.CheckOvertimeRequestAlreadyExist(model, appUser);

                if (alreadyExist != null) { return BadRequest(new { message = $"Overtime Request Already Exist For {model.RequestDate.ToString("dd MMM yyyy")}" }); }

                OvertimeRequest overtimeRequest = new();

                overtimeRequest.EmployeeId = model.EmployeeId;
                overtimeRequest.AuthorityId = appUser.EmployeeId;
                overtimeRequest.OvertimeId = model.OvertimeId;
                overtimeRequest.RequestDate = model.RequestDate;
                overtimeRequest.StartTime = model.StartTime;
                overtimeRequest.EndTime = model.EndTime;
                overtimeRequest.Remarks = model.Remarks;
                overtimeRequest.ApplicationDate = DateTime.Now;
                overtimeRequest.Status = StateStatus.Pending;

                foreach (var item in model.Approvers)
                {

                    var overtimeRequestDetails = new OvertimeRequestDetails();

                    overtimeRequestDetails.OvertimeApproverId = item.OvertimeApproverId;
                    overtimeRequestDetails.ApprovalOrder = item.ApprovalOrder;
                    if (item.ApprovalOrder == 1)
                    {
                        overtimeRequestDetails.ActionRequired = true;
                        var name = $"{item.Name} ({item.EmployeeCode})";

                        if (overtimeRequest.WaitingStage == "-")
                        {
                            overtimeRequest.WaitingStage = name;
                        }
                        else
                        {
                            overtimeRequest.WaitingStage = overtimeRequest.WaitingStage + " | " + name;
                        }

                    }
                    overtimeRequestDetails.Status = StateStatus.Pending;
                    overtimeRequest.OvertimeRequestDetails.Add(overtimeRequestDetails);

                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(item.OvertimeApproverId, appUser);

                    if (approver != null && approver.ProxyEnabled)
                    {

                        overtimeRequestDetails.OvertimeApproverId = approver.ProxyApproverId;
                        overtimeRequestDetails.ApprovalOrder = item.ApprovalOrder;
                        if (item.ApprovalOrder == 1)
                        {
                            overtimeRequestDetails.ActionRequired = true;
                        }
                        overtimeRequestDetails.Status = StateStatus.Pending;
                        overtimeRequest.OvertimeRequestDetails.Add(overtimeRequestDetails);
                    }
                }

                var result = await _overtimeBusiness.SaveOvertimeRequest(overtimeRequest, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });
                }

                return Ok(new { message = "Overtime Request Successfully Submitted." });
            }


            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveRequestFromApprover")]
        public async Task<ActionResult> SaveOvertimeRequestFromApprover(SaveOvertimeRequestDTO model)
        {
            var appUser = AppUser();

            try
            {

                //if (!model.Approvers.Any()) {
                //    return NotFound(new { message = "Approver Data Not Found." });
                //}

                var alreadyExist = await _overtimeBusiness.CheckOvertimeRequestAlreadyExist(model, appUser);

                if (alreadyExist != null) { return BadRequest(new { message = $"Overtime Request Already Exist For {model.RequestDate.ToString("dd MMM yyyy")}" }); }

                OvertimeRequest overtimeRequest = new();

                overtimeRequest.EmployeeId = model.EmployeeId;
                overtimeRequest.AuthorityId = appUser.EmployeeId;
                overtimeRequest.OvertimeId = model.OvertimeId;
                overtimeRequest.RequestDate = model.RequestDate;
                overtimeRequest.StartTime = model.StartTime;
                overtimeRequest.EndTime = model.EndTime;
                overtimeRequest.Remarks = model.Remarks;
                overtimeRequest.ApplicationDate = DateTime.Now;
                overtimeRequest.Status = StateStatus.Pending;

                var teamMemberList = await _overtimeBusiness.GetOvertimeTeamMembersApprovalMappingByTeamMemberId(model.EmployeeId, appUser);

                foreach (var member in teamMemberList)
                {
                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(member.OvertimeApproverId, appUser);
                    if (approver != null)
                    {
                        var obj = new OvertimeRequestApprover() { OvertimeApproverId = approver.OvertimeApproverId, ApprovalOrder = member.ApprovalLevel, Name = approver.Name, EmployeeCode = approver.EmployeeCode };
                        model.Approvers.Add(obj);
                    }

                }

                foreach (var item in model.Approvers)
                {

                    var overtimeRequestDetails = new OvertimeRequestDetails();

                    overtimeRequestDetails.OvertimeApproverId = item.OvertimeApproverId;
                    overtimeRequestDetails.ApprovalOrder = item.ApprovalOrder;
                    if (item.ApprovalOrder == 1)
                    {
                        overtimeRequestDetails.ActionRequired = true;
                        var name = $"{item.Name} ({item.EmployeeCode})";

                        if (overtimeRequest.WaitingStage == "-")
                        {
                            overtimeRequest.WaitingStage = name;
                        }
                        else
                        {
                            overtimeRequest.WaitingStage = overtimeRequest.WaitingStage + " | " + name;
                        }

                    }
                    overtimeRequestDetails.Status = StateStatus.Pending;
                    overtimeRequest.OvertimeRequestDetails.Add(overtimeRequestDetails);

                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(item.OvertimeApproverId, appUser);

                    if (approver != null && approver.ProxyEnabled)
                    {

                        overtimeRequestDetails.OvertimeApproverId = approver.ProxyApproverId;
                        overtimeRequestDetails.ApprovalOrder = item.ApprovalOrder;
                        if (item.ApprovalOrder == 1)
                        {
                            overtimeRequestDetails.ActionRequired = true;
                        }
                        overtimeRequestDetails.Status = StateStatus.Pending;
                        overtimeRequest.OvertimeRequestDetails.Add(overtimeRequestDetails);
                    }
                }

                var result = await _overtimeBusiness.SaveOvertimeRequest(overtimeRequest, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });
                }

                return Ok(new { message = "Overtime Request Successfully Submitted." });
            }


            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("ApprovalAction")]
        public async Task<ActionResult> OvertimeRequestApprovalAction(OvertimeRequestApprovalActionDTO model)
        {
            var appUser = AppUser();
            try
            {
                var request = await _overtimeBusiness.GetOvertimeRequestById(model.OvertimeRequestId, appUser);

                if (request == null) { return NotFound(new { message = "Request Data Not Found." }); }

                if (request.Status.ToLower() != StateStatus.Pending.ToLower()) { return BadRequest(new { message = "Can't Process The Request, Already Processed." }); }

                if (!request.OvertimeRequestDetails.Any()) { return NotFound(new { message = "Request Details Data Not Found." }); }

                var currentApproverDetailsList = request.OvertimeRequestDetails.Where(x => x.ActionRequired == true && x.Status.ToLower() == StateStatus.Pending.ToLower()).ToList();

                if (!currentApproverDetailsList.Any()) { return BadRequest(new { message = "Can't Process The Request, Already Processed." }); }

                int nextApprovalOrder = 0;
                long overtimeRequestDetailsId = 0;

                foreach (var currentApproverDetails in currentApproverDetailsList)
                {
                    var _approver = await _overtimeBusiness.GetOvertimeApproverByEmployeeId(appUser.EmployeeId, appUser);

                    if (_approver != null && _approver?.OvertimeApproverId == currentApproverDetails.OvertimeApproverId)
                    {
                        currentApproverDetails.Status = model.Status;
                    }
                    else
                    {

                        currentApproverDetails.Status = model.Status; //Will be commented  later
                        //currentApproverDetails.Status = "-";  //Will be Uncommented  later 
                    }

                    currentApproverDetails.IsReverted = model.Status.ToLower() == StateStatus.Reverted.ToLower();
                    currentApproverDetails.Remarks = model.Remarks.Trim();
                    currentApproverDetails.ActionRequired = false;
                    currentApproverDetails.ProcessAt = DateTime.Now;

                    nextApprovalOrder = currentApproverDetails.ApprovalOrder;
                    overtimeRequestDetailsId = currentApproverDetails.OvertimeRequestDetailsId;
                }

                nextApprovalOrder += 1;

                var nextApproverDetailsList = request.OvertimeRequestDetails.Where(x => x.ApprovalOrder == nextApprovalOrder && x.OvertimeRequestDetailsId > overtimeRequestDetailsId).ToList();
                var names = new List<string>();

                if (nextApproverDetailsList.Any())
                {
                    foreach (var nextApproverDetails in nextApproverDetailsList)
                    {

                        if (model.Status.ToLower() != StateStatus.Approved.ToLower() || nextApproverDetails == null)
                        {
                            request.Status = model.Status;
                            request.WaitingStage = "-";

                            if (model.Status.ToLower() == StateStatus.Reverted.ToLower())
                            {
                                foreach (var details in request.OvertimeRequestDetails.Where(x => !currentApproverDetailsList.Select(s => s.OvertimeRequestDetailsId).Contains(x.OvertimeRequestDetailsId)))
                                {
                                    details.IsReverted = true;
                                    details.ActionRequired = false;
                                }
                            }
                        }

                        else
                        {
                            if (nextApproverDetails.IsReverted == false)
                            {
                                nextApproverDetails.ActionRequired = true;
                            }

                            var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(nextApproverDetails.OvertimeApproverId, appUser);

                            if (approver == null) { return NotFound(new { message = "Next Level Approver Data Not Found." }); }

                            else
                            {
                                var name = $"{approver.Name} ({approver.EmployeeCode})";
                                names.Add(name);

                            }
                            request.WaitingStage = string.Join(" | ", names.Distinct());
                        }
                    }
                }
                else
                {
                    request.Status = model.Status;
                    request.WaitingStage = "-";
                }





                var result = await _overtimeBusiness.OvertimeRequestApprovalAction(request, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Perform The Action" });
                }

                return Ok(new { message = $"Overtime Request Successfully {model.Status}" });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("ApprovalAction2")]
        public async Task<ActionResult> OvertimeRequestApprovalAction2(OvertimeRequestApprovalActionDTO model)
        {
            var appUser = AppUser();
            try
            {
                var request = await _overtimeBusiness.GetOvertimeRequestById(model.OvertimeRequestId, appUser);

                if (request == null) { return NotFound(new { message = "Request Data Not Found." }); }

                if (request.Status.ToLower() != StateStatus.Pending.ToLower()) { return BadRequest(new { message = "Can't Process The Request, Already Processed." }); }

                if (!request.OvertimeRequestDetails.Any()) { return NotFound(new { message = "Request Details Data Not Found." }); }

                var currentApproverDetails = request.OvertimeRequestDetails.FirstOrDefault(x => x.ActionRequired == true && x.Status.ToLower() == StateStatus.Pending.ToLower());

                if (currentApproverDetails == null) { return BadRequest(new { message = "Can't Process The Request, Already Processed." }); }

                currentApproverDetails.Status = model.Status;
                currentApproverDetails.IsReverted = model.Status.ToLower() == StateStatus.Reverted.ToLower();
                currentApproverDetails.Remarks = model.Remarks.Trim();
                currentApproverDetails.ActionRequired = false;
                currentApproverDetails.ProcessAt = DateTime.Now;

                var nextApprovalOrder = currentApproverDetails.ApprovalOrder + 1;
                var nextApproverDetails = request.OvertimeRequestDetails.FirstOrDefault(x => x.ApprovalOrder == nextApprovalOrder);

                if (model.Status.ToLower() != StateStatus.Approved.ToLower() || nextApproverDetails == null)
                {
                    request.Status = model.Status;
                    request.WaitingStage = "-";

                    if (model.Status.ToLower() == StateStatus.Reverted.ToLower())
                    {
                        foreach (var details in request.OvertimeRequestDetails.Where(x => x.OvertimeRequestDetailsId != currentApproverDetails.OvertimeRequestDetailsId))
                        {
                            details.IsReverted = true;
                        }
                    }
                }

                else
                {
                    nextApproverDetails.ActionRequired = true;
                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(nextApproverDetails.OvertimeApproverId, appUser);
                    if (approver == null) { return NotFound(new { message = "Next Level Approver Data Not Found." }); }
                    else { request.WaitingStage = $"{approver.Name} ({approver.EmployeeCode})"; }
                }

                var result = await _overtimeBusiness.OvertimeRequestApprovalAction(request, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Perform The Action" });
                }

                return Ok(new { message = $"Overtime Request Successfully {model.Status}" });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("EditRequest/{id:long}")]
        public async Task<ActionResult> EditOvertimeRequest(long id)
        {
            var appUser = AppUser();
            try
            {
                var request = await _overtimeBusiness.GetOvertimeRequestById(id, appUser);

                if (request == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                if (request.Status.ToLower() != StateStatus.Reverted.ToLower())
                {
                    return BadRequest(new { message = "Invalid Request. Request Can't Be Processed" });
                }

                CreateOvertimeRequestDTO createRequest = new();
                var overtimeRequest = _mapper.Map<GetOvertimeRequestDTO>(request);
                var employee = await _overtimeBusiness.GetEmployeeDetailsById(request.EmployeeId, appUser);

                if (employee != null)
                {
                    overtimeRequest.Employee = employee;
                }

                var overtimeType = await _overtimeBusiness.GetOvertimePolicyById(request.OvertimeId, appUser);

                if (overtimeType != null)
                {

                    overtimeRequest.OvertimeType = _mapper.Map<GetOvertimePolicyDTO>(overtimeType);
                }

                if (request.OvertimeRequestDetails.Any())
                {

                    foreach (var reqDetails in overtimeRequest.OvertimeRequestDetails)
                    {

                        var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(reqDetails.OvertimeApproverId, appUser);
                        if (approver != null)
                        {
                            reqDetails.Approver = approver;
                        }
                    }
                }


                var polices = await _overtimeBusiness.GetAllOvertimePolicy(appUser);
                if (polices.Any())
                {
                    createRequest.OvertimeTypes = _mapper.Map<List<GetOvertimePolicyDTO>>(polices.ToList());
                }

                var levels = await _overtimeBusiness.GetAllOvertimeApprovalLevel(appUser);

                if (levels.Any())
                {
                    var first = _mapper.Map<List<GetOvertimeApprovalLevelDTO>>(levels.ToList()).First();
                    createRequest.MaxApprovalLevel = first.MaximumLevel;
                    createRequest.MinApprovalLevel = first.MinimumLevel;
                }

                var teamMemberList = await _overtimeBusiness.GetOvertimeTeamMembersApprovalMappingByTeamMemberId(request.EmployeeId, appUser);

                foreach (var member in teamMemberList)
                {
                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(member.OvertimeApproverId, appUser);

                    if (approver != null)
                    {
                        createRequest.Approvers.Add(approver);
                        createRequest.TeamMembers.Add(member);
                    }

                }

                return Ok(new { createRequest, appliedRequest = overtimeRequest });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }

        }

        [HttpPut, Route("UpdateRequest/{id:long}")]
        public async Task<ActionResult> UpdateOvertimeRequest(long id, SaveOvertimeRequestDTO model)
        {
            var appUser = AppUser();

            try
            {

                var overtimeRequest = await _overtimeBusiness.GetOvertimeRequestById(id, appUser);

                if (overtimeRequest == null) { return NotFound(new { message = "No Data Found." }); }

                if (overtimeRequest.Status.ToLower() != StateStatus.Reverted.ToLower()) { return BadRequest(new { message = "Invalid Request. Request Can't Be Processed" }); }

                if (!model.Approvers.Any()) { return NotFound(new { message = "Approver Data Not Found." }); }

                //var alreadyExist = await _overtimeBusiness.CheckOvertimeRequestAlreadyExist(model, appUser);

                //if (alreadyExist != null) { return BadRequest(new { message = $"Overtime Request Already Exist For {model.RequestDate.ToString("dd MMM yyyy")}" }); }

                overtimeRequest.EmployeeId = model.EmployeeId;
                overtimeRequest.AuthorityId = appUser.EmployeeId;
                overtimeRequest.OvertimeId = model.OvertimeId;
                overtimeRequest.RequestDate = model.RequestDate;
                overtimeRequest.StartTime = model.StartTime;
                overtimeRequest.EndTime = model.EndTime;
                overtimeRequest.Remarks = model.Remarks;
                overtimeRequest.ApplicationDate = DateTime.Now;
                overtimeRequest.Status = StateStatus.Pending;

                overtimeRequest.OvertimeRequestDetails = new();

                foreach (var item in model.Approvers)
                {

                    var overtimeRequestDetails = new OvertimeRequestDetails();

                    overtimeRequestDetails.OvertimeApproverId = item.OvertimeApproverId;
                    overtimeRequestDetails.ApprovalOrder = item.ApprovalOrder;
                    //if (item.ApprovalOrder == 1) {
                    //    overtimeRequestDetails.ActionRequired = true;
                    //    overtimeRequest.WaitingStage = $"{item.Name} ({item.EmployeeCode})";

                    //}

                    if (item.ApprovalOrder == 1)
                    {
                        overtimeRequestDetails.ActionRequired = true;
                        var name = $"{item.Name} ({item.EmployeeCode})";

                        if (overtimeRequest.WaitingStage == "-")
                        {
                            overtimeRequest.WaitingStage = name;
                        }
                        else
                        {
                            overtimeRequest.WaitingStage = overtimeRequest.WaitingStage + " | " + name;
                        }

                    }
                    overtimeRequestDetails.Status = StateStatus.Pending;
                    overtimeRequest.OvertimeRequestDetails.Add(overtimeRequestDetails);

                    var approver = await _overtimeBusiness.GetOvertimeApproverByApproverId(item.OvertimeApproverId, appUser);

                    if (approver != null && approver.ProxyEnabled)
                    {

                        overtimeRequestDetails.OvertimeApproverId = approver.ProxyApproverId;
                        overtimeRequestDetails.ApprovalOrder = item.ApprovalOrder;
                        if (item.ApprovalOrder == 1)
                        {
                            overtimeRequestDetails.ActionRequired = true;
                        }
                        overtimeRequestDetails.Status = StateStatus.Pending;
                        overtimeRequest.OvertimeRequestDetails.Add(overtimeRequestDetails);
                    }

                }

                var result = await _overtimeBusiness.UpdateOvertimeRequest(overtimeRequest, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Update Data." });
                }

                return Ok(new { message = "Overtime Request Successfully Updated." });
            }


            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpDelete, Route("DeleteRequest/{id:long}")]
        public async Task<ActionResult> DeleteOvertimeRequest(long id)
        {
            var appUser = AppUser();
            try
            {

                var request = await _overtimeBusiness.GetOvertimeRequestById(id, appUser);

                if (request == null) { return NotFound(new { message = "No Data Found." }); }

                var pendingAtFirstLevel = request.OvertimeRequestDetails.Select(x => x.ApprovalOrder == 1 && x.Status == StateStatus.Pending).FirstOrDefault();

                if (!pendingAtFirstLevel) { return BadRequest(new { message = "Request Can't Be Deleted, Already Processed" }); }

                var result = await _overtimeBusiness.DeleteOvertimeRequesById(id, appUser);

                if (!result.Status) { return BadRequest(new { message = "Something Went Wrong. Failed to Deleted Data." }); }

                return Ok(new { message = "Overtime Request Successfully Deleted." });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region Overtime Process

        [HttpGet, Route("GetAllProcess")]
        public async Task<ActionResult> GetAllOvertimeProcess()
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.GetAllOvertimeProcess(appUser);
                if (!result.Any())
                {
                    return NotFound(new { message = "No Overtime Process Data Found." });
                }
                return Ok(_mapper.Map<List<OvertimeProcess>>(result.ToList()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("Process")]
        public async Task<ActionResult> ProcessOvertime(OvertimeProcessViewModel model)
        {
            var appUser = AppUser();
            try
            {
                var result = await _overtimeBusiness.OvertimeProcess(model, appUser);

                if (result.Status)
                {
                    return Ok(new { message = result.Msg });
                }
                else
                {
                    return BadRequest(new { message = result.Msg });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpDelete, Route("Roll-Back/{id:long}")]
        public async Task<ActionResult> RollBackProcess(long id)
        {
            var appUser = AppUser();
            try
            {
                var overtimeProcess = await _overtimeBusiness.GetOvertimeProcessById(id, appUser);

                if (overtimeProcess == null)
                {
                    return NotFound(new { message = "Overtime Process Data Not Found." });
                }

                var result = await _overtimeBusiness.RollBackOvertimeProcess(overtimeProcess.Id, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Rool-Back Data." });

                }
                return Ok(new { message = "Overtime Process Successfully Roll-Back." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPut, Route("Disburse/{id:long}")]
        public async Task<ActionResult> DisburseOvertimeProcess(long id, OvertimeProcess model)
        {
            var appUser = AppUser();
            try
            {

                var overtimeProcess = await _overtimeBusiness.GetOvertimeProcessById(id, appUser);

                if (overtimeProcess == null)
                {
                    return NotFound(new { message = "Overtime Process Data Not Found." });
                }

                var result = await _overtimeBusiness.DisburseOvertimeProcess(model, appUser);
                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Update Data." });

                }
                return Ok(new { message = "Overtime Process Disbursed successfully." });

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }

        }

        [HttpPost, Route("UploadTimeCard")]
        public async Task<ActionResult> UploadOvertimeTimeCard([FromForm] UploadTimeCardDTO model)
        {
            var appUser = AppUser();

            try
            {
                if (model.ExcelFile?.Length == 0)
                {
                    return BadRequest(new { message = "No File Found." });
                }

                var stream = model.ExcelFile!.OpenReadStream();
                List<UploadOvertimeAllowances> uploadOTAllowanceList = new List<UploadOvertimeAllowances>();
                var errorEmpCode = new List<string>();

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    var rowCount = worksheet!.Dimension.Rows;
                    for (var row = 2; row <= rowCount; row++)
                    {

                        var empCode = worksheet.Cells[row, 1].Value?.ToString();

                        if (empCode != null)
                        {

                            var employee = await _overtimeBusiness.GetEmployeeDetailsByCode(empCode.Trim(), appUser);

                            if (employee != null)
                            {

                                var uploadAmount = Convert.ToDecimal(worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString() : 0);
                                var uploadArrearAmount = Convert.ToDecimal(worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : 0);
                                var remarks = worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString() : "-";

                                var proStartDate = new DateTime(model.Year, model.Month, 1);
                                var daysInMonth = DateTime.DaysInMonth(model.Year, model.Month);
                                DateTime proEndDate = new DateTime(model.Year, model.Month, daysInMonth);
                                DateTime salaryMonth = proEndDate;

                                var uploadOTAllowance = new UploadOvertimeAllowances
                                {
                                    EmployeeId = employee.EmployeeId,
                                    OvertimeId = model.OvertimeId,
                                    OvertimeName = model.OvertimeName,
                                    IsUnitUpload = model.IsUnitUpload,
                                    IsAmountUpload = model.isAmountUpload,
                                    SalaryMonth = salaryMonth
                                };


                                if (model.isAmountUpload)
                                {
                                    uploadOTAllowance.Amount = uploadAmount;
                                }
                                else
                                {
                                    uploadOTAllowance.Unit = uploadAmount;
                                }

                                uploadOTAllowance.ArrearAmount = uploadArrearAmount;
                                uploadOTAllowance.Remarks = remarks;

                                uploadOTAllowanceList.Add(uploadOTAllowance);

                            }
                            else
                            {
                                errorEmpCode.Add(empCode);
                            }
                        }

                    }
                }

                if (!uploadOTAllowanceList.Any())
                {
                    return BadRequest(new { message = "No Uploaded Data Found." });
                }

                var result = await _overtimeBusiness.UploadOvertimeTimeCard(uploadOTAllowanceList, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = "Something Went Wrong. Failed to Store Data." });

                }

                string errorEmpCodeMsg = string.Empty;

                if (errorEmpCode.Any())
                {
                    errorEmpCodeMsg = $"Could Not Found This Id(s) {string.Join(",", errorEmpCode)} ";
                }
                return Ok(new { message = $"Overtime Time Card Successfully Uploaded. {errorEmpCodeMsg}" });

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("RollBackTimeCard")]
        public async Task<ActionResult> RollBackOvertimeTimeCard(RollBackTimeCardDTO model)
        {
            var appUser = AppUser();

            try
            {

                var result = await _overtimeBusiness.RollBackUploadedTimeCard(model, appUser);

                if (!result.Status)
                {
                    return BadRequest(new { message = result.Msg });
                }

                return Ok(new { message = $"Uploaded Overtime Time Card Successfully Rolled Back." });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("DownloadTimeCardExcelFile")]
        public async Task<IActionResult> DownloadCashSalaryExcelFile(string fileName)
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

        #endregion


        [HttpPost, Route("GeoLocationAttendance")]
        public async Task<ActionResult> GeoLocationAttendance(GeoLocationAttendanceDTO model)
        {
            var appUser = AppUser();
            try
            {
                await Task.Delay(10);
                return Ok(new { message = "Successfully Submitted" });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

    }
}
