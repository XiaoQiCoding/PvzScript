using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public int sunNum;

    public GameObject bornParent;
    public GameObject zombiePrefab;
    public float createZombieTime;
    private int zOrderIndex = 0;
    void Start()
    {
        instance = this;

        UIManager.instance.InitUI();

        CreateZombie();

        InvokeRepeating("CreateSunDown", 10, 10);
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
        StartCoroutine(DalayCreateZombie());
    }

    // 等待一定时间后，在随机行生成一只僵尸
    IEnumerator DalayCreateZombie()
    {
        // 等待
        yield return new WaitForSeconds(createZombieTime);

        // 生成
        GameObject zombie = Instantiate(zombiePrefab);
        int index = Random.Range(0, 5);
        Transform zombieLine = bornParent.transform.Find("born" + index.ToString());
        zombie.transform.parent = zombieLine;
        zombie.transform.localPosition = Vector3.zero;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        zOrderIndex += 1;

        // 再次启动定时器
        StartCoroutine(DalayCreateZombie());
    }

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
