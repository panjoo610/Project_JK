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

        ui = transform.GetChild(0).gameObject.transform;
    }
    void LateUpdate()
    {
        ui.transform.position = Camera.main.WorldToScreenPoint(sq.transform.position);
    }
}
