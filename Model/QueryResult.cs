namespace CoreApiInNet.Model
{
    public class QueryResult<T>
    {
        public int Amount { get; set; }
        public int TotalAmount { get; set; }
        public int PageNumber { get; set; }
        public List<T> Items { get; set; }
    }
}