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
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gridChecker = GetComponent<GridCheck>();
        InitializePool();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {

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
            dmgText.transform.rotation = Quaternion.identity;
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
        List<Vector3> spawnPositions = gridChecker.GetRandomEmptyCells(enemyCount);
        foreach (Vector3 position in spawnPositions)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().modelint = Random.Range(0, 2);
        }
    }
}
