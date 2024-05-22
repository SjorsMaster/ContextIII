using Mirror;
using SharedSpaces.Managers;
using SharedSpaces.Menus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : NetworkBehaviour
{
    [SerializeField] private int scoreToWin;

    [SerializeField] private List<MiniGameBase> miniGames;

    [SerializeField] private GameMasterUI gameMasterUI;

    [SyncVar(hook = nameof(OnTimerUpdated))] private int timer;

    private Dictionary<int, MiniGamePlayer> players = new(); // PlayerID, Player, server only

    private IMiniGame currentGame; // Server only

    #region Event Handlers
    private void OnTimerUpdated(int oldTime, int newTime)
    {
        gameMasterUI.UpdateTimer(newTime);
    }
    #endregion

    [Command(requiresAuthority = false)]
    public void CmdStart()
    {
        players.Clear();
        foreach (MiniGamePlayer player in FindObjectsOfType<MiniGamePlayer>())
        {
            player.PlayerID = players.Count + 1;
            players.Add(player.PlayerID, player);
            player.Score = 0;
        }

        StartRandomGame();
        RpcCloseCurrentMenu();
    }

    [ClientRpc]
    private void RpcCloseCurrentMenu()
    {
        MenuManager.Instance.CloseCurrentMenu();
    }

    [ClientRpc]
    private void RpcShowMenu()
    {
        MenuManager.Instance.ShowMenu<MainMenu>();
    }

    [Server]
    private void StartRandomGame()
    {
        int randomGameIndex = Random.Range(0, miniGames.Count);
        currentGame = miniGames[randomGameIndex];
        StartCoroutine(PreGameRoutine(currentGame));
    }

    [Server]
    private IEnumerator PreGameRoutine(IMiniGame miniGame)
    {
        gameMasterUI.RpcPreGame(new()
        {
            Title = miniGame.Title,
            Description = miniGame.Description,
            MaxDuration = miniGame.MaxDuration,
        });
        yield return new WaitForSeconds(3);

        StartCoroutine(StartGameRoutine(miniGame.MaxDuration));
    }

    [Server]
    private IEnumerator StartGameRoutine(int duration)
    {
        gameMasterUI.RpcStartGame();

        currentGame.StartMiniGame();
        currentGame.RpcStartMiniGame();

        timer = duration;
        while (timer > 0 && !currentGame.IsFinished)
        {
            yield return new WaitForSeconds(1);
            timer--;
        }

        StartCoroutine(PostGameRoutine(currentGame.Result));
    }

    [Server]
    private IEnumerator PostGameRoutine(GameResult result)
    {
        currentGame.RpcEndMiniGame();
        currentGame.EndMiniGame();
        gameMasterUI.RpcPostGame(result);
        yield return new WaitForSeconds(3);

        bool isFinished = false;
        if (players.TryGetValue(currentGame.Result.WinnerID, out MiniGamePlayer winner))
        {
            winner.Score++;
            if (winner.Score >= scoreToWin)
            {
                isFinished = true;
                gameMasterUI.RpcFinished(winner.PlayerID);
                RpcShowMenu();
            }
        }

        if (!isFinished)
        {
            StartRandomGame();
        }
    }
}
