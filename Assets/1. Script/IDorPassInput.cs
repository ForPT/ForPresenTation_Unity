using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDorPassInput : MonoBehaviour
{
    public int id_or_pass;
    //0 is ID input
    //1 is Password input 

    void start(){
        id_or_pass = 0;
    } 

    public void onClick_login(){
        id_or_pass = 0;
    }
    public void onClick_Password(){
        id_or_pass = 1;
    }
}
