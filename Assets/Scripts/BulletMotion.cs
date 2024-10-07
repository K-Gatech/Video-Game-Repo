using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMotion : MonoBehaviour
{
    public float speed = 1f;

    private int frameTimer;

    void Start()
    {
        frameTimer = 0;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
        frameTimer++;

        if(frameTimer > 500)
            DestroySelf();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit Something, destroy self");
            Debug.Log("Hit object: " + collision.gameObject.name);
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
