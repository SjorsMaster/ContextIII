using Mirror;
using TMPro;
using UnityEngine;

public class MiniGamePlayer : NetworkBehaviour
{
    public static MiniGamePlayer LocalPlayer;

    [SerializeField] private TextMeshProUGUI playerIDText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SyncVar(hook = nameof(OnPlayerIDUpdated))] public int PlayerID;
    [SyncVar(hook = nameof(OnScoreUpdated))] public int Score;

    #region Event Handlers
    private void OnPlayerIDUpdated(int oldID, int newID)
    {
        playerIDText.text = $"Player {newID}";
    }

    private void OnScoreUpdated(int oldScore, int newScore)
    {
        scoreText.text = newScore.ToString();
    }
    #endregion

    private void Awake()
    {
        syncDirection = SyncDirection.ServerToClient;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        LocalPlayer = this;
    }
}
