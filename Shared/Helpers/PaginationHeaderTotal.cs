namespace Shared.Helpers
{
    public class PaginationHeaderTotal
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public long TotalItems { get; set; }
        public long TotalPages { get; set; }

        public PaginationHeaderTotal(int currentPage, int itemsPerPage,long totalItems, long totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}
