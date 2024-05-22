using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class SwordFight : MiniGameBase
{
    [SerializeField] private Sword SwordPrefab;

    private Dictionary<int, MiniGamePlayer> players;
    private List<Sword> swords;

    [ClientRpc]
    public override void RpcStartMiniGame()
    {
        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            
        }
    }

    [ClientRpc]
    public override void RpcEndMiniGame()
    {
    }

    [Server]
    public override void StartMiniGame()
    {
        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            players.Add(player.PlayerID, player);
        }
    }

    [Command]
    public override void CmdSendResult(GameResult result)
    {
    }

    public override void EndMiniGame()
    {
    }
}