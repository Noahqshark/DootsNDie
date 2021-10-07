using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private GameObject player;
    private Aim aim;
    private int timer;
    public Vector3 skel_vel;

    void Start()
    {
        player = GameObject.Find("DootSkeleton");
        aim = player.GetComponent<Aim>();
        direction =  GetMousePosition().normalized - transform.position.normalized;
        //transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(direction, transform.forward));
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 5f;

        transform.position += transform.up * speed * Time.deltaTime;
        transform.position += skel_vel * Time.deltaTime/10;
        if (timer > 500)
        {
            aim.projectileList.Remove(gameObject);
            timer = 0;
            Destroy(gameObject);
        }

        timer++;
    }

    public static Vector3 GetMousePosition()
    {
        Vector3 vec = GetMousePositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMousePositionWithZ(Camera worldCamera)
    {
        return GetMousePositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMousePositionWithZ(Vector3 screenPOsition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPOsition);
        return worldPosition;
    }
}
