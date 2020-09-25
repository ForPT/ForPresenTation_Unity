using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Loginmanager : MonoBehaviour
{
    [Header("LoginPanel")]
    //아이디 인풋 필드
    public InputField InputField_ID;

    //ID넘기기용 변수
    public static string IDpass;

    //패스워드 인풋 필드
    public InputField InputField_Password;

    //상태 메시지 택스트
    public Text Text_message;


    public string LoginUrl;


    void Start()
    {
        Text_message.text = "";
        LoginUrl = "http://ec2-3-34-253-194.ap-northeast-2.compute.amazonaws.com/Login.php";
    }


    public void LoginBtn()
    {
        StartCoroutine(LoginCo());
    }


    IEnumerator LoginCo()
    {
        Debug.Log(InputField_ID.text);
        Debug.Log(InputField_Password.text);


        WWWForm form = new WWWForm();
        form.AddField("Input_ID", InputField_ID.text);
        form.AddField("Input_Password", InputField_Password.text);


        WWW webRequest = new WWW(LoginUrl, form);
        yield return webRequest;

        Debug.Log(webRequest.text);
        string result = webRequest.text.Trim();

        if (result == "1")
        {
            Text_message.text = "아이디를 다시 확인해주세요.";
        }

        else if (result == "3")
        {
            Text_message.text = "비밀번호를 다시 확인해주세요.";
        }

        else if (result == "2")
        {
            IDpass = InputField_ID.text;
            SceneManager.LoadScene("LobyScene");
        }

        else
        {
            Text_message.text = "네트워크 문제입니다. 네트워크를 상태를 확인해주세요.";
        }

    }

}