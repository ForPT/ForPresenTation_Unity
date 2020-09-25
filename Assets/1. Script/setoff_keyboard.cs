using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setoff_keyboard : MonoBehaviour
{
    public GameObject keyboard;
    
    public void onClick_finish(){
        keyboard.SetActive(false);
    }

}
