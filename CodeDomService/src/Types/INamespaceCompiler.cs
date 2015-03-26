#region Header Comment


// SrsFrameworks - CodeDomService - INamespaceCompiler.cs - 16/03/2015


#endregion


#region


using System;



#endregion



namespace CodeDomService.Types
{

    internal interface INamespaceCompiler
    {

        Type compileAssembly( Type type );

    }

}
