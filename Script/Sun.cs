using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration;
    private float timer;
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        print("OnMouseDown: Sun");
        // TODO: 飞到UI太阳所在位置，然后销毁
        GameObject.Destroy(gameObject);
        // 点击后：增加阳光数量
        GameManager.instance.ChangeSunNum(25);
    }
}
