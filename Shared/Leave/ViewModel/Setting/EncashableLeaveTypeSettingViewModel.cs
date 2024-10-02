

namespace Shared.Leave.ViewModel.Setting
{
    public class EncashableLeaveTypeSettingViewModel
    {
        public long Id { get; set; }
        public decimal? TotalLeave { get; set; }
        public bool IsLeaveEncashable { get; set; }
        public short? MinEncashablePercentage { get; set; }
        public short? MaxEncashablePercentage { get; set; }
    }
}
