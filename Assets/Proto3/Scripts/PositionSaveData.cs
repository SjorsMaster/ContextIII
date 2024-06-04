using Mirror;
using System.Collections.Generic;
using UnityEngine;

public struct PositionSaveData : NetworkMessage
{
    public uint ID;
    public List<AnchoredPosition> AnchoredPositions;
}

public struct AnchoredPosition
{
    public string AnchorUUID;
    public Vector3 RelativePosition;
}