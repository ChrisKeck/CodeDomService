#region Header Comment


// SrsFrameworks - CodeDomService - FilterMetadataAttribute.cs - 16/03/2015


#endregion


#region


using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using CodeDomService.Types;



#endregion



namespace CodeDomService.Metadata.Plural
{

    [ MetadataAttribute, AttributeUsage( AttributeTargets.Class, AllowMultiple = true ) ]
    public class FilterMetadataAttribute : ExportAttribute
    {


        public FilterMetadataAttribute( string filter ) : base( typeof ( ITypeInjecterMetadata ) )
        {
            this.filter = filter;
        }


        [ DefaultValue( "" ) ]
        public string filter { get; set; }

    }

}
