using Mirror;
using SharedSpaces.SaveSystem;
using UnityEngine;

public class TrackingSaveDataManager : BaseSaveDataManager<TrackingSaveData>
{
    [SerializeField] private string TrackingDataFileName = "TrackingData";

    public override string FileNameSuffix { get; set; } = "TrackingData";

    public override void SaveData()
    {
        SetFileName(TrackingDataFileName);
        base.SaveData();
    }
    public override void LoadData()
    {
        SetFileName(TrackingDataFileName);
        base.LoadData();
    }

    public override void NewData()
    {
        SetFileName(TrackingDataFileName);
        base.NewData();
    }

    public void VisualizeOnClients()
    {
        foreach (var data in Data.PositionSaveDatas)
        {
            NetworkServer.SendToAll(data);
        }
    }
}
