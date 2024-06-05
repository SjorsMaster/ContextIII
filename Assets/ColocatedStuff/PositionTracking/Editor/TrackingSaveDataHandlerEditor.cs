using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackingSaveDataHandler))]
public class TrackingSaveDataHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Start all trackers"))
        {
            ((TrackingSaveDataHandler)target).StartAllTrackers();
        }

        if (GUILayout.Button("Stop all trackers"))
        {
            ((TrackingSaveDataHandler)target).StopAllTrackers();
        }

        if (GUILayout.Button("Render All Saved Paths"))
        {
            ((TrackingSaveDataHandler)target).RenderAllSavedPaths();
        }
    }
}
