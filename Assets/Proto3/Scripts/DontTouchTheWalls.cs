using System.Collections.Generic;
using UnityEngine;

public class DontTouchTheWalls : MiniGameBase
{
    [SerializeField] private GameObject PlayerDotPrefab;
    [SerializeField] private PathField[] pathfields;

    private Dictionary<int, MiniGamePlayer> players; // Key: PlayerID, value: MiniGamePlayer

    private PathField currentPath;

    public override void StartMiniGame()
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            players.Add(player.PlayerID, player);
            averagePosition += player.transform.position;
        }

        int random = Random.Range(0, pathfields.Length);
        currentPath = Instantiate(pathfields[random]);

    }

    public override void EndMiniGame()
    {
    }

    public override void RpcStartMiniGame()
    {
    }

    public override void RpcEndMiniGame()
    {
    }

    public override void CmdSendResult(GameResult result)
    {
    }
}
