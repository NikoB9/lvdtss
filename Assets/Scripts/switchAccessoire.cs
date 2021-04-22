using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class switchAccessoire : MonoBehaviour
{
    public Sprite[] listOfImgs;
    public GameObject[] listOfObjects;
    public bool[] selected;

    public GameObject arFace;
    public GameObject arSessionOrigin;
    private ARFaceManager arFaceManager;
    private GameObject arSessionOriginInstance;


    private Hashtable objInstantiates = new Hashtable();

    public int index = 0;

    public Button prev;
    public Button next;
    public Button selectButton;

    private Color clickColor = new Color(0.58f, 0.7f, 0.6f, 1f);
    private Color baseColor = Color.white;

    followImage fi;


    private void Start()
    {
        fi = GetComponent<followImage>();

        selectButton.GetComponent<Image>().sprite = listOfImgs[index];

        makeFace();

        next.onClick.AddListener(nextImg);
        prev.onClick.AddListener(prevImg);
        selectButton.onClick.AddListener(selectObj);
    }

    void nextImg()
    {
        index++;
        if (index >= listOfImgs.Length)
        {
            index = 0;
        }

        selectButton.GetComponent<Image>().sprite = listOfImgs[index];
        colorImg();

    }

    void prevImg()
    {
        index--;
        if (index < 0)
        {
            index = listOfImgs.Length - 1;
        }

        selectButton.GetComponent<Image>().sprite = listOfImgs[index];
        colorImg();
    }

    public void goToSpecObj(GameObject prefab)
    {

        for(int i = 0; i < listOfObjects.Length; i++)
        {
            if (listOfObjects[i].name == prefab.name) index = i;
        }
         
        selectButton.GetComponent<Image>().sprite = listOfImgs[index];
        colorImg();
    }

    public void selectObj()
    {
        //on désactive tous les objets statiques
        foreach (GameObject go in fi.spawnedPrefabs.Values)
        {
            go.SetActive(false);
        }

        //on sélectionne l'objet à positionner
        selected[index] = !selected[index];
        colorImg();
        placeOrRemoveObject();
    }

    void colorImg()
    {
        selectButton.GetComponent<Image>().color = (selected[index]) ? clickColor : baseColor;
    }

    void placeOrRemoveObject()
    {

        if (selected[index])
        {
            GameObject copy = Instantiate(listOfObjects[index]) as GameObject;
            copy.transform.SetParent(arFace.transform, false);

            objInstantiates.Add(index, copy);
        }
        else
        {
            Destroy((GameObject)objInstantiates[index]);
            objInstantiates.Remove(index);
        }

        makeFace();
    }

    void makeFace()
    {
        if (arSessionOriginInstance != null)
            DestroyImmediate(arSessionOriginInstance);

        arSessionOriginInstance = Instantiate(arSessionOrigin);
        arSessionOriginInstance.AddComponent<ARFaceManager>().facePrefab = (GameObject) arFace;
    }

}
