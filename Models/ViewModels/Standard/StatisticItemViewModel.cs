namespace FootballOracle.Models.ViewModels.Standard
{
    public class StatisticItemViewModel
    {
        public StatisticItemViewModel()
        {
            HasItem = true;
            ItemCount = 0;
        }

        public bool HasItem { get; set; }
        public int ItemCount { get; set; }
    }
}
