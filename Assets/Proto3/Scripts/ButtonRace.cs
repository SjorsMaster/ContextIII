using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRace : MiniGameBase
{
    [SerializeField] private ButtonPrefab buttonPrefab;

    [SerializeField] private int pointsToWin = 3;
    [SerializeField] private float buttonDespawnTime = 5f;
    [SerializeField] private float buttonSpawnRange = 5f;

    private readonly Dictionary<MiniGamePlayer, int> Players = new(); // Key: Player, Value: Points, server only

    private bool buttonPressed = false;

    #region Event Handlers
    private void OnButtonPressed(MiniGamePlayer player)
    {
        if (Players.ContainsKey(player))
        {
            Players[player]++;

            if (Players[player] == pointsToWin)
            {
                result = new()
                {
                    WinnerID = player.PlayerID
                };

                isFinished = true;
            }
            buttonPressed = true;
        }
    }
    #endregion

    [Server]
    public override void StartMiniGame()
    {
        isFinished = false;
        result = new();

        Players.Clear();
        foreach (MiniGamePlayer player in FindObjectsOfType<MiniGamePlayer>())
        {
            Players.Add(player, 0);
        }

        StartCoroutine(SpawnButton());
    }

    [ClientRpc]
    public override void RpcStartMiniGame()
    {
    }

    [Server]
    private IEnumerator SpawnButton()
    {
        buttonPressed = false;
        float timer = Time.time + buttonDespawnTime;
        ButtonPrefab button = Instantiate(buttonPrefab);
        button.OnButtonPressed += OnButtonPressed;

        NetworkServer.Spawn(button.gameObject);

        Vector3 averagePosition = Vector3.zero;
        foreach (var player in Players.Keys)
        {
            averagePosition += player.transform.position;
        }

        averagePosition /= Players.Count;
        averagePosition.y = 0;
        button.transform.position = new Vector3(
            averagePosition.x + Random.Range(-buttonSpawnRange, buttonSpawnRange),
            averagePosition.y + Random.Range(0f, 0.5f),
            averagePosition.z + Random.Range(-buttonSpawnRange, buttonSpawnRange));

        while (Time.time < timer && !buttonPressed && !isFinished)
        {
            yield return null;
        }

        NetworkServer.Destroy(button.gameObject);

        if (!isFinished)
        {
            StartCoroutine(SpawnButton());
        }
    }

    [ClientRpc]
    public override void RpcEndMiniGame()
    {
    }

    [Command(requiresAuthority = false)]
    public override void CmdSendResult(GameResult result)
    {
    }

    [Server]
    public override void EndMiniGame()
    {
        isFinished = true;
    }
}
