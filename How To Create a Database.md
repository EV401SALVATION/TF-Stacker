[]CREATING A DATABASE[]
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
    Since you can specify your own transformations, you can also create your own icons for the transformation buttons! These icons must be 256x256 pixels, and should be named EXACTLY the same as the transformations listed in the Transformations.txt file. For example, the icon for a transformation listed as "Inflatable" should be named "Inflatable.png". The game won't break if these icons are missing, and instead will display an "Image Missing" placeholder image for the button.
    In the future, I plan on changing it so that missing icons will instead be displayed as generic buttons with the transformation listed on them as text.

5. Add the default character image.
    Every database needs a default character image for you to start out with. This should be your character in their natural state, without any transformations applied to them. The image file should be named "<Character>.png" with <Character> being replaced with the character's name EXACTLY as it is specified in the Names.txt file (you're probably starting to see a pattern here...). Place this image into the "Character_Images" folder. Just like with the icons, if this is missing it will be displayed with the placeholder image instead.

6. Add the rest of the transformation images.
    Now it's time for the fun part! Each transformation image should follow the naming format <Character>%<Transformation>%<Transformation>.png with <Character> replaced by the character's name EXACTLY as it is specified in the Names.txt file and each <Transformation> being replaced with the corresponding transformation EXACTLY as it is specified in the Transformations.txt file. You can have up to ten transformations, separated by '%' symbols, and these are in the order of earliest transformation to latest (i.e. if a Draden was first transformed into an Inflatable and then a Standee, then the resulting image would be named "Draden%Inflatable%Standee.png").

    All of these images are placed into the "Character_Images" folder.

    Once again, if a transformation image is missing, the game will display it with the placeholder. As the number of these images quickly skyrockets the more TFs are added, it's only natural that many combinations won't have images.

And that should just about do it! Now with your completed database, all you need to do is copy and paste the path to it into the text field on the startup screen and you'll be able to play TF Stacker with your own character and transformations! You can create as many databases as you want too, so go wild!



[]Updating Databases Between TF Stacker Versions[]
Sometimes the structure of databases will change when TF Stacker is updated. Here you can find all of the changes that will need to be made to update a database from an older version to the current version.

[v0.1.0]
1. The initial transformation subfolders were removed following this version, so all transformation images need to be moved from them into the base "Character_Images" folder.