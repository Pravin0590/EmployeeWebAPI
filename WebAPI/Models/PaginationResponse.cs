namespace WebAPI.Models
{
    public class PaginationResponse<T> where T : class
    {
        public int PageSize { get; private set; }

        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public IEnumerable<T> Data { get; private set; }

        public PaginationResponse(int pageSize, int pageNumber, long totalCount,IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            Data = data;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
