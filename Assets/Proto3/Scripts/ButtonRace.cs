using Mirror;
using SharedSpaces.Managers;

public class ButtonRace : MiniGameBase
{
    #region Event Handlers
    private void InputManager_OnRPrimaryPressed()
    {
        CmdSendResult(new()
        {
            WinnerID = MiniGamePlayer.LocalPlayerID
        });
    }
    #endregion

    [Server]
    public override void StartMiniGame()
    {
        isFinished = false;
        result = new();
    }

    [ClientRpc]
    public override void RpcStartMiniGame()
    {
        InputManager.Instance.RPrimaryPress += InputManager_OnRPrimaryPressed;
    }

    [ClientRpc]
    public override void RpcEndMiniGame()
    {
        InputManager.Instance.RPrimaryPress -= InputManager_OnRPrimaryPressed;
    }

    [Command(requiresAuthority = false)]
    public override void CmdSendResult(GameResult result)
    {
        this.result = result;
        isFinished = true;
    }
}
