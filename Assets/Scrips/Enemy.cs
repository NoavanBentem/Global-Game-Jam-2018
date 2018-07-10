using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [Header("Properties")]
    public float speed;
    public float turnSpeed;
    public float lineOfSight;
    public float energyLeft;

    [Header("Booleans")]
    public bool playerInRange;
    public bool playerInSigh;
    public bool lookAtPlayer;

    [Header("Navigation")]
    NavMeshAgent agent;
    public GameObject destination;
    private RaycastHit hit;

    [Header("Sound")]
    public AudioSource audio;
    public AudioClip audio_hurt;
    public AudioClip audio_attack;

    [Header("References")]
    public EnemyManager enemyManager;
    public GameManager gameManager;
    public GameObject player;
    public LightningBolt lightning;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<NavMeshAgent>().enabled = true;
    }

    void Update() {
        if (!gameManager.isPaused && !gameManager.isGameOver) {
            agent.enabled = true;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            playerInRange = distance <= lineOfSight;

            if (lookAtPlayer) {
                if (playerInRange) {
                    destination = player;
                } else {
                    if (destination == player || agent.remainingDistance < 5) {
                        getNewTarget(true);
                    }
                }

                agent.SetDestination(destination.transform.position);

            } else if (!lookAtPlayer) {
                if (playerInRange) {
                    if (agent.remainingDistance < 5) {
                        getNewTarget(false);
                    }
                }
                agent.SetDestination(destination.transform.position);
            }

            if (distance <= 2.5 && lookAtPlayer) {
                if(audio.clip != audio_attack) {
                    audio.clip = audio_attack;
                    audio.loop = true;
                    audio.Play();
                }
                player.GetComponent<Player>().changeEnergy(-0.005f);
                player.GetComponent<Player>().health -= 0.001f;
                lightning.target = player.transform;
            } else if (distance >= 2.5 && lookAtPlayer) {
                lightning.target = null;
                audio.clip = null;
            }

            agent.speed = (enemyManager.dayNightManager.isNight()) ? speed : speed * 1.5f - Mathf.Abs(0.5f - enemyManager.dayNightManager.currentTimeOfDay) * 2;

            if (!lookAtPlayer && energyLeft <= 0) {
                StartCoroutine(Die());
            }
        } else {
            agent.enabled = false;
        }

    }

    public IEnumerator pushPlayerBack() {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        player.GetComponent<Rigidbody>().velocity = rb.velocity * -2;
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = true;
    }

    public void getNewTarget(bool isSpawnpoint) {
        GameObject new_destination = enemyManager.getRandomDestination(isSpawnpoint);
        while (new_destination == destination) {
            new_destination = enemyManager.getRandomDestination(isSpawnpoint);
        }
        destination = new_destination;
    }

    public void playSound(AudioClip a) {
        audio.clip = a;
        audio.loop = false;
        audio.Play();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            Debug.Log("Caught!");
        }
    }

    public IEnumerator Die() {
        yield return new WaitForSeconds(2.0f);
        enemyManager.spawnEnemy(true);
        enemyManager.enemies_hostile.Remove(gameObject);
        Destroy(gameObject);
    }
}
