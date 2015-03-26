#region Header Comment


// SrsFrameworks - CodeDomService - AbstractBaseInjecter.cs - 12/03/2015


#endregion


#region


using System.CodeDom;
using CodeDomService.Types;



#endregion



namespace CodeDomService.BodyInjection
{

    internal abstract class AbstractBaseInjecter : IBodyInjecter
    {

        protected abstract CodeStatement[ ] getInjectionStatement( );

        public CodeStatement[ ] injectionStatements { get { return this.getInjectionStatement( ); } }

        public CodeTypeMember[ ] requiredMembers { get { return this.getRequiredMembers( ); } }

        protected abstract CodeTypeMember[ ] getRequiredMembers( );

    }

}
