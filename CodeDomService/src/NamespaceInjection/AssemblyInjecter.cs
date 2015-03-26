#region Header Comment


// SrsFrameworks - CodeDomService - AssemblyInjecter.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CodeDomService.Types;



#endregion



namespace CodeDomService.NamespaceInjection
{

    [ Export( typeof ( IAssemblyInjecter ) ) ]
    internal class AssemblyInjecter : IAssemblyInjecter
    {

        public AssemblyInjecter( ) { MefInjectionService.service.compose( this ); }

        protected virtual string namespaceName { get { return "__aopCodeDomInjected"; } }


        [ ImportMany( typeof ( IDeclarationInjecter ) ) ]
        private IEnumerable<Lazy<IDeclarationInjecter, IDeclarationInjecterMetadata>> injecters { get; set; }


        private CodeNamespace createNamespace( string namespaceName )
        {
            return new CodeNamespace( namespaceName );
        }


        public CodeNamespace createForInst( Type t )
        {
            var nspace = this.createNamespace( this.namespaceName );
            var members = this.getDeclarations( t );
            foreach ( var member in members )
                if ( ! nspace.Types.Contains( member ) )
                    nspace.Types.Add( member );
            return nspace;
        }


        private IEnumerable<CodeTypeDeclaration> getDeclarations( Type type )
        {
            List<CodeTypeDeclaration> declarations = new List<CodeTypeDeclaration>( );
            foreach ( var injecter in this.injecters )
                declarations.Add( injecter.Value.createTypeDeclaration( type ) );
            return declarations;
        }

    }

}
