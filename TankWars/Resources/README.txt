-Client/Ps8
Design:
Put the Drawing inside the View
used a hashset to make movement feel better
Stored in game objects in dictionaries
Used objects for the animations
Death animation plays untill tank respawn
keeped track of the animation with a frame counter inside the object
Used a object for controlls called ControlCommand
Draw walls as segements of the overall wall
left the area outside the map white
Must restart clint if an error occers

Sprites:
We used the provided Images

Features:
No extra features, does what provided tank war client does
Does not include help menu


-Server/Ps9
Design:
Made a setting class to handel the settings
disconnect player if they send a bad input
handel projectile colistions simlarly to wall/tank colistions
can olny have 2 powerups
tank hp is always 3
use a busy loop for each frame
remove dc tanks from the tanks list
think everthing works as intented


Extra Features:
Settings has a Gamemode setting which can be "extra" or "basic"
extra makes projectiles bounce off the walls