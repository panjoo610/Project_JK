﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public struct SavedItems
{
    public List<Item> items;
    public List<Item> equipmentItems;

    public int PlayerGold;
    public int CurrentStage;

    public List<int> DamageModifiers;
    public List<int> AromorModifiers;
}

[CreateAssetMenu(fileName = "SaveInventory", menuName = "Inventory/SaveInventory")]
public class SaveInventory : ScriptableObject
{
    SavedItems savedItems;

    public string JasonData;

    public List<Item> items;

    public List<Item> equipmentItems;

    public List<int> DamageModifiers;

    public List<int> AromorModifiers;

    public int PlayerGold;
    public int CurrentStage;

    public void SaveItemListByJson() // json문서로 내보내기
    {
        savedItems.items = items;
        savedItems.equipmentItems = equipmentItems;

        savedItems.PlayerGold = PlayerGold;
        savedItems.CurrentStage = CurrentStage;

        savedItems.AromorModifiers = AromorModifiers;
        savedItems.DamageModifiers = DamageModifiers;

        JasonData = JsonUtility.ToJson(savedItems);
        
        File.WriteAllText(Application.dataPath + "/ItemList.json", JasonData);
    }

    public void LoadItemListFromJson()
    {
        if (!File.Exists(Application.dataPath + "/ItemList.json"))
        {
            File.CreateText(Application.dataPath + "/ItemList.json");

            SaveItemListByJson();        
        }
        else
        {
            LoabJson();
        }
        EquimentManager.instance.EquipToSaveInven();
        PlayerManager.instance.UpdateStatusUI();
    }
    void LoabJson()
    {
        string load = File.ReadAllText(Application.dataPath + "/ItemList.json");
        var LoadData = JsonUtility.FromJson<SavedItems>(load);

        items = LoadData.items;
        equipmentItems = LoadData.equipmentItems;
        PlayerGold = LoadData.PlayerGold;
        CurrentStage = LoadData.CurrentStage;
        AromorModifiers = LoadData.AromorModifiers;
        DamageModifiers = LoadData.DamageModifiers;

        SaveItemListByJson();
    }
    public void ResetData()
    {
        PlayerGold = 0;
        CurrentStage = 0;

        items.Clear();
        equipmentItems.Clear();

        AromorModifiers.Clear();
        DamageModifiers.Clear();

        // ---------------------------
        savedItems.PlayerGold = 0;
        savedItems.CurrentStage = 0;

        savedItems.items.Clear();
        savedItems.equipmentItems.Clear();

        savedItems.AromorModifiers.Clear();
        savedItems.DamageModifiers.Clear();


        JasonData = JsonUtility.ToJson(savedItems);

        File.WriteAllText(Application.dataPath + "/ItemList.json", JasonData);
    }
}
