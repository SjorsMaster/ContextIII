using Mirror;
using UnityEngine;

public abstract class MiniGameBase : NetworkBehaviour, IMiniGame
{
    public string Title => title;
    public string Description => description;
    public int MaxDuration => maxDuration;
    public bool IsFinished => isFinished;
    public GameResult Result => result;

    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int maxDuration;

    protected bool isFinished;
    protected GameResult result;

    public abstract void StartMiniGame();
    public abstract void EndMiniGame();
    public abstract void RpcStartMiniGame();
    public abstract void RpcEndMiniGame();
    public abstract void CmdSendResult(GameResult result);

}
