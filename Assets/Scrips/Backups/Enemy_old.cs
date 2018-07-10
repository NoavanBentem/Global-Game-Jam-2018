using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_old : MonoBehaviour {

    [Header("Properties")]
    public float speed;
    public float turnSpeed;
    public float lineOfSight;

    public float size;
    public Vector3 position;
    public Vector3 direction;

    public bool playerInRange;
    public bool playerInSigh;
    public bool lookAtPlayer;

    NavMeshAgent agent;
    public GameObject destination;

    public EnemyManager enemyManager;

    [Header("References")]
    public GameObject player;

    private RaycastHit hit;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate() {
        playerInRange = Vector3.Distance(transform.position, player.transform.position) <= lineOfSight;
        if (playerInRange) {
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, lineOfSight)) {
                playerInSigh = hit.transform.gameObject.CompareTag("Player");
                if (playerInSigh) {
                    //transform.GetComponent<Rigidbody>().MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((lookAtPlayer) ? player.transform.position - transform.position : transform.position - player.transform.position), Time.deltaTime * turnSpeed));
                    //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                }

                //debug_angle = Vector3.Angle(transform.forward, transform.position - player.transform.position);
                if (lookAtPlayer) {
                    agent.SetDestination(player.transform.position);
                }
                else if (!lookAtPlayer) {
                    if (agent.remainingDistance < 5) {
                        getNewTarget();
                    }
                }
            }
        }
        else {
            agent.SetDestination(transform.position);
        }
    }

    public void getNewTarget() {
        GameObject new_destination = enemyManager.getRandomDestination(false);
        while (new_destination == destination) {
            new_destination = enemyManager.getRandomDestination(false);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            Debug.Log("Caught!");
        }
    }
}
