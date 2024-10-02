
using Shared.Leave.ViewModel.Setting;

namespace Shared.Leave.ViewModel.Type
{
    public class LeaveTypesWithSettings
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string TitleInBengali { get; set; }
        public string ShortName { get; set; }
        public string ShortNameInBangali { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SerialNo { get; set; }
        public ICollection<LeaveSettingViewModel> LeaveSettings { get; set; }
    }
}
