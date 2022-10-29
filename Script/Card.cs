using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public GameObject objectPrefab;  // 卡片对应的物体预制件
    private GameObject curGameObject;  // 记录当前创建出来的物体
    private GameObject darkBg;
    private GameObject progressBar;
    public float waitTime;
    public int useSun;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        // 另一种获取物体的方式：获取子物体
        darkBg = transform.Find("dark").gameObject;
        progressBar = transform.Find("progress").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }

    void UpdateDarkBg()
    {
        // 倒计时结束，并且太阳数量足够
        if (progressBar.GetComponent<Image>().fillAmount == 0 && GameManager.instance.sunNum >= useSun)
        {
            darkBg.SetActive(false);
        }
        else
        {
            darkBg.SetActive(true);
        }
    }



    // 拖拽开始（鼠标点下的一瞬间）
    public void OnBeginDrag(BaseEventData data)
    {
        // 判断是否可以种植, 压黑存在则无法种植
        if(darkBg.activeSelf)
        {
            return;
        }
        Debug.Log("OnBeginDrag" + data.ToString());
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        curGameObject.transform.position = TranlateScreenToWorld(pointerEventData.position);
    }

    // 拖拽过程（鼠标按着没放开）
    public void OnDrag(BaseEventData data)
    {
        if(curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        // 根据鼠标移动的位置对应移动物体
        curGameObject.transform.position = TranlateScreenToWorld(pointerEventData.position);

    }

    // 拖拽结束（鼠标放开的一瞬间）
    public void OnEndDrag(BaseEventData data)
    {
        Debug.Log("OnEndDrag" + data.ToString());
        if(curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        // 拿到鼠标所在位置的碰撞体
        Collider2D [] col = Physics2D.OverlapPointAll(TranlateScreenToWorld(pointerEventData.position));
        // 遍历碰撞体
        foreach(Collider2D c in col)
        {
            // 判断物体为“土地”，并且土地上没有其他植物
            if(c.tag == "Land" && c.transform.childCount == 0)
            {
                // 把当前物体添加为土地的子物体
                curGameObject.transform.parent = c.transform;
                curGameObject.transform.localPosition = Vector3.zero;
                // 启动植物
                curGameObject.GetComponent<Plant>().SetPlantStart();
                // 重置默认值，生成结束
                curGameObject = null;
                GameManager.instance.ChangeSunNum(-useSun);
                timer = 0;
                break;
            }
        }
        // 如果没有符合条件的土地，则curGameObject还存在着，那么销毁他.
        if(curGameObject != null)
        {
            GameObject.Destroy(curGameObject);
            curGameObject = null;
        }
    }

    public static Vector3 TranlateScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }

}
