using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;

[Serializable]
public struct Test
{
    public List<Item> items;
    public List<Item> equipmentItems;
}

[CreateAssetMenu(fileName = "SaveInventory", menuName = "Inventory/SaveInventory")]
public class SaveInventory : ScriptableObject
{
    Test test;

    public List<Item> items = new List<Item>();

    public List<Item> equipmentItems = new List<Item>();

    public string JasonData;

    public void SaveItemListByJson() // json문서로 내보내기
    {
        test.items = items;
        test.equipmentItems = equipmentItems;

        JasonData = JsonUtility.ToJson(test);


        File.WriteAllText(Application.dataPath + "/ItemList.json", JasonData);
    }

    public void LoadItemListFromJson()
    {
        string load = File.ReadAllText(Application.dataPath + "/ItemList.json");
        var LoadData = JsonUtility.FromJson<Test>(load);

        LoadData.items = items;
        LoadData.equipmentItems = equipmentItems;
    }

    public void LoadItemList() // json문서를 가져오기
    {
        if(File.Exists(Application.dataPath + "/Resource/Data/ItemList.json"))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/Data/ItemList.json");

            Debug.Log(jsonStr);

            JsonData ItemData = JsonMapper.ToObject(jsonStr);

            for(int i = 0; i < ItemData.Count; i++)
            {
                Debug.Log(ItemData[i]["id"]);
            }
        }
        else
        {
            Debug.Log("파일이 존재하지 않습니다.");
        }
    }
}
