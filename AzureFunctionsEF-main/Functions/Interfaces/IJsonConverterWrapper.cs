namespace Functions.Interfaces
{
    public interface IJsonConverterWrapper
    {
        T DeserializeObject<T>(string value);
    }
}