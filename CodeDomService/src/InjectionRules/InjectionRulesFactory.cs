#region Header Comment


// SrsFrameworks - CodeDomService - InjectionRulesFactory.cs - 15/03/2015


#endregion


#region


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using CodeDomService.Helper;
using CodeDomService.Types;



#endregion



namespace CodeDomService.InjectionRules
{

    [ Export( typeof ( IRulesFactory ) ) ]
    internal class InjectionRulesFactory : IRulesFactory
    {

        public ISpec<T> createSpec<T>( params object[ ] args ) where T : class
        {
            ISpec<T> spec = null;
            if ( typeof ( T ) == typeof ( Type ) )
                spec = this.getClassRules( ) as ISpec<T>;
            else if ( typeof ( T ) == typeof ( PropertyInfo ) )
                spec = this.getPropertyRules( ) as ISpec<T>;
            else if ( typeof ( T ) == typeof ( MethodInfo ) )
                spec = this.getMethodRules( ) as ISpec<T>;
            else if ( typeof ( T ) == typeof ( ConstructorInfo ) )
                spec = this.getConstructorRules( ) as ISpec<T>;
            else if ( typeof ( T ) == typeof ( Assembly ) )
                spec = this.getAssemblyRules( ) as ISpec<T>;
            else if ( typeof ( T ) == typeof ( Assembly ) )
            { }
            else
                throw new ArgumentOutOfRangeException( typeof ( T ).ToString( ) );
            return spec;
        }


        private ISpec<Type> getClassRules( )
        {
            ISpec<Type> isAbstractSpec = new MemberSpec<Type>( item => item.IsAbstract ).not( );
            ISpec<Type> isSealedSpec = new MemberSpec<Type>( item => item.IsSealed ).not( );
            ISpec<Type> isClassSpec = new MemberSpec<Type>( item => item.IsClass );
            ISpec<Type> isPublicSpec = new MemberSpec<Type>( item => item.IsPublic );
            ISpec<Type> isNotClosed = isSealedSpec.and( isAbstractSpec );
            ISpec<Type> isOpenClosed = isClassSpec.and( isPublicSpec ).
                                                   and( isNotClosed );
            return isOpenClosed;
        }


        private ISpec<PropertyInfo> getPropertyRules( )
        {
            List<ISpec<MethodInfo>> specs = new List<ISpec<MethodInfo>>
                                            {
                                            new MemberSpec<MethodInfo>( item => item.isNotNull( ) ),
                                            new MemberSpec<MethodInfo>( item => ! item.IsFinal ),
                                            new MemberSpec<MethodInfo>( item => item.IsPublic ),
                                            new MemberSpec<MethodInfo>( item => item.IsVirtual )
                                            };
            var methodRules = specs.Aggregate( ( item, next ) => item.and( next ) );
            var getSpec = new MemberSpec<PropertyInfo>( item => methodRules.isValid( item.GetGetMethod( ) ) );
            List<ISpec<PropertyInfo>> methodSpecs = new List<ISpec<PropertyInfo>>
                                                    {
                                                    getSpec,
                                                    new MemberSpec<PropertyInfo>
                                                    ( item => methodRules.isValid( item.GetSetMethod( ) ) )
                                                    };
            var aggregated = methodSpecs.Aggregate( ( item, next ) => item.and( next ) );
            return aggregated.or( getSpec );
        }


        private ISpec<MethodInfo> getMethodRules( )
        {
            List<ISpec<MethodInfo>> specs = new List<ISpec<MethodInfo>>
                                            {
                                            new MemberSpec<MethodInfo>( item => item.isNotNull( ) ),
                                            new MemberSpec<MethodInfo>( item => ! item.IsFinal ),
                                            new MemberSpec<MethodInfo>( item => item.IsPublic ),
                                            new MemberSpec<MethodInfo>( item => item.IsVirtual ),
                                            new MemberSpec<MethodInfo>( item => ! item.IsSpecialName )
                                            ,new MemberSpec<MethodInfo>( item => item.Name=="Equals" ).not()
                                            ,new MemberSpec<MethodInfo>( item => item.Name=="GetHashCode" ).not()
                                            ,new MemberSpec<MethodInfo>( item => item.Name=="ToString" ).not()
                                            };
            return specs.Aggregate( ( item, next ) => item.and( next ) );
        }


        private ISpec<ConstructorInfo> getConstructorRules( )
        {
            ISpec<ConstructorInfo> isConstructorSpec = new MemberSpec<ConstructorInfo>
            ( item => item.GetParameters( ).
                           Length > 0 );
            ISpec<ConstructorInfo> isPublicSpec = new MemberSpec<ConstructorInfo>( item => item.IsPublic );
            return isConstructorSpec.and( isPublicSpec );
        }


        private ISpec<Assembly> getAssemblyRules( )
        {
            ISpec<Assembly> isDynamic = new MemberSpec<Assembly>( item => item.IsDynamic ).not( );
            return isDynamic;
        }

    }

}
