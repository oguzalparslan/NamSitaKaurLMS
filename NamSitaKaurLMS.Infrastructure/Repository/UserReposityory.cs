using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;

namespace NamSitaKaurLMS.Infrastructure.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(NamSitaKaurLMSContext context) : base(context)
        {
        }

        public async Task<User> GetUserById(string identityUserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.AppUserId == identityUserId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return Task.FromResult(user).Result;
        }


    }
}
