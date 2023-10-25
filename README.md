# SideScrollingHackAndSlash
2d side scrolling sword dueling game with thumbstick gesture based attack inputs
Built in XNA as a college project between 2010 and 2011
<br>
## Requirements (Play executable)
- XNA 3.1 Redistributable (Can be found in the root directory or here: https://www.microsoft.com/en-us/download/details.aspx?id=15163)
- 2x Xbox Controllers (Or some way to mimic it. Can mess around with one controller.)
<br>
## Requirements (Source code)
- Visual Studio 2010 with XNA 3.1
- Windows 7
<br>
## Setup
- Make sure you have XNA 3.1 installed (you may need to restart if you just installed it)
- Download the project and navigate to ..\SideScrollingHackandSlash\SideScrollingHackandSlash\bin\x86\Release
- Run SideScrollingHackandSlash.exe
<br>
## Controls
- RStick: Sword movement
  - Sword has a grid of 9 possible positions, moving the sword through these positions triggers gestures
  - There are 3 gestures:
    - Thrust: (Back, Center), (Center, Center), (Forward, Center)
    - Overhead: (Center, Top), (Forward, Top), (Forward, Center)
    - Underhead: (Center, Bottom), (Forward, Bottom), (Forward, Center)
    - You can also hold down to point your sword down and deal damage to things you land on
- LStick: Movement
- Double Tap LStick: Magic Dash
- L3: Facebackwards (useful for aiming backwards mid air)
- RB: Pick up/Throw (RStick to adjust throw direction)
- LB: Jump
- RT: Block (RStick to adjust block height [High, Mid, Low])
- LT: ?
- A: Use held item
- B: Spawn Fireball in hands (RB to throw, or run into enemy with it)
- X (hold): Force push
- Y: Lightning bolt

## Tips
- Fireball can ignite bombs
- Lightning bolt can trigger grenades
- Jumping into ledges causes you to hang from them, you can tap down to drop off or jump from that state
- You can catch fireballs if you grab them with the right timing
- Dashing into the thrust deals more damage (it scales based on speed)
- The falling stab also scales on speed
