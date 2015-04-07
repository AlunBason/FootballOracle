using System;
using System.Web.Mvc;

namespace FootballOracle.Models.Attributes
{
    public class HideLabelAttribute : Attribute, IMetadataAware
    {

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["HideLabel"] = null;
        }
    }
}
