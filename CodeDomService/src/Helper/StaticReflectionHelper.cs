#region Header Comment


// SrsFrameworks - CodeDomService - StaticReflectionHelper.cs - 15/03/2015


#endregion


#region


using System;
using System.Linq.Expressions;



#endregion



namespace CodeDomService.Helper
{

    public static class StaticReflectionHelper
    {

        public static string GetMemberName<T>( this T source, Expression<Func<T, object>> exp )
        {
            return GetMemberName( exp );
        }


        public static string GetMemberName<T>( Expression<Func<T, Object>> exp )
        {
            if ( exp == null )
                throw new ArgumentNullException( "exp" );
            return getMemberNameByExpression( exp.Body );
        }


        public static String GetMemberName<T>( Expression<Action> exp )
        {
            if ( exp == null )
                throw new ArgumentNullException( "exp" );
            return getMemberNameByExpression( exp.Body );
        }


        private static String getMemberNameByExpression( Expression exp )
        {
            if ( exp == null )
                throw new ArgumentNullException( "exp" );
            var memExp = exp as MemberExpression;
            if ( memExp != null )
                return getMemberNameByMemberExpression( memExp );
            var medCaExp = exp as MethodCallExpression;
            if ( medCaExp != null )
                return getMemberNameByMethodCallExpression( medCaExp );
            var unExp = exp as UnaryExpression;
            if ( unExp != null )
                return getMemberNameByUnaryExpression( unExp );
            throw new InvalidOperationException( "Invalid expression" );
        }


        private static String getMemberNameByMemberExpression( MemberExpression memExp )
        {
            return memExp.Member.Name;
        }


        private static String getMemberNameByMethodCallExpression( MethodCallExpression medCaExp )
        {
            return medCaExp.Method.Name;
        }


        private static String getMemberNameByUnaryExpression( UnaryExpression unExp )
        {
            var medCaExp = unExp.Operand as MethodCallExpression;
            return medCaExp != null
                   ? medCaExp.Method.Name
                   : ( ( MemberExpression ) unExp.Operand ).Member.Name;
        }

    }

}
