using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCardPanel : MonoBehaviour
{
    public GameObject Bg;
    public GameObject beforeCardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // 生成选卡栏的40个格子
        for (int i = 0; i < 40; i++)
        {
            GameObject beforeCard = Instantiate(beforeCardPrefab);
            beforeCard.transform.SetParent(Bg.transform, false);
            beforeCard.name = "Card" + i.ToString();
        }
    }

    public void InitCards()
    {
        print("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
        print(GameManager.instance.plantInfo) ;
        foreach (PlantInfoItem plantInfo in GameManager.instance.plantInfo.plantInfoList)
        {
            Transform cardParent = Bg.transform.Find("Card" + plantInfo.plantId);
            GameObject reallyCard = Instantiate(plantInfo.cardPrefab) as GameObject;
            reallyCard.transform.SetParent(cardParent, false);
            reallyCard.transform.localPosition = Vector2.zero;
            reallyCard.name = "BeforeCard";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
