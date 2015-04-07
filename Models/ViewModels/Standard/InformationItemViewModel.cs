using FootballOracle.Foundation;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class InformationItemViewModel
    {
        public InformationItemViewModel(string description, DisplayType displayType)
        {
            Description = description;
            DisplayType = displayType;
        }

        public string Description { get; set; }
        public DisplayType DisplayType { get; set; }
    }
}
