#region Header Comment


// SrsFrameworks - CodeDomService - MefInjectionService.cs - 15/03/2015


#endregion


#region


using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;



#endregion



namespace CodeDomService
{

    public class MefInjectionService
    {

        public static MefInjectionService service { get { return _serLazy.Value; } }

        private static readonly Lazy<MefInjectionService> _serLazy = new Lazy<MefInjectionService>
        ( ( ) => new MefInjectionService( ) );


        public void compose<T>( T part )
        {
            var catalog = new DirectoryCatalog( "." );
            var container = new CompositionContainer( catalog );
            container.ComposeParts( part );
        }

    }

}
