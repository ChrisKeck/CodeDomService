#region Header Comment


// SrsFrameworks - CodeDomService - IRulesFactory.cs - 16/03/2015


#endregion


namespace CodeDomService.Types
{

    internal interface IRulesFactory
    {

        ISpec<T> createSpec<T>( params object[ ] args ) where T : class;

    }

}
