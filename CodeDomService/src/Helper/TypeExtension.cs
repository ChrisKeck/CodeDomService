#region Header Comment


// SrsFrameworks - CodeDomService - TypeExtension.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;



#endregion



namespace CodeDomService.Helper
{

    internal static class TypeExtension
    {

        internal static bool isVirtual( this PropertyInfo pi )
        {
            var setMethod = pi.GetSetMethod( );
            var getMethod = pi.GetGetMethod( );
            var isSetVirtual = setMethod.isNotNull( ) && ( setMethod.IsVirtual || setMethod.IsAbstract )
                               && ! setMethod.IsFinal;
            var isGetVirtual = getMethod.isNotNull( ) && ( getMethod.IsVirtual || getMethod.IsAbstract )
                               && ! getMethod.IsFinal;
            var isVirtual = isGetVirtual || isSetVirtual;
            return isVirtual;
        }


        internal static bool isVirtual( this MethodBase pi )
        {
            var isSetVirtual = pi.isNotNull( ) && ( pi.IsVirtual || pi.IsAbstract ) && ! pi.IsFinal;
            return isSetVirtual;
        }


        internal static IEnumerable<MethodInfo> getMethodsInfos( this Type type )
        {
            var results = new List<MethodInfo>( type.GetMethods( ) );
            results.AddRange( type.GetMethods( BindingFlags.NonPublic | BindingFlags.Instance ) );
            return results;
        }


        internal static IEnumerable<PropertyInfo> getPropertyInfos( this Type type )
        {
            var results = new List<PropertyInfo>( type.GetProperties( ) );
            results.AddRange( type.GetProperties( BindingFlags.NonPublic | BindingFlags.Instance ) );
            return results;
        }


        internal static IEnumerable<MemberInfo> getFieldEventInfos( this Type type )
        {
            var results = new List<MemberInfo>( type.GetEvents( ) );
            results.AddRange( type.GetFields( ) );
            return results;
        }


        internal static IEnumerable<FieldInfo> getFieldsInfos( this Type type )
        {
            var results = new List<FieldInfo>( );
            results.AddRange( type.GetFields( ) );
            return results;
        }


        internal static IEnumerable<EventInfo> getEventInfos( this Type type )
        {
            var results = new List<EventInfo>( type.GetEvents( ) );
            return results;
        }


        internal static FieldDirection toDirection( this ParameterInfo pi )
        {
            if ( pi.IsIn )
                return FieldDirection.In;
            if ( pi.IsOut )
                return FieldDirection.Out;
            return pi.ParameterType.IsByRef
                   ? FieldDirection.Ref
                   : FieldDirection.In;
        }

    }


}
