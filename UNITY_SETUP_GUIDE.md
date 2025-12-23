# 🎮 Unity Setup & Development Guide

## Step 1: Install Unity

### Download Unity Hub
1. Go to: https://unity.com/download
2. Download **Unity Hub** (free)
3. Install Unity Hub

### Install Unity Editor
1. Open Unity Hub
2. Click **"Installs"** → **"Install Editor"**
3. Choose **Unity 2022.3 LTS** (Long Term Support)
4. Select these modules:
   - ✅ WebGL Build Support (for browser deployment)
   - ✅ Windows Build Support (if on Windows)
   - ✅ Mac Build Support (if on Mac)
   - ✅ Documentation
   - ✅ Microsoft Visual Studio Community (if you don't have it)

**Installation time:** ~30-60 minutes depending on internet speed

---

## Step 2: Create New Project

### In Unity Hub:
1. Click **"New Project"**
2. Choose **"3D (URP)"** template (Universal Render Pipeline)
3. Project Name: `AlgebraQuest`
4. Location: Choose where to save
5. Click **"Create Project"**

**First launch:** Takes 5-10 minutes to set up

---

## Step 3: Project Structure

Unity will create this structure:
```
AlgebraQuest/
├── Assets/               ← Your game content goes here
│   ├── Scenes/          ← Game levels/worlds
│   ├── Scripts/         ← C# code
│   ├── Prefabs/         ← Reusable objects
│   ├── Materials/       ← Colors, textures
│   ├── Models/          ← 3D objects
│   └── UI/              ← Menus, HUD
├── Packages/            ← Unity packages (auto-managed)
├── ProjectSettings/     ← Unity settings (auto-managed)
└── Library/             ← Unity cache (auto-managed)
```

---

## Step 4: Import Your Existing Algebra Logic

### Copy Your C# Models:
1. In Unity, right-click `Assets/Scripts` → Create → Folder → Name it `AlgebraCore`
2. Copy these files from your Blazor project:
   - `Models/Player.cs`
   - `Models/AlgebraQuestion.cs`
   - `Services/GameEngine.cs`

3. **Important:** Remove Blazor-specific code:
   - Remove `@code` blocks
   - Remove `[Parameter]` attributes
   - Remove `EventCallback` (use Unity Events instead)

### Example Conversion:
**Before (Blazor):**
```csharp
[Parameter] public EventCallback<bool> OnSolved { get; set; }
```

**After (Unity):**
```csharp
public UnityEvent<bool> OnSolved;
```

---

## Step 5: First Prototype - Player Movement

### Create Player Controller Script:

1. Right-click `Assets/Scripts` → Create → C# Script → Name: `PlayerController`
2. Double-click to open in Visual Studio
3. Replace with this code:

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float verticalRotation = 0f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Hide cursor
    }
    
    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        
        // Movement (WASD)
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S
        
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        // Jump (Space)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
        
        // Apply gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
```

---

## Step 6: Create the Player

### In Unity Editor:

1. **Create Player Object:**
   - Hierarchy → Right-click → 3D Object → Capsule
   - Rename to "Player"
   - Position: (0, 1, 0)

2. **Add Character Controller:**
   - Select Player
   - Inspector → Add Component → Character Controller
   - Adjust height: 2, radius: 0.5

3. **Add Player Script:**
   - Select Player
   - Inspector → Add Component → Search "PlayerController"
   - Click to add

4. **Create Camera:**
   - Hierarchy → Right-click → Camera
   - Drag Camera onto Player (make it a child)
   - Position: (0, 0.6, 0)

5. **Create Ground:**
   - Hierarchy → 3D Object → Plane
   - Scale: (10, 1, 10)
   - Position: (0, 0, 0)

6. **Press Play!** ▶️
   - Use WASD to move
   - Mouse to look around
   - Space to jump

---

## Step 7: Create First Challenge

### Create Challenge Trigger Script:

```csharp
using UnityEngine;
using UnityEngine.Events;

public class ChallengeTrigger : MonoBehaviour
{
    [Header("Challenge Settings")]
    public string challengeQuestion = "Solve: 2x + 3 = 11";
    public string correctAnswer = "4";
    public int xpReward = 50;
    public int coinReward = 10;
    
