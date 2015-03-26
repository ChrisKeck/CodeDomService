#region Header Comment


// SrsFrameworks - CodeDomService - IDeclarationInjecter.cs - 16/03/2015


#endregion


#region


using System;
using System.CodeDom;



#endregion



namespace CodeDomService.Types
{

    internal interface IDeclarationInjecter
    {

        CodeTypeDeclaration createTypeDeclaration( Type type );

    }

}
