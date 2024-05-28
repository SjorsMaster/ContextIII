using SharedSpaces.SaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommentSaveData : IPersistentData<PersistentDataProto3>
{
    public static Dictionary<Guid, CommentData> CommentSaveDataDict = new(); // key: ObjectID, value: Comment

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        PersistentDataManagerProto3.PersistentDataList.Add(new CommentSaveData());
    }

    public void NewData()
    {
        CommentSaveDataDict.Clear();
    }

    public void SaveData(ref PersistentDataProto3 data)
    {
        data.CommentDatas = CommentSaveDataDict.Values.ToArray();
    }

    public void LoadData(PersistentDataProto3 data)
    {
        CommentSaveDataDict.Clear();

        foreach (CommentData commentData in data.CommentDatas)
        {
            CommentSaveDataDict.Add(Guid.Parse(commentData.ObjectID), commentData);
        }
    }
}
