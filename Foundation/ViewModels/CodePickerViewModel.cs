using System;
using FootballOracle.Foundation.Interfaces;

namespace FootballOracle.Foundation.ViewModels
{
    public class CodePickerViewModel : ICodePickerData
    {
        public Guid Code { get; set; }
        public string Description { get; set; }
    }
}
