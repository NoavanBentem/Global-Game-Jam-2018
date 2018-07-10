using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Properties")]
    public float scoreMultiplier;

    [Header("Values")]
    public float time;
    public bool isGameOver;
    public bool wonGame;
    public bool isPaused;
    public int cam;
    [Range(0, 1)]
    public float progress;
    [Range(0, 1)]
    public float timeLeft;
    public List<AudioClip> sounds = new List<AudioClip>();

    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject ingameMenu;
    public Image img_progress;
    public Image img_timeleft;
    public Text feedback_text;

    [Header("Sound")]
    public AudioSource audio;
    public AudioClip audio_day;
    public AudioClip audio_night;

    [Header("Managers")]
    public DayNightController dayNightController;
    public EnemyManager enemyManager;

    public void startGame() {
        isGameOver = false;
        time = 0.0f;
        cam = 0;
    }

    public void gameOver() {
        isGameOver = true;
        wonGame = false;
        Debug.Log("Game Over!");
    }

    public void finishGame() {
        isGameOver = true;
        wonGame = true;
        Debug.Log("Time: " + getMinutes(time) + ":" + getSeconds(time));
    }

    private void Update() {
        if(!isGameOver && !isPaused){
            time += Time.deltaTime;
        } else if (isPaused) {
            
        }

        if(Input.GetButtonDown("Pause")) {
            isPaused = (isPaused) ? false : true;
            pauseMenu.SetActive(isPaused);
            ingameMenu.SetActive(!isPaused);
        }

        img_progress.fillAmount = progress;
        timeLeft -= 0.0001f;
        img_timeleft.fillAmount = Mathf.Clamp(Mathf.MoveTowards(img_timeleft.fillAmount, timeLeft, Time.deltaTime * 0.125f), 0, 1);
    }

    public int getMinutes(float t) {
        return Mathf.RoundToInt((t - (t % 60)) / 60);
    }

    public int getSeconds(float t) {
        return Mathf.RoundToInt(t % 60);
    }

    public void showWarningMessage(string message, float duration) {
        feedback_text.text = message;
        StartCoroutine(clearMessage(duration));
    }

    public IEnumerator clearMessage(float delay) {
        yield return new WaitForSeconds(delay);
        feedback_text.text = "";
    }
}
