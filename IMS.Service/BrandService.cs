using IMS.CustomException;
using IMS.DAO;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;
using IMS.Entity.EntityViewModels;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.ModelBinding;


namespace IMS.Service
{
    public interface IBrandService
    {
        Task CreateBrandService(BrandViewModel brandViewModel);
        Task<List<BrandViewModel>> GetAll();
        Task<BrandViewModel> GetById(long id);
        Task<BrandViewModel> BrandDetailsService(long  id);
        Task Update(long id, BrandViewModel brand);
        Task DeleteAsync(long id);
    }

    public class BrandService : IBrandService
    {
        private readonly IBrandDao _brandDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BrandService));
        public BrandService(IBrandDao brandDao)
        {
            _brandDao = brandDao;
        }
        public BrandService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _brandDao = new BrandDao(_session);
        }



        public async Task<List<BrandViewModel>> GetAll()
        {
            var brandView = new List<BrandViewModel>();
            var brand = new List<Brand>();
            try
            {
                brand =  await _brandDao.Load();

                if (brand != null)
                {
                    brandView = brand.Select(b =>  new BrandViewModel
                    {
                        Id = b.Id,
                        BrandName = b.BrandName,
                        CreatedBy = b.CreatedBy,
                        CreatedDate = b.CreatedDate,
                        ModifyBy = b.ModifyBy,
                        ModifyDate = b.ModifyDate,
                    }).ToList();
                }
                else
                {
                    throw new Exception("Brand not found!");
                }
                return brandView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BrandViewModel> GetById(long id)
        {
            var brandView = new BrandViewModel();

            try
            {
                var brand = await _brandDao.Get(id);

                if (brand == null)
                {
                    throw new ObjectNotFoundException(brand, "Brand is not found");
                }
                else
                {
                    brandView.Id = brand.Id;
                    brandView.BrandName = brand.BrandName;
                    brandView.CreatedBy = brand.CreatedBy;
                    brandView.CreatedDate = brand.CreatedDate;
                    brandView.ModifyBy = brand.ModifyBy;
                    brandView.ModifyDate = brand.ModifyDate;

                    return brandView;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(long id, BrandViewModel brand)
        {

            try
            {
                ModelValidatorMethod(brand);
                var valueForUpdate = await _brandDao.Get(brand.Id);
                if (valueForUpdate != null)
                {
                        valueForUpdate.BrandName = brand.BrandName.Trim();
                        valueForUpdate.ModifyBy = brand.ModifyBy;
                        valueForUpdate.ModifyDate = DateTime.Now;
                        await _brandDao.BrandUpdate(valueForUpdate);                  
                }
                else
                {
                    throw new ObjectNotFoundException(valueForUpdate, "brand");
                }
            }
            catch(InvalidNameException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var individualBrand = await _brandDao.Get(id);
                if (individualBrand != null)
                {
                    await _brandDao.BrandDelete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBrandService(BrandViewModel brandViewModelEntity)
        {

            try
            {
                var brandDuplicateCheck = new List<Brand>();
                var brand = await _brandDao.Load();

                if (brand.Count != 0)
                {
                    foreach(var item in brand)
                    {
                        if (brandViewModelEntity.BrandName.Contains(item.BrandName))
                        {
                            throw new DuplicateValueException ("Brand name can not be duplicate!");
                        }
                    }
                }
                else
                {
                    ModelValidatorMethod(brandViewModelEntity);

                    var brandMainEntity = new Brand();
                    try
                    {
                        brandMainEntity.BrandName = brandViewModelEntity.BrandName.Trim();
                        brandMainEntity.CreatedBy = 100;
                        brandMainEntity.CreatedDate = DateTime.Now;
                        brandMainEntity.ModifyBy = 100;
                        brandMainEntity.ModifyDate = DateTime.Now;
                        await _brandDao.BrandCreate(brandMainEntity);

                    }
                    catch (InvalidNameException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch(DuplicateValueException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BrandViewModel> BrandDetailsService(long id)
        {
            var brandDetails = new BrandViewModel();
            try
            {
                var brand = await _brandDao.Get(id);

                if (brand != null)
                {
                    brandDetails.Id = brand.Id;
                    brandDetails.BrandName = brand.BrandName;
                    brandDetails.CreatedBy = brand.CreatedBy;
                    brandDetails.CreatedDate = brand.CreatedDate;
                    brandDetails.ModifyBy = brand.ModifyBy;
                    brandDetails.ModifyDate = brand.ModifyDate;
                    
                }
                else
                {
                    throw new Exception("Brand is null!");
                }
                return brandDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ModelValidatorMethod (BrandViewModel modelToValidate)
        {
            if (String.IsNullOrWhiteSpace(modelToValidate.BrandName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.BrandName.Trim().Length <3 || modelToValidate.BrandName.Trim().Length >30)
            {
                //ModelState.AddModelError("sorry! your input field is empty.");
                throw new InvalidNameException("Brand name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.BrandName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidNameException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
        }
    }
}
