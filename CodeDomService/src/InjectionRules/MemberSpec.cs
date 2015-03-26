#region Header Comment


// SrsFrameworks - CodeDomService - MemberSpec.cs - 15/03/2015


#endregion


#region


using System;
using CodeDomService.Types;



#endregion



namespace CodeDomService.InjectionRules
{

    internal class MemberSpec<T> : ISpec<T> where T : class
    {

        private readonly Func<T, bool> validFunc;


        public MemberSpec( Func<T, bool> validFunc ) { this.validFunc = validFunc; }


        #region Implementation of ISpec<T>


        public bool isValid( T item ) { return this.validFunc.Invoke( item ); }


        public ISpec<T> and( ISpec<T> item )
        {
            return new MemberSpec<T>( current => ( this.isValid( current ) && item.isValid( current ) ) );
        }


        public ISpec<T> or( ISpec<T> item )
        {
            return new MemberSpec<T>( current => ( this.isValid( current ) || item.isValid( current ) ) );
        }


        public ISpec<T> not( ) { return new MemberSpec<T>( current => ( ! this.isValid( current ) ) ); }


        #endregion
    }

}
