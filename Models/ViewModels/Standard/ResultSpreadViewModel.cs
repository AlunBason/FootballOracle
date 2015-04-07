using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class ResultSpreadViewModel
    {
        public int HomeWin { get; set; }
        public int Draw { get; set; }
        public int AwayWin { get; set; }

        public int Population
        {
            get { return HomeWin + Draw + AwayWin; }
        }

        public double HomeWinPercentage
        {
            get { return (double)HomeWin / (double)Population; }
        }

        public double DrawPercentage
        {
            get { return (double)Draw / (double)Population; }
        }

        public double AwayWinPercentage
        {
            get { return (double)AwayWin / (double)Population; }
        }

        public double HomeWinOdds
        {
            get { return 1 / HomeWinPercentage; }
        }

        public double DrawOdds
        {
            get { return 1 / DrawPercentage; }
        }

        public double AwayWinOdds
        {
            get { return 1 / AwayWinPercentage; }
        }

    }
}
