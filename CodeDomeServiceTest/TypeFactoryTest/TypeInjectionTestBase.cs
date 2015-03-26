#region Header Comment


// SrsFrameworks - CodeDomeServiceTest - TypeInjectionTestBase.cs - 15/03/2015


#endregion


#region


using System;
using CodeDomeServiceTest.TypeFactoryTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;



#endregion



namespace CodeDomeServiceTest.TypeFactoryTest
{

    [ DeploymentItem( DEPLOY_TEST ) ]
    public abstract class TypeInjectionTestBase
    {

        protected const string DATA_SOURCE_TYPE = "Microsoft.VisualStudio.TestTools.DataSource.XML";

        protected const string DEPLOY_TEST = @"DataFiles\InjectionTest_DataSource.xml";

        protected const string DATA_SOURCE_TEST = @"|DataDirectory|" + @"\InjectionTest_DataSource.xml";

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        private TestContext testContextInstance;


        public TestContext TestContext
        {
            get { return this.testContextInstance; }
            set { this.testContextInstance = value; }
        }


        protected Type testType { get; private set; }


        [ TestInitialize ]
        public void init( )
        {
            this.testType = typeof ( InjectionRulesTargetMock );
            this.initEx( );
        }


        protected abstract void initEx( );

    }

}
