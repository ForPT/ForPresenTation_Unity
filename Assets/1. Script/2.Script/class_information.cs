using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class class_information : MonoBehaviour
{
    public Text file_chul_text;
    public GameObject menu_Canvas, player, file_chul;

    public Vector3 seat_vector;
    public Vector3 presentation_vector;
    // Start is called before the first frame update
    void Start()
    {
        menu_Canvas = GameObject.Find("Menu_Canvas");
        player = GameObject.Find("player");
        file_chul = GameObject.Find("file_chul");

        if (PhotonNetwork.InRoom)
        {
            file_chul_text.text = PhotonNetwork.NetworkClientState.ToString() + "\n현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name + "\n현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount + "\n현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        else file_chul_text.text = "연결되지 않았습니다.";
        seat_vector = player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom) file_chul_text.text = PhotonNetwork.NetworkClientState.ToString() + "\n현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name + "\n현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount + "\n현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers;
        else file_chul_text.text = "연결되지 않았습니다.";
    }

    public void onClick_Exit()
    {
        menu_Canvas.SetActive(false);
        file_chul.SetActive(true);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobyScene");
    }

    public void onClick_Back_to_seat()
    {
        seat_vector.x = 24.232f;
        seat_vector.y = -5f;
        seat_vector.z = 6.7f;
        player.transform.position = seat_vector;
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void onClick_Go_to_presentation()
    {
        presentation_vector.x = 22.8f;
        presentation_vector.y = -4.8f;
        presentation_vector.z = 2.912f;
        player.transform.position = presentation_vector;
        player.transform.rotation = Quaternion.Euler(5, -20, 0);
    }
}