using SharedSpaces.SaveSystem;

public class CommentSaveDataManager : BaseSaveDataManager<CommentSaveData>
{
    public override string FileNameSuffix { get; set; } = "Proto3";
}
