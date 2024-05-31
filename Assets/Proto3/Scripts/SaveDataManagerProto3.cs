using SharedSpaces.SaveSystem;

public class SaveDataManagerProto3 : SaveDataManager<SaveDataProto3>
{
    public override string FileNameSuffix { get; set; } = "Proto3";
}
