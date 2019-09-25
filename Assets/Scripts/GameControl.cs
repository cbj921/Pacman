using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    // UI部分
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameOver;
    public GameObject win;
    public GameObject countStart;
    public GameObject restartButton;
    public Text pacDotText;
    public Text nowEatText;
    public Text scoreText;
    // 音乐
    public AudioClip startClip;
    public AudioClip BGM;

    private int pacDotNum = 0;
    private int nowEat = 0;
    public int score = 0;

    private void Awake()
    {
        // 单例模式
        _instance = this;

        foreach (Transform item in GameObject.Find("Maze").transform)
        {
            pacDotList.Add(item.gameObject);
        }

        // 初始的时候先暂停游戏
        SetState(false);

        // 获取初始豆子数
        pacDotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void Update()
    {
        // 更新UI数据
        UpdateUI();
        // 判断胜利条件
        JudgeWin();
    }

    public void UpdateUI()
    {
        if (gamePanel.activeInHierarchy)
        {
            pacDotText.text = "Remain:\n\n" + (pacDotNum - nowEat);
            nowEatText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }

    private void CreatSuperDot()
    {
        int tempIndex = Random.Range(0, pacDotList.Count);
        GameObject superDot = pacDotList[tempIndex];
        superDot.transform.localScale = new Vector3(3, 3, 3);
        superDot.GetComponent<Pacdot>().isSuperDot = true;
        score += 50;
    }

    public void OnEatPacDot(GameObject pacDot)
    {
        pacDotList.Remove(pacDot);
        // 吃掉豆子后
        nowEat++;
        score += 10;
    }

    public void OnEatSuperDot(GameObject pacDot)
    {
        // 当豆子小于10个的时候就不再生成超级豆子
        if (pacDotList.Count < 20)
        {
            return;
        }
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

    public void SetState(bool state)
    {
        pacMan.GetComponent<Pacman>().enabled = state;
        redGhost.GetComponent<ghostMove>().enabled = state;
        blueGhost.GetComponent<ghostMove>().enabled = state;
        yellowGhost.GetComponent<ghostMove>().enabled = state;
        pinkGhost.GetComponent<ghostMove>().enabled = state;
    }

    // UI部分
    public void OnClickStart()
    {
        countStart.SetActive(true);
        startPanel.SetActive(false);
        AudioSource.PlayClipAtPoint(startClip, Camera.main.transform.position);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

    // 播放完倒计时后调用
    public void StartGame()
    {
        gamePanel.SetActive(true);
        countStart.SetActive(false);
        SetState(true);
        // 播放BGM
        GetComponent<AudioSource>().Play();
        // 开局5秒生成超级豆子
        Invoke("CreatSuperDot", 5.0f);

    }
    // 游戏结束时调用
    public void GameOver()
    {
        // 让gameover显示
        gameOver.SetActive(true);
        restartButton.SetActive(true);
        //SetState(false);
    }

    // 游戏胜利时调用
    public void GameWin()
    {
        win.SetActive(true);
        restartButton.SetActive(true);
        SetState(false);
    }

    // 判断是否胜利 
    public void JudgeWin()
    {
        if (nowEat == pacDotNum && pacMan.GetComponent<Pacman>().enabled != false)
        {
            GameWin();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
