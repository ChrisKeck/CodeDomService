#region Header Comment


// SrsFrameworks - CodeDomService - PropertyChangingInjecter.cs - 12/03/2015


#endregion


#region


using System.CodeDom;
using CodeDomService.Metadata;
using CodeDomService.Types;



#endregion



namespace CodeDomService.BodyInjection
{

    [ InjecterMetadata( targetCut = TargetCut.Property_Set, timeCut = TimeCut.Before, implementedInterface = new[ ]
                                                                                                             {
                                                                                                             IMPLEMENTED_INTERFACE
                                                                                                             } ) ]
    internal class PropertyChangingInjecter : AbstractBaseInjecter
    {

        private const string METHOD_NAME = "OnPropertyChanging";

        private const string CURRENTMETHOD = "System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4)";


        public CodeStatement[ ] createCodeStatements( )
        {
            var snippet = new CodeSnippetStatement( "this." + METHOD_NAME + "(" + CURRENTMETHOD + ");" );
            return new CodeStatement[ ]
                   {
                   snippet
                   };
        }


        #region Overrides of AbstractBaseInjecter


        protected override CodeStatement[ ] getInjectionStatement( ) { return this.createCodeStatements( ); }


        protected override CodeTypeMember[ ] getRequiredMembers( )
        {
            return new[ ]
                   {
                   this.createINotifyPropertyChanged( ),
                   this.propertyChangingEventMember
                   };
        }


        #endregion


        private CodeTypeMember propertyChangingEventMember
        {
            get
            {
                CodeTypeMember eventMember = new CodeMemberEvent
                                             {
                                             Name = EVENT_FIELD_NAME,
                                             Attributes = MemberAttributes.Public,
                                             Type = new CodeTypeReference( EVENT_TYPE ),
                                             ImplementationTypes =
                                             {
                                             new CodeTypeReference
                                             {
                                             BaseType = IMPLEMENTED_INTERFACE
                                             }
                                             }
                                             };
                return eventMember;
            }
        }


        private CodeTypeMember createINotifyPropertyChanged( )
        {
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
            var notify = new CodeMemberMethod
                         {
                         Name = NAME,
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
            notify.Statements.AddRange( collection );
            return notify;
        }


        private const string PARAM_NAME = "arg";


        private const string NAME = "OnPropertyChanging";

        private const string EVENTARG_TYPE = "System.ComponentModel.PropertyChangingEventArgs";


        private const string IMPLEMENTED_INTERFACE = "System.ComponentModel.INotifyPropertyChanging";


        private const string EVENT_FIELD_NAME = "PropertyChanging";

        private const string EVENT_TYPE = "System.ComponentModel.PropertyChangingEventHandler";

    }


}
