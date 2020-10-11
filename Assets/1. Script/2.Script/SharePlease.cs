using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Globalization;
using Photon.Pun;
using System;

public class SharePlease : MonoBehaviourPunCallbacks, IPunObservable
{
    int if_lastpage;
    
    [SerializeField]
    public RawImage rawImage;
    public String presentID;
    public int pptnum = 1;
    public int last_pptnum = 1;

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            presentID = Loginmanager.IDpass;
            Debug.Log(presentID);

            StartCoroutine(CoLoadImageTexture());
            Debug.Log("1");
            // 방에 들어가지는 시간차때문에 start함수가 불렸을땐 아직 방에 들어가기 전이다. 따라서 inroom이 된 후 masterclient를 불러와야한다.
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("d");

        if (stream.IsWriting)
        {
            stream.SendNext(presentID);
            stream.SendNext(pptnum);
            Debug.Log("2");
        }
        else if (stream.IsReading)
        {
            presentID = (String)stream.ReceiveNext();
            pptnum = (int)stream.ReceiveNext();
            Debug.Log("3");
        }
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                pptnum++;
                StartCoroutine(CoLoadImageTexture());
                // Debug.Log("4");
            }
        }
        else if (last_pptnum != pptnum) {
            StartCoroutine(CoLoadImageTexture());
            // Debug.Log("5");
        }
        last_pptnum = pptnum;
    }

    IEnumerator CoLoadImageTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://3.34.253.194:8000/media/files/ppts/" + presentID + "_" + MultiAccessManager.room_name + "_" + pptnum.ToString() + ".png");
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