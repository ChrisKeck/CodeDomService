#region Header Comment


// SrsFrameworks - CodeDomService - ITypeInjecterMetadata.cs - 16/03/2015


#endregion


#region


using System.ComponentModel;



#endregion



namespace CodeDomService.Types
{

    public interface ITypeInjecterMetadata
    {

        [ DefaultValue( "" ) ]
        string[ ] filter { get; }


    }

}
