#region Header Comment


// SrsFrameworks - CodeDomService - InjecterMetadataAttribute.cs - 15/03/2015


#endregion


#region


using System;
using System.ComponentModel.Composition;
using CodeDomService.Types;



#endregion



namespace CodeDomService.Metadata
{

    [ MetadataAttribute, AttributeUsage( AttributeTargets.Class ) ]
    internal class InjecterMetadataAttribute : ExportAttribute

    {

        public InjecterMetadataAttribute( ) : base( typeof ( IBodyInjecter ) ) { }


        #region Implementation of IInjecterMetadata


        public string[ ] implementedInterface { get; set; }


        #endregion


        #region Implementation of IMemberInjecterMetadata


        public TimeCut timeCut { get; set; }


        public TargetCut targetCut { get; set; }


        #endregion
    }

}
