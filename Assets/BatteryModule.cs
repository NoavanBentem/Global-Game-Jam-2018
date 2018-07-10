using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryModule : MonoBehaviour {

    public bool isPowered;
    public GameManager gameManager;
    public ParticleSystem particles;

	void Start () {
        particles.enableEmission = false;
	}
	
	public void power() {
        if (!isPowered) {
            gameManager.progress += 0.1f;
            isPowered = true;
            particles.enableEmission = true;
            gameManager.showWarningMessage("Charged battery " + (int)(gameManager.progress * 10.0f) + " out of 10", 4);
            gameManager.timeLeft = 1.0f;
            if(gameManager.progress >= 1.0f) {
                gameManager.finishGame();
            }
        } else {
            gameManager.showWarningMessage("This battery has already been charged!", 3);
        }
    }
}
