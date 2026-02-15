using NamSitaKaurLMS.Core.Concrete;

namespace NamSitaKaurLMS.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetUserById(string identityUserId);

    }
}
