#region Header Comment


// SrsFrameworks - CodeDomeServiceTest - TypeInjectionTest.cs - 08/03/2015


#endregion


#region


using System;
using System.Linq;
using CodeDomeServiceTest.TypeFactoryTest.Mocks;
using CodeDomService;
using CodeDomService.MemberInjection;
using CodeDomService.NamespaceInjection;
using CodeDomService.Types;
using CrossLayerService;
using Microsoft.VisualStudio.TestTools.UnitTesting;



#endregion



namespace CodeDomeServiceTest.TypeFactoryTest
{


    /// <summary>
    ///     Summary description for TypeInjectionTest
    /// </summary>
    [ DeploymentItem( DEPLOY_TEST ), TestClass ]
    public class TypeInjectionTest : TypeInjectionTestBase
    {


        [ TestMethod, DataSource( DATA_SOURCE_TYPE, DATA_SOURCE_TEST, "MethodRow", DataAccessMethod.Sequential ) ]
        public void methodMemberInjecterTest( )
        {
            string contextName = this.TestContext.DataRow [ "Name" ] as String;
            bool expexted = Convert.ToBoolean( this.TestContext.DataRow [ "isValid" ] );
            IMembersInjecter injecter = new MethodMemberInjecter( );
            var codeMembers = injecter.createCodeMembers( testType ).
                                       ToList( );
            Assert.IsNotNull( codeMembers );
            Assert.AreEqual
            ( expexted,
              codeMembers.FindAll( item => item.Name == contextName ).
                          Count == 1,
              contextName );
        }


        [ TestMethod, DataSource( DATA_SOURCE_TYPE, DATA_SOURCE_TEST, "PropertyRow", DataAccessMethod.Sequential )
        ]
        public void propertyMemberInjecterTest( )
        {
            string contextName = this.TestContext.DataRow [ "Name" ] as String;
            bool expexted = Convert.ToBoolean( this.TestContext.DataRow [ "isValid" ] );
            IMembersInjecter injecter = new PropertyMemberInjecter( );
            var codeMembers = injecter.createCodeMembers( testType ).
                                       ToList( );
            Assert.IsNotNull( codeMembers );
            Assert.AreEqual
            ( expexted,
              codeMembers.FindAll( item => item.Name == contextName ).
                          Count == 1,
              contextName );
        }


        [ TestMethod ]

        //        [DataSource( DATA_SOURCE_TYPE,
        //DATA_SOURCE_TEST,
        //"PropertyRow", DataAccessMethod.Sequential )]
        public void namespaceInjecterTest( )
        {
            //string contextName = this.TestContext.DataRow["Name"] as String;
            //bool expexted = Convert.ToBoolean( this.TestContext.DataRow["isValid"] );
            AssemblyInjecter injecter = new AssemblyInjecter( );
            var codeMember = injecter.createForInst( testType );
            Assert.IsNotNull( codeMember );
            Assert.IsTrue( codeMember.Types.Count == 1 );
        }


        [ TestMethod ]

        //        [DataSource( DATA_SOURCE_TYPE,
        //DATA_SOURCE_TEST,
        //"PropertyRow", DataAccessMethod.Sequential )]
        public void compilerServiceTest( )
        {
            //string contextName = this.TestContext.DataRow["Name"] as String;
            //bool expexted = Convert.ToBoolean( this.TestContext.DataRow["isValid"] );
            NamespaceCompiler injecter = new NamespaceCompiler( );
            var codeMember = injecter.compileAssembly( testType );
            Assert.IsNotNull( codeMember );
            Assert.AreNotSame( testType, codeMember );
        }


        [ TestMethod ]

        //        [DataSource( DATA_SOURCE_TYPE,
        //DATA_SOURCE_TEST,
        //"PropertyRow", DataAccessMethod.Sequential )]
        public void instanceServiceTest( )
        {
            //string contextName = this.TestContext.DataRow["Name"] as String;
            //bool expexted = Convert.ToBoolean( this.TestContext.DataRow["isValid"] );
            var codeMember = InjecterService.inject<InjectionRulesTargetMock>( );
            var inst = Activator.CreateInstance( codeMember );
            Assert.IsNotNull( inst );
            Assert.AreNotSame( testType, inst.GetType( ) );
            var notify = inst as INotifyErrorRaised;
            Assert.IsNotNull( notify );
        }


        [ TestMethod ]

        //        [DataSource( DATA_SOURCE_TYPE,
        //DATA_SOURCE_TEST,
        //"PropertyRow", DataAccessMethod.Sequential )]
        public void declarationInjecterTest( )
        {
            //string contextName = this.TestContext.DataRow["Name"] as String;
            //bool expexted = Convert.ToBoolean( this.TestContext.DataRow["isValid"] );
            IDeclarationInjecter injecter = new ClassInjecter( );
            var codeMember = injecter.createTypeDeclaration( testType );
            Assert.IsNotNull( codeMember );
            Assert.AreEqual( 12, codeMember.Members.Count );
        }


        #region Overrides of TypeInjectionTestBase


        protected override void initEx( ) { }


        #endregion
    }

}
