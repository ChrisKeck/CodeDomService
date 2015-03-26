#region Header Comment


// SrsFrameworks - CodeDomService - IAssemblyInjecter.cs - 16/03/2015


#endregion


#region


using System;
using System.CodeDom;



#endregion



namespace CodeDomService.Types
{

    internal interface IAssemblyInjecter
    {

        CodeNamespace createForInst( Type t );

    }

}
