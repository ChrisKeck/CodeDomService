#region Header Comment


// SrsFrameworks - CodeDomService - GenericExtensions.cs - 15/03/2015


#endregion


#region


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



#endregion



namespace CodeDomService.Helper
{

    internal static class GenericExtensions
    {

        internal static bool isNull<TClass>( this TClass current ) where TClass : class
        {
            try
            {
                return ( current == null );
            }
            catch
            {
                throw;
            }
        }


        public static bool isOverride( this MethodInfo methodInfo )
        {
            if ( methodInfo.isNull( ) )
                return false;
            return ( methodInfo.GetBaseDefinition( ) != methodInfo );
        }


        internal static bool isNotNull<TClass>( this TClass current ) where TClass : class
        {
            try
            {
                return ( ! current.isNull( ) );
            }
            catch
            {
                throw;
            }
        }


        internal static string getMemberName<T>( Expression<Action<T>> expression )
        {
            return ( ( MethodCallExpression ) expression.Body ).Method.Name;
        }


        internal static void addIfNotNull<T>( this ICollection<T> collection, T newValue ) where T : class
        {
            if ( newValue.isNotNull( ) )
                collection.Add( newValue );
        }


        internal static void addIfNoDuplicate<T>( this ICollection<T> collection,
                                                  T newValue,
                                                  IEqualityComparer<T> comparer = null ) where T : class
        {
            bool validToAdd = false;
            if ( newValue.isNotNull( ) )
                if ( comparer.isNotNull( ) )
                {
                    if ( ! collection.Contains( newValue, comparer ) )
                        validToAdd = true;
                }
                else if ( ! collection.Contains( newValue ) )
                    validToAdd = true;
            if ( validToAdd )
                collection.Add( newValue );
        }

    }

}
