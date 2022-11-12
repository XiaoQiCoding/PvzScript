using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool gameStart;
    public static GameManager instance;
    public int sunNum;

    public GameObject bornParent;
    // public GameObject zombiePrefab;
    public float createZombieTime;
    private int zOrderIndex = 0;
    [HideInInspector]
    public LevelData levelData;
    // [HideInInspector]
    public List<GameObject> curProgressZombie;
    private int curLevelId = 1;
    private int curProgressId = 1;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ReadData();
    }

    private void GameStart()
    {
        UIManager.instance.InitUI();
        gameStart = true;
        CreateZombie();
        InvokeRepeating("CreateSunDown", 10, 10);
    }
    void ReadData()
    {
        StartCoroutine(LoadTable());
    }
    IEnumerator LoadTable()
    {
        ResourceRequest request = Resources.LoadAsync("Level");
        yield return request;
        levelData = request.asset as LevelData;
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;
        if (sunNum <= 0)
        {
            sunNum = 0;
        }
        // 阳光数量发生改变，通知卡片压黑等...
        UIManager.instance.UpdateUI();

    }

    public void CreateZombie()
    {
        // StartCoroutine(DalayCreateZombie());
        curProgressZombie = new List<GameObject>();
        TableCreateZombie();
    }

    // 表格创建僵尸
    private void TableCreateZombie()
    {
        // todo 选择关卡
        bool canCreate = false;
        for (int i = 0; i < levelData.LevelDataList.Count; i++)
        {
            LevelItem levelItem = levelData.LevelDataList[i];
            if (levelItem.levelId == curLevelId && levelItem.progressId == curProgressId)
            {
                canCreate = true;
                StartCoroutine(ITableCreateZombie(levelItem));
            }
        }
        if (!canCreate)
        {
            StopAllCoroutines();
            curProgressZombie = new List<GameObject>();
            // todo: 获得胜利之后的一些表现
            gameStart = false;
        }
    }

    IEnumerator ITableCreateZombie(LevelItem levelItem)
    {
        yield return new WaitForSeconds(levelItem.createTime);
        // 生成
        // GameObject zombie = Instantiate(zombiePrefab);
        GameObject zombiePrefab = Resources.Load("Prefab/Zombie" + levelItem.zombieType.ToString()) as GameObject;

        GameObject zombie = Instantiate(zombiePrefab);
        Transform zombieLine = bornParent.transform.Find("born" + levelItem.bornPos.ToString());
        zombie.transform.parent = zombieLine;
        zombie.transform.localPosition = Vector3.zero;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        zOrderIndex += 1;
        curProgressZombie.Add(zombie);
    }

    public void ZombieDied(GameObject gameObject)
    {
        if (curProgressZombie.Contains(gameObject))
        {
            curProgressZombie.Remove(gameObject);
        }
        // GameObject.Destroy(gameObject);
        if (curProgressZombie.Count == 0)
        {
            curProgressId++;
            TableCreateZombie();
        }
    }

    // // 等待一定时间后，在随机行生成一只僵尸
    // IEnumerator DalayCreateZombie()
    // {
    //     // 等待
    //     yield return new WaitForSeconds(createZombieTime);

    //     // 生成
    //     GameObject zombie = Instantiate(zombiePrefab);
    //     int index = Random.Range(0, 5);
    //     Transform zombieLine = bornParent.transform.Find("born" + index.ToString());
    //     zombie.transform.parent = zombieLine;
    //     zombie.transform.localPosition = Vector3.zero;
    //     zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
    //     zOrderIndex += 1;

    //     // 再次启动定时器
    //     StartCoroutine(DalayCreateZombie());
    // }

    public void CreateSunDown()
    {
        // 获取左下角、右上角的世界坐标
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);
        // 加载Sun预制件（另一种办法）
        GameObject sunPrefab = Resources.Load("Prefab/Sun") as GameObject;
        // 初始化太阳的位置
        float x = Random.Range(leftBottom.x + 30, rightTop.x - 30);
        Vector3 bornPos = new Vector3(x, rightTop.y, 0);
        GameObject sun = Instantiate(sunPrefab, bornPos, Quaternion.identity);
        // 设置目标位置
        float y = Random.Range(leftBottom.y + 100, leftBottom.y + 30);
        sun.GetComponent<Sun>().SetTargetPos(new Vector3(bornPos.x, y, 0));
    }
}
