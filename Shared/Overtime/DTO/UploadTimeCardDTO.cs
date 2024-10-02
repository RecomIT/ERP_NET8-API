using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;

namespace Shared.Overtime.DTO
{
    public class UploadTimeCardDTO
    {
        public long OvertimeId { get; set; } = 0;
        public string OvertimeName { get; set; } = "-";
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public bool IsUnitUpload { get; set; } = true;
        public bool isAmountUpload { get; set; } = false;

        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile ExcelFile { get; set; }

    }
}
