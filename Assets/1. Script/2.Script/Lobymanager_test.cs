using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Lobymanager_test : MonoBehaviour
{
    //방 버튼 배열
    public GameObject[] room_GameObject_array = new GameObject[6]; 
    //개인정보 인풋필드
    public InputField InputField_Info;

    public string Personal_Info_Url;
    public string Managed_ClassUrl;
    public string Joined_ClassUrl;

    public string[] managedClass_array;
    public string[] joinedClass_array;
    //버튼 텍스트
    public Text[] button = new Text[6];
    char separatorChar = '/';

    public Button PreviousBtn, NextBtn;

    public int page = 1, maxPage, multiple, managed, joined, sum;

    void Start()
    {
        room_GameObject_array[0] = GameObject.Find("room0");
        room_GameObject_array[1] = GameObject.Find("room1");
        room_GameObject_array[2] = GameObject.Find("room2");
        room_GameObject_array[3] = GameObject.Find("room3");
        room_GameObject_array[4] = GameObject.Find("room4");
        room_GameObject_array[5] = GameObject.Find("room5");

        //개인정보 읽어오기
        Personal_Info_Url = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Personal_Info.php";
        StartCoroutine(Personal_Info());

        //관리클레스 정보 읽어오기
        Managed_ClassUrl = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Managed_Class.php";
        //참여클레스 정보 읽어오기
        Joined_ClassUrl = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Joined_Class.php";
        StartCoroutine(Class());
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

        managed = managedClass_array.Length;
        joined = joinedClass_array.Length;
        sum = managed + joined;
        maxPage = 1 + (sum / 6);

        PreviousBtn.interactable = (page <= 1) ? false : true;
        NextBtn.interactable = (page >= maxPage) ? false : true;

        for(int i = 0; i < 6; i++){
            if(page == maxPage){
                if(i < (sum % 6))
                {
                    make_button(i);
                }
                else{
                    room_GameObject_array[i].SetActive(false);
                }
            }
            else{
                make_button(i);
            }
        }
    }

    public void make_button(int i){
        room_GameObject_array[i].SetActive(true);
        room_GameObject_array[i].GetComponent<Outline>().enabled = true;
        if(managed > (page-1)*6 + i){
            button[i].text = managedClass_array[(page-1)*6 + i];
            room_GameObject_array[i].GetComponent<Outline>().effectColor = Color.blue;
        }
        else{
            button[i].text = joinedClass_array[(page-1)*6 + i - managed];
            room_GameObject_array[i].GetComponent<Outline>().effectColor = Color.green;
        }
        
    }
    public void Back_Btn()
    {
        page = page - 1;
        //else print(classList[multiple + num]);
        StartCoroutine(Class());
    }
    public void Next_Btn()
    {
        page = page + 1;
        //else print(classList[multiple + num]);
        StartCoroutine(Class());
    }

}