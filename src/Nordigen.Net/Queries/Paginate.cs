namespace Nordigen.Net.Queries;

public class Paginate<T>
{
    public Paginate(int limit = 100, int offset = 0)
    {
        Limit = limit;
        Offset = offset;
    }

    public int Limit { get; }

    public int Offset { get; }
}