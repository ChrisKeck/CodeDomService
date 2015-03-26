#region Header Comment


// SrsFrameworks - CodeDomService - InjecterService.cs - 15/03/2015


#endregion


#region


using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using CodeDomService.Helper;
using CodeDomService.Types;



#endregion



namespace CodeDomService
{

    internal class InjecterService
    {

        private static readonly ConcurrentDictionary<Type, Type> _dict = new ConcurrentDictionary<Type, Type>( );


        [ Import( typeof ( INamespaceCompiler ) ) ]
        private Lazy<INamespaceCompiler> compiler { get; set; }


        private InjecterService( ) { MefInjectionService.service.compose( this ); }

        private static readonly Lazy<InjecterService> _serLazy = new Lazy<InjecterService>
        ( ( ) => new InjecterService( ) );


        public static Type inject<T>( )
        {
            var value = _dict.GetOrAdd( typeof ( T ), ( Type ) null );
            if ( value.isNull( ) )
                value = _serLazy.Value.injectType( typeof ( T ) );
            return value;
        }


        private Type injectType( Type type )
        {
            Type value = null;
            try
            {
                value = this.compiler.Value.compileAssembly( type );
                if ( value.isNull( ) )
                    value = type;
            }
            catch ( Exception e )
            {
                value = type;
            }
            return value;
        }

    }

}
