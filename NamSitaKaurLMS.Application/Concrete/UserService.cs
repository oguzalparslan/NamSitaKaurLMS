using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Concrete
{
    public class UserService : IUserService
    {
        public IUserRepository userRepository { get; set; }
        public IUnitOfWork unitOfWork { get; set; }

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddUserAsync(User user)
        {
            await unitOfWork.Repository<User>().AddAsync(user);
            await unitOfWork.SaveAsync();
        }
    }
}
