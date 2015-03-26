#region Header Comment


// SrsFrameworks - CodeDomService - ProxyGenerationException.cs - 15/03/2015


#endregion


#region


using System;
using System.Runtime.Serialization;



#endregion



namespace CodeDomService.Helper
{

    [ Serializable ]
    internal class ProxyGenerationException : Exception
    {

        public ProxyGenerationException( ) { }

        public ProxyGenerationException( string message ) : base( message ) { }

        public ProxyGenerationException( string message, Exception inner ) : base( message, inner ) { }


        protected ProxyGenerationException( SerializationInfo info, StreamingContext context )
        : base( info, context )
        { }

    }

}
