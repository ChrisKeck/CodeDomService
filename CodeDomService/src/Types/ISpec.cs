#region Header Comment


// SrsFrameworks - CodeDomService - ISpec.cs - 16/03/2015


#endregion


namespace CodeDomService.Types
{

    internal interface ISpec<T> where T : class
    {

        bool isValid( T item );

        ISpec<T> and( ISpec<T> item );

        ISpec<T> or( ISpec<T> item );

        ISpec<T> not( );

    }

}
