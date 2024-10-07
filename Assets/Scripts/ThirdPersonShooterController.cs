using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimCam;
    [SerializeField] GameObject crosshair;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private Transform debugTransform;
    [SerializeField] ThirdPersonController thirdPersonController;
    [SerializeField] Camera mainCam;

    StarterAssetsInputs starterAssetsInputs;

    Vector3 mouseWorldPosition;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        debugTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        mouseWorldPosition = Vector3.zero;

        if (starterAssetsInputs.aim)
        {
            aimCam.Priority = 21;
            crosshair.SetActive(true);
            thirdPersonController.SetRotateOnMove(false);
            debugTransform.gameObject.SetActive(true);

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = mainCam.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }
            // Debug.Log(mouseWorldPosition);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            //rotate player to mouse
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimCam.Priority = 1;
            crosshair.SetActive(false);
            debugTransform.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
        }

    }

    public Vector3 GetMouseWorldPosition()
    {
        return mouseWorldPosition;
    }
    public bool isAim()
    {
        return starterAssetsInputs.aim;
    }
}
