#region Header Comment


// SrsFrameworks - CodeDomService - AbstractMemberInjecter.cs - 12/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CodeDomService.Helper;
using CodeDomService.Types;



#endregion



namespace CodeDomService.MemberInjection
{

    internal abstract class AbstractMemberInjecter<TMember> : IMembersInjecter where TMember : class
    {

        private const string EXCEPTION_NAME = "ex";


        protected AbstractMemberInjecter( ) { MefInjectionService.service.compose( this ); }


        protected virtual MemberAttributes getMemberAttributes( )
        {
            MemberAttributes attributes = MemberAttributes.Public | MemberAttributes.Override;
            return attributes;
        }


        [ ImportMany( typeof ( IBodyInjecter ) ) ]
        protected virtual IEnumerable<Lazy<IBodyInjecter, IInjecterMetadata>> bodyParts { get; private set; }


        [ Import( typeof ( IRulesFactory ) ) ]
        protected virtual Lazy<IRulesFactory> rulesLazyFactoryLazy { get; private set; }


        private IEnumerable<CodeTypeMember> getRequiredMembers( Type type )
        {
            //return no duplicates
            return this.bodyParts.Where( item => this.isMemberRequired( item.Metadata ) ).
                        SelectMany( item => item.Value.requiredMembers ).
                        ToArray( );
        }


        protected abstract bool isMemberRequired( IInjecterMetadata item );

        protected abstract CodeTypeMember createCodeMember( TMember methodInfo );


        protected CodeTryCatchFinallyStatement createTryCatchBlock( )
        {
            var catchExpression = new CodeCatchClause
            ( EXCEPTION_NAME, new CodeTypeReference( typeof ( Exception ) ) );
            catchExpression.Statements.AddRange( this.createErrorStatements( ) );
            catchExpression.Statements.Add( new CodeSnippetStatement( "throw " + EXCEPTION_NAME + ";" ) );
            var tryCatchStatement = new CodeTryCatchFinallyStatement
                                    {
                                    CatchClauses =
                                    {
                                    catchExpression
                                    }
                                    };
            return tryCatchStatement;
        }


        protected virtual CodeStatement[ ] createErrorStatements( )
        {
            return new CodeStatement[ ]
                   { };
        }


        public CodeTypeMember[ ] createCodeMembers( Type type )
        {
            var spec = this.rulesLazyFactoryLazy.Value.createSpec<TMember>( );
            var list = new List<CodeTypeMember>
            ( this.getMembers( type ).
                   Where( item => spec.isValid( item ) ).
                   Select( this.createCodeMember ).
                   Where( member => member.isNotNull( ) ) );
            list.AddRange( this.getRequiredMembers( type ) );
            return list.ToArray( );
        }


        public string[ ] implementedInterfaces
        {
            get
            {
                return this.bodyParts.SelectMany( bodyPart => bodyPart.Metadata.implementedInterface ).
                            ToArray( );
            }
        }


        protected abstract IEnumerable<TMember> getMembers( Type type );


        protected virtual CodeStatement[ ] getStatements( TimeCut timeCut,
                                                          TargetCut targetCut,
                                                          IEnumerable<Lazy<IBodyInjecter, IInjecterMetadata>> parts )
        {
            List<CodeStatement> statements = new List<CodeStatement>( );
            foreach ( var bodyPart in
            parts.Where( item => item.Metadata.targetCut == targetCut && item.Metadata.timeCut == timeCut ) )
                statements.AddRange( bodyPart.Value.injectionStatements );
            return statements.ToArray( );
        }

    }


}
