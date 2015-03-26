#region Header Comment


// SrsFrameworks - CodeDomService - SerializeHelper.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;



#endregion



namespace CodeDomService.Helper
{

    internal class SerializeHelper
    {

        private readonly string path;


        public SerializeHelper( string path ) { this.path = path; }


        public SerializeHelper( ) { this.path = getPath( ); }


        public void serialize<T>( T notify )
        {
            IEnumerable<Type> knownTypesList = getKnownTypesList( );
            var writer = new XmlTextWriter( String.Format( this.path ), Encoding.UTF8 )
                         {
                         Formatting = Formatting.Indented
                         };
            var dcs = new DataContractSerializer
            ( typeof ( T ),
              knownTypesList,
              int.MaxValue,
              true,
              false,
              new InvalidEnumContractSurrogate( typeof ( MemberAttributes ) ),
              new CodeDomResolver( ) );
            dcs.WriteObject( writer, notify );
            writer.Close( );
        }


        private static string getPath( )
        {
            var assemblyPath = Assembly.GetExecutingAssembly( ).
                                        Location;
            var dir = Path.GetDirectoryName( assemblyPath );
            return String.Format( @"{0}\{1}", dir, "injectionConfig.config" );
        }


        public T deserialize<T>( )
        {
            IEnumerable<Type> knownTypesList = getKnownTypesList( );
            TextReader reader = new StreamReader( this.path );
            var writer = new XmlTextReader( reader );
            var dcs = new DataContractSerializer
            ( typeof ( T ),
              knownTypesList,
              int.MaxValue,
              true,
              true,
              new InvalidEnumContractSurrogate( typeof ( MemberAttributes ) ) );
            var result = ( T ) dcs.ReadObject( writer );
            writer.Close( );
            return result;
        }


        private static IEnumerable<Type> getKnownTypesList( )
        {
            Type[ ] knownTypesList =
            {
            //typeof ( CodeTypeDeclaration ),
            //typeof ( CodeNamespaceImport ),
            //typeof ( CodeAttributeDeclaration ),
            //typeof ( CodeTypeReference ),
            //typeof ( DataContractAttribute ),
            //typeof ( CodeMemberField ),
            //typeof ( CodeMemberProperty ),
            //typeof ( MemberAttributes ),
            //typeof ( CodeFieldReferenceExpression ),
            //typeof ( CodeThisReferenceExpression ),
            //typeof ( CodeMethodReturnStatement ),
            //typeof ( CodeAssignStatement ),
            //typeof ( CodeArgumentReferenceExpression ),
            //typeof ( TypeAttributes ),
            //typeof ( CodeConditionStatement ),
            //typeof ( CodeBinaryOperatorExpression ),
            //typeof ( CodePrimitiveExpression ),
            //typeof ( CodeFieldReferenceExpression ),
            //typeof ( CodeVariableReferenceExpression ),
            //typeof ( CodeThisReferenceExpression ),
            //typeof ( CodeExpressionStatement ),
            //typeof ( CodeTypeReference ),
            //typeof ( CodeMethodInvokeExpression ),
            //typeof ( CodeMethodReferenceExpression ),
            //typeof ( CodeMemberMethod ),
            //typeof ( CodeMemberProperty ),
            //typeof ( CodePropertySetValueReferenceExpression ),
            //typeof ( CodeMemberField ),
            //typeof ( CodeMemberEvent ),
            //typeof ( CodeBaseReferenceExpression ),
            //typeof ( CodeParameterDeclarationExpression ),
            //typeof ( CodePropertyReferenceExpression ),
            //typeof ( CodeObjectCreateExpression ),typeof(CodeTryCatchFinallyStatement),
            //typeof(CodeCatchClause),typeof(CodeSnippetExpression),typeof(CodeSnippetStatement)
            };
            return knownTypesList;
        }


        public class CodeDomResolver : DataContractResolver
        {
            #region Overrides of DataContractResolver


            private Dictionary<string, int> serializationDictionary;

            private Dictionary<int, string> deserializationDictionary;

            private int serializationIndex = 0;

            private XmlDictionary dic;


            public CodeDomResolver( )
            {
                this.serializationDictionary = new Dictionary<string, int>( );
                this.deserializationDictionary = new Dictionary<int, string>( );
                this.dic = new XmlDictionary( );
            }


