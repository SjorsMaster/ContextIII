using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameMasterUI : NetworkBehaviour
{
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject preGamePanel;
    [SerializeField] private TextMeshProUGUI gameTitle;
    [SerializeField] private TextMeshProUGUI gameDescription;
    [SerializeField] private TextMeshProUGUI gameDuration;

    [SerializeField] private GameObject postGamePanel;
    [SerializeField] private TextMeshProUGUI GameResultText;

    [SerializeField] private GameObject FinishedPanel;
    [SerializeField] private TextMeshProUGUI FinishedText;

    private GameObject ActivePanel;

    private void SwitchPanel(GameObject newPanel)
    {
        if (ActivePanel != null)
        {
            ActivePanel.SetActive(false);
        }

        ActivePanel = newPanel;
        ActivePanel.SetActive(true);
    }

    public void UpdateTimer(int time)
    {
        timerText.text = $"Time left: {time}";
    }

    [ClientRpc]
    public void RpcPreGame(MiniGameData miniGame)
    {
        SwitchPanel(preGamePanel);
        gameTitle.text = miniGame.Title;
        gameDescription.text = miniGame.Description;
        gameDuration.text = $"Max duration: {miniGame.MaxDuration}";
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        SwitchPanel(timerPanel);
    }

    [ClientRpc]
    public void RpcPostGame(GameResult result)
    {
        SwitchPanel(postGamePanel);

        if (result.WinnerID == 0)
        {
            GameResultText.text = "No winner this time!";
        }
        else
        {
            GameResultText.text = $"Player {result.WinnerID} wins!";
        }
    }

    [ClientRpc]
    public void RpcFinished(int winnerID)
    {
        SwitchPanel(FinishedPanel);
        FinishedText.text = $"Player {winnerID} wins the game!";

        CloseAfter(3);
    }

    private IEnumerator CloseAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        ActivePanel.SetActive(false);
    }
}
