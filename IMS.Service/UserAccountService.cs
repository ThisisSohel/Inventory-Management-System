using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using IMS.DAO;
using ISession = NHibernate.ISession;

namespace IMS.Service
{
    public interface IuserAccountService
    {
        Task UserDbConnection();
    }
    public class UserAccountService: IuserAccountService
    {
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;

        public UserAccountService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
        }
        public Task UserDbConnection()
        {
            return Task.CompletedTask;
        }
    }
}
