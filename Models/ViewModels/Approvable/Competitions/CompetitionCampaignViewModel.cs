using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFootball.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionCampaignViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Key
        {
            get
            {
                return StartDate.ToString("yyyyMMdd");
            }
        }

        public string Description
        {
            get{ return string.Format("{0} / {1}", StartDate.Year, EndDate.Year); }
        }
    }
}
