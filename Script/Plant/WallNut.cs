using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override float ChangeHealth(float num)
    {
        float currentHealth = base.ChangeHealth(num);
        // 修改动画参数：BloodPercent，扣血时触发对应的动画
        animator.SetFloat("BloodPercent", (float)currentHealth / health);
        return currentHealth;
    }
}
