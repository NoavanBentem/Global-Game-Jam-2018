using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {

    [Header("Settings")]
    public int amountOfEnemies;
    public int ratio;
    public int spawnRadius;
    public GameObject enemy;
    public GameObject player;
    public GameObject terrain;
    public Material material_red;
    public Material meterial_green;

    [Header("Spawnpoints")]
    public List<GameObject> fixed_spawnpoints = new List<GameObject>();
    public List<GameObject> spawnpoints = new List<GameObject>();

    [Header("Enemies")]
    public List<GameObject> enemies_hostile = new List<GameObject>();
    public List<GameObject> enemies_passive = new List<GameObject>();
    public List<GameObject> destinations = new List<GameObject>();

    [Header("Managers")]
    public DayNightController dayNightManager;
    public GameManager gameManager;

    // Use this for initialization
    void Start() {
        spawnpoints = new List<GameObject>(fixed_spawnpoints);
        destinations = new List<GameObject>(fixed_spawnpoints);
        spawnAllEnemies();
    }

    public void spawnAllEnemies() {
        for (int i = 0; i < amountOfEnemies; i++) {
            spawnEnemy(i % ratio == 0);
        }
    }

    public void killAllEnemies() {
        foreach (GameObject g in enemies_hostile) {
            Destroy(g);
        }

        foreach (GameObject g in enemies_passive) {
            Destroy(g);
        }
    }

    public void spawnEnemy(bool isPassive) {
        GameObject g = Instantiate(enemy) as GameObject;
        g.GetComponent<NavMeshAgent>().enabled = false;
        Enemy e = g.GetComponent<Enemy>();
        if (isPassive) {
            enemies_passive.Add(g);
            e.lookAtPlayer = false;
            g.name = "Enemy Passive";
            e.destination = g;
            //g.GetComponent<Renderer>().material = meterial_green;
            e.lineOfSight *= 1.5f;
            g.tag = "Energy";
            e.energyLeft = 1.0f;
            g.GetComponent<Light>().color = meterial_green.color;

        } else {
            enemies_hostile.Add(g);
            e.lookAtPlayer = true;
            g.name = "Enemy Hostile";
            e.destination = getRandomDestination(true);
            //g.GetComponent<Renderer>().material = material_red;
            g.GetComponent<Light>().color = material_red.color;
        }
        e.enemyManager = GetComponent<EnemyManager>();
        e.gameManager = gameManager;
        e.player = player;
        g.transform.position = new Vector3(Random.Range(-150, 150), 5, Random.Range(-150, 150));
        g.transform.SetParent(transform);
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(g.transform.position.x, 50, g.transform.position.z), Vector3.down);
        if (terrain.GetComponent<Collider>().Raycast(ray, out hit, 2.0f * 50)) {
            g.transform.position = hit.point + new Vector3(0, -5, 0);
        }
    }

    public GameObject getSpawnpoint() {
        if(spawnpoints.Count <= 1) {
            GameObject s = spawnpoints[0];
            spawnpoints = new List<GameObject>(fixed_spawnpoints);
            return s;
        } else {
            int index = Random.Range(0, spawnpoints.Count - 1);
            GameObject s = spawnpoints[index];
            spawnpoints.RemoveAt(index);
            return s;
        }
    }

    public GameObject getRandomDestination(bool isSpawnpoint) {
        if (isSpawnpoint) {
            return fixed_spawnpoints[(int)Random.Range(0, fixed_spawnpoints.Count - 1)];
        } else {
            return enemies_hostile[(int)Random.Range(0, enemies_hostile.Count - 1)];
        }

    }
}
