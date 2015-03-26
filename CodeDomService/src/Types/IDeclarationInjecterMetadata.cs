#region Header Comment


// SrsFrameworks - CodeDomService - IDeclarationInjecterMetadata.cs - 16/03/2015


#endregion


#region


using System.ComponentModel;



#endregion



namespace CodeDomService.Types
{

    public interface IDeclarationInjecterMetadata
    {

        [ DefaultValue( new[ ]
                        {
                        ""
                        } ) ]
        string[ ] filter { get; }

    }

}
