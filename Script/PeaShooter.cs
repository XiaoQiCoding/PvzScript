using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    // Start is called before the first frame update
    public float interval;
    private float timer;
    public GameObject bullet;
    public Transform bulletPos;

    protected override void Start()
    {
        base.Start();
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(! start)
            return;
        timer += Time.deltaTime;
        if(timer >= interval)
        {
            timer = 0;
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
        }
    }

}
