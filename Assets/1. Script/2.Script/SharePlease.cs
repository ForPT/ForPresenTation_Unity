using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class SharePlease : MonoBehaviourPun, IPunObservable
{

    [SerializeField]
    public RawImage pleaseImage;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            pleaseImage = get_image.sendImage;
            stream.SendNext(pleaseImage);
        }
        else if (stream.IsReading)
        {
            pleaseImage = (RawImage)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        if(PhotonView.IsMine)
        {

        }
        pleaseImage = GetComponent<RawImage>();
    }
}
