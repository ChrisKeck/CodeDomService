#region Header Comment


// SrsFrameworks - CodeDomService - MembersInjecterMetadataAttribute.cs - 16/03/2015


#endregion


#region


using System;
using System.ComponentModel.Composition;
using CodeDomService.Types;



#endregion



namespace CodeDomService.Metadata
{

    [ MetadataAttribute, AttributeUsage( AttributeTargets.Class ) ]
    internal class MembersInjecterMetadataAttribute : ExportAttribute
    {

        public MembersInjecterMetadataAttribute( ) : base( typeof ( IMembersInjecter ) ) { }


        #region Implementation of IInjecterMetadata


        public TimeCut timeCut { get; set; }


        public TargetCut targetCut { get; set; }


        #endregion
    }

}
