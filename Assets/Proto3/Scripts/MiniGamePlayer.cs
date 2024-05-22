using Mirror;
using TMPro;
using UnityEngine;

public class MiniGamePlayer : NetworkBehaviour
{
    public static int LocalPlayerID = 0;

    [SerializeField] public Transform RightHandTransform;

    [SerializeField] private TextMeshProUGUI playerIDText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SyncVar(hook = nameof(OnPlayerIDUpdated))] public int PlayerID;
    [SyncVar(hook = nameof(OnScoreUpdated))] public int Score;

    #region Event Handlers
    private void OnPlayerIDUpdated(int oldID, int newID)
    {
        playerIDText.text = $"Player {newID}";

        LocalPlayerID = newID;
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
}
