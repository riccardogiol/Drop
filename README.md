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
For example: to water a tile (or burn it) I added for each tile a collider. Once a trigger enter the collider, I ask the PlaygroundManager to water (or burn) the tile at the collider position, transforming world coordinates to tilemap coordinates.
Drop can water a tile in different ways:
 1. walking over a burnt tile
 2. shooting over a tile or to a bush
 3. casting a waterwave over the tile

The *PlaygroundManager* is in charge of terminate the stage if we have no more burnt tiles (win) or if the burn tiles are too many (loose).

**Enemies logic**

Enemy (flames and fires) can move around randomly or follow some paths or targets. I used the Astar library to make the enemies able to follow a target evaluating the best path to follow. I wrote for them 2 different behaviours:
 1. the Patrol over some coordinate
 2. the Chasing of the player
Then I wrote a script that alternate from a behaviour to the other when the enemy has more life of the hero, and so can kill him, triggering the Game Over once it catch Drop.


### Note on Graphic

All textures and images in the game are drawn by me (:

### Further improovements

I would like to publish the game, and to do so there is still a lot to do:
 1. graphic: add animations, elements and details
 2. gameplay: I'm currently re-arrange the first levels to make them more enjoyable (first 2 levels ok so far)
 3. puzzle elements: add puzzle elements in the levels like moving rocks, keys and doors
 4. game economy: makes good progression curve, challenging and motivating
 5. book of biome: add description for plants and animals saved
 6. sound: finish to integrate all sounds needed
