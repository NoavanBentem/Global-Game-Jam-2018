using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public LightningBolt lightning;
    public Player player;
    public ParticleSystem partices;
    public LineRenderer linerender;
    public AudioSource audio_source;
    public AudioClip audio_shoot;

    // Use this for initialization
    void Start() {

    }

    public void shoot(GameObject hit, float duration) {
        StartCoroutine(IE_shoot(hit, duration));
    }

    public IEnumerator IE_shoot(GameObject hit, float duration) {
        //linerender.positioncount = 2;
        //linerender.setposition(0, transform.position);
        //linerender.setposition(1, hit.transform.position);
        lightning.target = hit.transform;
        audio_source.clip = audio_shoot;
        audio_source.Play();
        if (hit.GetComponent<Enemy>() != null) {
            hit.transform.GetComponent<Enemy>().energyLeft -= 0.25f;
            hit.transform.GetComponent<Enemy>().playSound(hit.transform.GetComponent<Enemy>().audio_hurt);
            hit.transform.GetComponent<Enemy>().speed *= 1.2f;
            player.energy += 0.25f;
        }
        yield return new WaitForSeconds(duration);
        audio_source.Stop();
        lightning.target = null;
    }

    public void playSound(AudioClip a) {
        audio_source.clip = a;
        audio_source.Play();
    }

    public void startShootDelay() {

    }
}
