using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [Header("Properties")]
    [Range(0, 1)]
    public float energy;
    [Range(0, 1)]
    public float health;

    [Header("Weapon")]
    public float range;
    public float delay;
    public float duration;
    public bool canFire;
    public Gun gun;
    public GameObject crosshair;

    [Header("UI")]
    public Image slider_energy;
    public Image sprite_health;
    public Text warning_text;

    [Header("Managers")]
    public GameManager gameManager;

    void Start() {

    }

    public void changeEnergy(float value) {
        energy = Mathf.Clamp(energy += value, 0, 10);
    }

    public void damage(float value) {
        health -= value;
        if (health <= 0) {
            gameManager.gameOver();
        }
    }

    private void Update() {
        if (!gameManager.isPaused && !gameManager.isGameOver) {
            GetComponent<FirstPersonController>().enabled = true;
            if (Input.GetMouseButtonDown(0)) {
                shoot();
            }

            if (Input.GetMouseButtonDown(1)) {
                checkBattery();
            }

            slider_energy.fillAmount = Mathf.Clamp(Mathf.MoveTowards(slider_energy.fillAmount, energy, Time.deltaTime * 0.125f), 0, 1);
            if (slider_energy.fillAmount >= 1) {
                slider_energy.GetComponent<Animation>().Play();
                warning_text.text = "Return to the satelite to return the energy";
            } else {
               // warning_text.text = "";
            }
            sprite_health.color = new Color(255, 0, 0, 0.5f - health / 2);
            health = Mathf.Clamp(health += 0.0001f, 0, 1);
        } else {
            GetComponent<FirstPersonController>().enabled = false;
        }
    }


    public void shoot() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.distance <= range) {
                if (hit.transform.CompareTag("Energy") && canFire) {
                    gun.shoot(hit.transform.gameObject, duration);
                    StartCoroutine(cooldown());
                } else if (canFire) {
                    gun.shoot(crosshair, duration);
                    StartCoroutine(cooldown());
                }
            }
        }

    }

    public void checkBattery() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.distance <= range) {
                if (hit.transform.CompareTag("Battery")) {
                    if(energy >= 0.95f) {
                        hit.transform.GetComponent<BatteryModule>().power();
                        energy = 0;
                    } else {
                        gameManager.showWarningMessage("You need to collect more energy!", 3);
                    }
                }
            }
        }
    }

    public IEnumerator cooldown() {
        canFire = false;
        yield return new WaitForSeconds(delay);
        canFire = true;
    }
}
