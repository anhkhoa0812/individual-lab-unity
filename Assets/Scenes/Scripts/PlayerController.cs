using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int, int> onHPChanged; //Quản lý khi máu của player thay đổi

    [SerializeField] private float moveSpeed; //Tốc độ di chuyển của Player
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private Transform firingPoint; //vị trí bắn viên đạn
    [SerializeField] private float firingCooldown; //tốc độ bắn
    [SerializeField] private int hp; //máu của Player

    private int currentHp;//Máu hiện tại của Player
    private float tempCoolDown;//cooldown hiện tại
    private SpawnManager spawnManager;
    private GameManager gameManager;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        if (onHPChanged != null)
            onHPChanged(currentHp, hp);
        spawnManager = FindAnyObjectByType<SpawnManager>(); //Tìm đến SpawnManager
        gameManager = FindAnyObjectByType<GameManager>(); //Tìm đến GameManager
        audioManager = FindAnyObjectByType<AudioManager>();//Tìm đến AudioManager

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isActive())
            return;
        float horizontal = Input.GetAxis("Horizontal"); //Kiểm tra tình trạng 2 phím mũi tên trái/phải và A/D
        float vertical = Input.GetAxis("Vertical"); //Kiểm tra tình trạng 2 phím mũi trên lên/xuống và W/S
        Vector2 direction = new Vector2(horizontal, vertical); // x la horizontal, y la vertical (quang duong di chuyen)
        transform.Translate(direction * Time.deltaTime * moveSpeed);   //deltaTime la thoi gian cho 1 lan update

        ClampPlayerToCamera(); //Dùng để cố định player trong MainCamera

        if(Input.GetKey(KeyCode.Space)) // Kiểm tra tình trạng phím Space
        {
            if(tempCoolDown <= 0)
            {
                Fire(); //Bắn ra 1 Projectile
                tempCoolDown = firingCooldown; //Set cooldown để ko bắn liên tục được
            }
            
        }

        tempCoolDown -= Time.deltaTime; //Giảm thời gian cooldown
    }
    //Xử lý để bắn
    private void Fire()
    {
        ProjectileController projectile_1 = spawnManager.SpawnPlayerProjectile(firingPoint.position); //Khởi tạo ra 1 Projectile
        projectile_1.Fire(); 

        audioManager.PlayLaserSFX(); //Phát âm thanh khi bắn
    }
    //Xử lý khi bị Player bị trúng đạn
    public void Hit(int damage)
    {
        currentHp -= damage; //Lấy máu hiện tại trừ đi dame bị nhận
        if (onHPChanged != null)
            onHPChanged(currentHp, hp); 
        if(currentHp <= 0) //Nếu máu hiện tại nhỏ hơn 0 thì game kết thúc
        {
            Destroy(gameObject); //Xoá object Player
            gameManager.Gameover(false); //Set gameOver
            audioManager.PlayExplosionSFX(); //Phát âm thanh phá huỷ

        }
        audioManager.PlayHitSFX(); //Phát âm thanh bị trúng đạn
    }
    //Khoá Player trong 1 khung hình Camera để không di chuyển ra ngoài
    private void ClampPlayerToCamera()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); 

        // Ensure the player stays within the screen 
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }
}
