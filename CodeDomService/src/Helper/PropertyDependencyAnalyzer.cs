#region Header Comment


// SrsFrameworks - CodeDomService - PropertyDependencyAnalyzer.cs - 15/03/2015


#endregion


#region


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;



#endregion



namespace CodeDomService.Helper
{

    internal class PropertyDependencyAnalyzer : IEnumerable<MethodBase>
    {


        private readonly Byte[ ] bytes;

        private Int32 pos;

        private readonly MethodBase method;

        private static readonly OpCode[ ] _smallOpCodes;

        private static readonly OpCode[ ] _largeOpCodes;


        [ SuppressMessage( "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline" ) ]
        static PropertyDependencyAnalyzer( )
        {
            _smallOpCodes = new OpCode[ 0x100 ];
            _largeOpCodes = new OpCode[ 0x100 ];
            foreach ( FieldInfo fi in typeof ( OpCodes ).GetFields( BindingFlags.Public | BindingFlags.Static ) )
            {
                OpCode opCode = ( OpCode ) fi.GetValue( null );
                UInt16 value = ( UInt16 ) opCode.Value;
                if ( value < 0x100 )
                    _smallOpCodes [ value ] = opCode;
                else if ( ( value & 0xff00 ) == 0xfe00 )
                    _largeOpCodes [ value & 0xff ] = opCode;
            }
        }


        public static Dictionary<string, List<string>> getPropertyInfluences( Type t )
        {
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>( );
            try
            {
                foreach ( PropertyInfo pi in t.GetProperties( ).
                                               Where( q => q.isVirtual( ) ) )
                {
                    var analyzer = createPropertyDependencyAnalyzer( pi );
                    foreach ( var methodBase in analyzer )
                        if ( methodBase.DeclaringType == t
                             && methodBase.IsSpecialName
                             && methodBase.Name.StartsWith( "get_", StringComparison.Ordinal ) )

                        // property dependency found
                            storeFound( map, pi.Name, methodBase.Name.Substring( 4 ) );
                }
            }
            catch
            {
                //ignore IL exception
            }
            return map;
        }


        private static PropertyDependencyAnalyzer createPropertyDependencyAnalyzer( PropertyInfo pi )
        {
            return new PropertyDependencyAnalyzer( pi.GetGetMethod( ) );
        }


        private static void storeFound( IDictionary<string, List<string>> map, string influenced, string by )
        {
            if ( ! map.ContainsKey( by ) )
                map [ by ] = new List<string>( );
            if ( ! map [ by ].Contains( influenced ) )
                map [ by ].Add( influenced );
        }


        private readonly Module module;


        private PropertyDependencyAnalyzer( MethodBase enclosingMethod )
        {
            this.method = enclosingMethod;
            if ( this.method.DeclaringType != null )
            {
                this.bytes = new Byte[ 0 ];
                this.module = this.method.DeclaringType.Assembly.GetModules( ).
                                    FirstOrDefault
                ( m => m.GetTypes( ).
                         Any( t => t == this.method.DeclaringType ) );
                MethodBody methodBody = this.method.GetMethodBody( );
                this.bytes = ( methodBody == null )
                              ? new Byte[ 0 ]
                              : methodBody.GetILAsByteArray( );
            }
            this.pos = 0;
        }


        public IEnumerator<MethodBase> GetEnumerator( )
        {
            while ( this.pos < this.bytes.Length )
            {
                var x = this.next( );
                if ( null == x )
                {
                    this.pos = 0;
                    yield break;
                }
                yield return x;
            }
            this.pos = 0;
        }


        IEnumerator IEnumerable.GetEnumerator( ) { return this.GetEnumerator( ); }


        [ SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "index16" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "index8" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "float64" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "float32" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "int64" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "int32" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "int8" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "delta" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "shortDelta" ),
          SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "offset" ) ]
        private MethodBase next( )
        {
            while ( this.pos < this.bytes.Length )
            {
                Int32 offset = this.pos;
                OpCode opCode = OpCodes.Nop;
                Int32 token = 0;

                // read first 1 or 2 bytes as opCode
                Byte code = this.readByte( );
                if ( code != 0xFE )
                    opCode = _smallOpCodes [ code ];
                else
                {
                    code = this.readByte( );
                    opCode = _largeOpCodes [ code ];
                }
                switch ( opCode.OperandType )
                {
                    case OperandType.InlineNone:
                        continue;
                    case OperandType.ShortInlineBrTarget:
                        SByte shortDelta = this.readSByte( );
                        continue;
                    case OperandType.InlineBrTarget:
                        Int32 delta = this.readInt32( );
                        continue;
                    case OperandType.ShortInlineI:
                        Byte int8 = this.readByte( );
                        continue;
                    case OperandType.InlineI:
                        Int32 int32 = this.readInt32( );
                        continue;
                    case OperandType.InlineI8:
                        Int64 int64 = this.readInt64( );
                        continue;
                    case OperandType.ShortInlineR:
                        Single float32 = this.readSingle( );
                        continue;
                    case OperandType.InlineR:
                        Double float64 = this.readDouble( );
                        continue;
                    case OperandType.ShortInlineVar:
                        Byte index8 = this.readByte( );
                        continue;
                    case OperandType.InlineVar:
                        UInt16 index16 = this.readUInt16( );
                        continue;
                    case OperandType.InlineString:
                        token = this.readInt32( );
                        continue;
                    case OperandType.InlineSig:
                        token = this.readInt32( );
                        continue;
                    case OperandType.InlineField:
                        token = this.readInt32( );
                        continue;
                    case OperandType.InlineType:
                        token = this.readInt32( );
                        continue;
                    case OperandType.InlineTok:
                        token = this.readInt32( );
                        continue;
                    case OperandType.InlineMethod:
                        token = this.readInt32( );
                        return this.module.ResolveMethod( token );
                    case OperandType.InlineSwitch:
                        Int32 cases = this.readInt32( );
                        Int32[ ] deltas = new Int32[ cases ];
                        for ( Int32 i = 0; i < cases; i++ )
                            deltas [ i ] = this.readInt32( );
                        continue;
                    default:
                        throw new BadImageFormatException( "unexpected OperandType " + opCode.OperandType );
                }
            }
            return null;
        }


        private Byte readByte( ) { return this.bytes [ this.pos++ ]; }

        private SByte readSByte( ) { return ( SByte ) this.readByte( ); }


        private UInt16 readUInt16( )
        {
            this.pos += 2;
            return BitConverter.ToUInt16( this.bytes, this.pos - 2 );
        }


        [ SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ) ]
        private UInt32 readUInt32( )
        {
            this.pos += 4;
            return BitConverter.ToUInt32( this.bytes, this.pos - 4 );
        }


        [ SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ) ]
        private UInt64 readUInt64( )
        {
            this.pos += 8;
            return BitConverter.ToUInt64( this.bytes, this.pos - 8 );
        }


        private Int32 readInt32( )
        {
            this.pos += 4;
            return BitConverter.ToInt32( this.bytes, this.pos - 4 );
        }


        private Int64 readInt64( )
        {
            this.pos += 8;
            return BitConverter.ToInt64( this.bytes, this.pos - 8 );
        }


        private Single readSingle( )
        {
            this.pos += 4;
            return BitConverter.ToSingle( this.bytes, this.pos - 4 );
        }


        private Double readDouble( )
        {
            this.pos += 8;
            return BitConverter.ToDouble( this.bytes, this.pos - 8 );
        }

    }

}
