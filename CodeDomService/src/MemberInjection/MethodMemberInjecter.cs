#region Header Comment


// SrsFrameworks - CodeDomService - MethodMemberInjecter.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using CodeDomService.Helper;
using CodeDomService.Metadata;
using CodeDomService.Types;



#endregion



namespace CodeDomService.MemberInjection
{

    [ MembersInjecterMetadata( targetCut = TargetCut.Method, timeCut = TimeCut.All ) ]
    internal class MethodMemberInjecter : AbstractMemberInjecter<MethodInfo>
    {


        protected override bool isMemberRequired( IInjecterMetadata item )
        {
            return ( item.timeCut == TimeCut.Before || item.timeCut == TimeCut.Error
                     || item.timeCut == TimeCut.After ) && ( item.targetCut == TargetCut.Method );
        }


        protected override IEnumerable<MethodInfo> getMembers( Type type ) { return type.getMethodsInfos( ); }


        protected override CodeStatement[ ] createErrorStatements( )
        {
            return this.getStatements( TimeCut.Error, TargetCut.Method, bodyParts );
        }


        protected override CodeTypeMember createCodeMember( MethodInfo methodInfo )
        {
            var codeMember = new CodeMemberMethod
                             {
                             Attributes = getMemberAttributes( ),
                             Name = methodInfo.Name,
                             ReturnType = new CodeTypeReference( methodInfo.ReturnType )
                             };
            var para = new CodeParameterDeclarationExpressionCollection( );
            List<string> paramNames = new List<string>( );
            foreach ( var parameter in methodInfo.GetParameters( ) )
            {
                paramNames.Add( parameter.Name );
                para.Add( new CodeParameterDeclarationExpression( parameter.ParameterType, parameter.Name ) );
            }
            var tryCatchExpression = createTryCatchBlock( );
            CodeSnippetStatement baseExpression = methodInfo.ReturnType == typeof ( void )
                                                  ? new CodeSnippetStatement
                                                    ( "base." + methodInfo.Name + "("
                                                      + string.Join( ",", paramNames ) + ");" )
                                                  : new CodeSnippetStatement
                                                    ( "return base." + methodInfo.Name + "("
                                                      + string.Join( ",", paramNames ) + ");" );
            tryCatchExpression.TryStatements.AddRange( this.createBeforeStatements( ) );
            tryCatchExpression.TryStatements.Add( baseExpression );
            tryCatchExpression.TryStatements.AddRange( this.createAfterStatements( ) );
            codeMember.Statements.Add( tryCatchExpression );
            codeMember.Parameters.AddRange( para );
            return codeMember;
        }


        private CodeStatement[ ] createAfterStatements( )
        {
            return this.getStatements( TimeCut.After, TargetCut.Method, bodyParts );
        }


        private CodeStatement[ ] createBeforeStatements( )
        {
            return this.getStatements( TimeCut.Before, TargetCut.Method, bodyParts );
        }


    }

}
