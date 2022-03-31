using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This script is simply so the sprites in the winscreen float up and down as if they're in space
// - Anthony hehe c:
public class SpriteFloat : MonoBehaviour
{
    public GameObject sprite;
    public float speed;
    public float distance;
    public float pointOne;
    public float pointTwo;

    public void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, distance) * pointOne - pointTwo;

        sprite.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
