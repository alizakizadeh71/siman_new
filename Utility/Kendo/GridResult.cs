namespace Utilities.Kendo
{
    public class GridResult<T> : System.Object
    {
        public GridResult()
        {
        }

        public int Total { get; set; }
        public System.Collections.Generic.IEnumerable<T> Data { get; set; }
    }
}
