using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public struct PrefabWithName
{
    public string name;
    public GameObject prefab;
}

public class GameMaster : MonoBehaviour
{
    [SerializeField] int nbCrates = 2;

    [Header("Enemies")]
    [SerializeField] TextAsset enemyCSV = null;
    [SerializeField] PrefabWithName[] enemyPrefabs = null;

    [Header("Pickups")]
    [SerializeField] Vector2[] pickupsPositions = null;
    [SerializeField] GameObject ammoPrefab = null;
    [SerializeField] float ammoDelay = 5;
    [SerializeField] int minAmmoToSpawn = 1;
    [SerializeField] int maxAmmoToSpawn = 4;
    [SerializeField] float minAmmoSpawnDistance = 1.5f;

    // enemy configs
    List<float> enemySpawnTimes = new List<float>();
    List<string> enemyNames = new List<string>();
    List<float> enemyPositionXs = new List<float>();
    List<float> enemyPositionYs = new List<float>();
    List<int> enemyDirections = new List<int>();

    float levelTimer = 0f;
    float ammoTimer = 0f;
    int nextEnemyIndex = 0;
    float speedLevel = 0.5f;

    PP_Player myPlayer = null;

    void Start()
    {
        LoadEnemyCSV();
        myPlayer = FindObjectOfType<PP_Player>();
    }

    void Update()
    {
        if (nextEnemyIndex < enemySpawnTimes.Count)
        {
            SpawnEnemies();
        }
        else
        {
            NextSpeedLevel();
        }

        SpawnAmmos();

        levelTimer += Time.deltaTime * speedLevel;
        ammoTimer += Time.deltaTime * speedLevel;
    }

    public void CrateDestroyed()
    {
        nbCrates -= 1;
        if(nbCrates <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        // MusicAndSoundManager.PlaySound("End", Camera.main.transform.position); // No because too ugly
        FindObjectOfType<ScoreController>().SendScoreToMemory();
        FindObjectOfType<SceneLoader>().LoadPPMenuEnd();
    }

    private void LoadEnemyCSV()
    {
        float spawnTime = 0;
        string name = "";
        float positionX = 0;
        float positionY = 0;
        int direction = 0;
        
        string[] lines = enemyCSV.text.Split("\n"[0]);

        for (var i = 1; i < lines.Length; i++) // start to 1 to skip header
        {
            string[] parts = lines[i].Split(","[0]);

            float.TryParse(parts[0], out spawnTime);
            name = parts[1];
            float.TryParse(parts[2], out positionX);
            float.TryParse(parts[3], out positionY);
            int.TryParse(parts[4], out direction);

            enemySpawnTimes.Add(spawnTime);
            enemyNames.Add(name);
            enemyPositionXs.Add(positionX);
            enemyPositionYs.Add(positionY);
            enemyDirections.Add(direction);
        }
    }

    private void SpawnEnemies()
    {
        if (levelTimer >= enemySpawnTimes[nextEnemyIndex])
        {
            GameObject enemyPrefab = null;
            foreach(PrefabWithName enemyPwN in enemyPrefabs)
            {
                if(enemyPwN.name == enemyNames[nextEnemyIndex])
                {
                    enemyPrefab = enemyPwN.prefab;
                }
            }

            GameObject spawnedEnemy = Instantiate(
                enemyPrefab, 
                new Vector2(enemyPositionXs[nextEnemyIndex], enemyPositionYs[nextEnemyIndex]), 
                Quaternion.identity);

            // Todo create a generic enemy script to simplify
            spawnedEnemy.GetComponent<EnemyMouette>().setDirection(enemyDirections[nextEnemyIndex]);

            nextEnemyIndex += 1;
        }
    }

    private void SpawnAmmos()
    {
        int numberBullets = myPlayer.GetNumberBullets();
        if ( ((ammoTimer >= ammoDelay && numberBullets <= maxAmmoToSpawn) || (numberBullets <= minAmmoToSpawn)) && !FindObjectOfType<PickupAmmo>() )
        {
            Vector2 ammoSpawnPosition;
            do
            {
                ammoSpawnPosition = pickupsPositions[UnityEngine.Random.Range(0, pickupsPositions.Length)];
                Debug.Log(Vector2.Distance(myPlayer.transform.position, ammoSpawnPosition));
            }
            while (Vector2.Distance(myPlayer.transform.position, ammoSpawnPosition) < minAmmoSpawnDistance);
            Instantiate(
                ammoPrefab,
                ammoSpawnPosition,
                Quaternion.identity);
            ammoTimer = 0;
        }
    }

    private void NextSpeedLevel()
    {
        speedLevel += 0.5f;
        Debug.Log(speedLevel);
        // Print something on screen
        nextEnemyIndex = 0;
        levelTimer = 0;
    }
}
