using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementKeysMap : MonoBehaviour
{    
    Vector3 movement;
    PlayerMovementPath pathMovement;

    public GameObject levelTileParent;
    List<GameObject> levelTiles = new List<GameObject>();
    GameObject[] orderedStageSpots;
    int selectedStage = 0;
    int lastAvailableStageSpot = 1;

    float timer = 0.6f;
    float countdown = 0;
    string destinationSpot;

    Vector2 dpadDir;
    bool gamepadInput = false;

    void Awake()
    {
        foreach (Transform child in levelTileParent.transform)
        {
            if (child.GetComponent<LevelTileManager>() != null)
                levelTiles.Add(child.gameObject);
        }

        orderedStageSpots = new GameObject[levelTiles.Count * 4];

        foreach (GameObject levelTile in levelTiles)
            foreach (Transform child in levelTile.transform)
                if (child.name == "StageSpotParent")
                {
                    int i = 0;
                    foreach (Transform spot in child.transform)
                        if (spot.GetComponent<LevelEnterTrigger>() != null && spot.gameObject.activeInHierarchy)
                        {
                            orderedStageSpots[(levelTile.GetComponent<LevelTileManager>().codeLvl - 1) * 4 + i] = spot.gameObject;
                            i++;
                        }
                }
        LevelEnterTrigger lETAux;
        GameObject currentSpot, previousSpot, nextSpot;
        int cp, cn = 0;
        for(int i = 0; i < orderedStageSpots.Length; i++)
        {
            currentSpot = orderedStageSpots[i];
            if (currentSpot == null)
                continue;
            lETAux = currentSpot.GetComponent<LevelEnterTrigger>();
            if (lETAux == null)
               continue;
            
            previousSpot = null;
            cp = 1;
            while (previousSpot == null && (i-cp) >= 0)
            {
                previousSpot = orderedStageSpots[i-cp];
                cp ++;
            }

            nextSpot = null;
            cn = 1;
            while (nextSpot == null && (i+cn) < orderedStageSpots.Length)
            {
                nextSpot = orderedStageSpots[i+cn];
                cn ++;
            }

            lETAux.RegisterSpotOrder(previousSpot, nextSpot);
        }
    }

    void Start()
    {
        selectedStage = (PlayerPrefs.GetInt("LastLevelPlayed", 1) - 1) * 4 + (PlayerPrefs.GetInt("LastStagePlayed", 1) - 1);
        bool selectedStageValid = orderedStageSpots[selectedStage] != null;
        while (!selectedStageValid)
        {
            // for construction on boss stages
            selectedStage--;
            selectedStageValid = orderedStageSpots[selectedStage] != null;
        }

        lastAvailableStageSpot = Math.Min(PlayerPrefs.GetInt("LastLevelCompleted", 0) * 4 + PlayerPrefs.GetInt("LastStageCompleted", 0), orderedStageSpots.Length - 1);

        pathMovement = GetComponent<PlayerMovementPath>();
    }

    void Update()
    {
        if(MapMessageManager.messageOnScreen)
            return;

        if (Gamepad.current != null)
             gamepadInput = Gamepad.current.buttonSouth.wasPressedThisFrame;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || gamepadInput)
            orderedStageSpots[selectedStage].GetComponent<CircleCollider2D>().enabled = true;
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            return;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Gamepad.current != null)
        {
            dpadDir = Gamepad.current.dpad.ReadValue();
                if (dpadDir.magnitude > 0.5)
                    movement = dpadDir;   
        }

        if (movement.magnitude > 0.7)
        {
            countdown = timer;
            // chiedi al bottone in cui sono dove andare in funzione del movement
            destinationSpot = orderedStageSpots[selectedStage].GetComponent<LevelEnterTrigger>().GetDestinationSpot(movement);
            if (destinationSpot == "after")
                MoveForward();
            else if (destinationSpot == "before")
                MoveBackward();
            // if (Math.Abs(movement.x) > Math.Abs(movement.y))
            // {
            //     if (movement.x > 0) // guarda se in questa direzione é assegnato qualcosa, altrimenti passa al prossimo
            //         MoveForward();
            //     else
            //         MoveBackward();
            // } else {
            //     if (movement.y > 0)
            //         MoveForward();
            //     else
            //         MoveBackward();
            // }
        }
    }

    void MoveForward()
    {
        int lastValidSpot = selectedStage;
        selectedStage = Math.Min(selectedStage + 1, lastAvailableStageSpot);
        bool selectedStageValid = orderedStageSpots[selectedStage] != null;
        while (!selectedStageValid)
        {
            selectedStage++;
            if (selectedStage > lastAvailableStageSpot || selectedStage >= orderedStageSpots.Length)
            {
                selectedStage = lastValidSpot;
                return;
            }
            selectedStageValid = orderedStageSpots[selectedStage] != null;
        }
        pathMovement.NewTarget(orderedStageSpots[selectedStage].transform.position, true);
        FindFirstObjectByType<MapMoveCamera>().Exit();
    }

    void MoveBackward()
    {
        int lastValidSpot = selectedStage;
        selectedStage = Math.Max(selectedStage - 1 , 0);
        bool selectedStageValid = orderedStageSpots[selectedStage] != null;
        while (!selectedStageValid)
        {
            selectedStage--;
            if (selectedStage < 0)
            {
                selectedStage = lastValidSpot;
                return;
            }
            selectedStageValid = orderedStageSpots[selectedStage] != null;
        }
        pathMovement.NewTarget(orderedStageSpots[selectedStage].transform.position, true);
        FindFirstObjectByType<MapMoveCamera>().Exit();
    }

    public void SetSelectedStage(int levelCode, int stageCode)
    {
        selectedStage = (levelCode-1)*4 + (stageCode -1);
        bool selectedStageValid = orderedStageSpots[selectedStage] != null;
        while (!selectedStageValid)
        {
            // for construction on boss stages
            selectedStage--;
            selectedStageValid = orderedStageSpots[selectedStage] != null;
        }

    }

    public void SetLastAvailableStage(int absoluteStageCode)
    {
        lastAvailableStageSpot = Math.Min( absoluteStageCode, lastAvailableStageSpot);
    }
}
