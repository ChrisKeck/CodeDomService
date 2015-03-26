#region Header Comment


// SrsFrameworks - CrossLayerService - INotifyErrorRaised.cs - 08/03/2015


#endregion


#region


using System;



#endregion



namespace CrossLayerService
{

    public interface INotifyErrorRaised
    {

        event EventHandler<NotfiyErrorEventArgs> errorRaised;

    }


}
