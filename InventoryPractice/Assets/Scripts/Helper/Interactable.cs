using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius = 10.0f;
    public Transform interactionTransform;
    bool isFocus = false;

    bool hasInteracted = false;

    public virtual void Interact()
    {
        
    }

    public void Update()
    {
        if (isFocus && !hasInteracted)
        {
            hasInteracted = true;
            Interact();
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        hasInteracted = false;
    }

    public void OnDeFocused()
    {
        isFocus = false;
        hasInteracted = false;
    }
}
