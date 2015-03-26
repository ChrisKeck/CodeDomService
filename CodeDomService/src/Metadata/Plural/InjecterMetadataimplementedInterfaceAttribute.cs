#region Header Comment


// SrsFrameworks - CodeDomService - InjecterMetadataimplementedInterfaceAttribute.cs - 16/03/2015


#endregion


#region


using System;
using System.ComponentModel;
using System.ComponentModel.Composition;



#endregion



namespace CodeDomService.Metadata.Plural
{

    [ MetadataAttribute, AttributeUsage( AttributeTargets.Class, AllowMultiple = true ) ]
    internal class InjecterMetadataimplementedInterfaceAttribute : Attribute
    {

        public InjecterMetadataimplementedInterfaceAttribute( string implementedInterface )
        {
            this.implementedInterface = implementedInterface;
        }


        [ DefaultValue( "" ) ]
        public string implementedInterface { get; set; }

    }

}
