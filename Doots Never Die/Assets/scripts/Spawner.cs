using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> children;
    public GameObject child;
    public List<GameObject> spawnPoints;
    public Camera camera;
    int timer;
    int difficulty;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(children.Count <= 100 && timer >= 180)
        {
            int randomSpawn = Random.Range(0, spawnPoints.Count);
            Vector3 toCam = camera.transform.position - spawnPoints[randomSpawn].transform.position;
            if (toCam.sqrMagnitude > 6*6)
            {
                GameObject spawn = Instantiate(child, spawnPoints[randomSpawn].transform.position, Quaternion.identity);
                children.Add(spawn);
                timer = 0;
                Debug.Log("Spawner: " + randomSpawn);
            }
        }
        timer++;
    }
}
