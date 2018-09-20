using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadHealthUI : MonoBehaviour
{
    Transform cam;
    Transform ui;
    GameObject sq;

    float uiXPosition;
    float uiYPosition;
    void Start()
    {
        sq = transform.parent.gameObject;
        Debug.Log(sq);
        cam = Camera.main.transform;

        //uiXPosition = sq.transform.position.x;

        //uiYPosition = sq.transform.position.y;
        ui = transform.GetChild(0).gameObject.transform;
    }
    void LateUpdate()
    {
        //gameObject.transform.forward = new Vector3(cam.position.x, transform.rotation.y, -cam.position.z); ;// = new Vector3(cam.position.x, cam.position.y, -cam.position.z);
        ui.transform.position = Camera.main.WorldToScreenPoint(sq.transform.position);
    }
    
    


    //void OnGUI()
    //{

    //    GUI.Box(new Rect(uiXPosition - (healthBarLength / 2), uiYPosition, healthBarLength, 20), curHealth + "/" + maxHealth);

    //}
}
