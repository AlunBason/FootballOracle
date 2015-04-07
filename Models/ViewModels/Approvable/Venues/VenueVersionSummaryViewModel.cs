using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballOracle.Models.ViewModels.Approvable.Venues
{
    public class VenueVersionSummaryViewModel : VenueEditorViewModel
    {
        [Display(Name = "Name")]
        [ReadOnly(true)]
        public override string VenueName { get; set; }

        public new string Capacity
        {
            get { return base.Capacity != null ? string.Format("{0:#,#}", base.Capacity) : string.Empty; }
        }

        [Display(Name = "Address 1")]
        [ReadOnly(true)]
        public override string Address1 { get; set; }

        [Display(Name = "Address 2")]
        [ReadOnly(true)]
        public override string Address2 { get; set; }

        [Display(Name = "Town/City")]
        [ReadOnly(true)]
        public override string Address3 { get; set; }

        [Display(Name = "Region")]
        [ReadOnly(true)]
        public override string Address4 { get; set; }

        [Display(Name = "Post code")]
        [ReadOnly(true)]
        public override string PostCode { get; set; }

        [ReadOnly(true)]
        public string Country
        {
            get { return CountryViewModel.ToString(); }
        }

        [DataType(DataType.Date)]
        [Display(Name = "Effective from")]
        [ReadOnly(true)]
        public override DateTime EffectiveFrom { get; set; }
    }
}
