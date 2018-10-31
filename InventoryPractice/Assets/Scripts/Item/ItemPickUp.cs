﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;
    public GameObject particle, itemMesh;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }
    void PickUp()
    {
        bool wasPickedUp = InventoryManager.instance.Add(item);

        if (wasPickedUp)
        {        
            Vector3 playerPosition = new Vector3(PlayerManager.instance.Player.transform.position.x, PlayerManager.instance.Player.transform.position.y + 1.0f, PlayerManager.instance.Player.transform.position.z);
            iTween.MoveTo(gameObject, iTween.Hash("position", playerPosition, "easeType", iTween.EaseType.linear, "oncomplete", "Destroy", "time", 0.1f));
        }
            
    }

    void Destroy()
    {
        StartCoroutine(DestroyItem());
    }

    IEnumerator DestroyItem()
    {
        particle.SetActive(true);
        Destroy(itemMesh);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

}
