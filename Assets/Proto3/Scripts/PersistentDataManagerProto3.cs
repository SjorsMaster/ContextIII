using SharedSpaces.SaveSystem;

public class PersistentDataManagerProto3 : PersistentDataManager<PersistentDataProto3>
{
    public override string FileNameSuffix { get; set; } = "Proto3";
}
