using Shared.Leave.DTO.Balance;
using System.ComponentModel.DataAnnotations;

public class EmployeeLeaveBalanceDto
{
    public long EmployeeId { get; set; }

    public string EmployeeName { get; set; }

    public List<LeaveBalanceDto> LeaveBalances { get; set; }
}