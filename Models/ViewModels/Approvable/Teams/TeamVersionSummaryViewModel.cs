using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamVersionSummaryViewModel : TeamEditorViewModel
    {
        [ReadOnly(true)]
        [Display(Name = "Team name (Native language)")]
        public override string EditorTeamNameNative { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Team name (English language)")]
        public override string EditorTeamNameEnglish { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Full name")]
        public override string EditorFullName { get; set; }

        [Display(Name = "Short name")]
        [ReadOnly(true)]
        public override string EditorShortname { get; set; }

        [Display(Name = "Nickname")]
        [ReadOnly(true)]
        public override string EditorNickname { get; set; }

        [Display(Name = "Home venue")]
        [ReadOnly(true)]
        public string HomeVenue
        {
            get { return HomeVenueGuid != null ? VenueViewModel.ToString() : string.Empty; }
        }

        [ReadOnly(true)]
        public string Country
        {
            get { return CountryGuid != null ? CountryViewModel.ToString() : string.Empty; }
        }

        [Display(Name = "Url")]
        [ReadOnly(true)]
        public override string WebAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective from")]
        [ReadOnly(true)]
        public override DateTime EffectiveFrom { get; set; }
    }
}
