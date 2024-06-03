using Mirror;
using SharedSpaces.Managers;
using SharedSpaces.NetorkMessages;
using System.Collections.Generic;
using UnityEngine;

public class DontTouchTheWalls : MiniGameBase
{
    [SerializeField] private PlayerDot PlayerDotPrefab;
    [SerializeField] private PathField[] pathfields;
    [SerializeField] private float fieldSpawnRange = 5f;

    private readonly Dictionary<int, MiniGamePlayer> players = new(); // Key: PlayerID, value: MiniGamePlayer

    private PathField currentPath;

    #region Event Handlers
    private void OnPlayerFinishedPath(PlayerDot dot)
    {
        if (players.TryGetValue(dot.OwnerID, out MiniGamePlayer player))
        {
            result = new()
            {
                WinnerID = player.PlayerID
            };
            isFinished = true;
        }
    }

    private void OnPlayerCollidedWithPath(PlayerDot dot)
    {
        NetworkServer.Destroy(dot.gameObject);

        GameObject playerDotObject = Instantiate(PlayerDotPrefab.gameObject, currentPath.PlayerDotParent);
        playerDotObject.transform.SetPositionAndRotation(currentPath.StartPoint.position, currentPath.StartPoint.rotation);
        NetworkServer.Spawn(playerDotObject);

        PlayerDot playerDot = playerDotObject.GetComponent<PlayerDot>();
        playerDot.SpawnTransform = currentPath.StartPoint;

        NetworkServer.SendToAll(new MsgSetParentMessage()
        {
            ChildNetId = playerDot.netId,
            ParentNetId = currentPath.GetComponent<NetworkIdentity>().netId
        });
    }
    #endregion

    public override void StartMiniGame()
    {
        isFinished = false;
        result = new();
        players.Clear();

        Vector3 averagePosition = Vector3.zero;
        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            players.Add(player.PlayerID, player);
            averagePosition += player.transform.position;
        }

        averagePosition /= players.Count;
        averagePosition.y = 0;

        int random = Random.Range(0, pathfields.Length);
        Vector2 circlePoint = Random.insideUnitCircle.normalized * fieldSpawnRange;
        GameObject pathObject = Instantiate(pathfields[random].gameObject, averagePosition + new Vector3(circlePoint.x, 0, circlePoint.y), Quaternion.identity);
        pathObject.transform.LookAt(averagePosition);
        NetworkServer.Spawn(pathObject);

        currentPath = pathObject.GetComponent<PathField>();

        for (int i = 0; i < players.Count; i++)
        {
            GameObject playerDotObject = Instantiate(PlayerDotPrefab.gameObject, currentPath.PlayerDotParent);
            playerDotObject.transform.SetPositionAndRotation(currentPath.StartPoint.position, currentPath.StartPoint.rotation);
            NetworkServer.Spawn(playerDotObject);

            PlayerDot playerDot = playerDotObject.GetComponent<PlayerDot>();
            playerDot.SpawnTransform = currentPath.StartPoint;

            NetworkServer.SendToAll(new MsgSetParentMessage()
            {
                ChildNetId = playerDot.netId,
                ParentNetId = currentPath.GetComponent<NetworkIdentity>().netId
            });
        }

        currentPath.OnPlayerFinishedPath += OnPlayerFinishedPath;
        currentPath.OnPlayerCollidedWithPath += OnPlayerCollidedWithPath;
    }

    public override void EndMiniGame()
    {
        NetworkServer.Destroy(currentPath.gameObject);
        currentPath = null;
        players.Clear();

        isFinished = true;
    }

    public override void RpcStartMiniGame()
    {
    }

    public override void RpcEndMiniGame()
    {
    }

    public override void CmdSendResult(GameResult result)
    {
    }
}
