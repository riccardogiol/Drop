# Hello I'm Drop!
Drop is a waterdrop shaped hero sent by the good rainy-clouds to save the forest! His goal is to enter the forest extinguish all fires he founds on the path and so clean the skies from the evil smoke-clouds.

### Presentation

The game is a Pacman like arcade game with puzzle elements.
Each Level is composed of stages. To clear the stage Drop has to extinguish all fires, and water all burnt grass and bushes.
Drop has his water reservoir that acts as a life bar AND energy. Pass over the burnt grass to water it consume some of its energy. Shoot consume some water from the reservoir and can water bushes and damage flames and wildfire enemies.
Drop can enter in contact with flame and wildfire enemies to estinguish them, but lose a lot of water from the reservoir. Shoot to them is more efficient.

### Some technical feature

**Tilemap reactions**

The payer moves over a map created with tiles. The tiles dynamically change aspect during the game changing from burnt tiles to cleared tiles.
Their aspect has to change in function of the surrounding tile, for this reason I used Rule Tiles. 
The component *PlaygroundManager* has the role of managing the state of the tiles for the walkable map and the wall bushes.
So when we place a flame in the map, the *PlaygroundManager* burn all tiles around that flame, in the walkable map and in the wall map.
It does the same when the player water an area. Drop can do this in 2 different ways:
 1. walking over a burnt tile
 2. shooting to a bush

For the first case we water the tile under the player all the time it moves. To do so we change the coordinate from world to tilemap coordinates.
For the second case we chack the collision of the spawned bullet to a colliding box positioned over each wall tile. Identified the colliding box we water the tile under it.
The *PlaygroundManager* is in charge of ending the stage if we have no more burnt tiles in both the tilemaps.

**Enemies logic**

I used the Astar library to make the enemies able to follow a target evaluating the best path to follow. I wrote for them 2 different behaviours:
 1. the Patrol over some coordinate
 2. the Chasing of the player
Then I wrote a script that alternate from a behaviour to the other when the enemy has more life of the hero, and so can kill him, triggering the Game Over.

**Persistent data**

On the world map I made buttons and images change graphic once the previous level is completed. To do so I used the PlayerPrefs as a local store to save progression data and keep them persistent on disk.

**Coroutines**

The pond element spawns water orbs all around every X seconds thanks to a *IEnumerator* coroutine of the script *sparkler*.


### Note on Graphic

All textures and images in the game are drawn by me (:

### Further improovements

I would like to publish the game, and to do so there is still a lot to do:
 1. graphic: refine everything, add elements and details
 2. animations: make all entities (at least) animated when they move
 3. bar for the restoration of the level: make visible the state of restoration of the level, and make the game over if we go under a threshold
 4. new wave action for the player: give the possibility to the player to water all 8 tiles around it in a shot
 5. new graphic for the worldmap: maybe refactor the worldmap to make it more interactable
 6. sound: finish to integrate all sounds needed
