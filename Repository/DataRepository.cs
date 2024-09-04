using AutoMapper;
using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Model;

namespace CoreApiInNet.Repository
{
    public class DataRepository : GenericRepository<DbModelData>, InterfaceDataRepository
    {
        public DataRepository(ModelDbContext context,IMapper mapper) : base(context,mapper)
        {
        }
    }
}
