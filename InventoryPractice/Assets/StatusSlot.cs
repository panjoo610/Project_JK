using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusSlot : MonoBehaviour {

    public Text NameText,ExplainText;
    public Image Icon;

    public PlayerStatusData playerStatusData;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public void UpdateUI()
    {
        NameText.text = playerStatusData.StatusName;
        ExplainText.text = playerStatusData.StatusExplain;

        Icon.sprite = playerStatusData.icon;
    }
}
