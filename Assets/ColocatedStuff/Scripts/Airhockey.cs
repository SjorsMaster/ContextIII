using Mirror;
using TMPro;
using UnityEngine;

public class Airhockey : NetworkBehaviour
{
    [SerializeField] private Transform puckTransform;
    [SerializeField] private Rigidbody puckRigidbody;

    [SerializeField] private Transform puckSpawn;

    [SerializeField] private AirhockeyGoal leftGoal;
    [SerializeField] private AirhockeyGoal rightGoal;

    [SerializeField] private TMP_Text leftScoreText;
    [SerializeField] private TMP_Text rightScoreText;

    private int leftScore;
    private int rightScore;

    #region Event Handlers
    [Server]
    private void LeftGoal_OnGoal()
    {
        rightScore++;
        UpdateScore();
    }

    [Server]
    private void RightGoal_OnGoal()
    {
        leftScore++;
        UpdateScore();
    }
    #endregion

    [Server]
    private void OnEnable()
    {
        leftGoal.OnGoal += LeftGoal_OnGoal;
        rightGoal.OnGoal += RightGoal_OnGoal;
    }

    [Server]
    private void OnDisable()
    {
        leftGoal.OnGoal -= LeftGoal_OnGoal;
        rightGoal.OnGoal -= RightGoal_OnGoal;
    }

    [ServerCallback]
    public void BtnReset()
    {
        leftScore = 0;
        rightScore = 0;
        UpdateScore();
    }

    [Server]
    private void UpdateScore()
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightScore.ToString();

        puckRigidbody.velocity = Vector3.zero;
        puckTransform.SetPositionAndRotation(puckSpawn.position, Quaternion.identity);

        RpcUpdateScore(leftScore, rightScore);
    }

    [ClientRpc]
    private void RpcUpdateScore(int leftScore, int rightSCore)
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightSCore.ToString();
    }
}
