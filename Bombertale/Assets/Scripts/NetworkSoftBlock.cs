using UnityEngine;
using System.Collections;

public class NetworkSoftBlock : MonoBehaviour {
    private Mapper map;
    private int xLoc, yLoc;
    private ServerGameManager _serverGameManager;

    void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Mapper>();
        xLoc = (int)this.transform.position.x;
        yLoc = (int)this.transform.position.y;
        _serverGameManager = GameObject.Find("GameManager").GetComponent<ServerGameManager>();
    }

    public void GetRekt()
    {
        if (_serverGameManager != null)
        {
            //Notify serverGameManager that I got rekt
            _serverGameManager.WrekSoftBlock(this.xLoc, this.yLoc);
        }
    }

    public void Fizzle()
    {
        //Play Animations
        this.GetComponent<Animator>().Play("SoftBlockFizzle");
        float animTime = .5f;
        StartCoroutine(this.Destroy(animTime));
    }

    private IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        if (_serverGameManager != null)
        {
            float rand = Random.Range(0, 100);
            int randPU = Random.Range(0, 4);
            if (rand >= 30)
            {
                _serverGameManager.charMap[xLoc][yLoc] = randPU.ToString()[0];
            }
            else
            {
                _serverGameManager.charMap[xLoc][yLoc] = CellID.Empty;
            }
            _serverGameManager._serverManager.SendAll(MessageType.StateUpdate, new StateUpdate(_serverGameManager.charMap, _serverGameManager.clientToPlayer, Time.time));
        }
        //Destroy(this.gameObject);
        //map.grid[xLoc][yLoc] = ".";
    }
}
