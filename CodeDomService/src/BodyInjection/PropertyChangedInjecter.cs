#region Header Comment


// SrsFrameworks - CodeDomService - PropertyChangedInjecter.cs - 12/03/2015


#endregion


#region


using System.CodeDom;
using CodeDomService.Metadata;
using CodeDomService.Types;



#endregion



namespace CodeDomService.BodyInjection
{


    [ InjecterMetadata( targetCut = TargetCut.Property_Set, timeCut = TimeCut.After, implementedInterface = new[ ]
                                                                                                            {
                                                                                                            IMPLEMENTED_INTERFACE
                                                                                                            } ) ]
    internal class PropertyChangedInjecter : AbstractBaseInjecter
    {

        private const string EVENTARG_TYPE = "System.ComponentModel.PropertyChangedEventArgs";

        private const string EVENT_TYPE = "System.ComponentModel.PropertyChangedEventHandler";

        private const string IMPLEMENTED_INTERFACE = "System.ComponentModel.INotifyPropertyChanged";

        private const string METHOD_NAME = "OnPropertyChanged";

        private const string PARAM_NAME = "OnPropertyChanged";

        private const string EVENT_FIELD_NAME = "PropertyChanged";

        private const string CURRENTMETHOD = "System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4)";


        protected override CodeStatement[ ] getInjectionStatement( )
        {
            var snippet = new CodeSnippetStatement( "this." + METHOD_NAME + "(" + CURRENTMETHOD + ");" );
            return new CodeStatement[ ]
                   {
                   snippet
                   };
        }


        #region Implementation of IBodyInjecter


        protected override CodeTypeMember[ ] getRequiredMembers( )
        {
            CodeTypeMember eventMember = new CodeMemberEvent
                                         {
                                         Name = EVENT_FIELD_NAME,
                                         Attributes = MemberAttributes.Public,
                                         Type = new CodeTypeReference( EVENT_TYPE ),
                                         ImplementationTypes =
                                         {
                                         IMPLEMENTED_INTERFACE
                                         }
                                         };
            return new[ ]
                   {
                   eventMember,
                   this.createOnPropertyChangedMethod( )
                   };
        }


        #endregion


        private CodeTypeMember createOnPropertyChangedMethod( )
        {
            var notify = new CodeMemberMethod
                         {
                         Name = METHOD_NAME,
                         Attributes = MemberAttributes.Family,
                         ReturnType = new CodeTypeReference( typeof ( void ) ),
                         Parameters =
                         {
                         new CodeParameterDeclarationExpression
                         {
                         Name = PARAM_NAME,
                         Type = new CodeTypeReference( typeof ( string ) )
                         }
                         }
                         };
            var eventargCreation = new CodeSnippetStatement
            ( "var resultArgs=new " + EVENTARG_TYPE + "(" + PARAM_NAME + ");" );
            var invokeSnippet = new CodeSnippetStatement( "this." + EVENT_FIELD_NAME + "(this,resultArgs);" );
            var equalEx = new CodeSnippetExpression( "!Equals(this." + EVENT_FIELD_NAME + ",null)" );
            var conditonStatemewnt = new CodeConditionStatement
                                     {
                                     Condition = equalEx
                                     };
            conditonStatemewnt.TrueStatements.Add( eventargCreation );
            conditonStatemewnt.TrueStatements.Add( invokeSnippet );
            var collection = new CodeStatementCollection
                             {
                             conditonStatemewnt
                             };
            notify.Statements.AddRange( collection );
            return notify;
        }

    }


}
