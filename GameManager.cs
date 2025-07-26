using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public int curStage;
    public float distance;
    public bool hintType;
    [Header("# Player Info")]
    public int initGold = 10;
    int rewardStageGold;
    public int curGold;
    public int bulletCost = 5;
    public int curBullet;
    [Header("# Game Object")]
    public PoolManager pool;
    public PlayerMove player;
    public Spawner spawner;
    public Result uiResult;
    public GameResult uiGameResult;
    public RectTransform childToFlip;
    public Text disText;
    public Text goldText;
    public Wall wall;
    public Enemy enemy;
    public Border border;
    public StageMessage stageMessage;

    [Header("# Stage Info")]
    public List<StageConfig> stages;

    public enum Difficulty { Normal, Hard }
    public Difficulty selDifficult;

    void Awake()
    {
        //disText = GetComponent<Text>();
        instance = this;
    }

    public void OnDifficultySelected(int difficulty)
    {
        selDifficult = (Difficulty)difficulty;
        GameStart();
    }

    public void GameStart()
    {
        curGold = initGold;
        curBullet = 2;
        curStage = 0;
        isLive = true;

        LoadStage(curStage);
        wall.WallDisable();
        spawner.SpawnEnemy();
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiGameResult.gameObject.SetActive(true);
        uiGameResult.Lose();
        //Stop();
    }
    public void GameVictroy()
    {
        StartCoroutine(GameVictroyRoutine());
    }

    IEnumerator GameVictroyRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiGameResult.gameObject.SetActive(true);
        uiGameResult.Win();
        //Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    public void NextStage()
    {
        if (++curStage < stages.Count) {
            curBullet += 2;
            LoadStage(curStage);
            spawner.SpawnEnemy();
        }
        else {
            Debug.Log("���� �մϴ�!! ��� �������� �Ϸ�!");
            GameVictroy();
        }
    }

    void LoadStage(int idx)
    {
        player.curBulletStage = 0;
        StageConfig cfg = stages[idx];

        // 1) ��Ʈ  ����
        if (selDifficult == Difficulty.Normal) {
            if (cfg.hintType == HintType.DirectionOnly)
                hintType = false;
            else
                hintType = true;
        }
        else { hintType = false; }
        // 2) �ʻ����� ����
        border.ResizeMap(cfg);

        // 3) �� ��ũ �ݶ��̴� ������ ����
        enemy.ChangeTankSize(cfg);

        // 4) �� ���� ����
        wall.RandomMakeWall(cfg);
        stageMessage.ShowStageMessage(idx + 1);
        Debug.Log($"Stage {idx + 1} �ε�: {cfg.hintType}, Map={cfg.mapSize}, Tank={cfg.tankSize}, Wall={cfg.wallHeight}");
    }

    public bool TrySpendGold(int amount)
    {
        if (curGold < amount) return false;
        curGold -= amount;
        return true;
    }
    public void ToggleFlip(bool onOff)
    {
        var s = childToFlip.localScale;
        if (onOff) childToFlip.localScale = new Vector3(-s.x, s.y, s.z);
        else childToFlip.localScale = new Vector3(1, s.y, s.z);

    }
    public void BulletMiss()
    {
        StartCoroutine(Miss());
    }
    public void BulletWin()
    {
        GetCoin();
        StartCoroutine(Win());
        Invoke("NextStage", 1f);
    }

    void GetCoin()
    {
        //stage ���� 100���� �Ѿ� �߻� �� 10 ���� + stage �ܰ躰�� �߰� ��� ����
        rewardStageGold = (100 - ((player.curBulletStage-1) * 10)) + (((curStage) / 3) * 20);
        if (rewardStageGold < 0) rewardStageGold = 0;
        curGold += rewardStageGold;
        Debug.Log($"��� ȹ��: {rewardStageGold}");
    }
    IEnumerator Win()
    {
        GameManager.instance.player.isFire = true;
        goldText.text = string.Format("Gold Reward : {0:F0}", rewardStageGold);
        uiResult.gameObject.SetActive(true);
        uiResult.HitOn();
        yield return new WaitForSeconds(2f);
        uiResult.gameObject.SetActive(false);
        uiResult.HitOff();
    }
    IEnumerator Miss()
    {
        GameManager.instance.player.isFire = true;
        if (curGold <= 0) { GameOver(); }
        else {
            if (hintType) { disText.text = string.Format("{0:F2}m", Mathf.Abs(distance)); }
            else { disText.text = null;  }

            if (distance < 0) {
                ToggleFlip(true);
            }

            uiResult.gameObject.SetActive(true);
            uiResult.MissOn();
            yield return new WaitForSeconds(2f);
            uiResult.gameObject.SetActive(false);
            uiResult.MissOff();
            //���� �ʱ�ȭ
            ToggleFlip(false);
        }
    }
    public void CheckDistances(Vector3 explosionPos)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies) {
            Vector3 enemyPos = enemy.transform.position;
            distance = (enemyPos.x - explosionPos.x);

            BulletMiss();
            Debug.Log($"[�� ����] {enemy.name} - X�� �Ÿ� & ����: {distance}");
        }
    }
}

