using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> startingPoints;

    private PlayerInputManager playerInputManager;
    private readonly List<PlayerInput> players = new();

    private void Awake()
    {
        playerInputManager = FindAnyObjectByType<PlayerInputManager>();

        // Configure PlayerInputManager for split-screen multiplayer
        if (playerInputManager != null)
        {
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
        }
        else if (playerIndex == 1)
        {
            // Player 2 uses gamepad
            if (Gamepad.current != null) {
                player.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
                Debug.Log("Player 2 assigned to Gamepad");
            } else {
                Debug.LogWarning("No gamepad detected for Player 2!");
            }
        }

        //need to use the parent due to the structure of the prefab
        var playerParent = startingPoints[playerIndex].transform.parent;
        
        // Position the player at the designated starting point
        player.transform.SetPositionAndRotation(
            startingPoints[playerIndex].position,
            startingPoints[playerIndex].rotation
        );
    }
}