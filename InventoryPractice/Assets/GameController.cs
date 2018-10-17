using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject playerObject, cameraObject, canvasObject;

    #region Singleton
    public static GameController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion


    // Update is called once per frame
    public void DestoryObject ()
    {
        if(playerObject != null)
        {
            Destroy(playerObject);
        }
        if (cameraObject != null)
        {
            Destroy(cameraObject);
        }
        if (canvasObject != null)
        {
            Destroy(canvasObject);
        }
    }
}
