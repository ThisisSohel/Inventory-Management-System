using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;
using IMS.DAO.Mappings;
using System.Configuration;

public static class NHibernateConfig
{
    private static ISessionFactory _sessionFactory = null;
    public static void DatabaseConfiguration()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        var config = Fluently.Configure()
                             .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                             .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BrandMapping>())
                             .BuildConfiguration();
        _sessionFactory = config.BuildSessionFactory();
    }
    public static ISessionFactory GetSession()
    {
        if (_sessionFactory == null)
        {
            DatabaseConfiguration();
            return _sessionFactory;
        }
        return _sessionFactory;
    }

}

