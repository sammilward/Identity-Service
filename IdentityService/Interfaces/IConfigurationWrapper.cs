namespace IdentityService.Interfaces
{
    public interface IConfigurationWrapper
    {
        T GetValue<T>(string key);
    }
}
