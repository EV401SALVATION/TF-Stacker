[]OVERVIEW[]
Welcome to TF Stacker, a fully customizable transformation "game" where you click buttons to transform a character and watch what happens!

In this game, you can stack multiple different transformations on top of one another, and the order in which you do so will have an effect on the result!

The game itself doesn't include any of the images. Instead, these are stored in a separate database, along with the name of the character and the different transformations available for them. You specify which database you want to use when starting the game, so you can have any number of them! Thanks to this, it's incredibly easy to modify the game with your own characters and transformations! The number of possible combinations increases exponentially as the number of TFs increases, so it's a good thing that the game can tell when an image is missing and use a placeholder for it instead!

The game is currently in a very early prototype state and features only the bare minimum features. There are some other things that I want to do with this project when I have the time, and while I can make no promises that they'll actually be done, I figured I'd share them anyway:
    - An overall improved visual presentation, with sound effects and nice-looking UI.
    - A way for artists to be credited for their images in the case of collaborative databases (currently the only way to do this is to have the artists sign their 
    names on the images themselves, I'm still not sure how I'd go about this).
    - A small *poof* animation that plays when you transform the character.
    - A selection of backdrops to swap between (customizable in databases).
    - A background music track (customizable in databases).
    - Random funny dialogue lines that the character says when you first start the game (customizable in databases).
    - A tree that shows all possible transformation combinations and which ones have images (very unlikely to actually happen, but it's fun to think about).

This project has been designed to run on Windows 64-bit machines. I might try making it work for Mac and Linux at some point, but that depends on how differently the file systems are for those platforms.
The wonderful Matojeje created an HTML version of the game too, which you can play here: https://matojeje.github.io/TF-Stacker/



[]PLAYING THE GAME[]
You'll first want to download the zip files for the latest release and its associated databased from the latest release listed on the repository. Unzip them wherever you'd like, and in the open the "TF Stacker" Unity application inn the release's folder to run it! When the game starts, copy the directory path for the database you want to use (located near the top of the file explorer) and paste that into the dialogue box on the startup screen. The game will warn you of any issues with the database when you press start, though this shouldn't be an issue if you're using unmodified databases from the latest release. From there, you can press the buttons along the top of the screen to transform the character, and use the undo button at the top-right to undo the most recent transformation! A small green circle near a transformation button means pressing it will result in an image, with no circle meaning that image isn't in the database yet. Applied transformations are listed on the bottom left, with the most recent on top.

If you're having trouble finding where to download the files from the GitHub repo, here's some quick steps to do download the latest build:
1. Click on the "Builds" folder shown on the main GitHub page.
2. Click on the .zip file for the latest release of TF Stacker. (Would be names something like "v#-#-#.zip".)
3. Click on the little download button shown on the top left of the area with the blue "View raw" text.
You follow pretty much the same steps to download the .zip of the demo database.

An example of a correct directory path when starting the game would be something like: "C:\Users\<User>\Documents\<DatabaseFolder>"