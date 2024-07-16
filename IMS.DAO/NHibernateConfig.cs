using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Configuration;
using System.Reflection;

public static class NHibernateConfig
{
    private static ISessionFactory _sessionFactory = null;
    public static void DatabaseConfiguration()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        var config = Fluently.Configure()
                             .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                             .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
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




