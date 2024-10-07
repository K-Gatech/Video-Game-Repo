using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    // Start is called before the first frame update

    private Material targetMaterial;
    private int targetCooldown, targetCounter;
    private bool isHit;

    void Start()
    {
        targetMaterial = Resources.Load<Material>("Target Materials/Red");
        this.GetComponent<Renderer>().material = targetMaterial;
        targetCooldown = 180;
        targetCounter = 0;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit)
        {
            if(targetCounter == targetCooldown)
            {
                isHit = false;
                targetMaterial = Resources.Load<Material>("Target Materials/Red");
                this.GetComponent<Renderer>().material = targetMaterial;
            }
            else
                targetCounter++;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("HIT!!!");
            targetMaterial = Resources.Load<Material>("Target Materials/Green");
            this.GetComponent<Renderer>().material = targetMaterial;
            isHit = true;
            targetCounter = 0;
        }
    }
}
