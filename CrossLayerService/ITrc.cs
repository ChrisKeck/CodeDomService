#region Header Comment


// SrsFrameworks - CrossLayerService - ITrc.cs - 08/03/2015


#endregion


namespace CrossLayerService
{

    public interface ITrc
    {

        void info( object obj );

        void debug( object obj );

        void warn( object obj );

        void error( object obj );

        void fatal( object obj );

    }

}
