using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> startingPoints;
    [SerializeField]
    private List<LayerMask> playerLayers;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = FindAnyObjectByType<PlayerInputManager>();
        
        // Configure PlayerInputManager for split-screen multiplayer
        if (playerInputManager != null) {
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            playerInputManager.splitScreen = true;
        }
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        int playerIndex = players.Count - 1;

        Debug.Log($"Player {playerIndex + 1} joined with device: {player.devices[0].displayName}");

        // Assign control scheme based on player number
        if (playerIndex == 0)
        {
            // Player 1 uses keyboard and mouse
            player.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
            Debug.Log("Player 1 assigned to Keyboard&Mouse");
        } else if (playerIndex == 1) {
            // Player 2 uses gamepad
            if (Gamepad.current != null)
            {
                player.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
                Debug.Log("Player 2 assigned to Gamepad");
            }
            else
            {
                Debug.LogWarning("No gamepad detected for Player 2!");
            }
        }

        //need to use the parent due to the structure of the prefab
        Transform playerParent = player.transform.parent;
        playerParent.position = startingPoints[playerIndex].position;

        //convert layer mask (bit) to an integer 
        int layerToAdd = (int)Mathf.Log(playerLayers[playerIndex].value, 2);

        //set the layer
        playerParent.GetComponentInChildren<CinemachineFreeLookModifier>().gameObject.layer = layerToAdd;
        
        // Get the camera for this player
        Camera playerCamera = playerParent.GetComponentInChildren<Camera>();
        
        // Configure split-screen viewports
        if (playerIndex == 0)
        {
            // Player 1 - Top half of screen
            playerCamera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
        }
        else if (playerIndex == 1)
        {
            // Player 2 - Bottom half of screen
            playerCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
        }
        
        //add the layer to camera culling mask
        playerCamera.cullingMask |= 1 << layerToAdd;
        
        //set the action in the custom cinemachine Input Handler
        playerParent.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("Look");

    }
}