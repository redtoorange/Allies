Controllers work as the brains of an entity.  

##PlayerController
Handles input from the player and moves the player's entity.

##ZombieController
Shambles before combat, moving towards the closest none-zombie.  During combat move more 
quickly towards non-infected.

##InnocentController
Randomly wanders the Innocent before combat. Moves away from Zombies and sprints around
during combat.

##AllyController
Handles shooting for Allies when combat begins.