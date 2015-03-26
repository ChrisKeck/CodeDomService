#region Header Comment


// SrsFrameworks - CodeDomService - ErrorRaisedInjecter.cs - 12/03/2015


#endregion


#region


using System;
using System.CodeDom;
using CodeDomService.Metadata;
using CodeDomService.Types;



#endregion



namespace CodeDomService.BodyInjection
{

    [ InjecterMetadata( targetCut = TargetCut.Property_Set, timeCut = TimeCut.Error, implementedInterface = new[ ]
                                                                                                            {
                                                                                                            IMPLEMENTED_TYPE
                                                                                                            } ) ]
    internal class ErrorRaisedInjecter : AbstractBaseInjecter
    {

        private const string EVENT_FIELD_NAME = "errorRaised";

        private const string EVENTARGS_TYPE = "CrossLayerService.NotfiyErrorEventArgs";

        private const string IMPLEMENTED_TYPE = "CrossLayerService.INotifyErrorRaised";

        private const string EVENTTYPE = "System.EventHandler";


        private const string METHOD_NAME = "OnErrorRaised";

        private const string PARAM_ARG = "ex";

        //publish requirements like must-have-method or interface
        public CodeStatement[ ] createCodeStatements( )
        {
            var snippet = new CodeSnippetStatement( "this." + METHOD_NAME + "(" + PARAM_ARG + ");" );
            return new CodeStatement[ ]
                   {
                   snippet
                   };
        }


        private CodeTypeMember createINotifyPropertyChanged( )
        {
            var eventargCreation = new CodeSnippetStatement
            ( "var resultArgs=new " + EVENTARGS_TYPE + "(" + PARAM_ARG + ");" );
            var invokeSnippet = new CodeSnippetStatement( "this." + EVENT_FIELD_NAME + "(this,resultArgs);" );
            var argsPropEx = new CodeSnippetExpression( "resultArgs.resolved" );
            var equalsnippet = new CodeSnippetExpression( "!Equals(this." + EVENT_FIELD_NAME + ",null)" );
            var conditonStatemewnt = new CodeConditionStatement
                                     {
                                     Condition = equalsnippet
                                     };
            conditonStatemewnt.TrueStatements.Add( eventargCreation );
            conditonStatemewnt.TrueStatements.Add( invokeSnippet );
            conditonStatemewnt.TrueStatements.Add( new CodeMethodReturnStatement( argsPropEx ) );
            conditonStatemewnt.FalseStatements.Add
            ( new CodeMethodReturnStatement( new CodePrimitiveExpression( false ) ) );
            var collection = new CodeStatementCollection
                             {
                             conditonStatemewnt
                             };
            CodeMemberMethod notify = this.onErrorRaisedMethodMember;
            notify.Statements.AddRange( collection );
            return notify;
        }


        private CodeMemberMethod onErrorRaisedMethodMember
        {
            get
            {
                var notify = new CodeMemberMethod
                             {
                             Name = METHOD_NAME,
                             Attributes = MemberAttributes.Family,
                             ReturnType = new CodeTypeReference( typeof ( bool ) ),
                             Parameters =
                             {
                             new CodeParameterDeclarationExpression
                             {
                             Name = PARAM_ARG,
                             Type = new CodeTypeReference( typeof ( Exception ) )
                             }
                             }
                             };
                return notify;
            }
        }


        #region Overrides of AbstractBaseInjecter


        protected override CodeStatement[ ] getInjectionStatement( ) { return this.createCodeStatements( ); }


        protected override CodeTypeMember[ ] getRequiredMembers( )
        {
            CodeTypeMember eventMember = new CodeMemberEvent
                                         {
                                         Name = EVENT_FIELD_NAME,
                                         Attributes = MemberAttributes.Public,
                                         Type = new CodeTypeReference( EVENTTYPE )
                                                {
                                                TypeArguments =
                                                {
                                                new CodeTypeReference( EVENTARGS_TYPE )
                                                }
                                                },
                                         ImplementationTypes =
                                         {
                                         IMPLEMENTED_TYPE
                                         }
                                         };
            return new[ ]
                   {
                   eventMember,
                   this.createINotifyPropertyChanged( )
                   };
        }


        #endregion
    }

}
