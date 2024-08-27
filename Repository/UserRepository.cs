using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Model;

namespace CoreApiInNet.Repository
{
    public class UserRepository : GenericRepository<DbModelUser>, InterfaceUserRepository
    {
        public UserRepository(ModelDbContext context) : base(context)
        {

        }
    }
}
