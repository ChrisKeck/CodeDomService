#region Header Comment


// SrsFrameworks - CodeDomService - IBodyInjecter.cs - 16/03/2015


#endregion


#region


using System.CodeDom;



#endregion



namespace CodeDomService.Types
{

    internal interface IBodyInjecter
    {

        CodeStatement[ ] injectionStatements { get; }

        CodeTypeMember[ ] requiredMembers { get; }

    }

}
