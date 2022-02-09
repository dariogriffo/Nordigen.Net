namespace Nordigen.Net.Queries
{
    public class Paginate<T>
    {
        public Paginate(int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
        }

        public int Limit { get; }

        public int Offset { get; }
    }
}
