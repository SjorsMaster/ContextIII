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

        TrackedAnchorObject rightHand = LocalPlayerManager.Instance.RightHand;

        InstantiableObjectData instantiableObjectData = new()
        {
            ObjectID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            PrefabName = "ChatBubble",
            TargetAnchorKey = rightHand.RelativeOffsetData.AnchorKey,
            OffsetPosition = rightHand.RelativeOffsetData.OffsetPosition,
            OffsetRotation = rightHand.RelativeOffsetData.OffsetRotation
        };

        NetworkAssetSpawnManager.Instance.RequestSpawn(instantiableObjectData);
    }
}
