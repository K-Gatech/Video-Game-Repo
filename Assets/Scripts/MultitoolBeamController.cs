using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultitoolBeamController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject toolBeam;
    public GameObject multiTool;
    public Transform muzzle;
    public GameObject flashLight;
    public GameObject projectilePrefab;

    [SerializeField] ThirdPersonShooterController shootController;
    [SerializeField] AudioSource shootclip;
    // public Image energyBar;

    private string[] beamTypes = {"Mine Beam"};
    private int beamIndex;
    private float projectTileTimer, projectTileCooldown;

    // public int toolEnergy, maxEnergy;
    
    private string currentBeamType;

    private bool isEquipped, isBeam, isFlash;

    private Material beamMaterial;

    void Start()
    {
        toolBeam.SetActive(false);
        multiTool.SetActive(true);
        flashLight.SetActive(false);
        isEquipped = true;
        isBeam = false;
        isFlash = false;
        beamIndex = 0;
        projectTileCooldown = 0.3f;
        // toolEnergy = 100;
        // maxEnergy = 100;
        shootclip = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        projectTileTimer += Time.deltaTime;

        // if(Input.GetKeyUp("e"))
        // {
        //     ToolEquipCheck();
        // }

        if(Input.GetKeyUp("z") && isEquipped)
        {
            SwitchBeam();
        }

        if(Input.GetKeyUp("q") && isEquipped)
        {
            SwitchMode();
        }

        if(Input.GetKeyUp("f") && isEquipped)
        {
            ToggleFlashlight();
        }

        if(Input.GetMouseButton(0) && isEquipped && shootController.isAim())
            StartTool();
        else
            StopTool();
    }

    void StartTool()
    {
        if(isBeam)
            toolBeam.SetActive(true);
        else
        {
            if(projectTileTimer > projectTileCooldown)
            {
                Vector3 aimDir = (shootController.GetMouseWorldPosition() - muzzle.position).normalized;
                Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(aimDir, Vector3.up));
                projectTileTimer = 0.0f;
                Debug.Log("Play shooting");
                shootclip.Play();
            }
        }
    }

    void StopTool()
    {
        if(isBeam)
            toolBeam.SetActive(false);
        else
        {
            // toolProjectile.SetActive(false);
        }
    }

    // void ToolEquipCheck()
    // {
    //     if(isEquipped)
    //         {
    //             isEquipped = false;
    //             multiTool.SetActive(isEquipped);
    //             Debug.Log("Stored Multitool");
    //         }
    //         else
    //         {
    //             isEquipped = true;
    //             multiTool.SetActive(isEquipped);
    //             Debug.Log("Equipped Multitool");
    //         }
    // }

    void SwitchBeam()
    {
        beamIndex = (beamIndex + 1) % beamTypes.Length;
        currentBeamType = beamTypes[beamIndex];
        beamMaterial = Resources.Load<Material>("Beam Materials/" + currentBeamType);
        toolBeam.GetComponent<Renderer>().material = beamMaterial;
        Debug.Log("Current Beam: " + currentBeamType);
    }

    void SwitchMode()
    {
        isBeam = !isBeam;
    }

    void ToggleFlashlight()
    {
        if(isFlash)
        {
            isFlash = false;
            flashLight.SetActive(false);
        }
        else
        {
            isFlash = true;
            flashLight.SetActive(true);
        }
    }

    // public void UpdateEnergyBar()
    // {
    //     energyBar.fillAmount = Mathf.Clamp(toolEnergy / maxEnergy, 0, 1f);
    //     Debug.Log(toolEnergy / maxEnergy);
    // }
}
