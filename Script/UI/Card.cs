using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectPrefab;  // 卡片对应的物体预制件
    private GameObject curGameObject;  // 记录当前创建出来的物体
    private GameObject darkBg;
    private GameObject progressBar;
    public float waitTime;
    public int useSun;
    private float timer;
    public PlantInfoItem plantInfo;
    public bool hasUse = false;
    public bool hasLock = false;
    public bool isMoving = false;
    public bool hasStart = false;
    // Start is called before the first frame update
    void Start()
    {
        // 另一种获取物体的方式：获取子物体
        darkBg = transform.Find("dark").gameObject;
        progressBar = transform.Find("progress").gameObject;

        darkBg.SetActive(false);
        progressBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameStart)
        {
            return;
        }
        if (!hasStart)
        {
            hasStart = true;
            darkBg.SetActive(true);
            progressBar.SetActive(true);
        }
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
        if (!hasStart)
        {
            return;
        }
        // 判断是否可以种植, 压黑存在则无法种植
        if (darkBg.activeSelf)
        {
            return;
        }
        Debug.Log("OnBeginDrag" + data.ToString());
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        curGameObject.transform.position = TranlateScreenToWorld(pointerEventData.position);
        // 播放点击卡片的声音
        SoundManager.instance.PlaySound(Globals.S_Seedlift);
    }

    // 拖拽过程（鼠标按着没放开）
    public void OnDrag(BaseEventData data)
    {
        if (curGameObject == null)
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
        if (curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        // 拿到鼠标所在位置的碰撞体
        Collider2D[] col = Physics2D.OverlapPointAll(TranlateScreenToWorld(pointerEventData.position));
        // 遍历碰撞体
        foreach (Collider2D c in col)
        {
            // 判断物体为“土地”，并且土地上没有其他植物
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                // 把当前物体添加为土地的子物体
                curGameObject.transform.parent = c.transform;
                curGameObject.transform.localPosition = Vector3.zero;
                // 启动植物
                curGameObject.GetComponent<Plant>().SetPlantStart();
                // 播放种植到土地上的声音
                SoundManager.instance.PlaySound(Globals.S_Plant);
                // 重置默认值，生成结束
                curGameObject = null;
                GameManager.instance.ChangeSunNum(-useSun);
                timer = 0;
                break;
            }
        }
        // 如果没有符合条件的土地，则curGameObject还存在着，那么销毁他.
        if (curGameObject != null)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMoving)
            return;
        if (hasLock)
            return;
        if (hasUse)
        {
            RemoveCard(gameObject);
        }
        else
        {
            AddCard(gameObject);
        }
    }

    public void RemoveCard(GameObject removeCard)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
        if (chooseCardPanel.ChooseCard.Contains(removeCard))
        {
            // 移除操作
            removeCard.GetComponent<Card>().isMoving = true;
            chooseCardPanel.ChooseCard.Remove(removeCard);
            chooseCardPanel.UpdateCardPosition();
            // 移动回到原来的位置
            Transform cardParent = UIManager.instance.allCardPanel.Bg.transform.Find("Card" + removeCard.GetComponent<Card>().plantInfo.plantId);
            Vector3 curPosition = removeCard.transform.position;
            removeCard.transform.SetParent(UIManager.instance.transform, false);
            removeCard.transform.position = curPosition;
            // DOMove
            removeCard.transform.DOMove(cardParent.position, 0.3f).OnComplete(
                () =>
                {
                    // hasLock = false;
                    // darkBg.SetActive(false);
                    cardParent.Find("BeforeCard").GetComponent<Card>().darkBg.SetActive(false);
                    cardParent.Find("BeforeCard").GetComponent<Card>().hasLock = false;
                    removeCard.GetComponent<Card>().isMoving = false;
                    Destroy(removeCard);
                }
            );
        }

    }

    public void AddCard(GameObject gameObject)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
        int curIndex = chooseCardPanel.ChooseCard.Count;
        if (curIndex >= 8)
        {
            // Destroy(useCard);
            print("已经选中的卡片超过最大数量");
            return;
        }
        GameObject useCard = Instantiate(plantInfo.cardPrefab);
        useCard.transform.SetParent(UIManager.instance.transform);
        useCard.transform.position = transform.position;
        useCard.name = "Card";
        useCard.GetComponent<Card>().plantInfo = plantInfo;
        hasLock = true;
        darkBg.SetActive(true);
        // 移动到目标位置
        Transform targetObject = chooseCardPanel.cards.transform.Find("Card" + curIndex);
        useCard.GetComponent<Card>().isMoving = true;
        useCard.GetComponent<Card>().hasUse = true;
        chooseCardPanel.ChooseCard.Add(useCard);
        // DoMove进行移动
        useCard.transform.DOMove(targetObject.position, 0.3f).OnComplete(
            () =>
            {
                useCard.transform.SetParent(targetObject, false);
                useCard.transform.localPosition = Vector3.zero;
                useCard.GetComponent<Card>().isMoving = false;
            }
        );
    }
}
