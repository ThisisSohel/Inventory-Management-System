using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using IMS.Entity.Entities;
using NHibernate.Linq;

namespace IMS.DAO
{
    public interface IBrandDao
    {
        Task<IEnumerable<Brand>> Load();
        Task<Brand> Get(long id);
        Task BrandCreate(Brand brand);
        Task BrandUpdate(Brand brand);
        Task BrandDelete(long id);
    }

    public class BrandDao : IBrandDao
    {
        private readonly ISession _session;

        public BrandDao(ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Brand>> Load()
        {
            try
            {
                return await _session.Query<Brand>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve brands", ex);
            }
        }

        public async Task<Brand> Get(long id)
        {
            try
            {
                return await _session.GetAsync<Brand>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve brand with the ID {id}", ex);
            }
        }

        public async Task BrandCreate(Brand brand)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.SaveAsync(brand);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create brand", ex);
            }
        }

        public async Task BrandUpdate(Brand brand)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.UpdateAsync(brand);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to update the category", ex);
            }
        }

        public async Task BrandDelete(long id)
        {
            try
            {
                var brand = await _session.GetAsync<Brand>(id);
                if (brand != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        await _session.DeleteAsync(brand);
                        await transaction.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw new Exception($"Failed to delete Brand with ID {id}", ex);
            }
        }
    }
}
