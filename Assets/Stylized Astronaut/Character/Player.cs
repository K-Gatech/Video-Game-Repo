using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {


	public enum GameTransitionState
    {
        // DesertPlanet
        Started,
        TalkedToAlien,
        ReachedMaze,
        FinishedMaze,
        FoundTreasure1,
        CameToAlienAgain,
        TalkedToAlienAgain,
        FoundTreasure2,
        CameToAlienYetAgain,
        PaidToAlien
    }

	public GameObject spaceShip;
	public ParticleSystem walkParticle;
	public Transform cam;

	public bool isTransitioningToOverviewScreen = false;
	public float speed = 30f;
	public float turnSpeed = 1f;
	public float jumpForce = 20f;
	public float gravity = 20.0f;
	public float turnSmoothTime = 0.1f;

	//Teleport
	public Transform teleport;
	[SerializeField] Player player;

	// Ground check variables
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundRadius;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] public bool isGrounded;
	StarterAssetsInputs starterAssetsInputs;

	private Animator anim;
	private CharacterController controller;
	private AudioSource jumpSound;
	private float turnSmoothVelocity;
	private float directionY;
	private Vector3 moveDirection;
	private bool isWalkParticleEnabled = false;
	private bool guardianDesire = false;
	private bool repairKit = false;
	static string SavePath;
	
	bool init = false;
	
	void initClass() {
		try {
			Vector3 playerPos = SaveGameDesert.GetSavedPlayerPosition();
			if (playerPos.x != int.MaxValue)
			{
				transform.position = SaveGameDesert.GetSavedPlayerPosition();
			}
		} catch (Exception e) {
			Debug.LogError("Unable to read player's position from the savedata file");
		}  
		controller = GetComponent <CharacterController>();
		starterAssetsInputs = GetComponent<StarterAssetsInputs>();
		anim = gameObject.GetComponentInChildren<Animator>();
		jumpSound = gameObject.GetComponent<AudioSource>();
		spaceShip = GameObject.FindGameObjectWithTag("Spaceship");
		spaceShip.SetActive(false);
		guardianDesire = false;
		repairKit = false;
		SavePath = Path.Combine(Application.persistentDataPath, "savedata.txt");
	}

    void Update (){
		if (!init) {
			initClass();
			init = true;
			
			return;
		}
		
		float horizontal = starterAssetsInputs.move.x;
		float vertical = starterAssetsInputs.move.y;

		isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        //Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //anim.SetFloat("Speed", direction.magnitude);

        // if grounded, always take new direction.
        // if not grounded, and no input, keep direction
        //if (isGrounded) moveDirection = direction;
        //else
        //{
        //	if(direction.magnitude >= 0.1f) moveDirection = direction;
        //}
        //if (direction.magnitude >= 0.1f)
        //{
        //	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //	transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //	// get move from controller and mouse movements, exclude jump and gravity
        //	moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //}

        //// handle jumping
        //if (isGrounded)
        //{
        //	if (Input.GetButtonDown("Jump"))
        //	{
        //		Debug.Log("Jumped");
        //		jumpSound.Play();
        //		directionY = jumpForce;
        //	}
        //}

        CheckForOverviewTransition();
		CheckForLava();

		//// handle y direction due to jump and gravity
		//directionY -= gravity * Time.deltaTime;
		//moveDirection.y = directionY;

		//controller.Move(moveDirection * speed * Time.deltaTime);

		if (isGrounded && Mathf.Abs(vertical) + Mathf.Abs(horizontal) != 0)
		{
			EnableWalkParticle();
		}
		else
		{
			DisableWalkParticle();
		}

	}

    private void CheckForOverviewTransition()
    {
		if (!isTransitioningToOverviewScreen && Input.GetKeyDown(KeyCode.V))
		{
			if(GetCurrentState() == GameTransitionState.PaidToAlien && SceneManager.GetActiveScene().name == "DesertPlanet")
			{
				spaceShip.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
				spaceShip.SetActive(true);
				isTransitioningToOverviewScreen = true;
			}
		}

		if (isTransitioningToOverviewScreen && Input.GetKeyDown(KeyCode.Escape))
		{
			isTransitioningToOverviewScreen = false;
		}
	}

	private void CheckForLava()
    {
		if (transform.position.y < 0 && SceneManager.GetActiveScene().name == "DesertPlanet")
        {
			transform.GetChild(1).GetComponent<PlayerHealthController>().ChangeHealth(-0.2f);
			
		}

		if (transform.position.y < -0.5 && SceneManager.GetActiveScene().name == "DoomPlanet")
		{
			transform.GetChild(1).GetComponent<PlayerHealthController>().ChangeHealth(-0.05f);

		}
	}

	private void EnableWalkParticle()
    {
		if (isWalkParticleEnabled)
			return;

		isWalkParticleEnabled = true;
		walkParticle.Play();
    }

	private void DisableWalkParticle()
    {
		if (!isWalkParticleEnabled)
			return;

		isWalkParticleEnabled = false;
		walkParticle.Stop();
	}

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Projectile"))
		{
			transform.GetChild(1).GetComponent<PlayerHealthController>().ChangeHealth(-5f);
		}
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Potion") {
			transform.GetChild(1).GetComponent<PlayerHealthController>().ChangeHealth(30f);
			Destroy(c.gameObject);
		}
	}

	//Inventory health increase button
	public void IncreaseHealth()
    {
		transform.GetChild(1).GetComponent<PlayerHealthController>().ChangeHealth(30f);
	}

	public bool HasGuardianDesire()
    {
		return guardianDesire;
    }
	public void GetGuardianDesire()
	{
		guardianDesire = true;
	}
	public bool HasRepairKit()
	{
		return repairKit;
	}
	public void GetRepairKit()
	{
		repairKit = true;
	}

    private void LateUpdate()
    {
		if (Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log("T is pressed");
			GetComponent<Player>().transform.position = teleport.transform.position;
		}
	}
	private static List<string> ReadSaveData()
    {
        var result = new List<string>();
        if (File.Exists(SavePath))
        {
            StreamReader reader = new StreamReader(SavePath);
            while(!reader.EndOfStream)
            {
                result.Add(reader.ReadLine());
            }

            reader.Close();
            return result;
        }

        return null;
    }

	public static GameTransitionState GetCurrentState()
    {
		List<string> SavedDataCache = null;

        if (SavedDataCache == null)
        {
            SavedDataCache = ReadSaveData();
        }

        if (SavedDataCache == null)
        {
            return GameTransitionState.Started;
        }

        GameTransitionState result;
        if (Enum.TryParse(SavedDataCache[1].Split(':')[1], out result))
        {
            return result;
        }
        else
        {
            return GameTransitionState.Started;
        }
    }
}
