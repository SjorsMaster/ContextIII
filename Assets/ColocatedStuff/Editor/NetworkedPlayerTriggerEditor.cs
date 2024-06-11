using Mirror;
using UnityEditor;

[CustomEditor(typeof(NetworkedPlayerTrigger))]
public class NetworkedPlayerTriggerEditor : NetworkBehaviourInspector
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sync Settings", EditorStyles.boldLabel);

        // sync direction
        SerializedProperty syncDirection = serializedObject.FindProperty("syncDirection");
        EditorGUILayout.PropertyField(syncDirection);

        // sync mdoe: only show for ServerToClient components
        if (syncDirection.enumValueIndex == (int)SyncDirection.ServerToClient)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("syncMode"));

        // sync interval
        EditorGUILayout.PropertyField(serializedObject.FindProperty("syncInterval"));

        // apply
        serializedObject.ApplyModifiedProperties();
    }
}

