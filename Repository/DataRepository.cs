using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Model;

namespace CoreApiInNet.Repository
{
    public class DataRepository : GenericRepository<DbModelData>, InterfaceDataRepository
    {
        public DataRepository(ModelDbContext context) : base(context)
        {
        }
    }
}
