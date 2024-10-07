using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random=System.Random;

public class BossAI : MonoBehaviour
{

    public enum BossState
    {
        Passive,
        Aggressive
    };

    public BossState bossState;
    public bool defaultAgressive;

    public GameObject[] turrets;
    public GameObject player;
    public GameObject projectilePrefab;
    public GameObject healthBar;

    public bool playerInRange;
    public bool hasSpecialAttack, initiateSpecialAttack;
    private float projectTileCooldown, specialAttackCooldown, specialAttackTimer;
    private float[] projectileTimer;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        if(defaultAgressive)
        {
            bossState = BossState.Aggressive;
            foreach(GameObject turret in turrets)
            {
                turret.SetActive(true);
                healthBar.SetActive(true);
            }
        }
        else
        {
            bossState = BossState.Passive;
            foreach(GameObject turret in turrets)
            {
                turret.SetActive(false);
                healthBar.SetActive(false);
            }
        }
        
        playerInRange = false;
        projectTileCooldown = 0.2f;
        specialAttackCooldown = 60f;
        projectileTimer = new float[turrets.Length];
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && bossState == BossState.Aggressive)
        {
            AttackPlayer();
        }

        // for(int i = 0; i < turrets.Length; i++)
        // {
        //     Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        //     Vector3 thisPos = new Vector3(turrets[i].transform.position.x, turrets[i].transform.position.y, turrets[i].transform.position.z);
        //     Debug.DrawLine(playerPos, thisPos);
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;

            Random random = new Random();
            for(int i = 0; i < turrets.Length; i++)
            {
                projectileTimer[i] = (float)(random.NextDouble() * (projectTileCooldown));
                // projectileTimer[i] = 0.0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void AttackPlayer()
    {
        for(int i = 0; i < turrets.Length; i++)
        {
            if(projectileTimer[i] > projectTileCooldown)
            {
                // Debug.Log("Firing turret " + i);
                Vector3 playerAim = new Vector3(player.transform.position.x, player.transform.position.y+2, player.transform.position.z);
                Vector3 objLoc = new Vector3(turrets[i].transform.position.x, turrets[i].transform.position.y, turrets[i].transform.position.z);
                Vector3 aimDir = playerAim - objLoc;
                Instantiate(projectilePrefab, objLoc, Quaternion.LookRotation(aimDir, Vector3.up));
                // Debug.DrawLine(playerAim, objLoc);
                projectileTimer[i] = 0.0f;
            }
            else
            {
                projectileTimer[i] += Time.deltaTime;
            }
        }
        // if(specialAttackTimer > specialAttackCooldown)
        // {
        //     initiateSpecialAttack = true;
        // }
        // else
        // {
        //     specialAttackTimer += Time.deltaTime;
        // }
        // if(initiateSpecialAttack)
        // {
        //     if(specialAttackTimer == specialAttackCooldown + 5.0f)
        //     {
        //         Vector3 objLoc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        //         Vector3 aimLoc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-10);
        //         Vector3 aimDir = aimLoc - objLoc;
        //         Instantiate(specialProjectilePrefab, objLoc, Quaternion.LookRotation(aimDir, Vector3.up));
        //     }
        // }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            if(bossState == BossState.Passive && sceneName == "GreenPlanet")
            {
                bossState = BossState.Aggressive;
                SphereCollider trigger = this.transform.GetComponent<SphereCollider>();
                trigger.radius *= 3;
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                foreach(GameObject turret in turrets)
                {
                    turret.SetActive(true);
                    healthBar.SetActive(true);
                }
                //GameObject[] npcs = GameObject.FindGameObjectsWithTag("GreenNPC");
                //foreach(GameObject npc in npcs)
                //{
                //    npc.SetActive(false);
                //}
            }
        }
    }
}
