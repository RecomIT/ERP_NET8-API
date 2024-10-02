namespace Shared.Employee.ViewModel.Info
{
    public class ReadInfoFromExcel
    {
        public ReadInfoFromExcel()
        {
            Collections = new List<ExcelInfoCollection>();
        }
        public List<string> Columns { get; set; }
        public List<ExcelInfoCollection> Collections { get; set; }
    }
    public class ExcelInfoCollection
    {
        public ExcelInfoCollection()
        {
            ExcelInfos = new List<ExcelInfo>();
        }
        public long Id { get; set; } = 0;
        public string Code { get; set; } = "";
        public bool IsValid { get; set; } = true;
        public string ErrorMsg { get; set; }
        public bool IsNew { get; set; } = false;
        public List<ExcelInfo> ExcelInfos { get; set; }
    }
    public class ExcelInfo
    {
        public string Column { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public string ErrorMsg { get; set; }
        public bool HasDefualtValue { get; set; } = false;
        public bool IsValid { get; set; } =true;
        public string Group { get; set; } = "";
    }
}
