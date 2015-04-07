using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Base;

namespace FootballOracle.Models.ViewModels.Approvable.Venues
{
    public class BaseVenueViewModel : BaseApprovableViewModel<Venue, VenueV>, ISearchResult, IApprovableLinkData
    {
        public override string ToString()
        {
            return VersionEntity != null ? VersionEntity.VenueName : string.Empty;
        }

        private BaseCountryViewModel countryViewModel;
        public BaseCountryViewModel CountryViewModel
        {
            get { return countryViewModel = countryViewModel ?? VersionEntity.Country.ToViewModel(ViewDate); }
        }

        private string address = string.Empty;
        public string Address
        {
            get
            {
                if (address == string.Empty)
                {
                    if (!string.IsNullOrEmpty(VersionEntity.Address1))
                        address = address + VersionEntity.Address1 + ", ";

                    if (!string.IsNullOrEmpty(VersionEntity.Address2))
                        address = address + VersionEntity.Address2 + ", ";

                    if (!string.IsNullOrEmpty(VersionEntity.Address3))
                        address = address + VersionEntity.Address3 + ", ";

                    if (!string.IsNullOrEmpty(VersionEntity.Address4))
                        address = address + VersionEntity.Address4 + ", ";

                    if (!string.IsNullOrEmpty(VersionEntity.PostCode))
                        address = address + VersionEntity.PostCode + ", ";

                    if (address.Length > 2)
                    {
                        address = address.Substring(0, address.Length - 2);
                    }
                }

                return address;
            }
        }

        public IApprovableLinkData ParentLinkData
        {
            get { return CountryViewModel; }
        }

        public AreaType AreaType
        {
            get { return AreaType.Ven; }
        }

        public override string Description
        {
            get { return Address; }
        }

        public override Venue HeaderEntity
        {
            get { return VersionEntity.Venue; }
        }
    }
}
