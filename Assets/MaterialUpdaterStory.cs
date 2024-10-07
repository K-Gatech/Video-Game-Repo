using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUpdaterStory : MonoBehaviour
{
    public Material m;
    public float damp = 100;
    Vector2 v = Vector2.zero;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        v = v + (new Vector2(1, 0.3f) / damp);
        m.SetTextureOffset("_MainTex", v);
    }
}
