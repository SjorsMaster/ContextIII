using Mirror;
using SharedSpaces.SaveSystem;

public class TrackingSaveDataManager : BaseSaveDataManager<TrackingSaveData>
{
    public override string FileNameSuffix { get; set; } = "TrackingData";

    public void VisualizeOnClients()
    {
        foreach (var data in Data.PositionSaveDatas)
        {
            NetworkServer.SendToAll(data);
        }
    }
}
