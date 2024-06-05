using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spleef : MiniGameBase
{
    [SerializeField] private SpleefField SpleefFieldPrefab;
    [SerializeField] private SpleefChecker SpleefCheckerPrefab;

    private List<SpleefChecker> spleefCheckers = new(); // Local and server
    private Dictionary<int, MiniGamePlayer> playerDict = new(); // PlayerID, Player, server only

    private SpleefField activeField; // Server only

    [ClientRpc]
    public override void RpcStartMiniGame()
    {
        StartCoroutine(DelayedStart());
    }

    [Client]
    private IEnumerator DelayedStart()
    {
        spleefCheckers.Clear();

        VRDebugPanel.Instance.SendDebugMessage("Get in the red area!");
        int timer = 5;
        while (timer > 0)
        {
            VRDebugPanel.Instance.SendDebugMessage("Game starts in " + timer);
            yield return new WaitForSeconds(1);
            timer--;
        }

        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            SpleefChecker current = Instantiate(SpleefCheckerPrefab, player.transform.position, Quaternion.identity);
            current.myMiniGamePlayer = player;
            current.OnPlayerFall += SpleefChecker_OnPlayerFall;
            spleefCheckers.Add(current);
        }

        VRDebugPanel.Instance.SendDebugMessage("GO!");
    }

    [Client]
    private void SpleefChecker_OnPlayerFall(MiniGamePlayer player)
    {
        if (player.PlayerID == MiniGamePlayer.LocalPlayerID)
        {
            CmdSendResult(new GameResult
            {
                LoserID = player.PlayerID
            });
        }
    }

    [ClientRpc]
    public override void RpcEndMiniGame()
    {
        foreach (var spleefChecker in spleefCheckers)
        {
            Destroy(spleefChecker.gameObject);
        }

        SpleefField[] fields = FindObjectsOfType<SpleefField>();
        if (fields.Length == 0)
        {
            VRDebugPanel.Instance.SendDebugMessage("No field found!");
        }
        foreach (var field in fields)
        {
            Destroy(field.gameObject);
        }

        spleefCheckers.Clear();
    }

    [Server]
    public override void StartMiniGame()
    {
        isFinished = false;
        result = new();

        playerDict.Clear();
        spleefCheckers.Clear();

        foreach (MiniGamePlayer player in FindObjectsOfType<MiniGamePlayer>())
        {
            playerDict.Add(player.PlayerID, player);

            // For server visuals
            SpleefChecker current = Instantiate(SpleefCheckerPrefab, player.transform.position, Quaternion.identity);
            current.myMiniGamePlayer = player;
            spleefCheckers.Add(current);
        }

        SpawnSpleefField();
    }

    [Command(requiresAuthority = false)]
    public override void CmdSendResult(GameResult result)
    {
        if (playerDict.ContainsKey(result.LoserID))
        {
            playerDict.Remove(result.LoserID);
        }

        if (playerDict.Count <= 1)
        {
            isFinished = true;
            foreach (var key in playerDict.Keys)
            {
                this.result = new()
                {
                    WinnerID = key
                };
            }
        }
    }

    [Server]
    public void SpawnSpleefField()
    {
        if (playerDict.Count == 0)
        {
            return;
        }

        activeField = Instantiate(SpleefFieldPrefab);
        NetworkServer.Spawn(activeField.gameObject);

        // Spawn field somewhere inbetween active players
        Vector3 averagePosition = Vector3.zero;
        foreach (var player in playerDict.Values)
        {
            averagePosition += player.transform.position;
        }

        averagePosition /= playerDict.Count;
        averagePosition.y = 0;
        activeField.transform.position = averagePosition;
    }

    [Server]
    public override void EndMiniGame()
    {
        if (activeField != null)
        {
            NetworkServer.Destroy(activeField.gameObject);
        }

        foreach (var spleefChecker in spleefCheckers)
        {
            Destroy(spleefChecker.gameObject);
        }

        spleefCheckers.Clear();
    }
}
