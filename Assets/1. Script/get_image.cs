using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Globalization;

public class get_image : MonoBehaviour
{
    public int pptnum = 1;
    int if_lastpage;
    public RawImage rawImage;

    void Start () {
        StartCoroutine(CoLoadImageTexture());
    }
    void Update() {
        if(OVRInput.GetDown(OVRInput.Button.One)){
            pptnum++;
            StartCoroutine(CoLoadImageTexture());
        }
    
    }

    IEnumerator CoLoadImageTexture()
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://3.34.253.194:8000/media/files/ppts/" + Loginmanager.IDpass + "_" + MultiAccessManager.room_name + "_" + pptnum.ToString() + ".png");
        yield return www.SendWebRequest();
        Debug.Log(MultiAccessManager.room_name);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            pptnum = 1;
            StartCoroutine(CoLoadImageTexture());
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
