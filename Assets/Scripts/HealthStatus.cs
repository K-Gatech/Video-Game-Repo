using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatus
{
    public float currentLevel;

    public HealthStatus()
    {
        currentLevel = 100f;
    }

    public HealthStatus(float level)
    {
        currentLevel = level;
    }
}
