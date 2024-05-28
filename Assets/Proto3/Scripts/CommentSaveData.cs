using SharedSpaces.SaveSystem;
using System;
using System.Collections.Generic;
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
        data.CommentData = new CommentData[CommentSaveDataDict.Count];

        int i = 0;
        foreach (CommentData commentData in CommentSaveDataDict.Values)
        {
            data.CommentData[i] = commentData;
        }
    }

    public void LoadData(PersistentDataProto3 data)
    {
        CommentSaveDataDict.Clear();

        foreach (CommentData commentData in data.CommentData)
        {
            CommentSaveDataDict.Add(Guid.Parse(commentData.ObjectID), commentData);
        }
    }
}
