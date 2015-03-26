#region Header Comment


// SrsFrameworks - CodeDomService - ConstructorMemberInjecter.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using CodeDomService.Helper;
using CodeDomService.Metadata;
using CodeDomService.Types;



#endregion



namespace CodeDomService.MemberInjection
{

    [ MembersInjecterMetadata( targetCut = TargetCut.Constructor, timeCut = TimeCut.All ) ]
    internal class ConstructorMemberInjecter : AbstractMemberInjecter<ConstructorInfo>
    {
        #region Overrides of AbstractMemberInjecter<ConstructorInfo>


        public ConstructorMemberInjecter( ) { MefInjectionService.service.compose( this ); }

        [ Import( typeof ( IRulesFactory ) ) ]
        private Lazy<IRulesFactory> lazyRulesFactory;

        protected override bool isMemberRequired( IInjecterMetadata item ) { return false; }


        protected override CodeTypeMember createCodeMember( ConstructorInfo methodInfo )
        {
            CodeTypeMember member = null;
            var spec = this.lazyRulesFactory.Value.createSpec<ConstructorInfo>( );
            if ( spec.isValid( methodInfo ) )
                member = this.createCodeConstructor( methodInfo );
            return member;
        }


        private CodeTypeMember createCodeConstructor( MethodBase methodBase )
        {
            CodeConstructor codeConstructor = new CodeConstructor
                                              {
                                              Attributes = this.getMemberAttributes( )
                                              };
            foreach ( var parameterInfo in methodBase.GetParameters( ) )
            {
                codeConstructor.Parameters.Add
                ( new CodeParameterDeclarationExpression
                  {
                  Name = parameterInfo.Name,
                  Type = new CodeTypeReference( parameterInfo.ParameterType ),
                  Direction = parameterInfo.toDirection( )
                  } );
                codeConstructor.BaseConstructorArgs.Add
                ( new CodeVariableReferenceExpression( parameterInfo.Name ) );
            }
            return codeConstructor;
        }


        #region Overrides of AbstractMemberInjecter<ConstructorInfo>


        protected override MemberAttributes getMemberAttributes( ) { return MemberAttributes.Public; }


        #endregion


        protected override IEnumerable<ConstructorInfo> getMembers( Type type )
        {
            return new ConstructorInfo[ ]
                   { };
        }


        #endregion
    }

}