    [Header("Events")]
    public UnityEvent OnChallengeComplete;
    
    private bool isCompleted = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCompleted)
        {
            ShowChallengeUI();
        }
    }
    
    void ShowChallengeUI()
    {
        // TODO: Show UI overlay with problem
        Debug.Log("Challenge: " + challengeQuestion);
        
        // For now, auto-complete for testing
        CompleteChallengeChallenge();
    }
    
    public void SubmitAnswer(string answer)
    {
        if (answer == correctAnswer)
        {
            CompleteChallenge();
        }
        else
        {
            Debug.Log("Incorrect! Try again.");
        }
    }
    
    void CompleteChallenge()
    {
        isCompleted = true;
        Debug.Log("Challenge Complete! +" + xpReward + " XP");
        OnChallengeComplete?.Invoke();
    }
}
```

### Create Challenge Object:

1. Hierarchy → Create Empty → Name: "Challenge_Bridge"
2. Add Component → Box Collider
   - Check "Is Trigger"
   - Size: (3, 3, 3)
3. Add Component → ChallengeTrigger script
4. Position near player

---

## Step 8: Free Assets to Get Started

### Unity Asset Store (Free):

1. **Environments:**
   - "Polygon Starter Pack" (Low poly world)
   - "Nature Starter Kit 2" (Trees, rocks)
   - "Simple Apocalypse - Cartoon Assets" (Buildings)

2. **Characters:**
   - "Mixamo" (Free rigged characters + animations)
   - "Polygon - Adventure Pack" (Low poly characters)

3. **UI:**
   - "Modern UI Pack" (Menus, buttons)
   - "Simple Fantasy UI" (Game HUD)

4. **Effects:**
   - "Cartoon FX Free" (Particles, magic effects)
   - "Free Skybox Pack" (Sky backgrounds)

### How to Import:
1. Window → Asset Store
2. Search for asset
3. Click "Add to My Assets"
4. Window → Package Manager → My Assets
5. Download → Import

---

## Step 9: Build & Test

### Test in Unity:
- Press **Play** button (▶️) anytime
- Press again to stop

### Build for Web (WebGL):
1. File → Build Settings
2. Select "WebGL"
3. Click "Switch Platform"
4. Click "Build and Run"
5. Choose folder to save
6. Wait 5-15 minutes for first build
7. Opens in browser automatically!

### Build for PC:
1. File → Build Settings
2. Select "Windows/Mac/Linux"
3. Click "Build"
4. Creates .exe file

---

## Step 10: Next Steps

### Week 1: Basic Prototype
- ✅ Player movement working
- ✅ One challenge trigger
- ✅ Simple environment (ground + obstacles)
- ✅ Debug UI showing challenge text

### Week 2: Challenge System
- [ ] UI overlay for problems
- [ ] Input field for answers
- [ ] Hint system
- [ ] Reward feedback (coins, XP)

### Week 3: First Region
- [ ] Import environment assets
- [ ] Create Forest of Variables
- [ ] 5 different challenges
- [ ] Collectible coins

### Week 4: Polish
- [ ] Particle effects
- [ ] Sound effects
- [ ] Character customization
- [ ] Save system

---

## 🆘 Troubleshooting

### "Script won't compile"
- Check for typos
- Make sure using `UnityEngine;`
- Save file before returning to Unity

### "Player falls through ground"
- Add Mesh Collider to ground plane
- Check Character Controller is attached

### "Camera not following player"
- Make sure Camera is child of Player
- Check Camera position is (0, 0.6, 0)

### "Can't move"
- Check PlayerController script is attached
- Press Play button
- Make sure game window is focused

---

## 📚 Learning Resources

### Unity Learn (Free):
- "Create with Code" - Beginner course
- "Junior Programmer" - Pathway
- "3D Game Kit" - Sample project

### YouTube Channels:
- Brackeys (best beginner tutorials)
- Code Monkey (clean code practices)
- Sebastian Lague (advanced techniques)

### Documentation:
- Unity Manual: https://docs.unity3d.com/Manual/
- C# Scripting: https://docs.unity3d.com/ScriptReference/

---

**Ready to start? Install Unity and let me know when you're ready for the next step!** 🚀

