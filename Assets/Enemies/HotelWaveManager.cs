using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HotelWaveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemiesHolder;
    [SerializeField]
    private GameObject slimePrefab;
    [SerializeField]
    private GameObject batPrefab;
    [SerializeField]
    private GameObject rabbitPrefab;
    [SerializeField]
    private GameObject upgradeScreen;

    private int wave;
    private bool waveHasStarted;
    private int[] slimeCount = { -1, -1, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4 };
    private int[] batCount = { -1, -1, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 7 };
    private int[] rabbitCount = { -1, -1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 2 };
    private Vector3[] spawnLocations = { new Vector3(-50f, -1f, -25f), new Vector3(-3f, -1f, -23f), new Vector3(-8f, -1f, -64f) };

    private void Start()
    {
        wave = 1;
        waveHasStarted = true;
    }

    private void Update()
    {
        if (enemiesHolder.transform.childCount <= 1 && waveHasStarted)
        {
            waveHasStarted = false;
            StartCoroutine(nextWave());
        }
    }

    private IEnumerator nextWave()
    {
        Debug.Log("Next wave condition reached");
        wave++;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        upgradeScreen.SetActive(true);

        if (wave > 12)
        {
            SceneManager.LoadScene("Outside");
        }

        yield return new WaitForSeconds(20);

        // Spawn all the enemies for that wave by type
        for (int i = 0; i < slimeCount[wave]; i++)
        {
            yield return new WaitForSeconds(1);
            spawnEnemy(slimePrefab);
        }

        for (int i = 0; i < batCount[wave]; i++)
        {
            yield return new WaitForSeconds(1);
            spawnEnemy(batPrefab);
        }

        for (int i = 0; i < rabbitCount[wave]; i++)
        {
            yield return new WaitForSeconds(1);
            spawnEnemy(rabbitPrefab);
        }

        waveHasStarted = true;
    }

    private void spawnEnemy(GameObject enemy)
    {
        // Pick random spawn location
        Vector3 spawnLocation = spawnLocations[Random.Range(0, 3)];
        // Spawn enemy
        GameObject newEnemy = Instantiate(enemy, spawnLocation, Quaternion.identity) as GameObject;
        // Set enemy parent to enemies holder (necessary so we can track later on if all enemies have been 
        newEnemy.transform.parent = enemiesHolder.transform;
    }

    public void closeUpgradeMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
