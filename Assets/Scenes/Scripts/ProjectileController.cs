using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float moveSpeed; //Tốc độ di chuyển của đạn
    [SerializeField] private Vector2 direction; //Hướng bắn của đạn
    [SerializeField] private int damage; //Dame của 1 viên đạn

    private bool fromPlayer;
    private SpawnManager spawnManager;
    private float lifeTime;
    // Start is called before the firstx frame update
    void Start()
    {
        spawnManager = FindAnyObjectByType<SpawnManager>(); //Tìm đến SpawnManager
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * moveSpeed); //Di chuyển projectile

        if(lifeTime <= 0) 
        {
            Release();
        }
        lifeTime -= Time.deltaTime;
    }

    private void Release() // Nếu giá trị fromPlayer  = true thì khởi tạo , false thì khởi tạo Enemy
    {
        if (fromPlayer) 
            spawnManager.ReleasePlayerProjectile(this);
        else
            spawnManager.ReleaseEnemyProjectile(this);
    }
    //Set 
    public void Fire()
    {
        lifeTime = 10f;
    }
    //Set giá trị của fromPlayer
    public void SetFromPlayer(bool value)
    {
        fromPlayer = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Release();
            EnemyController enemy;
            collision.gameObject.TryGetComponent(out enemy);
            enemy.Hit(damage);

        }

        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
            Release();
            PlayerController player;
            collision.gameObject.TryGetComponent(out player);
            player.Hit(damage);
        }
    }
}
