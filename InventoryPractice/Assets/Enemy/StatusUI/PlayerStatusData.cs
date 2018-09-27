using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "Data/Status")]
public class PlayerStatusData : ScriptableObject
{
    public string StatusName = "New Status";
    public string StatusExplain = "Test Status";
    public Sprite icon = null;
    public float Damege = 0f;
    public float Armor = 0f;
    public int Price = 0;


    public void Increase()
    {
        Debug.Log("increase " + name +" "+Armor);
        Price += Price;
    }

    
}
