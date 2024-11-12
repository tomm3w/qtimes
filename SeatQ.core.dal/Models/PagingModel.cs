namespace SeatQ.core.dal.Models
{
    public class PagingModel
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? TotalData { get; set; }
        public int? TotalPages { get; set; }
    }
}