#region Header Comment


// SrsFrameworks - CodeDomService - IMembersInjecter.cs - 16/03/2015


#endregion


#region


using System;
using System.CodeDom;



#endregion



namespace CodeDomService.Types
{

    public interface IMembersInjecter
    {

        CodeTypeMember[ ] createCodeMembers( Type type );

        string[ ] implementedInterfaces { get; }

    }

}
