using Mirror;
using System;
using UnityEngine;

public class ScoreCounter : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnScoreUpdated))] private int score;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdIncrementScore();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdIncrementScore()
    {
        score++;
    }

    [Client]
    private void OnScoreUpdated(int oldScore, int newScore)
    {
        Debug.Log($"Score updated: {oldScore} -> {newScore}");

        // Do something with the new score
    }


}
