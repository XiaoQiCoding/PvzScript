using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Plant
{
    // Start is called before the first frame update
    public GameObject sunPrefab;
    public float readyTime;
    private float timer;
    private int sunNum;
    protected override void Start()
    {
        base.Start();
        timer = 0;
        sunNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(! start)
            return;
        timer += Time.deltaTime;
        if (timer > readyTime)
        {
            animator.SetBool("Ready", true);
        }
    }

    public void BornSunOver()
    {
        // 太阳花变亮结束生成太阳
        BornSun();
        animator.SetBool("Ready", false);
        timer = 0;
    }

    private void BornSun()
    {
        GameObject sunNew = Instantiate(sunPrefab);
        sunNum += 1;
        float randomX;
        // 奇数太阳在左边生成
        if (sunNum % 2 == 1)
        {
            randomX = Random.Range(transform.position.x - 30, transform.position.x - 20);
        }
        // 偶数太阳在右边生成
        else
        {
            randomX = Random.Range(transform.position.x + 20, transform.position.x + 30);
        }
        float randomY = Random.Range(transform.position.y - 20, transform.position.y + 20);
        sunNew.transform.position = new Vector2(randomX, randomY);
    }
}
