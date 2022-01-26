namespace Nordigen.Net.Internal
{
    internal interface ISerializer
    {
        T Deserialize<T>(string value);

        string Serialize<T>(T value);
    }
}
