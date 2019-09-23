using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    private static GameControl _instance;
    public static GameControl Instance
    {
        get
        {
            return _instance;
        }
    }

    public bool isSuperPacman = false;
    public GameObject pacMan;
    public GameObject redGhost;
    public GameObject blueGhost;
    public GameObject yellowGhost;
    public GameObject pinkGhost;
    private List<GameObject> pacDotList = new List<GameObject>();

    private void Awake()
    {
        // 单例模式
        _instance = this;

        foreach (Transform item in GameObject.Find("Maze").transform)
        {
            pacDotList.Add(item.gameObject);
        }
    }

    private void Start()
    {
        // 开局十秒生成超级豆子
        Invoke("CreatSuperDot", 5.0f);
    }

    private void CreatSuperDot()
    {
        int tempIndex = Random.Range(0, pacDotList.Count);
        GameObject superDot = pacDotList[tempIndex];
        superDot.transform.localScale = new Vector3(3, 3, 3);
        superDot.GetComponent<Pacdot>().isSuperDot = true;
    }

    public void OnEatPacDot(GameObject pacDot)
    {
        pacDotList.Remove(pacDot);
    }

    public void OnEatSuperDot(GameObject pacDot)
    {
        OnEatPacDot(pacDot);
        FreezeEnemy();
        Invoke("CreatSuperDot", 10.0f);
        Invoke("RecoverEnemy", 3.0f);
		// 上面这句话还可以采用协程的写法,但在这里没必要
    }

    private void FreezeEnemy()
    {
        isSuperPacman = true;
        // 失能脚本，只会让update方法失效，其他的不会影响
        redGhost.GetComponent<ghostMove>().enabled = false;
        blueGhost.GetComponent<ghostMove>().enabled = false;
        yellowGhost.GetComponent<ghostMove>().enabled = false;
        pinkGhost.GetComponent<ghostMove>().enabled = false;

        // 改变冻结后的sprite
        redGhost.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        blueGhost.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        yellowGhost.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        pinkGhost.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);

    }

    private void RecoverEnemy()
    {
        isSuperPacman = false;
        // 使能脚本，只会让update方法失效，其他的不会影响
        redGhost.GetComponent<ghostMove>().enabled = true;
        blueGhost.GetComponent<ghostMove>().enabled = true;
        yellowGhost.GetComponent<ghostMove>().enabled = true;
        pinkGhost.GetComponent<ghostMove>().enabled = true;

        // 改变恢复后的sprite
        redGhost.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        blueGhost.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yellowGhost.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        pinkGhost.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
}
