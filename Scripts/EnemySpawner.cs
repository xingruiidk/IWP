using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public GameObject showDmg;
    public int poolSize = 10;
    private Queue<GameObject> damageTextPool;
    private GridCheck gridChecker;
    public GameObject enemyPrefab;
    public int enemyCount;
    private List<GameObject> enemyList;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gridChecker = GetComponent<GridCheck>();
        enemyList = new List<GameObject>();
        InitializePool();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemiesDead();
    }
    private void InitializePool()
    {
        damageTextPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(showDmg, transform);
            obj.SetActive(false);
            damageTextPool.Enqueue(obj);
        }
    }
    public void ShowDMGText(float playerDMG, Transform enemyT)
    {
        if (damageTextPool.Count > 0)
        {
            GameObject dmgText = damageTextPool.Dequeue();
            dmgText.SetActive(true);
            dmgText.transform.position = enemyT.position + Vector3.up * 3.5f; 
            Camera mainCamera = Camera.main; 
            if (mainCamera != null)
            {
                Vector3 direction = dmgText.transform.position - mainCamera.transform.position;
                dmgText.transform.rotation = Quaternion.LookRotation(direction);
            }
            dmgText.GetComponent<TMP_Text>().text = playerDMG.ToString();
            StartCoroutine(ReturnToPoolAfterTime(dmgText, 1f));
        }
    }
    private IEnumerator ReturnToPoolAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        EnemySpawner.instance.damageTextPool.Enqueue(obj);
    }
    public void SpawnEnemies()
    {
        gridChecker.CheckGrid();
        List<Vector3> spawnPositions = gridChecker.GetRandomEmptyCells(enemyCount);
        foreach (Vector3 position in spawnPositions)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().modelint = Random.Range(0, 2);
            enemyList.Add(spawnedEnemy);
        }
    }
    public void CheckEnemiesDead()
    {
        bool allDead = true;
        foreach (GameObject enemySpawned in enemyList)
        {
            if (enemySpawned != null) 
            {
                Healthbar healthbar = enemySpawned.GetComponent<Healthbar>();
                if (healthbar != null && healthbar.health > 0)
                {
                    allDead = false;
                    break;
                }
            }
        }
        if (allDead)
        {
            foreach (GameObject spawnedEnemy in enemyList)
            {
                Destroy(spawnedEnemy);
            }
            enemyList.Clear();
            SpawnEnemies();
        }
    }
}
