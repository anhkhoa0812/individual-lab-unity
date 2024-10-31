using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemiesPool
{
    public EnemyController prefab;
    public List<EnemyController> inactiveObjs;
    public List<EnemyController> activeObjs;

    public EnemyController Spawn(Vector3 positon, Transform parent)
    {
        if(inactiveObjs.Count == 0)
        {
            EnemyController newObj = GameObject.Instantiate(prefab, parent);
            newObj.transform.position = positon;
            activeObjs.Add(newObj);
            return newObj;
        }
        else
        {
            EnemyController oldObj = inactiveObjs[0];
            oldObj.gameObject.SetActive(true);
            oldObj.transform.SetParent(parent);
            oldObj.transform.position = positon;
            activeObjs.Add(oldObj);
            inactiveObjs.RemoveAt(0);
            return oldObj;
        }
    }

    public void Release(EnemyController obj)
    {
        if(activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public void Clear()
    {
        while(activeObjs.Count > 0)
        {
            EnemyController obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}
[System.Serializable]
public class ProjectilesPool
{
    public ProjectileController prefab;
    public List<ProjectileController> inactiveObjs;
    public List<ProjectileController> activeObjs;

    public ProjectileController Spawn(Vector3 positon, Transform parent)
    {
        if (inactiveObjs.Count == 0) //Nếu không có inactive Objs nào
        {
            ProjectileController newObj = GameObject.Instantiate(prefab, parent); //Tạo 1 Projectile mới
            newObj.transform.position = positon; //Set vị trí của Projectile
            activeObjs.Add(newObj);//Thêm vào activeObjs
            return newObj;
        }
        else //Nếu tồn tại các inactive Objs
        {
            ProjectileController oldObj = inactiveObjs[0]; //Lấy Projectile đầu tiên trong inActiveObjs
            oldObj.gameObject.SetActive(true); //Hiển thị projectile đó;
            oldObj.transform.position = positon; //Set vị trí của Projectile
            oldObj.transform.SetParent(parent); //Set parent cho ProjectileController
            activeObjs.Add(oldObj); //Thêm Project vừa được lấy ra vào list activeObjs
            inactiveObjs.RemoveAt(0); //Xoá Project vừa được lấy ra khỏi list inActiveObjs
            return oldObj; 
        }
    }

    public void Release(ProjectileController obj)
    {
        if(activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while (activeObjs.Count > 0)
        {
            ProjectileController obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}
public class SpawnManager : MonoBehaviour
{
    //[SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private EnemiesPool enemiesPool;
    [SerializeField] private ProjectilesPool playerProjectilesPool;
    [SerializeField] private ProjectilesPool enemyProjectilesPool;
    [SerializeField] private float enemySpawnInterval;
    [SerializeField] private int minTotalEnemies;
    [SerializeField] private int maxTotalEnemies;
    [SerializeField] private EnemyPath[] enemyPaths;
    [SerializeField] private bool active;
    [SerializeField] private int totalGroups;
    [SerializeField] private PlayerController playerController;

    

    private bool isSpawning;
    private PlayerController player;

    public PlayerController Player => player;

    public void StartGame()
    {
        if (player == null)
            player = Instantiate(playerController); //Nếu chưa có Player thì sẽ khởi tạo
        player.transform.position = Vector3.zero; //(0,0,0)
        StartCoroutine(IESpawnGroup(totalGroups));
    }

    private IEnumerator IESpawnGroup(int groups)
    {
        isSpawning = true;

        for (int i=0; i <  groups; i++)
        {
            int totalEnemies = Random.Range(minTotalEnemies, maxTotalEnemies);
            int pathIndex = Random.Range(0, enemyPaths.Length); //Chọn ngẫu nhiên 1 đường đi từ enemyPaths
            EnemyPath path = enemyPaths[pathIndex]; //Lấy ra đường đi
            yield return StartCoroutine(IESpawnEnemies(totalEnemies, path));
            if(i < groups - 1)
                yield return new WaitForSeconds(3); //Nếu chưa phải nhóm cuối cùng, đợi 3s để spawn nhóm tiếp theo
        }

        isSpawning = false;
    }

    private IEnumerator IESpawnEnemies(int totalEnemies, EnemyPath path)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            yield return new WaitUntil(() => active); //Đợi đến khi active là true
            yield return new WaitForSeconds(enemySpawnInterval); //Đợi 1 khoảng thời gian giữa mỗi lần spawn

            //EnemyController enemy = Instantiate(enemyPrefab, transform); //doi null thanh transform de enenmy spawn la con cua spawnManager
            EnemyController enemy = enemiesPool.Spawn(path.WayPoints[0].position, transform); //Spawn từ pool tại ví trị bên đầu của path
            enemy.gameObject.SetActive(true); //kích hoạt enemy
            enemy.Init(path.WayPoints);
        }
    }

    public void ReleaseEnemy(EnemyController obj)
    {
        enemiesPool.Release(obj);
    }

    public ProjectileController SpawnEnemyProjectile(Vector3 position)
    {
        ProjectileController obj = enemyProjectilesPool.Spawn(position, transform);
        obj.SetFromPlayer(false);
        return obj;
    }

    public void ReleaseEnemyProjectile(ProjectileController projectile)
    {
        enemyProjectilesPool.Release(projectile);
    }

    public ProjectileController SpawnPlayerProjectile(Vector3 positon)
    {
        ProjectileController obj = playerProjectilesPool.Spawn(positon, transform);
        obj.SetFromPlayer(true);
        return obj;
    }

    public void ReleasePlayerProjectile(ProjectileController projectile)
    {
        playerProjectilesPool.Release(projectile);
    }

    public bool IsClear()
    {
        if (isSpawning || enemiesPool.activeObjs.Count > 0)
            return false;
        return true;
    }

    public void Clear()
    {
        enemiesPool.Clear();
        enemyProjectilesPool.Clear();
        playerProjectilesPool.Clear();

        StopAllCoroutines(); //dung SpawnGroups and SpawnEnmies
    }
    //private IEnumerator IFTestCoroutine()
    //{
    //    yield return new WaitUntil(() => active);
    //    for(int i=0; i < 5; i++)
    //    {
    //        Debug.Log(i);
    //        yield return new WaitForSeconds(1f);
    //    }
    //}
}
