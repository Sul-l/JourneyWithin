using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviour
{


    Vector2 resolution;

    void Start()
    {
        resolution = new Vector2(Screen.width, Screen.height);  
    }



    void Update()
    {
        FollowMouse();   
    }


private void FollowMouse()
{
    Debug.Log(Input.mousePosition/resolution); 
}

}