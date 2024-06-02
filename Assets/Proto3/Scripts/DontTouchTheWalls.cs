using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class DontTouchTheWalls : MiniGameBase
{
    [SerializeField] private PlayerDot PlayerDotPrefab;
    [SerializeField] private PathField[] pathfields;
    [SerializeField] private float fieldSpawnRange = 5f;

    private Dictionary<int, MiniGamePlayer> players; // Key: PlayerID, value: MiniGamePlayer

    private PathField currentPath;

    public override void StartMiniGame()
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (var player in FindObjectsOfType<MiniGamePlayer>())
        {
            players.Add(player.PlayerID, player);
            averagePosition += player.transform.position;
        }

        averagePosition /= players.Count;

        int random = Random.Range(0, pathfields.Length);
        Vector2 circlePoint = Random.insideUnitCircle.normalized * fieldSpawnRange;
        currentPath = Instantiate(pathfields[random], averagePosition + new Vector3(circlePoint.x, 0, circlePoint.y), Quaternion.identity);
        currentPath.transform.LookAt(averagePosition);
        
        for (int i = 0; i < players.Count; i++)
        {
            PlayerDot playerDot = Instantiate(PlayerDotPrefab, currentPath.StartPoint.position, Quaternion.identity, currentPath.transform);
            playerDot.SetRespawnPoint(currentPath.StartPoint.position);
        }

        NetworkServer.Spawn(currentPath.gameObject); // Does this also spawn the children that are instantiated in the for loop?
    }

    public override void EndMiniGame()
    {
        NetworkServer.Destroy(currentPath.gameObject);
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
