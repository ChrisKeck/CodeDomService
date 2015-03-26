#region Header Comment


// SrsFrameworks - CodeDomService - ClassInjecter.cs - 15/03/2015


#endregion


#region


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using CodeDomService.Metadata;
using CodeDomService.Types;



#endregion



namespace CodeDomService.NamespaceInjection
{

    [ TypeInjecterMetadata( filter = new[ ]
                                     {
                                     ""
                                     } ) ]
    internal class ClassInjecter : IDeclarationInjecter
    {

        public ClassInjecter( ) { MefInjectionService.service.compose( this ); }


        [ ImportMany( typeof ( IMembersInjecter ) ) ]
        private IEnumerable<Lazy<IMembersInjecter, IMemberInjecterMetadata>> membersInjecters { get; set; }


        [ Import( typeof ( IRulesFactory ) ) ]
        private IRulesFactory rulesFactory { get; set; }


        #region Implementation of IDeclarationInjecter


        public CodeTypeDeclaration createTypeDeclaration( Type type )
        {
            CodeTypeDeclaration declaration = null;
            var specFac = this.rulesFactory;
            var spec = specFac.createSpec<Type>( );
            if ( spec.isValid( type ) )
            {
                declaration = this.createCodeTypeDeclaration( type );
                foreach ( Lazy<IMembersInjecter, IMemberInjecterMetadata> lazy in this.membersInjecters )
                {
                    var factory = lazy.Value;
                    if ( isValid( lazy.Metadata ) )
                        declaration.Members.AddRange( factory.createCodeMembers( type ) );
                }
            }
            return declaration;
        }


        private static bool isValid( IMemberInjecterMetadata item )
        {
            return ( item.targetCut == TargetCut.Method || item.targetCut == TargetCut.Property
                     || item.targetCut == TargetCut.Constructor ) && item.timeCut == TimeCut.All;
        }


        private const string TYPE_PREFIX = "AoPCd__";


        private CodeTypeDeclaration createCodeTypeDeclaration( Type type )
        {
            var decl = new CodeTypeDeclaration
                       {
                       Name = string.Format( "{0}{1}", TYPE_PREFIX, type.Name ),
                       TypeAttributes = TypeAttributes.Public,
                       Attributes = MemberAttributes.Private
                       };
            decl.BaseTypes.AddRange( this.addBaseTypes( type ) );
            return decl;
        }


        private CodeTypeReference[ ] addBaseTypes( Type type )
        {
            List<CodeTypeReference> references = new List<CodeTypeReference>
                                                 {
                                                 new CodeTypeReference( type )
                                                 };
            foreach ( Lazy<IMembersInjecter, IMemberInjecterMetadata> item1 in this.membersInjecters )
            {
                var member = item1.Value;
                var interfaces = member.implementedInterfaces;
                foreach ( string fieldTypeMember in interfaces )
                    if ( type.GetInterfaces( ).
                              All( item => item.FullName != fieldTypeMember ) )
                        if ( references.FindAll( item => item.BaseType == fieldTypeMember ).
                                        Count == 0 )
                            references.Add( new CodeTypeReference( fieldTypeMember ) );
            }
            return references.ToArray( );
        }


        #endregion
    }


}
