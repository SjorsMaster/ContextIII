using Mirror;

public interface IMiniGame
{
    string Title { get; }
    string Description { get; }
    int MaxDuration { get; }
    bool IsFinished { get; }
    GameResult Result { get; }

    [Server]
    void StartMiniGame();

    [ClientRpc]
    void RpcStartMiniGame();

    [Command(requiresAuthority = false)]
    void CmdSendResult(GameResult result);

    [ClientRpc]
    void RpcEndMiniGame();
}
