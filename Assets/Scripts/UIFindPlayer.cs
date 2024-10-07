using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFindPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerObject;

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(playerObject.transform);
    }
}
