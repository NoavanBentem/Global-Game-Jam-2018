using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour {

    [Header("Decoration")]
    public int objects;
    public List<GameObject> decorations = new List<GameObject>();

    [Header("Terrain")]
    public GameObject terrain;

    // Use this for initialization
    void Start() {
        for (int i = 0; i < objects; i++) {
            spawnDecoration();
        }
    }

    public void spawnDecoration() {
        GameObject g = Instantiate(getRandomObject()) as GameObject;
        g.transform.position = getRandomLocation();
        g.transform.localScale = new Vector3(3, 3, 3);
        g.transform.SetParent(transform.transform, false);
    }

    public GameObject getRandomObject() {
        return decorations[Random.Range(0, decorations.Count - 1)];
    }

    public Vector3 getRandomLocation() {
        float x = (Random.Range(0.0f, 1.0f) > 0.5f) ? Random.Range(-150, -20) : Random.Range(20, 150);
        float z = (Random.Range(0.0f, 1.0f) > 0.5f) ? Random.Range(-150, -20) : Random.Range(20, 150);

        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 50, z), Vector3.down);
        Vector3 result = (terrain.GetComponent<Collider>().Raycast(ray, out hit, 2.0f * 50)) ? hit.point: Vector3.zero;
        return result;
    }
}
