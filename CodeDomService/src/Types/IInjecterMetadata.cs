#region Header Comment


// SrsFrameworks - CodeDomService - IInjecterMetadata.cs - 16/03/2015


#endregion


#region


using System.ComponentModel;



#endregion



namespace CodeDomService.Types
{


    public interface IInjecterMetadata
    {

        [ DefaultValue( new[ ]
                        {
                        ""
                        } ) ]
        string[ ] implementedInterface { get; }


        [ DefaultValue( TimeCut.None ) ]
        TimeCut timeCut { get; }


        [ DefaultValue( TargetCut.None ) ]
        TargetCut targetCut { get; }

    }

}
