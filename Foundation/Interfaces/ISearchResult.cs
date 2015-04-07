using System;

namespace FootballOracle.Foundation.Interfaces
{
    public interface ISearchResult
    {
        DateTime ViewDate { get; }
        DateTime EffectiveFrom { get; }
        DateTime EffectiveTo { get; }
        bool IsParentDisplayed { get; set; }
        IApprovableLinkData ParentLinkData { get; }
        IApprovableLinkData ParentLinkData2 { get; }
        IApprovableLinkData MainLinkData { get; }
        string Description { get; }
        string WebAddress { get; }
        byte[] ResourceBytes { get; }
    }
}
