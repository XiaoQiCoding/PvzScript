using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : MonoBehaviour
{
    public Vector3 direction = new Vector3(-1, 0, 0);
    public float speed = 1;
    private bool isWalk;
    private Animator animator;

    public float damage;
    public float damageInterval = 0.5f;
    private float damageTimer;

    public float health = 100;
    public float lostHeadHealth = 50;
    private float currentHealth;
    private GameObject head;
    private bool lostHead;
    private bool isDie;

    // Start is called before the first frame update
    void Start()
    {
        isWalk = true;
        animator = GetComponent<Animator>();
        damageTimer = 0;

        currentHealth = health;
        head = transform.Find("Head").gameObject;
        isDie = false;
        lostHead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
            return;
        Move();
    }

    private void Move()
    {
        if(isWalk)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // 碰撞开始
    private void OnTriggerEnter2D(Collider2D other) {
        if(isDie)
            return;
        if(other.tag == "Plant")
        {
            // 碰到植物，停止移动
            isWalk = false;
            animator.SetBool("Walk", false);
        }
    }

    // 碰撞持续中
    private void OnTriggerStay2D(Collider2D other) {
        if(isDie)
            return;
        if(other.tag == "Plant")
        {
            // 持续接触植物，造成伤害
            damageTimer += Time.deltaTime;
            if(damageTimer >= damageInterval)
            {
                damageTimer = 0;
                // 对所有植物造成伤害
                Plant plant = other.GetComponent<Plant>();
                float newHealth = plant.ChangeHealth(-damage);
                if(newHealth <= 0)
                {
                    isWalk = true;
                    animator.SetBool("Walk", true);
                }
            }
        }
    }

    // 碰撞退出
    private void OnTriggerExit2D(Collider2D other) {
        if(isDie)
            return;
        if(other.tag == "Plant")
        {
            // 离开植物，或者植物被消灭了，继续移动
            isWalk = true;
            animator.SetBool("Walk", true);
        }
    }

    public void ChangeHealth(float num)
    {
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        // 血线低于lostHeadHealth时，进入LostHead状态
        if (currentHealth < lostHeadHealth && !lostHead)
        {
            lostHead = true;
            animator.SetBool("LostHead", true);
            head.SetActive(true);
        }
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            isDie = true;
        }
    }

    public void DieAniOver()
    {
        animator.enabled = false;
        GameManager.instance.ZombieDied(gameObject);
        GameObject.Destroy(gameObject);
    }
}
