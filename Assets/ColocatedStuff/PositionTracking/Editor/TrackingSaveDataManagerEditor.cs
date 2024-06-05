using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackingSaveDataManager))]
public class TrackingSaveDataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("SaveData"))
        {
            ((TrackingSaveDataManager)target).SaveData();
        }

        if (GUILayout.Button("LoadData"))
        {
            ((TrackingSaveDataManager)target).LoadData();
        }

        if (GUILayout.Button("Visualize on clients"))
        {
            ((TrackingSaveDataManager)target).VisualizeOnClients();
        }
    }
}