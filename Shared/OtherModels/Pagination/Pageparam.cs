namespace Shared.OtherModels.Pagination
{
    public class Pageparam
    {
        const int maxPageSize = 50;
        private int _pageSize = 15;
        public Pageparam()
        {

        }
        public int PageSize {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value < 1 ? 15 : value; }
        }
        public int PageNumber { get; set; } = 1;
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
    }
}
