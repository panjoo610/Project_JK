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
        Debug.Log("Interacting with "+ transform.name);
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

    public void OnDrawGizomosSelected()
    {
        if(interactionTransform == null)
        {
            interactionTransform = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.transform.position, radius);
    }
}
