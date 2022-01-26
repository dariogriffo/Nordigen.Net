namespace Nordigen.Net.Internal
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    internal class Serializer : ISerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value, Settings);

        public string Serialize<T>(T value) => JsonConvert.SerializeObject(value, Settings);
    }
}
