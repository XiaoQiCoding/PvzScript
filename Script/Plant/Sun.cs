using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration;
    private float timer;
    public Vector3 targetPos;
    void Start()
    {
        timer = 0;
    }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
    }

    // Update is called once per frame
    void Update()
    {
        // 先移动到落点
        if(targetPos != Vector3.zero && Vector3.Distance(targetPos, transform.position) > 0.1f){
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.1f);
            return;
        }
        // 再延时销毁
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
