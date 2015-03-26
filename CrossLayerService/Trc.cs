#region Header Comment


// SrsFrameworks - CrossLayerService - Trc.cs - 08/03/2015


#endregion


#region


using System.ComponentModel.Composition;
using System.Reflection;
using log4net;



#endregion



namespace CrossLayerService
{

    [ Export( typeof ( ITrc ) ) ]
    public class Trc : ITrc
    {


        private static readonly ILog _logger = LogManager.GetLogger
        ( MethodBase.GetCurrentMethod( ).
                     DeclaringType );


        #region Implementation of ITrc


        public void info( object obj ) { _logger.Info( obj ); }

        public void debug( object obj ) { _logger.Debug( obj ); }

        public void warn( object obj ) { _logger.Warn( obj ); }

        public void error( object obj ) { _logger.Error( obj ); }

        public void fatal( object obj ) { _logger.Fatal( obj ); }


        #endregion
    }

}
