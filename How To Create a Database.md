----------CREATING A DATABASE----------
Creating a database for TF Stacker is incredibly easy! Just follow these steps and you'll have your own version of TF Stacker running in no time! You can refer to the demo database in the Databases folder for an example of a correctly prepared database whenever you need.

1. Download the database template zip file.
    You can find this in the latest release of the game, as well as in the Databases folder. Unpack it wherever you want, and you're ready to start setting up your database!
    The database template comes with a folder named "Character_Images" and a folder named "Transformation_Icons", as well as a "Names.txt" file and a "Transformations.txt" file.

2. Name your database.
    Rename your database folder to whatever you want.
    In the "Names.txt" file, replace the line "<Character Name>" with the name of the character you will use for this database, and replace the "<Author Name>" line with your own name. The name of your character will be used in various file names, so make sure not to use any spaces or funky characters in it!An example "Names.txt" file would be:

        Draden
        DonHp

3. Setup the Transformations.txt file.
    Next open the "Transformations.txt" file. This file will store a list of the different transformations in your database. The first line should be a number from 1 to 10, which represents how many transformations are in this database (you're limited to 10, but even just 5 creates a ridiculous number of possible combinations). Each subsequent line should be the name of a transformation. These transformation names will also be used in file names later, so be sure not to put in any spaces or funky characters! An example Transformations.txt file would be:

        3
        Inflatable
        Standee
        Plane

4. [OPTIONAL] Add the transformation button icons.
    Since you can specify your own transformations, you can also create your own icons for the transformation buttons! These icons must be named EXACTLY the same as the transformations listed in the Transformations.txt file. For example, the icon for a transformation listed as "Inflatable" should be named "Inflatable.png". If a transformation icon is missing, the button will instead display text for that transformation.
    Transformation icons are placed into the "Transformation_Icons" sub-folder in the main database folder. The game will run fine if this folder is missing.
    Every transformation button icon needs to have square dimensions. I use 256x256 images, though I don't think the exact size matters.

5. [OPTIONAL] Add custom button images.
    You can also customize the following button images! These custom button images are all placed into the main database folder, and must be named exactly as specified. The sizes shown are just recommendations, though each image needs to have square dimensions.

    - Image_Missing.png (1280x1280)
    - Transformation_Button_Background.png (256x256)
    - Undo_Icon.png (256x256)
    - Exit_Icon.png (256x256)

   [TIP] You can have transparent transformation button backgrounds by putting a fully transparent PNG file named "Transformation_Button_Background.png" into your database's main folder!

5. Add the default character image.
    Every database needs a default character image for you to start out with. This should be your character in their natural state, without any transformations applied to them. The image file should be named "<Character>.png" with <Character> being replaced with the character's name EXACTLY as it is specified in the Names.txt file (you're probably starting to see a pattern here...). Place this image into the "Character_Images" folder. Just like with the icons, if this is missing it will be displayed with the placeholder image instead.
    Every character image needs to have square dimensions. I'd reccommend a resolution of around 1280x1280, though I don't think the exact size matters.

6. Add the rest of the transformation images.
    Now it's time for the fun part! Each transformation image should follow the naming format <Character>%<Transformation>%<Transformation>.png with <Character> replaced by the character's name EXACTLY as it is specified in the Names.txt file and each <Transformation> being replaced with the corresponding transformation EXACTLY as it is specified in the Transformations.txt file. You can have up to ten transformations, separated by '%' symbols, and these are in the order of earliest transformation to latest (i.e. if a Draden was first transformed into an Inflatable and then a Standee, then the resulting image would be named "Draden%Inflatable%Standee.png").

    All of these images are placed into the "Character_Images" folder.

    Once again, if a transformation image is missing, the game will display it with the placeholder. As the number of these images quickly skyrockets the more TFs are added, it's only natural that many combinations won't have images.

7. [OPTIONAL] Customize the game's color pallette.
    You can fully customize the color pallete of the game! To do this, you'll need to put a file named "Colors.txt" into your database's main folder. An example of the "Colors.txt" file from the Draden Demo is shown below.

    Background:
    179
    175
    255
    Bottom Bar:
    30
    7
    80
    Text:
    179
    175
    255
    TF Button Text:
    179
    175
    255
    Transformation Text:
    30
    7
    80

    Each of the groups of three numbers represent the red, green, and blue values for a color, ranging from 0 to 255. Make sure that your file is structured exactly like this, or else the colors might not be read properly and display as the defaults instead! The game will run fine if the "Colors.txt" file is missing too, so you can completely ignore this if you are fine with the default colors.
    (The Transformation Text color refers to the text stack that appears displaying each of the currently applied transformations, while TF Button Text refers to the text displayed on TF buttons that don't have a specified icon.)

And that should just about do it! Now with your completed database, all you need to do is copy and paste the path to it into the text field on the startup screen and you'll be able to play TF Stacker with your own character and transformations! You can create as many databases as you want too, so go wild!



----------Updating Databases Between TF Stacker Versions----------
Sometimes the structure of databases will change when TF Stacker is updated. Here you can find all of the changes that will need to be made to update a database from an older version to the current version.

[v0.1.0 to v0.2.0]
1. The initial transformation subfolders were removed following this version, so all transformation images need to be moved from them into the base "Character_Images" folder.
[v0.2.0 to v0.3.0]
No changes necessary.