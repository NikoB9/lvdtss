using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class followImage : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private Text debug;

    [SerializeField]
    private GameObject[] placeablePrefabs;
    public Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private GameObject selectedPrefab;

    switchAccessoire sa;

    bool objSelect = false;

    private void Awake()
    {
        //on récupère ce dont on a besoin
        debug = FindObjectOfType<Text>(); 

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        sa = GetComponent<switchAccessoire>();

        //on prépare les objets
        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            if (prefab.name == "stpatrickhat") newPrefab.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);

            if (prefab.name == "CowboyHat_OBJ") newPrefab.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

            if (prefab.name == "earrings_left_cata")
            {
                newPrefab.name = "pair_earrings";
                newPrefab.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                spawnedPrefabs.Add("pair_earrings", newPrefab);
            }
            else
            {
                newPrefab.name = prefab.name;
                spawnedPrefabs.Add(prefab.name, newPrefab);
            }
            
            //on désactive les objets pour le moment
            newPrefab.SetActive(false);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        if (!objSelect)
        {
            //debug.text = "Image traking : \n";

            foreach (ARTrackedImage trackedImage in eventArgs.added)
            {
                //debug.text = trackedImage.referenceImage.name + " added\n";
                UpdateImage(trackedImage);
            }
            foreach (ARTrackedImage trackedImage in eventArgs.updated)
            {
                //debug.text = trackedImage.referenceImage.name + " updated\n";
                UpdateImage(trackedImage);
            }
            foreach (ARTrackedImage trackedImage in eventArgs.removed)
            {
                //debug.text = trackedImage.referenceImage.name + " removed\n";
                spawnedPrefabs[trackedImage.name].SetActive(false);
            }
        }
        
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        //Vector3 position = trackedImage.transform.position;
        float distance = 0.5f;

        GameObject prefab = spawnedPrefabs[name];
        selectedPrefab = prefab;
        prefab.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        prefab.SetActive(true);

        //debug.text = name + " == " + prefab.name + " ; pos : " + prefab.transform.position;

        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (go.name != name)
            {
                go.SetActive(false);
            }
        }

        sa.goToSpecObj(prefab);
    }

   

    private void Update()
    {

        //si on touche l'objet en exposition on dit qu'il est sélectionné
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            //debug.text = "click but where?\n";

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name == selectedPrefab.name)
            {
                objSelect = true;
            }
           /* else
            {
                //debug.text = "click on :"+ hit.transform.gameObject.name + "\n";

            }*/
        }

        //si l'objet est sélectionné on le fait tourner avant de le positionner sur nous
        if (objSelect)
        {
            if (selectedPrefab.name == "stpatrickhat")
            {
                selectedPrefab.transform.Rotate(0, 0, 90.0f * Time.deltaTime, Space.Self);

                if (selectedPrefab.transform.rotation.z < 0)
                {
                    objSelect = false;
                    sa.selectObj();
                }
                /*else
                {
                    debug.text = "rotation = " + selectedPrefab.transform.rotation.z + "\n";
                }*/
            }
            else
            {
                selectedPrefab.transform.Rotate(0, 90.0f * Time.deltaTime, 0, Space.Self);

                if (selectedPrefab.transform.rotation.y < 0)
                {
                    objSelect = false;
                    sa.selectObj();
                }
            }


        }
    }
}
