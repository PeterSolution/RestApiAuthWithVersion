using AutoMapper;
using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Model;

namespace CoreApiInNet.Repository
{
    public class UserRepository : GenericRepository<DbModelUser>, InterfaceUserRepository
    {
        public UserRepository(ModelDbContext context,IMapper mapper) : base(context, mapper)
        {

        }
    }
}
