using SharedSpaces.SaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommentSaveDataHandler : MonoBehaviour, ISaveDataHandler<CommentSaveData>
{
    public static Dictionary<Guid, CommentData> CommentSaveDataDict = new(); // key: ObjectID, value: Comment

    private void Awake()
    {
        CommentSaveDataManager.SaveDataList.Add(this);
    }

    public void NewData()
    {
        CommentSaveDataDict.Clear();
    }

    public void SaveData(ref CommentSaveData data)
    {
        data.CommentDatas = CommentSaveDataDict.Values.ToArray();
    }

    public void LoadData(CommentSaveData data)
    {
        CommentSaveDataDict.Clear();

        foreach (CommentData commentData in data.CommentDatas)
        {
            CommentSaveDataDict.Add(Guid.Parse(commentData.ObjectID), commentData);
        }
    }
}