            /// <summary>
            ///     Überschreiben diese Methode, um einem xsi:type-Name und -Namespace bei der Serialisierung einen Datenvertragstyp
            ///     zuzuordnen.
            /// </summary>
            /// <returns>
            ///     true, wenn die Zuordnung erfolgreich war; anderenfalls false.
            /// </returns>
            /// <param name="type">Der zuzuordnende Typ.</param>
            /// <param name="declaredType">Der im Datenvertrag deklarierte Typ.</param>
            /// <param name="knownTypeResolver">Der Resolver des bekannten Typs.</param>
            /// <param name="typeName">Der xsi:type-Name.</param>
            /// <param name="typeNamespace">Der xsi:type-Namespace.</param>
            public override bool TryResolveType( Type type,
                                                 Type declaredType,
                                                 DataContractResolver knownTypeResolver,
                                                 out XmlDictionaryString typeName,
                                                 out XmlDictionaryString typeNamespace )
            {
                if (
                ! knownTypeResolver.TryResolveType( type, declaredType, null, out typeName, out typeNamespace ) )
                {
                    XmlDictionary dictionary = new XmlDictionary( );
                    typeName = dictionary.Add( type.FullName );
                    typeNamespace = dictionary.Add( type.Assembly.FullName );
                }
                return true;
            }


            /// <summary>
            ///     Überschreiben diese Methode, um den angegebenen xsi:type-Name und -Namespace bei der Deserialisierung einem
            ///     Datenvertragstyp zuzuordnen.
            /// </summary>
            /// <returns>
            ///     Der Typ, dem der xsi:type-Name und -Namespace zugeordnet ist.
            /// </returns>
            /// <param name="typeName">Der zuzuordnende xsi:type-Name.</param>
            /// <param name="typeNamespace">Der zuzuordnende xsi:type-Namespace.</param>
            /// <param name="declaredType">Der im Datenvertrag deklarierte Typ.</param>
            /// <param name="knownTypeResolver">Der Resolver des bekannten Typs.</param>
            public override Type ResolveName( string typeName,
                                              string typeNamespace,
                                              Type declaredType,
                                              DataContractResolver knownTypeResolver )
            {
                return knownTypeResolver.ResolveName( typeName, typeNamespace, declaredType, null )
                       ?? Type.GetType( typeName + ", " + typeNamespace );
            }


            #endregion
        }


        /// <summary>
        ///     IDataContractSurrogate to map Enum to int for handling invalid values
        /// </summary>
        private class InvalidEnumContractSurrogate : IDataContractSurrogate
        {

            private readonly HashSet<Type> typelist;


            /// <summary>
            ///     Create new Data Contract Surrogate to handle the specified Enum type
            /// </summary>
            /// <param name="type">Enum Type</param>
            public InvalidEnumContractSurrogate( Type type )
            {
                this.typelist = new HashSet<Type>( );
                if ( ! type.IsEnum )
                    throw new ArgumentException( type.Name + " is not an enum", "type" );
                this.typelist.Add( type );
            }


            /// <summary>
            ///     Create new Data Contract Surrogate to handle the specified Enum types
            /// </summary>
            /// <param name="types">IEnumerable of Enum Types</param>
            public InvalidEnumContractSurrogate( IEnumerable<Type> types )
            {
                this.typelist = new HashSet<Type>( );
                foreach ( var type in types )
                {
                    if ( ! type.IsEnum )
                        throw new ArgumentException( type.Name + " is not an enum", "type" );
                    this.typelist.Add( type );
                }
            }


            #region Interface Implementation


            public Type GetDataContractType( Type type )
            {
                //If the provided type is in the list, tell the serializer it is an int
                if ( this.typelist.Contains( type ) )
                    return typeof ( int );
                return type;
            }


            public object GetObjectToSerialize( object obj, Type targetType )
            {
                //If the type of the object being serialized is in the list, case it to an int
                if ( this.typelist.Contains( obj.GetType( ) ) )
                    return ( int ) obj;
                return obj;
            }


            public object GetDeserializedObject( object obj, Type targetType )
            {
                //If the target type is in the list, convert the value (we are assuming it to be int) to the enum
                if ( this.typelist.Contains( targetType ) )
                    return Enum.ToObject( targetType, obj );
                return obj;
            }


            public void GetKnownCustomDataTypes( Collection<Type> customDataTypes )
            {
                //not used
            }


            public object GetCustomDataToExport( Type clrType, Type dataContractType )
            {
                //Not used
                return null;
            }


            public object GetCustomDataToExport( MemberInfo memberInfo, Type dataContractType )
            {
                //not used
                return null;
            }


            public Type GetReferencedTypeOnImport( string typeName, string typeNamespace, object customData )
            {
                //not used
                return null;
            }


            public CodeTypeDeclaration ProcessImportedType( CodeTypeDeclaration typeDeclaration,
                                                            CodeCompileUnit compileUnit )
            {
                //not used
                return typeDeclaration;
            }


            #endregion
        }

    }

}
