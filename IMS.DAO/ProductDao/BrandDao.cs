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
        Task<IEnumerable<Brand>> GetAll();
        Task<Brand> GetById(long id);
        Task CreateBrand(Brand brand);
        Task Update(Brand brand);
        Task DeleteById(long id);
    }

    public class BrandDao : IBrandDao
    {
        private readonly ISession _session;

        public BrandDao(ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Brand>> GetAll()
        {
            try
            {
                return await _session.Query<Brand>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Faild to retrieve brands", ex);
            }
        }

        public async Task<Brand> GetById(long id)
        {
            try
            {
                return await _session.GetAsync<Brand>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to retrieve brand with the ID {id}", ex);
            }
        }

        public async Task CreateBrand(Brand brand)
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

        public async Task Update(Brand brand)
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
                throw ;
            }
        }

        public async Task DeleteById(long id)
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
