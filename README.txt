Hello, this is the read me file for "Circles Must Die!"

Objective:
  Kill all the circles to move on to the next level.

Notes:
  Different Circles have different ranges.
  Your range is limited as well.
  Making levels is easy and will be detailed in this document.
  If the game fails to load level 1 on start up, it will be set to level -1, and refuse to load.
  If the game fails to load any level at any time, it will go to level -1 and be unplayable.

Controls:
  Menu:
    Q: Quit
    P: Unpause
    Enter: Unpause
    Arrows (Left/Right): Change Level
  
  In Game:
    W: Jump
    Space: Jump
    A: Move Left
    D: Move Right
    Up Arrow: Shoot Up
    Left Arrow: Shoot Left
    Down Arrow: Shoot Down
    Right Arrow: Shoot Right
    R: Restart Level
    Q: Quit level
        
Custom Levels:
  Sample: Level 10
    Formatted in a .txt         //Comments that are NOT part of the level file
    level10.txt:
      WWWWWWWWWWWWWWW           //Start of file
      W  R   B     TW
      W W    W  W   W
      WRWWW  W BN WWW
      W     WW B    W
      W  WWWWWWWVVW W
      WWN    W      W
      WWW    W WWWWWW
      WP  B  B    BTW
      WWWWWWWWWWWWWWW
      S                         //The S by itself on one line denotes the end of reading for the level.
      Made By: Mark Delfavero   //Any text below the S-line will be ignored by the game, use the space for notes if you want.
  Definition of Parts:
    W: A Wall block
    P: A player block
    B: A pushable yellow block
    R: An enemy that can move horizontally and shoot vertically
    V: An enemy that can shoot horizontally and move vertically
    T: An enemy that does not move, and can shoot at any angle
    N: An enemy that moves away from the player and can shoot at any angle
  File Naming convention
    "level" + consecutive integer + ".txt"
    Notes: 
      ".txt" is the file extension. The game can only support files in a .txt format.
      The game will only look for a level that it is trying to load.
        Example One:
          level1.txt exists
          level10.txt exists
        Result:
          Game loads on level one, level ten is inaccessible.
        Example Two:
          level2.txt exists
          level10.txt exists
        Result:
          Game fails to find level one, so it defaults to level -1, and refuses to allow playing.
      All Level numbers must be greater than 0.
      There must always be a level 1.
    Room Size:
      Rooms can be made to be any size. The game will center any sized level. 
      The game WILL NOT SCALE GRAPHICS TO MAKE THE LEVEL FIT ON THE SCREEN.
      Nor will the game provide a panning camera.
      Intended size is 15 wide 10 tall.

Credits
  This game was made using C# and Monogame.
  GIMP was used to make all of the textures.
  Microsoft Visual Studio was used as the coding environment.
