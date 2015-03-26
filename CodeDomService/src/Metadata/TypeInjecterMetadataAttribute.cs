#region Header Comment


// SrsFrameworks - CodeDomService - TypeInjecterMetadataAttribute.cs - 16/03/2015


#endregion


#region


using System;
using System.ComponentModel.Composition;
using CodeDomService.Types;



#endregion



namespace CodeDomService.Metadata
{

    [ MetadataAttribute, AttributeUsage( AttributeTargets.Class ) ]
    internal class TypeInjecterMetadataAttribute : ExportAttribute

    {

        public TypeInjecterMetadataAttribute( ) : base( typeof ( IDeclarationInjecter ) ) { }


        #region Implementation of IInjecterMetadata


        public string[ ] filter { get; set; }


        #endregion
    }

}
