using System.Threading.Tasks;

namespace IdentityService.Interfaces
{
    public interface IEfDatabaseRepository
    {
        Task<bool> IsDatabaseHealthyAsync();
    }
}
