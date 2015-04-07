using FootballOracle.Models.RepositoryProviders.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Controllers
{
    public class JsonController : BaseController
    {
        #region Constructor
        public JsonController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        [OutputCache(Duration = 0)]
        public async Task<JsonResult> GetCombinedAutoCompleteList(string text)
        {
            var normalizedText = text.RemoveDiacritics();

            var competitions = await DbProvider.GetCompetitionAutoCompleteListAsync(UserId, IsAdmin, normalizedText);
            var countries = await DbProvider.GetCountryAutoCompleteListAsync(UserId, IsAdmin, normalizedText);
            var organisations = await DbProvider.GetOrganisationAutoCompleteList(UserId, IsAdmin, normalizedText);
            var people = await DbProvider.GetPersonAutoCompleteList(UserId, IsAdmin, normalizedText);
            var teams = await DbProvider.GetTeamAutoCompleteList(UserId, IsAdmin, normalizedText);
            var venues = await DbProvider.GetVenueAutoCompleteList(UserId, IsAdmin, normalizedText);

            var returnValue = competitions.Concat(countries).Concat(organisations).Concat(people).Concat(teams).Concat(venues).OrderBy(o => o).Take(20);

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public async Task<JsonResult> GetCountryCodePickerData()
        {
            var countries = await DbProvider.GetCountryCodePickerData(DateTime.Now);

            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public async Task<ActionResult> GetOrganisationCodePickerData()
        {
            var organisations = await DbProvider.GetOrganisationCodePickerData(DateTime.Now);

            return Json(organisations, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public async Task<JsonResult> GetVenueCodePickerData()
        {
            var venues = await DbProvider.GetVenueCodePickerData(DateTime.Now);

            return Json(venues, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public async Task<JsonResult> GetCascadeVenueCodePickerData(string countryGuidString)
        {
            Guid countryKey;

            if (!Guid.TryParse(countryGuidString, out countryKey))
                return null;

            var venues = await DbProvider.GetCascadeVenueCodePickerData(DateTime.Now, countryKey);

            return Json(venues, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0)]
        public async Task<JsonResult> GetPersonAutoCompleteList(string text)
        {
            var normalizedText = text.RemoveDiacritics();
            var returnValue = await DbProvider.GetPersonAutoCompleteList(UserId, IsAdmin, normalizedText);

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }
	}
}