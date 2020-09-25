using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Lobymanager : MonoBehaviour
{
    public GameObject[] room_GameObject_array = new GameObject[6];
    //개인정보 인풋필드
    public InputField InputField_Info;

    public string Personal_Info_Url;
    public string Managed_ClassUrl;
    public string Joined_ClassUrl;

    public Button[] RoomBtn;
    public string[] managedClass_array;
    public string[] joinedClass_array;
    public Text[] button = new Text[6];
    char separatorChar = '/';

    public Button PreviousBtn, NextBtn;

    int page = 1, maxPage, multiple;

    void Start()
    {
        room_GameObject_array[0] = GameObject.Find("room0");
        room_GameObject_array[1] = GameObject.Find("room1");
        room_GameObject_array[2] = GameObject.Find("room2");
        room_GameObject_array[3] = GameObject.Find("room3");
        room_GameObject_array[4] = GameObject.Find("room4");
        room_GameObject_array[5] = GameObject.Find("room5");

        /*for (int i = 0; i < 6; i++)
        {
            room_GameObject_array[i].SetActive(false);
        }*/

        //개인정보 읽어오기
        Personal_Info_Url = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Personal_Info.php";
        StartCoroutine(Personal_Info());

        //관리클레스 정보 읽어오기
        Managed_ClassUrl = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Managed_Class.php";
        //참여클레스 정보 읽어오기
        Joined_ClassUrl = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Joined_Class.php";
        StartCoroutine(Class());

        int managed = managedClass_array.Length;
        int joined = joinedClass_array.Length;
        int sum = managed + joined;

        maxPage = (sum % room_GameObject_array.Length == 0) ? sum / room_GameObject_array.Length : sum / room_GameObject_array.Length + 1;

        PreviousBtn.interactable = (page <= 1) ? false : true;
        NextBtn.interactable = (page >= maxPage) ? false : true;

        multiple = (page - 1) * room_GameObject_array.Length;
        
        for (int i = 0; i < room_GameObject_array.Length; i++)
        {
            if (multiple + i < sum)
            {
                room_GameObject_array[i].SetActive(true);
            }
            else
            {
                room_GameObject_array[i].SetActive(false);
            }
            
            //room_GameObject_array[i].interactable = (multiple + i < managed) ? true : false;
        }

    }

    IEnumerator Personal_Info()
    {
        Debug.Log(Loginmanager.IDpass);


        WWWForm form1 = new WWWForm();
        form1.AddField("Input_ID", Loginmanager.IDpass);


        WWW personalData = new WWW(Personal_Info_Url, form1);
        yield return personalData;


        Debug.Log(personalData.text);
        string info = personalData.text.Trim();
        InputField_Info.text = (Loginmanager.IDpass + " " + info);
    }

    IEnumerator Class()
    {
        Debug.Log(Loginmanager.IDpass);
        WWWForm form2 = new WWWForm();
        form2.AddField("Input_ID", Loginmanager.IDpass);


        WWW classData = new WWW(Managed_ClassUrl, form2);
        yield return classData;

        Debug.Log(classData.text);
        string managing_room = classData.text.Trim();
        managedClass_array = managing_room.Split(separatorChar);


        WWW joined_class = new WWW(Joined_ClassUrl, form2);
        yield return joined_class;

        Debug.Log(joined_class.text);
        string joined_room = joined_class.text.Trim();
        joinedClass_array = joined_room.Split(separatorChar);


        int managed = managedClass_array.Length;
        int joined = joinedClass_array.Length;
        int next_index = managed % 6;

        for (int i = 0; i < managed; i++)
        {
            int managed_index = i % 6;
            if (i != 0 && i % 6 == 0)
            {
                //room_GameObject_array[i].SetActive(false);
                Back_Forth_Btn(-1);
            }
            room_GameObject_array[managed_index].SetActive(true);
            button[managed_index].text = managedClass_array[i];
            room_GameObject_array[managed_index].GetComponent<Outline>().enabled = true;
            room_GameObject_array[managed_index].GetComponent<Outline>().effectColor = Color.blue;
        }

        for (int j = 0; j < joined; j++)
        {
            int joined_index = (next_index + j) % 6;
            if (managed != 0 && joined_index % 6 == 0) // logical error ; think again
            {
                //room_GameObject_array[j].SetActive(false);
                Back_Forth_Btn(-1);
            }
            room_GameObject_array[joined_index].SetActive(true);
            button[joined_index].text = joinedClass_array[j];
            room_GameObject_array[joined_index].GetComponent<Outline>().enabled = true;
            room_GameObject_array[joined_index].GetComponent<Outline>().effectColor = Color.green;
        }


    }


    //◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void Back_Forth_Btn(int num)
    {
        if (num == -2)
        {
            --page;
        }
        else if (num == -1)
        {
            ++page;
        }
        //else print(classList[multiple + num]);
        Start();
    }
}