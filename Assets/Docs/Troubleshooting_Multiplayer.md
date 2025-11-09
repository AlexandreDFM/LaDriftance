# Troubleshooting Multiplayer Input & Spawn Points

## Problem: Both keyboard and controller control both cars

### Diagnosis Steps
1. Check the console for `[PlayerManager]` logs when players join
2. Look for lines showing which devices are assigned to each player
3. Verify that each player has DIFFERENT devices listed

### Common Causes

#### 1. PlayerInput Auto-Switch is enabled
**Fix:**
- Open the Player prefab in the Inspector
- Find the `PlayerInput` component
- **Uncheck "Auto Switch Control Schemes"**
- This prevents the Input System from automatically switching devices between players

#### 2. No Control Schemes defined in Input Actions
**Fix:**
- Open `Assets/InputSystem_Actions.inputactions`
- Check if Control Schemes are defined (top right corner)
- You need at least two schemes:
  - "Keyboard&Mouse" (Keyboard + Mouse devices)
  - "Gamepad" (Gamepad device)
- Make sure each action has bindings for BOTH schemes

#### 3. PlayerInputManager is not properly configured
**Fix:**
- Select the GameObject with `PlayerInputManager` component
- Set "Join Behavior" to "Join Players When Button Is Pressed"
- Set "Notification Behavior" to "Invoke Unity Events"
- Make sure "Player Prefab" is assigned

#### 4. Both players are in the same InputUser
**Fix (Advanced):**
This requires code changes. The issue is that Unity's Input System may be pairing multiple devices to the same InputUser. The current `PlayerManager.cs` attempts to fix this with `SwitchCurrentControlScheme`, but if that's not working:

```csharp
// After player joins, manually unpair devices from other players
using UnityEngine.InputSystem.Users;

// In AddPlayer, after getting primaryDevice:
var primaryDevice = player.devices[0];

// Unpair this device from all other players first
foreach (var otherPlayer in players)
{
    if (otherPlayer == player) continue;
    
    // Get the InputUser for this player
    var user = otherPlayer.user;
    if (user.valid)
    {
        // Remove this device from other player's user
        InputUser.PerformPairingWithDevice(primaryDevice, user: user, options: InputUserPairingOptions.UnpairCurrentDevicesFromUser);
    }
}

// Then pair exclusively to new player
player.SwitchCurrentControlScheme(player.currentControlScheme, primaryDevice);
```

### Testing Checklist
- [ ] Only ONE player prefab is instantiated per join
- [ ] Each player shows different devices in console logs
- [ ] Pressing keyboard only moves one car
- [ ] Pressing gamepad only moves the other car
- [ ] Auto-Switch is disabled on PlayerInput
- [ ] Control Schemes are properly defined

---

## Problem: Spawn points not working (players spawn at same location)

### Diagnosis Steps
1. Check console for `[PlayerManager]` logs about spawn points
2. Look for "Moving player #X to spawn point Y: (position)"
3. Check if spawn points are actually assigned in Inspector

### Common Causes

#### 1. Spawn Points not assigned in Inspector
**Fix:**
- Select the GameObject with `PlayerManager` script
- Expand "Spawn Points" list
- Set Size to 2 (or number of players)
- Drag Transform GameObjects into each element

#### 2. Spawn Point Transforms are at (0,0,0)
**Fix:**
- Select each spawn point GameObject in the scene
- Move them to different positions using the Transform tool
- Make sure they're far apart (at least 10 units)

#### 3. Player prefab structure issue
The code looks for `player.transform.parent` to move. If your player prefab structure is:
```
PlayerRoot (this should move)
  ├─ PlayerInput (PlayerInput component here)
  ├─ Model
  └─ Camera
```

Then the code will move PlayerRoot. But if PlayerInput is on the root, there's no parent, so it falls back to moving the PlayerInput object itself.

**Fix:**
Check your player prefab structure. The `PlayerInput` component should be on a CHILD, not the root, for the spawn system to work as designed.

#### 4. Spawn happens but player is moved afterward
If spawn logs show correct positions but players end up together:
- Check if there's another script moving the player after spawn
- Check if there's a CharacterController or Rigidbody that's resetting position
- Disable other movement scripts temporarily to test

### Quick Test
Add this to check spawn point setup:
```csharp
// In Unity Editor, select PlayerManager object
// In Inspector, you should see:
// - Spawn Points: Size = 2
//   - Element 0: [Assigned Transform at position A]
//   - Element 1: [Assigned Transform at position B]
```

Run the game and check console:
```
[PlayerManager] Player #1 - playerParent: PlayerRoot, position BEFORE spawn: (0, 0, 0)
[PlayerManager] Moving player #1 to spawn point 0: (10, 0, 5)
[PlayerManager] Player #1 position AFTER spawn: (10, 0, 5)
```

If "BEFORE" and "AFTER" are the same, the parent isn't moving. Check prefab structure.

---

## Quick Debug Commands

Add these temporary logs to narrow down issues:

```csharp
// At the start of AddPlayer:
Debug.Log($"Player object hierarchy: {player.name}, parent: {(player.transform.parent != null ? player.transform.parent.name : "NULL")}");

// After trying to move to spawn:
Debug.Log($"Spawn assignment worked: {playerParent.position} (expected: {spawnPoints[playerIndex].position})");

// For input devices:
Debug.Log($"All players device check:");
foreach (var p in players)
{
    Debug.Log($"  Player user {p.user.id}: {string.Join(", ", p.devices.Select(d => d.displayName))}");
}
```

---

## Still Not Working?

If the above steps don't fix it:
1. **Share the console logs** from when players join (the `[PlayerManager]` lines)
2. **Screenshot your PlayerManager Inspector** showing Spawn Points configuration
3. **Screenshot your Player Prefab hierarchy** in the Project window
4. Check if there are any errors/warnings in the console before players join
