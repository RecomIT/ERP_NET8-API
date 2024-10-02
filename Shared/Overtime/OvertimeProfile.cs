using AutoMapper;
using Shared.Overtime.Domain;
using Shared.Overtime.DTO;

namespace Shared.Overtime
{
    public class OvertimeProfile : Profile
    {
        public OvertimeProfile()
        {
            CreateMap<OvertimePolicy, CreateOvertimePolicyDTO>().ReverseMap();
            CreateMap<OvertimePolicy, GetOvertimePolicyDTO>().ReverseMap();

            //Overtime Approval Level
            CreateMap<OvertimeApprovalLevel, CreateOvertimeApprovalLevelDTO>().ReverseMap();
            CreateMap<OvertimeApprovalLevel, GetOvertimeApprovalLevelDTO>().ReverseMap();

            //Overtime Approver
            CreateMap<OvertimeApprover, OvertimeApproverDTO>().ReverseMap();

            //Overtime Team Approver Mapping
            CreateMap<OvertimeTeamApprovalMapping, CreateOvertimeTeamApprovalMappingDTO>().ReverseMap();


            //Overtime Request Mapping
            CreateMap<OvertimeRequest, GetOvertimeRequestDTO>().ReverseMap();
            CreateMap<OvertimeRequestDetails, OvertimeRequestDetailsDTO>().ReverseMap();
        }
    }
}
