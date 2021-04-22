using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class switchCamera : MonoBehaviour
{

    private ARCameraManager aRCameraManager;
    public Button switchCamBtn;
    private ARTrackedImageManager m_ARTrackedImageManager;
    followImage fi;

    // Start is called before the first frame update
    void Start()
    {
        fi = GetComponent<followImage>();
        switchCamBtn.onClick.AddListener(switchCam);
    }

    void switchCam()
    {
        aRCameraManager = FindObjectOfType<ARCameraManager>();
        m_ARTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        //on désactive tous les objets statiques
        foreach (GameObject go in fi.spawnedPrefabs.Values)
        {
            go.SetActive(false);
        }

        //On change la caméra de direction pour accéder soit au faceTracking soit à l'imagge tracking
        CameraFacingDirection newFacingDirection = CameraFacingDirection.User;

        switch (aRCameraManager.requestedFacingDirection)
        {
            case CameraFacingDirection.World:
                newFacingDirection = CameraFacingDirection.User;
                m_ARTrackedImageManager.enabled = false;
                break;
            case CameraFacingDirection.User:
                newFacingDirection = CameraFacingDirection.World;
                m_ARTrackedImageManager.enabled = true;
                break;
        }

        aRCameraManager.requestedFacingDirection = newFacingDirection;
    }
}
