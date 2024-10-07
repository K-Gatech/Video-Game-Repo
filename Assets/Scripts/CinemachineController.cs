using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    public Transform character;
    public Transform camera;

    public CinemachineFreeLook cinemachine;
    public float []radius = {10, 14, 12};

    // Start is called before the first frame update
    void Start()
    {
        cinemachine.m_Orbits[0].m_Radius = 5;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 pos = camera.position;
        pos.y = character.position.y;
        Vector3 dir = pos - character.position;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(character.position, dir.normalized, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(character.position, dir.normalized * hit.distance, Color.green);

            if (hit.distance < 14)
            {
                for (int i = 0; i < 3; i++)
                {
                    cinemachine.m_Orbits[i].m_Radius = Mathf.Max(1, (hit.distance / 14f) * radius[i] - 1f);
                }
            }
        } else
        {
            for (int i = 0; i < 3; i++)
            {
                cinemachine.m_Orbits[i].m_Radius = radius[i];
            }
        }

    }
}
