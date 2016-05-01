using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerGameManager : MonoBehaviour
{

    public Dictionary<int, NetworkPlayer> clientToPlayer = new Dictionary<int, NetworkPlayer>();

    private ServerManager _serverManager;
    private List<NetworkPlayer> _playerList = new List<NetworkPlayer>();

    void Awake()
    {
        for (int i = 1; i <= 4; i++)
            _playerList.Add(GameObject.Find("Player" + i).GetComponent<NetworkPlayer>());
        _serverManager = GameObject.Find("ServerManager").GetComponent<ServerManager>();
        _serverManager.serverGameManager = this;
    }

    void Start()
    {
        for (int i = 0; i < _serverManager.clientList.Count && i < 4; i++)
        {
            clientToPlayer.Add(_serverManager.clientList[i], _playerList[i]);
        }

        _serverManager.SendSetup();
    }

    void LateUpdate()
    {
        //foreach (NetworkPlayer player in clientToPlayer.Values)
        //{
        //    MovePlayer(player);
        //}
    }

    public void SetPlayerDirection(int clientID, Direction direction)
    {
        clientToPlayer[clientID].data.direction = direction;
    }

    public bool DropBomb(int clientID)
    {
        NetworkPlayer player = clientToPlayer[clientID];
        if (player.data.bombCount > 0)
        {
            player.data.bombCount--;
            //Invoke("RefillBombCount", 2.5f);
            StartCoroutine(RefillBombCount(clientID, 2.5f));            
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator RefillBombCount(int clientID, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        clientToPlayer[clientID].data.bombCount++;
    }

    //private void MovePlayer(NetworkPlayer player)
    //{
    //    switch (player.data.direction)
    //    {
    //        case Direction.UP:
    //            player.transform.Translate(new Vector2(0, player.data.speed * Time.deltaTime));
    //            break;
    //        case Direction.LEFT:
    //            player.transform.Translate(new Vector2(-player.data.speed * Time.deltaTime, 0));
    //            break;
    //        case Direction.DOWN:
    //            player.transform.Translate(new Vector2(0, -player.data.speed * Time.deltaTime));
    //            break;
    //        case Direction.RIGHT:
    //            player.transform.Translate(new Vector2(player.data.speed * Time.deltaTime, 0));
    //            break;
    //        default:
    //            break;
    //    }
    //}
}