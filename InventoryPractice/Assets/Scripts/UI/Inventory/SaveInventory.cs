using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;



[CreateAssetMenu(fileName = "SaveInventory", menuName = "Inventory/SaveInventory")]
public class SaveInventory : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public List<Item> equipmentItems = new List<Item>();

    public void SaveItemList() // json문서로 내보내기
    {
        JsonData infoJson = JsonMapper.ToJson(items);

        File.WriteAllText(Application.dataPath + "/Resources/Data/ItemList.json", infoJson.ToString());
    }
    public void LoadItemList() // json문서를 가져오기
    {
        if(File.Exists(Application.dataPath + "/Resources/Data/ItemList.json"))
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
