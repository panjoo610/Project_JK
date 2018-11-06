using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixGameTitle : MonoBehaviour {

    Image gameTitleImage;
    void Start()
    {
        gameTitleImage = GetComponent<Image>();
        StartCoroutine(ClearFixTitle());
    }
    public IEnumerator ClearFixTitle()
    {
        yield return new WaitForSeconds(1.0f);
        gameTitleImage.raycastTarget = true;
    }
}
