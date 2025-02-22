using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;
    
    void Start()
    {
        startPos = transform.position.x;
    }

    void Update()
    {
        
        if (cam != null)
        {
            
            float distance = cam.transform.position.x * parallaxEffect;

            
            transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        }
        else
        {
            Debug.LogError("camera isn't assigned");
        }
    }

}
