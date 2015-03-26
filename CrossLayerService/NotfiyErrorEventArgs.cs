#region Header Comment


// SrsFrameworks - CrossLayerService - NotfiyErrorEventArgs.cs - 17/03/2015


#endregion


#region


using System;
using System.IO;



#endregion



namespace CrossLayerService
{

    public class NotfiyErrorEventArgs : ErrorEventArgs
    {

        public NotfiyErrorEventArgs( Exception exception ) : base( exception ) { }

        public bool resolved { get; set; }

    }

}
