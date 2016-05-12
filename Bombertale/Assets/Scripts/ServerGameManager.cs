using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerGameManager : MonoBehaviour
{

    public Dictionary<int, NetworkPlayer> clientToPlayer = new Dictionary<int, NetworkPlayer>();
    public Mapper map;

    private ServerManager _serverManager;
    private ClientManager _clientManager;
    private List<NetworkPlayer> _playerList = new List<NetworkPlayer>();

    void Awake()
    {
        for (int i = 1; i <= 4; i++)
            _playerList.Add(GameObject.Find("Player" + i).GetComponent<NetworkPlayer>());
        map = GameObject.Find("Map").GetComponent<Mapper>();
        GameObject serverObject = GameObject.Find("NetworkManager");
        _clientManager = serverObject.GetComponent<ClientManager>();
        _serverManager = serverObject.GetComponent<ServerManager>();
        _serverManager.serverGameManager = this;
    }

    void Start()
    {
        for (int i = 0; i < _serverManager.clientList.Count && i < 4; i++)
        {
            clientToPlayer.Add(_serverManager.clientList[i], _playerList[i]);
            clientToPlayer[_serverManager.clientList[i]].data.userName = _serverManager.clientUsernames[i];
        }

        _serverManager.SendSetup();
    }

    void LateUpdate()
    {
        CheckGameOver();
        //foreach (NetworkPlayer player in clientToPlayer.Values)
        //{
        //    MovePlayer(player);
        //}
    }

    public void SetPlayerDirection(int clientID, Direction direction)
    {
        clientToPlayer[clientID].data.direction = direction;
    }

    public void TriggerUpdate(int clientID, TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.Explosion:
                if (!clientToPlayer[clientID].data.isInvulnerable)
                    clientToPlayer[clientID].data.isAlive = false;
                break;
            case TriggerType.SpeedUp:
                clientToPlayer[clientID].data.speed += .5f;
                break;
            case TriggerType.BombCount:
                clientToPlayer[clientID].data.bombCount++;
                break;
            case TriggerType.ExplosionRadius:
                clientToPlayer[clientID].data.explosionRadius++;
                break;
            case TriggerType.Determination:
                clientToPlayer[clientID].data.isInvulnerable = true;
                clientToPlayer[clientID].data.invulnTimeRemaining = 5f;
                break;
            default:
                break;
        }
    }

    public bool DropBomb(int clientID)
    {
        NetworkPlayer player = clientToPlayer[clientID];
        if (player.data.bombCount > 0)
        {
            player.data.bombCount--;
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

    public void WrekSoftBlock(int xLoc, int yLoc)
    {
        _serverManager.SendAll(MessageType.DestroySoftBlock, new DestroySoftBlock(xLoc, yLoc));
        float rand = Random.Range(0, 100);
        int randPU = Random.Range(0, _clientManager.powerUps.Length);
        if (rand >= 60)
        {
            _serverManager.SendAll(MessageType.PowerUpDrop, new PowerUpDrop(randPU, xLoc, yLoc));
        }
        else
        {
            _serverManager.SendAll(MessageType.PowerUpDrop, new PowerUpDrop(-1, xLoc, yLoc));
        }
    }

    private void CheckGameOver()
    {
        List<NetworkPlayer> alivePlayers = new List<NetworkPlayer>();

        foreach (NetworkPlayer player in _playerList)
        {
            if (player.data.isAlive)
                alivePlayers.Add(player);
            if (alivePlayers.Count >= 2)
                return;
        }

        if (alivePlayers.Count == 1)
            _serverManager.SendAll(MessageType.GameOver, new GameOver(alivePlayers[0].data.userName + " Wins!"));
        else if (alivePlayers.Count == 0)
            _serverManager.SendAll(MessageType.GameOver, new GameOver("Tie!"));
    }
}