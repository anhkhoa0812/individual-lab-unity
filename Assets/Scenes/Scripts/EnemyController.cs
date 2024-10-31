using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed; //Tốc độ của Enemy
    [SerializeField] private Transform[] wayPoints; //lộ trình di chuyển của Enemy
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private Transform firingPoint; //vị trí bắn viên đạn
    [SerializeField] private float minFiringCooldown; //tốc độ bắn
    [SerializeField] private float maxFiringCooldown; 
    [SerializeField] private int hp;

    private int currentHp;
    private float tempCoolDown;
    private int currentWayPointIndex;
    private bool active;
    private SpawnManager spawnManager;
    private GameManager gameManager;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return; 
        int nextWayPoint = currentWayPointIndex + 1; //Lấy wayPoint tiếp theo bằng cách lấy vị trí của WayPoint hiện tại + 1
        if (nextWayPoint > wayPoints.Length - 1) //Check nếu là wayPoint cuối cùng để lặp lại lộ trình di chuyển
        {
            nextWayPoint = 0;
        }
        //Di chuyển Enemy từ wayPoint này tới wayPoint khác
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[nextWayPoint].position, moveSpeed * Time.deltaTime);
        if (transform.position == wayPoints[nextWayPoint].position) //Check nếu Enemy đã đến điểm đích  
        {
            currentWayPointIndex = nextWayPoint;
        }

        //Vector3 direction = wayPoints[nextWayPoint].position - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //rad2deg de sang he toa do 360
        //transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        if(tempCoolDown <= 0)
        {
            Fire();
            tempCoolDown = Random.Range(minFiringCooldown, maxFiringCooldown);
        }

        tempCoolDown -= Time.deltaTime; 
    }

    public void Init(Transform[] wayPoints)
    {
        this.wayPoints = wayPoints;
        active = true;
        transform.position = wayPoints[0].position;
        tempCoolDown = Random.Range(minFiringCooldown, maxFiringCooldown);
        currentHp = hp;
    }
    private void Fire()
    {
        ProjectileController projectile_1 = spawnManager.SpawnEnemyProjectile(firingPoint.position);
        projectile_1.Fire();

        audioManager.PlayPlasmaSFX();
    }

    public void Hit(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            //Destroy(gameObject);
            spawnManager.ReleaseEnemy(this);
            gameManager.AddScore(1);
            audioManager.PlayExplosionSFX();
        }
        audioManager.PlayHitSFX();
    }
}
