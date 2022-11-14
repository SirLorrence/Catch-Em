# Catch-Em
Recreation of the Jak and Daxter - Hand Over Fish mini game 

Game rules a pretty much the same: 
- Regular fish are 1 pound, catching one score 1 point
- Bigger fish are 5 pounds, catching one score 5 points
- Catching the Toxic Fish endings the game
- Missing 20 points (or pounds) will end the game

I took inspiration from the cheat code you can enter to make it less and watch some people collect [2000 points](https://youtu.be/vXWJgVl4xjc) (pounds or whatever) 


<p align="center">
  <img src="https://github.com/SirLorrence/ReadMeImages/blob/main/CatchEm/CatchEmGame.gif?raw=true">
</p>

<div align="center">
 <h2> Development Overview  </h2>
</div>


First Playable/Early Alpha | Beta
:-------------------------:|:-------------------------:
![](https://github.com/SirLorrence/ReadMeImages/blob/main/CatchEm/CatchEm-EarlyAlpha.gif?raw=true) | ![](https://github.com/SirLorrence/ReadMeImages/blob/main/CatchEm/CatchEm-Beta.gif?raw=true)


To get the core gameplay working, I first started with the spawn system. I created a dynamic line that will be as long as the width of the river (plane) and coded a placement system that would allow me to place as many points with the proper amount of spacing as I need. 

### Spawn System
<!-- Spawner -->
<p align="center">
  <img src="https://github.com/SirLorrence/ReadMeImages/blob/main/CatchEm/CatchEm-SpawnSystem.gif?raw=true">
</p>

Then I wanted to try something new with shaders, never learn much about them - so I gave them a try. This took the longest out of everything in this project. My goal was to create the illusion of the river and the first curving towards the player, which I did succeed in. But many of the problems I've faced were with getting the curve calculates right and understanding what well am I doing in the shader graph. Working with good guides I've managed to get what I wanted.

### Level Shader
<!-- Shader Graph -->
<p align="center">
  <img src="https://github.com/SirLorrence/ReadMeImages/blob/main/CatchEm/CurvedShader.png?raw=true">
</p>

After that, I continue to finish up the gameplay logic and slowly started to add in the assets and animations. Instead of using Unity's IK animation tool, I decided to work with their IK API so I can do it through code. With some semantic errors and finding some fun bugs while doing so, I've managed to get it working. Added some polish (the best I can, I am not an artist or animator), and boom created this little mini-game. 

### IK Animations
<!-- IK Animation Bug -->
<p align="center">
  <img src="https://github.com/SirLorrence/ReadMeImages/blob/main/CatchEm/CatchEm-AnimBug.gif?raw=true">
</p>


### References 
- [World Bending Effect](https://notslot.com/tutorials/2020/04/world-bending-effect)
- [Start curving from a specific distance](https://gamedev.stackexchange.com/questions/196801/unity-world-curve-shader-graph-how-to-start-curving-from-a-specific-distance)
