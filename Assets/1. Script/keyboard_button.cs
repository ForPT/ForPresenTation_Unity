using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class keyboard_button : MonoBehaviour
{
    public InputField InputField_ID;
    public InputField InputField_Password;
    public TextMeshProUGUI alphabet;
    public int id_or_pass;

    void start(){
        id_or_pass = GameObject.Find("Button_IDinput").GetComponent<IDorPassInput>().id_or_pass;
    }
    public void onClick_key(){
        if(GameObject.Find("Button_IDinput").GetComponent<IDorPassInput>().id_or_pass == 0){
            InputField_ID.text = InputField_ID.text + alphabet.text;
        }else{
            InputField_Password.text = InputField_Password.text + alphabet.text;
        }
    }
    public void onClick_space(){
        if(GameObject.Find("Button_IDinput").GetComponent<IDorPassInput>().id_or_pass == 0){
            InputField_ID.text = InputField_ID.text + " ";
        }else{
            InputField_Password.text = InputField_Password.text + " ";
        }
    }

    public void onClick_backspace(){
        if(GameObject.Find("Button_IDinput").GetComponent<IDorPassInput>().id_or_pass == 0){
            if(InputField_ID.text.Length > 0){
                InputField_ID.text = InputField_ID.text.Substring(0,InputField_ID.text.Length-1);
            }
        }else{
            if(InputField_Password.text.Length > 0){
                InputField_Password.text = InputField_Password.text.Substring(0,InputField_Password.text.Length-1);
            }
        }   
    }
}
