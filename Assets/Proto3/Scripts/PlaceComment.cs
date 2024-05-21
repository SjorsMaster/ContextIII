using SharedSpaces;
using SharedSpaces.Data;
using SharedSpaces.Managers;
using System;
using UnityEngine;

public class PlaceComment : MonoBehaviour
{
    [SerializeField] private float cooldown = 10f;
    
    private float timeSinceLastComment = 0f;

    public void CreateEmptyComment()
    {
        if (Time.time - timeSinceLastComment < cooldown)
        {
            return;
        }
        timeSinceLastComment = Time.time;

        AnchoredObject rightHand = LocalPlayerManager.Instance.LeftHand;

        AnchoredObjectData data = new()
        {
            IsInstantiated = true,
            ObjectID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            PrefabName = "ChatBubble",
            UUID = rightHand.UUID,
            LocalPosition = rightHand.LocalPosition,
            LocalRotation = rightHand.LocalRotation,
        };

        NetworkAssetSpawnManager.Instance.RequestSpawn(data);
    }
}
