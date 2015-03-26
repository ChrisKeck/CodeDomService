#region Header Comment


// SrsFrameworks - CodeDomService - PropertyMemberInjecter.cs - 14/03/2015


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


    [ MembersInjecterMetadata( targetCut = TargetCut.Property, timeCut = TimeCut.All ) ]
    internal class PropertyMemberInjecter : AbstractMemberInjecter<PropertyInfo>
    {


        protected override IEnumerable<PropertyInfo> getMembers( Type type ) { return type.getPropertyInfos( ); }


        private CodeMemberProperty setBodyStatements( PropertyInfo pinfo )
        {
            CodeMemberProperty codeMember = this.getInjectedMember( pinfo );
            if ( codeMember.HasGet )
                codeMember.GetStatements.AddRange( this.createGetStatement( pinfo ) );
            if ( codeMember.HasSet )
                codeMember.SetStatements.AddRange( this.createSetStatement( pinfo ) );
            if ( codeMember.HasGet
                 || codeMember.HasSet )
                if ( codeMember.GetStatements.Count > 0
                     || codeMember.SetStatements.Count > 0 )
                    return codeMember;
            return null;
        }


        private CodeMemberProperty getInjectedMember( PropertyInfo pinfo )
        {
            MemberAttributes attributes = getMemberAttributes( );
            var codeMember = new CodeMemberProperty
                             {
                             Attributes = attributes,
                             Name = pinfo.Name,
                             HasGet = pinfo.GetGetMethod( ).
                                            isVirtual( ),
                             HasSet = pinfo.GetSetMethod( ).
                                            isVirtual( ),
                             Type = new CodeTypeReference( pinfo.PropertyType )
                             };
            return codeMember;
        }


        protected virtual CodeStatement[ ] createSetStatement( PropertyInfo pinfo )
        {
            CodeTryCatchFinallyStatement tryCatchStatement = this.createTryCatchBlock( );
            var isNewValueCondition = new CodeConditionStatement
                                      {
                                      Condition =
                                      new CodeSnippetExpression
                                      ( string.Format( EQUAL_FORMAT_LITERAL, "!", "value", "base." + pinfo.Name ) )
                                      };
            var newValueAssign = new CodeSnippetStatement( "base." + pinfo.Name + "=value;" );
            isNewValueCondition.TrueStatements.AddRange( this.createBeforeStatements( ) );
            isNewValueCondition.TrueStatements.Add( newValueAssign );
            isNewValueCondition.TrueStatements.AddRange( this.createAfterStatements( ) );
            tryCatchStatement.TryStatements.Add( isNewValueCondition );
            return new CodeStatement[ ]
                   {
                   tryCatchStatement
                   };
        }


        private const string EQUAL_FORMAT_LITERAL = "{0}Equals({1},{2})";


        protected override bool isMemberRequired( IInjecterMetadata item )
        {
            return ( item.timeCut == TimeCut.Before || item.timeCut == TimeCut.Error
                     || item.timeCut == TimeCut.After )
                   && ( item.targetCut == TargetCut.Property_Set || item.targetCut == TargetCut.Property_Get );
        }


        protected override CodeTypeMember createCodeMember( PropertyInfo pinfo )
        {
            return this.setBodyStatements( pinfo );
        }


        protected override CodeStatement[ ] createErrorStatements( )
        {
            return this.getStatements( TimeCut.Error, TargetCut.Property_Set, bodyParts );
        }


        protected virtual CodeStatement[ ] createAfterStatements( )
        {
            return this.getStatements( TimeCut.After, TargetCut.Property_Set, bodyParts );
        }


        protected virtual CodeStatement[ ] createBeforeStatements( )
        {
            return this.getStatements( TimeCut.Before, TargetCut.Property_Set, bodyParts );
        }


        protected virtual CodeStatement[ ] createGetStatement( PropertyInfo pinfo )
        {
            return new CodeStatement[ ]
                   {
                   new CodeSnippetStatement( "return base." + pinfo.Name + ";" )
                   };
        }

    }

}
