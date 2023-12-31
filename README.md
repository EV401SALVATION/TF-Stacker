----------OVERVIEW----------
Welcome to TF Stacker, a fully customizable transformation "game" where you click buttons to transform a character and watch what happens!

In this game, you can stack multiple different transformations on top of one another, and the order in which you do so will have an effect on the result!

The game itself doesn't include any of the images. Instead, these are stored in a separate database, along with the name of the character and the different transformations available for them. You specify which database you want to use when starting the game, so you can have any number of them! Thanks to this, it's incredibly easy to modify the game with your own characters and transformations! The number of possible combinations increases exponentially as the number of TFs increases, so it's a good thing that the game can tell when an image is missing and use a placeholder for it instead!

This project has been designed to run on Windows 64-bit machines.

The wonderful Matojeje created an HTML version of the game too, which you can play here: https://matojeje.github.io/TF-Stacker/



----------PLAYING THE GAME----------
You'll first want to download the zip files for the latest release and an associated database for that release (these can be found on the releases page). Unzip them wherever you'd like, and in the open the "TF Stacker" Unity application in the release's folder to run it! When the game starts, copy the directory path for the database you want to use (located near the top of the file explorer) and paste that into the dialogue box on the startup screen. The game will warn you of any issues with the database when you press start, though there shouldn't be any if you're using unmodified databases from the latest release. From there, you can press the buttons along the bottom of the screen to transform the character, and use the undo button at the bottom-right to undo the most recent transformation! A small green circle near a transformation button means pressing it will result in an image, with no circle meaning that image isn't in the database yet. Applied transformations are listed on the bottom left, with the most recent at the top of the list. You can also swap between different backgrounds if those are included in the database!

An example of a correct directory path when starting the game would be something like: "C:\Users\<User>\Documents\<DatabaseFolder>"

Of course, the real gameplay of TF Stacker is in the making of the databases themselves! You can add your own character images, transformations, backgrounds, a background music track, and customize almost the entire appearance of the game! You can find directions for making your own database in the "How To Create a Database" file on the Github repository.