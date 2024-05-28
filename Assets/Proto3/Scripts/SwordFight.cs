using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class SwordFight : MiniGameBase
{
    [SerializeField] private Sword SwordPrefab;
    [SerializeField] private GameObject GroundPrefab;
    [SerializeField] private float swordSpawnRange = 5f;

    private readonly List<MiniGamePlayer> players = new(); // Server only.
    private readonly List<Sword> swords = new(); // Server only.

    private GameObject ground;

    #region Event Handlers
    private void OnPlayerHit(MiniGamePlayer player)
    {
        players.Remove(player);

        if (players.Count == 0)
        {
            isFinished = true;
        }

        if (players.Count == 1)
        {
            result = new()
            {
                WinnerID = players[0].PlayerID
            };

            isFinished = true;
        }
    }
    #endregion

    [ClientRpc]
    public override void RpcStartMiniGame()
    {
    }

    [ClientRpc]
    public override void RpcEndMiniGame()
    {
    }

    [Server]
    public override void StartMiniGame()
    {
        isFinished = false;
        result = new();

        players.Clear();
        swords.Clear();

        Vector3 averagePosition = Vector3.zero;
        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            players.Add(player);
            averagePosition += player.transform.position;
        }
        averagePosition /= players.Count;
        averagePosition.y = 0;

        ground = Instantiate(GroundPrefab, averagePosition, Quaternion.identity);
        NetworkServer.Spawn(ground);

        foreach (var player in players)
        {
            Vector3 randomPos = averagePosition + new Vector3(
                Random.Range(-swordSpawnRange, swordSpawnRange),
                0.5f,
                Random.Range(-swordSpawnRange, swordSpawnRange));
            Sword sword = Instantiate(SwordPrefab, randomPos, Quaternion.identity);
            sword.OnPlayerHit += OnPlayerHit;
            swords.Add(sword);
            NetworkServer.Spawn(sword.gameObject);
        }
    }

    [Command]
    public override void CmdSendResult(GameResult result)
    {
    }

    public override void EndMiniGame()
    {
        foreach (var sword in swords)
        {
            NetworkServer.Destroy(sword.gameObject);
        }
        swords.Clear();

        NetworkServer.Destroy(ground);
        ground = null;
    }
}