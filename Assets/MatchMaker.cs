using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchMaker : MonoBehaviourPunCallbacks
{

    public GameObject photonObject;

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");

        float randomX = Random.Range(-6f, 6f);

        PhotonNetwork.Instantiate(photonObject.name, new Vector3(randomX, 1f, 0f), Quaternion.identity, 0);
    }
}
