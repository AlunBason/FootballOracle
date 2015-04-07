using System;

namespace FootballOracle.Foundation.Interfaces
{
    public interface ICodePickerData
    {
        Guid Code { get; set; }
        string Description { get; set; }
    }
}
