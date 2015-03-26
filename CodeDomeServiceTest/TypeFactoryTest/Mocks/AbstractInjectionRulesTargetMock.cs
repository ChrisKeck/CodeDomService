#region Header Comment


// SrsFrameworks - CodeDomeServiceTest - AbstractInjectionRulesTargetMock.cs - 15/03/2015


#endregion


#region


using System;



#endregion



namespace CodeDomeServiceTest.TypeFactoryTest.Mocks
{

    public abstract class AbstractInjectionRulesTargetMock
    {

        public string publicBasePropGetSet { get; set; }

        public abstract string publicSealedPropGetSet { get; set; }

        public abstract string publicOverridePropGetSet { get; set; }

        public abstract string publicOverridePropGet { get; }

        public abstract string publicOverridePropSet { set; }

        protected abstract string protectedOverridePropGet { get; }

        protected abstract string protectedOverridePropSet { set; }

        public abstract string publicSealedMethod( );

        public string publicBaseMethod( ) { return String.Empty; }

        public abstract string publicOverrideMethod( );

        protected abstract string protectedOverrideMethod( );

    }

}
