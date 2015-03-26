#region Header Comment


// SrsFrameworks - CodeDomService - NamespaceCompiler.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeDomService.Helper;
using CodeDomService.Types;
using Microsoft.CSharp;



#endregion



namespace CodeDomService
{

    [ Export( typeof ( INamespaceCompiler ) ) ]
    internal class NamespaceCompiler : INamespaceCompiler
    {

        public NamespaceCompiler( ) { MefInjectionService.service.compose( this ); }


        [ Import( typeof ( IAssemblyInjecter ) ) ]
        private Lazy<IAssemblyInjecter> injecter { get; set; }


        [ Import( typeof ( IRulesFactory ) ) ]
        public IRulesFactory rulesFactory { get; set; }


        public Type compileAssembly( Type type )
        {
            Type result = null;
            var spec = this.rulesFactory.createSpec<Type>( );
            if ( spec.isValid( type ) )
                using ( CSharpCodeProvider codeProvider = new CSharpCodeProvider( ) )
                {
                    CompilerParameters compilerParams = new CompilerParameters
                                                        {
                                                        GenerateInMemory = true
                                                        };
                    CodeCompileUnit compilerUnit = new CodeCompileUnit( );
                    compilerUnit.Namespaces.Add( this.injecter.Value.createForInst( type ) );
                    this.addAllAssemblyAsReference( compilerUnit.ReferencedAssemblies );
                    CompilerResults cr = codeProvider.CompileAssemblyFromDom( compilerParams, compilerUnit );
                    this.writeDebugInfos( codeProvider, compilerUnit );
                    if ( cr.Errors.Count > 0 )
                        this.throwErrors( cr.Errors.OfType<CompilerError>( ) );
                    result = cr.CompiledAssembly.GetTypes( ) [ 0 ];
                }
            else
                result = type;
            return result;
        }


        protected void writeDebugInfos( CodeDomProvider codeProvider, CodeCompileUnit compilerUnit )
        {
#if DEBUG
            try
            {
                StringWriter sw = new StringWriter( );
                codeProvider.GenerateCodeFromCompileUnit
                ( compilerUnit,
                  sw,
                  new CodeGeneratorOptions
                  {
                  BracingStyle = "C"
                  } );
                Debug.WriteLine( sw.GetStringBuilder( ) );
            }
            catch ( Exception error )
            {
                Debug.WriteLine( error );
            }
#endif
        }


        protected void addAllAssemblyAsReference( StringCollection referencedAssemblies )
        {
            foreach ( var v in this.getCurrentAssemblies( ).
                                    Where( item => ! item.IsDynamic ) )
                referencedAssemblies.Add( v.Location );
        }


        private IEnumerable<Assembly> getCurrentAssemblies( ) { return AppDomain.CurrentDomain.GetAssemblies( ); }


        protected void throwErrors( IEnumerable compilerErrorCollection )
        {
            try
            {
                StringBuilder sb = new StringBuilder( );
                foreach ( CompilerError e in compilerErrorCollection )
                    sb.AppendLine( e.ErrorText );
                var errorMsg = "Compiler errors:\n" + sb;
                Debug.WriteLine( errorMsg );
                throw new ProxyGenerationException( errorMsg );
            }
            catch ( Exception )
            {
                throw;
            }
        }

    }

}
