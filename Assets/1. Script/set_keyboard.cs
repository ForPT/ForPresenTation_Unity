using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_keyboard : MonoBehaviour
{
    public GameObject keyboard;
    
    public void onClick_inputfield(){
        keyboard.SetActive(true);
    }

}
