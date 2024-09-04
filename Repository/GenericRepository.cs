using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace CoreApiInNet.Repository
{
    public class GenericRepository<T> : InterfaceGenericReposotory<T> where T : class
    {
        private readonly ModelDbContext context;
        public IMapper Mapper { get; }

        public GenericRepository(ModelDbContext context,IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }


        public async Task<T> AddAsync(T entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity=await GetAsync(id);
            context.Set<T>().Remove(entity);
            return entity;
        }

        public async Task<bool> Exist(int id)
        {
            var entity=await GetAsync(id);
            //return entity != null;
            if (entity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            }
            else
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<QueryResult<TResult>> GetResultsAsync<TResult>(QueryAntiFlood queryAntiFlood)
        {
            var Totalsize = await context.Set<T>().CountAsync();
            var items=await context.Set<T>()
                .Skip(queryAntiFlood.StartIndex)
                .Take(queryAntiFlood.AmountItems)
                .ProjectTo<TResult>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return new QueryResult<TResult>
            {
                Items = items,
                PageNumber = queryAntiFlood.StartIndex,
                Amount = queryAntiFlood.AmountItems,
                TotalAmount = Totalsize
            };
        }

        public async Task<IDbContextTransaction> StartTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

    }
}
