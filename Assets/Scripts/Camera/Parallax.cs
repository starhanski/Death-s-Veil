using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float length, startpPos;
    public GameObject cam;
    public float parallaxEffectMultiplier;
    void Start()
    {
        startpPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffectMultiplier));
        float distance = (cam.transform.position.x * parallaxEffectMultiplier);

        transform.position = new Vector3(startpPos + distance, transform.position.y, transform.position.z);

        if (temp > startpPos + length) startpPos += length;
        else if (temp < startpPos - length) startpPos -= length;
    }
}
