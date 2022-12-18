using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float damage = 15;
    public bool TorchwoodCreate;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Zombie")
        {
            // 僵尸受击
            other.GetComponent<ZombieNormal>().ChangeHealth(-damage);
            DestroyBullet();
        }
    }

    // 销毁子弹
    public virtual void DestroyBullet()
    {
        GameObject.Destroy(gameObject);
    }
}
