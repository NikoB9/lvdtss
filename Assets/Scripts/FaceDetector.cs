/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;

public class FaceDetector : MonoBehaviour
{
    private WebCamTexture _webCamTexture;
    private CascadeClassifier cascadeEyeClose;
    private CascadeClassifier cascadeSmile;
    private OpenCvSharp.Rect MyFace;
    public Text theText;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        _webCamTexture = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();
        cascadeEyeClose = new CascadeClassifier(Application.dataPath + @"/Detectors/haarcascade_lefteye_2splits.xml");
        cascadeSmile = new CascadeClassifier(Application.dataPath + @"/Detectors/haarcascade_smile.xml");
    }

    // Update is called once per frame
    void Update()
    {

        theText.text = "";

        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);


        findSmile(frame);
        findClosedEye(frame);
        //display(frame);
    }

    void findSmile(Mat frame)
    {

        //Convert image to grayscale

        var recs = cascadeSmile.DetectMultiScale(frame, 1.3, 5);

        if(recs.Length >= 1)
        {
            //Debug.Log(faces[0].Location);
            //MyFace = faces[0];
            theText.text += "- Smile detected\n";
        }
        else
        {
            theText.text += "- No smile\n";
        }
    }

    void findClosedEye(Mat frame)
    {

        //Convert image to grayscale

        var recs = cascadeEyeClose.DetectMultiScale(frame, 1.3, 5);

        if (recs.Length >= 1)
        {
            //Debug.Log(faces[0].Location);
            //MyFace = faces[0];
            theText.text += "- closed eye detected\n";
        }
        else
        {
            theText.text += "- No eye is closed\n";
        }
    }

    /*void display(Mat frame)
    {
        if(MyFace != null)
        {
            frame.Rectangle(MyFace, new Scalar(250, 0, 0), 2);

            Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
            GetComponent<Renderer>().material.mainTexture = newTexture;
        }
    }*/
//}
