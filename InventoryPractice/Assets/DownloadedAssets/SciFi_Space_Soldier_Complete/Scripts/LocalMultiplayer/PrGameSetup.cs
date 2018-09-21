using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrGameSetup : MonoBehaviour {

    public enum GameMode
    {
        SinglePlayer,
        Coop,
        DeathMatch,
        TeamDeathMatch,
        TowerDefense
    }

    [Header("Game Setup")]
    public GameMode mode = GameMode.Coop;
    public int actualPlayerCount = 4;
    public GameObject[] playersPrefabs;
    private GameObject[] actualPlayerPrefabs;
    private PrTopDownCharController[] playersControllers;
    private PrTopDownCharInventory[] playersInventorys;
    private GameObject[] playersForCamera;
    public Transform[] playersSpawnPos;
    public bool[] spawnPointFull;
    public PrPlayerSettings playersSettings;

    public enum GameStage
    {
        inGame,
        EndedMatch,
    }

    [Header("In Game Stats")]
    public GameStage stage = GameStage.inGame;

    [Header("DeathMatch Setup")]
    public int fragsToWin = 10;
    public GameObject[] fragCounter;
    private int[] playersFrags;
    public GameObject playerWinsText;
    public PrPlayerSettings playerSettings;

    [Header("Camera Setup")]
    public bool useSplitScreen = false;
    public bool useSingleScreenCameraLimits = false;
    public GameObject multiplayerCam;
    public Vector2 targetHeightVariation = new Vector2(8, 50);
    public float targetHeightDistanceFactor = 1.0f;
    public float targetHeightCorrection = -3.0f;
    [HideInInspector]
    public PrTopDownMutiplayerCam actualCameraScript;

    [Header("Debug")]
    
    public Mesh areaMesh;
    public Mesh targetArrow;

    // Use this for initialization
    void Start () {

        //Set initialarrays
        playersControllers = new PrTopDownCharController[4];
        playersInventorys = new PrTopDownCharInventory[4];
        actualPlayerPrefabs = new GameObject[4];
        spawnPointFull = new bool[playersSpawnPos.Length];
        
        //Start Game Setup
        StartGame();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F5))
        {
            NewMultiplayerMatch(1);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            NewMultiplayerMatch(2);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            NewMultiplayerMatch(3);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            NewMultiplayerMatch(4);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (useSplitScreen)
                useSplitScreen = false;
            else
                useSplitScreen = true;

            NewMultiplayerMatch(actualPlayerCount);
        }

        if (mode == GameMode.DeathMatch || mode == GameMode.TeamDeathMatch)
        {
            if (Input.GetButtonDown("Player1Start") && actualPlayerCount >= 1)
            {
                if (stage == GameStage.EndedMatch)
                {
                    NewMultiplayerMatch(actualPlayerCount);
                }
                else
                {
                    if (playersInventorys[0].isDead == true)
                    {
                        DestroyPlayer(0);
                        SpawnPlayer(0, true);
                        PlayerSpawned();
                    }
                }
                    
            }

            if (Input.GetButtonDown("Player2Start") && actualPlayerCount >= 2)
            {
                if (stage == GameStage.EndedMatch)
                {
                    NewMultiplayerMatch(actualPlayerCount);
                }
                else
                {
                    if (playersInventorys[1].isDead == true)
                    {
                        DestroyPlayer(1);
                        SpawnPlayer(1, true);
                        PlayerSpawned();
                    }
                }

            }
            if (Input.GetButtonDown("Player3Start") && actualPlayerCount >= 3)
            {
                if (stage == GameStage.EndedMatch)
                {
                    NewMultiplayerMatch(actualPlayerCount);
                }
                else
                {
                    if (playersInventorys[2].isDead == true)
                    {
                        DestroyPlayer(2);
                        SpawnPlayer(2, true);
                        PlayerSpawned();
                    }
                }
            }
            if (Input.GetButtonDown("Player4Start") && actualPlayerCount >= 4)
            {
                if (stage == GameStage.EndedMatch)
                {
                    NewMultiplayerMatch(actualPlayerCount);
                }
                else
                {
                    if (playersInventorys[3].isDead == true)
                    {
                        DestroyPlayer(3);
                        SpawnPlayer(3, true);
                        PlayerSpawned();
                    }
                }
            }
        }
        
        if (spawnPointFull.Length > 0 && playersSpawnPos.Length > 0)
        {
            for (int i = 0; i < spawnPointFull.Length; i++)
            {
                spawnPointFull[i] = playersSpawnPos[i].GetComponent<PrSpawnPoint>().isFull;
            }
        }
    }

    void NewMultiplayerMatch(int playerCount)
    {
        for (int i = 0; i < 4; i++)
        {
            if (actualPlayerPrefabs[i] != null)
                DestroyPlayer(i);
            
            if (playersInventorys[i] != null)
                playersInventorys[i].DestroyHUD();

        }

        actualPlayerCount = playerCount;
        StartGame();
        
    }
    /*
    void HideSpawnPoints()
    {
        foreach (Transform spawn in playersSpawnPos)
        {
            spawn.gameObject.SetActive(false);
        }
    }*/

    void StartGame()
    {
        playersFrags = new int[4];
        for (int i = 0; i < 4; i++)
        {
            playersFrags[i] = 0;
        }
     
        stage = GameStage.inGame;

        if (playersPrefabs.Length >= actualPlayerCount && playersSpawnPos.Length >= actualPlayerCount)
        {
            for (int x = 0; x < actualPlayerCount; x++)
            {
                SpawnPlayer(x, false);
            }

        }

        if (mode == GameMode.DeathMatch || mode == GameMode.TeamDeathMatch | mode == GameMode.Coop)
        {
            if (playerWinsText)
                playerWinsText.SetActive(false);

            CreateCamera();//Create the Camera

        }

        //HUD Reset
        if (mode == GameMode.DeathMatch || mode == GameMode.TeamDeathMatch )
        {
            ResetFragHUD();
            UpdateFragHUD();
            OrganizeFragHUD();

        }
        
    }

    void SetPlayersForCamera()
    {
        int playerCount = 0;

        for (int i = 0; i < actualPlayerCount; i++)
        {
            if (playersInventorys[i].isDead)
            {
                //DoNothing
            }
            else
            {
                playerCount += 1; 
            }
        }

        if (playerCount > 0)
        {
            playersForCamera = new GameObject[playerCount];

            int finalCount = 0;
            for (int i = 0; i < actualPlayerCount; i++)
            {
                if (playersInventorys[i].isDead)
                {
                    //DoNothing
                }
                else
                {
                    playersForCamera[finalCount] = playersInventorys[i].gameObject;
                    finalCount += 1;
                }
            }
        }
        
    }

    void CreateCamera()
    {
        if (useSplitScreen)
        {
            //waits until players are created
        }
        else
        {
            if (!actualCameraScript)
            {
                GameObject actualCameraGO = Instantiate(multiplayerCam, transform.position, Quaternion.Euler(0, 45, 0)) as GameObject;
                actualCameraGO.transform.parent = this.transform;
                actualCameraGO.name = "MutiplayerCamera";
                actualCameraScript = actualCameraGO.GetComponent<PrTopDownMutiplayerCam>();
            }

            SetPlayersForCamera();
            actualCameraScript.MultiplayerCam(playersForCamera, actualPlayerCount);
            actualCameraScript.ResetWalls();
            actualCameraScript.targetHeightVariation = targetHeightVariation;
            actualCameraScript.targetHeightDistanceFactor = targetHeightDistanceFactor;
            actualCameraScript.targetHeightCorrection = targetHeightCorrection;
            actualCameraScript.useCameraColisions = useSingleScreenCameraLimits;


        }
        
    }

    void DestroyPlayer(int playerNumber)
    {
        Destroy(actualPlayerPrefabs[playerNumber]);
    }

    int RandomNum(int lastRandNum)
    {
        int randNum = Random.Range(0, playersSpawnPos.Length);
        
        return randNum;
    }
    
    void SpawnPlayer(int playerNumber, bool randomPos)
    {
        int posInt = playerNumber;

        if (randomPos)
        {
            posInt = RandomNum(posInt);
            int tries = 0;
            while (spawnPointFull[posInt] == true && tries < 12)
            {
                posInt = RandomNum(posInt);
                tries += 1;
            }
            
        }

        //Instantiate player Prefab in Scene
        GameObject tempPlayer = Instantiate(playersPrefabs[playerNumber], playersSpawnPos[posInt].position, playersSpawnPos[posInt].rotation) as GameObject;
        tempPlayer.transform.parent = this.transform;
        actualPlayerPrefabs[playerNumber] = tempPlayer;
        playersControllers[playerNumber] = tempPlayer.transform.GetComponentInChildren<PrTopDownCharController>();
        playersInventorys[playerNumber] = tempPlayer.transform.GetComponentInChildren<PrTopDownCharInventory>();

        playersControllers[playerNumber].playerNmb = playerNumber + 1;
        //set split screen var
        playersInventorys[playerNumber].SetSplitScreen(useSplitScreen, actualPlayerCount);
        
        //Sets player Team settings
        if (mode == GameMode.DeathMatch)
        {
            playersInventorys[playerNumber].team = playerNumber;
            playersControllers[playerNumber].JoystickEnabled = true;
        }

        //Set player Colors
        if (mode == GameMode.SinglePlayer)
        {
            playersInventorys[playerNumber].SetPlayerColors(0, playerNumber, playerSettings);
        }
        else if (mode == GameMode.DeathMatch)
        {
            playersInventorys[playerNumber].SetPlayerColors(1, playerNumber, playerSettings);
        }
        else if (mode == GameMode.Coop)
        {
            playersInventorys[playerNumber].SetPlayerColors(2, playerNumber, playerSettings);
        }
        else if (mode == GameMode.TeamDeathMatch)
        {
            playersInventorys[playerNumber].SetPlayerColors(3, playerNumber, playerSettings);
        }

        if (mode != GameMode.SinglePlayer)
        {
            if (useSplitScreen)
            {
                //Debug.Log("Split Screen Active");
                //Get Player Camera
                playersControllers[playerNumber].CamScript.gameObject.SetActive(true);
                Camera tempCam = playersControllers[playerNumber].CamScript.transform.GetComponentInChildren<Camera>();

                if (actualPlayerCount == 1)
                {
                    //DoNothing
                }
                else if (actualPlayerCount == 2)
                {
                    if (playerNumber == 0)
                        SetCamSplitScreen(tempCam, 0, 0, 0.5f, 1);
                    else if (playerNumber == 1)
                        SetCamSplitScreen(tempCam, 0.5f, 0, 0.5f, 1);
                }
                else if (actualPlayerCount == 3)
                {
                    if (playerNumber == 0)
                        SetCamSplitScreen(tempCam, 0.25f, 0.5f, 0.5f, 0.5f);
                    else if (playerNumber == 1)
                        SetCamSplitScreen(tempCam, 0.0f, 0, 0.5f, 0.5f);
                    else if (playerNumber == 2)
                        SetCamSplitScreen(tempCam, 0.5f, 0, 0.5f, 0.5f);
                }
                else if (actualPlayerCount == 4)
                {
                    if (playerNumber == 0)
                        SetCamSplitScreen(tempCam, 0.0f, 0.5f, 0.5f, 0.5f);
                    else if (playerNumber == 1)
                        SetCamSplitScreen(tempCam, 0.5f, 0.5f, 0.5f, 0.5f);
                    else if (playerNumber == 2)
                        SetCamSplitScreen(tempCam, 0.0f, 0, 0.5f, 0.5f);
                    else if (playerNumber == 3)
                        SetCamSplitScreen(tempCam, 0.5f, 0, 0.5f, 0.5f);

                }
                //SetCamSplitScreen()
                // playersControllers[playerNumber].CamScript.transform.GetComponentInChildren<Camera>().rect.
            }
            else
            {
                playersControllers[playerNumber].CamScript.gameObject.SetActive(false);
                playersInventorys[playerNumber].HUDDamageFullScreen.SetActive(false);

            }
        }
        

    }

    void SetCamSplitScreen(Camera cam, float x, float y, float width, float height)
    {
        cam.rect = new Rect(x, y, width, height);
    }

    void OnDrawGizmos()
    {
        if (playersSettings && targetArrow)
        {
            int n = 0;
            foreach (Transform spawnPos in playersSpawnPos)
            {
                Gizmos.color = playersSettings.playerColor[n] * 2;
                Gizmos.DrawMesh(targetArrow, spawnPos.position + Vector3.up, Quaternion.Euler(0, 10, 0), Vector3.one);
                n += 1;

               // Gizmos.color = Color.white;
                Gizmos.DrawMesh(areaMesh, spawnPos.position, Quaternion.Euler(0, 0, 0), Vector3.one);

            }
        }
   
    }

    void ResetFragHUD()
    {
        int i = 1;

        foreach (GameObject text in fragCounter)
        {
            playersFrags[i - 1] = 0;
            text.GetComponent<Text>().text = "P" + i.ToString() + " " + playersFrags[i - 1].ToString();
        }
    }

    void OrganizeFragHUD()
    {
        int i = 1;
        foreach (GameObject text in fragCounter)
        {
            if (actualPlayerCount <= i - 1)
            {

                text.GetComponent<Text>().text = "";
            }
            else
            {
                
                text.GetComponent<Text>().text = "P" + i.ToString() + " " + playersFrags[i - 1].ToString() + "0";
            }

            i += 1;
        }


    }

    void UpdateFragHUD()
    {
        int i = 1;
        foreach (GameObject text in fragCounter)
        {
            if (actualPlayerCount >= i)
            {
                if (playersFrags[i - 1] < 10)
                {
                    text.GetComponent<Text>().text = "P" + i.ToString() + " 0" + playersFrags[i - 1].ToString();
                }
                else
                {
                    text.GetComponent<Text>().text = "P" + i.ToString() + " " + playersFrags[i - 1].ToString();
                }

                i += 1;
            }
            
        }
    }

    public void NewFrag(int team)
    {
        if (stage == GameStage.inGame)
        {
            if (mode == GameMode.DeathMatch || mode == GameMode.TeamDeathMatch)
            {
                playersFrags[team] += 1;
                UpdateFragHUD();

                if (playersFrags[team] >= fragsToWin)
                {
                    SetPlayerWin(team);
                }
            }
        }

    }

    public void PlayerSpawned()
    {
        SetPlayersForCamera();
        actualCameraScript.MultiplayerCam(playersForCamera, actualPlayerCount);
    }

    public void PlayerDied(int playerNumber)
    {
        SetPlayersForCamera();
        if (!useSplitScreen)
        {
            actualCameraScript.MultiplayerCam(playersForCamera, actualPlayerCount);
            
        }
        //Do whatever you need
    }

    public void SetPlayerWin(int playerToWin)
    {
        stage = GameStage.EndedMatch;

        playerWinsText.SetActive(true);

        playerWinsText.GetComponent<Text>().color = playerSettings.playerColor[playerToWin];

        int finalPlayer = playerToWin + 1;
        string finalText = "Player " + finalPlayer.ToString() + " Wins";

        playerWinsText.GetComponent<Text>().text = finalText;
        //playerWinsText.transform.GetComponentInChildren<Text>().text = finalText;
    }


}
