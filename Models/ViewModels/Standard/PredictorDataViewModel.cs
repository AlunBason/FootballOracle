using System;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class PredictorDataViewModel
    {
        public static double GetFormMultiplier(int matchCount)
        {
            if (matchCount == 0)
                return 1;

            double formMultiplier = 0;

            for (var count = 0; count <= matchCount - 1; count++)
                formMultiplier += Math.Pow(0.8, (double)count);

            return formMultiplier;
        }
    }
}
