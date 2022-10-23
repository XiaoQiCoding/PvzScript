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
    void Start()
    {
        instance = this;

        UIManager.instance.InitUI();

        CreateZombie();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSunNum(int changeNum){
        sunNum += changeNum;
        if(sunNum <= 0)
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
        
        // 再次启动定时器
        StartCoroutine(DalayCreateZombie());
    }
}
