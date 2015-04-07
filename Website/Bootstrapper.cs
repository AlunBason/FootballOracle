using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using FootballOracle.Models.DbContexts;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Website.Areas.Acc.Controllers;
using Unity.Mvc4;
using FootballOracle.Models.RepositoryProviders.Interfaces;

namespace FootballOracle.Website
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
        container.RegisterType<IRepositoryProvider, RepositoryProvider>();
    }
  }
}