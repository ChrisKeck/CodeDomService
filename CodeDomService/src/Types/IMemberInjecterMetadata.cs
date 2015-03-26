#region Header Comment


// SrsFrameworks - CodeDomService - IMemberInjecterMetadata.cs - 16/03/2015


#endregion


#region


using System.ComponentModel;



#endregion



namespace CodeDomService.Types
{

    public interface IMemberInjecterMetadata
    {

        [ DefaultValue( TimeCut.None ) ]
        TimeCut timeCut { get; }


        [ DefaultValue( TargetCut.None ) ]
        TargetCut targetCut { get; }

    }

}
