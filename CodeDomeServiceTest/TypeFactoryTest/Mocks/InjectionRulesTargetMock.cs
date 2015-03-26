#region Header Comment


// SrsFrameworks - CodeDomeServiceTest - InjectionRulesTargetMock.cs - 15/03/2015


#endregion


#region


using System;



#endregion



namespace CodeDomeServiceTest.TypeFactoryTest.Mocks
{

    public class InjectionRulesTargetMock : AbstractInjectionRulesTargetMock
    {

        private readonly string publicOverridePropGet1;

        private string publicOverridePropSet1;

        private readonly string protectedOverridePropGet1;

        private string protectedOverridePropSet1;

        public string publicPropGetSet { get; set; }

        public virtual string publicVirtualPropSetGet { get; set; }

        public virtual string publicVirtualPropSet { private get; set; }

        public virtual string publicVirtualPropGet { get; private set; }

        protected virtual string protectedVirtualPropSetGet { get; set; }

        protected virtual string protectedVirtualPropSet { private get; set; }

        protected virtual string protectedVirtualPropGet { get; private set; }

        private string privatePropGetSet { get; set; }

        public virtual string publicVirtualMethod( ) { return String.Empty; }

        public string publicMethod( ) { return String.Empty; }

        protected virtual string protectedVirtualMethod( ) { return String.Empty; }

        protected string privateMethod( ) { return String.Empty; }


        #region Overrides of AbstractInjectionRulesTargetMock


        public override sealed string publicSealedPropGetSet { get; set; }


        public override string publicOverridePropGetSet { get; set; }


        public override string publicOverridePropGet { get { return this.publicOverridePropGet1; } }


        public override string publicOverridePropSet { set { this.publicOverridePropSet1 = value; } }


        public override string publicOverrideMethod( ) { return string.Empty; }

        protected override string protectedOverrideMethod( ) { return string.Empty; }

        protected override string protectedOverridePropGet { get { return this.protectedOverridePropGet1; } }


        protected override string protectedOverridePropSet { set { this.protectedOverridePropSet1 = value; } }


        public override sealed string publicSealedMethod( ) { return string.Empty; }


        #endregion
    }

}
