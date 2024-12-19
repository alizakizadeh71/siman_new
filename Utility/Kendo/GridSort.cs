namespace Utilities.Kendo
{
    public class GridSort : System.Object
    {
        public GridSort(string field, string direction)
        {
            Field = field;
            Direction = direction;
        }

        public string Field { get; set; }
        public string Direction { get; set; }
    }
}
