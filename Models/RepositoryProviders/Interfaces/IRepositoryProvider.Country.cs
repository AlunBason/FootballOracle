using FootballOracle.Models.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(Country country);
        void Remove(Country country);
    }
}
