#region Header Comment


// SrsFrameworks - CodeDomeServiceTest - RulesInjectionTest.cs - 15/03/2015


#endregion


#region


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeDomService;
using CodeDomService.Helper;
using CodeDomService.InjectionRules;
using CodeDomService.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;



#endregion



namespace CodeDomeServiceTest.TypeFactoryTest
{

    /// <summary>
    ///     Summary description for TypeInjectionTest
    /// </summary>
    [ DeploymentItem( DEPLOY_TEST ), TestClass ]
    public class RulesInjectionTest : TypeInjectionTestBase
    {


        protected override void initEx( )
        {
            this.propertyInfos = new List<PropertyInfo>( );
            this.injectionRulesFactory = new InjectionRulesFactory( );
            var myArrayMethodInfo = this.testType.GetMethods( PUBLIC_BINDING_FLAGS );
            var myArrayMethodInfo1 = this.testType.GetMethods( NON_PUBLIC_BINDING_FLAGS );
            var propINfos1 = this.testType.GetProperties( PUBLIC_BINDING_FLAGS );
            var propINfos2 = this.testType.GetProperties( NON_PUBLIC_BINDING_FLAGS );
            this.propertyInfos.AddRange( propINfos1 );
            this.propertyInfos.AddRange( propINfos2 );
            this.methods = new List<MethodInfo>( );
            if ( myArrayMethodInfo.isNotNull( ) )
                this.methods.AddRange( myArrayMethodInfo );
            if ( myArrayMethodInfo1.isNotNull( ) )
                this.methods.AddRange( myArrayMethodInfo1 );
        }


        private const BindingFlags NON_PUBLIC_BINDING_FLAGS =
        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;


        private const BindingFlags PUBLIC_BINDING_FLAGS =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;


        private List<MethodInfo> methods;

        private List<PropertyInfo> propertyInfos;


        private IRulesFactory injectionRulesFactory;

        private TestContext testContext;


        [ TestMethod, DataSource( DATA_SOURCE_TYPE, DATA_SOURCE_TEST, "MethodRow", DataAccessMethod.Sequential ) ]
        public void injectionRulesFactoryMethodInfoTest( )
        {
            string contextName = this.TestContext.DataRow [ "Name" ] as String;
            bool expexted = Convert.ToBoolean( this.TestContext.DataRow [ "isValid" ] );
            var method = this.methods.FindAll( item => item.Name == contextName ).
                              FirstOrDefault( );
            ISpec<MethodInfo> spec = this.injectionRulesFactory.createSpec<MethodInfo>( );
            var valid = spec.isValid( method );
            Assert.AreEqual( expexted, valid, contextName );
        }


        [ TestMethod, DataSource( DATA_SOURCE_TYPE, DATA_SOURCE_TEST, "PropertyRow", DataAccessMethod.Sequential )
        ]
        public void injectionRulesFactoryPropertyInfoTest( )
        {
            string contextName = this.TestContext.DataRow [ "Name" ] as String;
            bool expexted = Convert.ToBoolean( this.TestContext.DataRow [ "isValid" ] );
            var method = this.propertyInfos.FindAll( item => item.Name == contextName ).
                              FirstOrDefault( );
            ISpec<PropertyInfo> spec = this.injectionRulesFactory.createSpec<PropertyInfo>( );
            var valid = spec.isValid( method );
            Assert.AreEqual( expexted, valid, contextName );
        }


    }

}
