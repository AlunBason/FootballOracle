using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Base
{
    public abstract class BaseApprovableViewModel<THeader, TVersion> : BaseViewModel, IApprovableViewModel<THeader, TVersion>
        where THeader : BaseHeaderEntity<TVersion>
        where TVersion : BaseApprovableEntity
    {
        public Guid PrimaryKey { get; set; }

        public string ShortPrimaryKey
        {
            get { return PrimaryKey.ToShortGuid(); }
        }

        public string ShortHeaderKey
        {
            get { return HeaderKey.ToShortGuid(); }
        }

        public Guid HeaderKey { get; set; }
        public TVersion VersionEntity { get; set; }

        [Display(Name = "Url")]
        public virtual string WebAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective from")]
        [Required]
        public virtual DateTime EffectiveFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective to")]
        public DateTime EffectiveTo { get; set; }

        public IList<BreadcrumbViewModel> Breadcrumbs { get; set; }

        public virtual bool IsParentDisplayed { get; set; }

        public static TViewModel CreateNew<TViewModel>()
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
        {
            return new TViewModel()
            {
                PrimaryKey = Guid.NewGuid(),
                HeaderKey = Guid.NewGuid(),
                EffectiveFrom = Date.LowDate,
                EffectiveTo = Date.HighDate
            };
        }

        public virtual IApprovableLinkData ParentLinkData2
        {
            get { return null; }
        }

        public virtual IApprovableLinkData MainLinkData
        {
            get { return (IApprovableLinkData)this; }
        }

        public virtual string Description
        {
            get { return string.Empty; }
        }

        public virtual byte[] ResourceBytes { get; set; }

        public IEnumerable<char> InitialLetters { get; set; }
        public char SelectedInitialLetter { get; set; }

        public abstract THeader HeaderEntity { get; }
    }
}
