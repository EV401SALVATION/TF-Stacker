using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.IO;

public class Startup : MonoBehaviour
{
    private string errorMessage;
    private bool hasIconsFolder;
    private bool hasBackgrounds;
    private bool hasMusic;

    [Header("Primary Game Controller")]
    public Controller controller;

    [Header("Start Menu UI Elements")]
    public Image titleLogo;
    public TMP_InputField pathInput;
    public Button startButton;
    public TMP_Text errorText;

    [Header("Gameplay Elements")]
    public Camera camera;
    public Image bottomBar;
    public Image characterImage;
    public TMP_Text characterNameText;
    public TMP_Text authorNameText;
    public TMP_Text versionText;
    public Button undoButton;
    public Button exitButton;
    public Button muteButton;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public AudioSource audioSource;

    [Header("Placeholder Sprite")]
    public Sprite imageMissing;

    [Header("Colors")]
    public Color bgColor;
    public Color bottomBarColor;
    public Color textColor;
    public Color tfButtonTextColor;
    public Color transformationTextColor;

    private void Start()
    {
        hasIconsFolder = false;
        hasBackgrounds = false;
        hasMusic = false;
        startButton.onClick.AddListener(StartGame);
    }

    // Prepares everything from the database and ensures the database is properly set up, then starts the game.
    void StartGame()
    {
        // Check for files and folders.
        if (GetDatabasePath() == false)
        {
            DisplayErrorText();
            return;
        }
        if (GetSubfolderPaths() == false)
        {
            DisplayErrorText();
            return;
        }
        if (GetNames() == false)
        {
            DisplayErrorText();
            return;
        }
        if (GetTransformations() == false)
        {
            DisplayErrorText();
            return;
        }
        if (GetColors() == false)
        {
            DisplayErrorText();
            return;
        }
        GetImageOverrides();
        GetBackgrounds();
        GetMusic();

        // Hide start screen elements.
        errorText.enabled = false;
        titleLogo.gameObject.SetActive(false);
        pathInput.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        // Set gameplay colors.
        SetColors();

        // Set gameplay text.
        characterNameText.text = controller.characterName;
        authorNameText.text = "By " + controller.authorName;

        // Show gameplay elements.
        bottomBar.gameObject.SetActive(true);
        characterImage.gameObject.SetActive(true);
        characterNameText.gameObject.SetActive(true);
        authorNameText.gameObject.SetActive(true);
        if (hasMusic)
            muteButton.gameObject.SetActive(true);
        if (hasBackgrounds)
        {
            leftArrowButton.gameObject.SetActive(true);
            rightArrowButton.gameObject.SetActive(true);
        }
        SetupImageOverrides();
        SetupDefaultImage();
        SetupTransformationButtons();

        // Play background music
        if (hasMusic)
        {
            Debug.Log("Playing music...");
            audioSource.clip = controller.backgroundMusic;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
        }
    }

    // Displays an error message to the start menu and to the console.
    void DisplayErrorText()
    {
        Debug.Log(errorMessage);
        errorText.text = errorMessage;
        errorText.enabled = true;
    }

    // Checks if the entered database path is correct and stores that path.
    bool GetDatabasePath()
    {
        // Check for empty input field.
        if (pathInput.text == "")
        {
            errorMessage = "Error: Please enter a file path to a database.";
            return false;
        }

        // Check if the specified database exists.
        controller.databasePath = pathInput.text;
        if (System.IO.Directory.Exists(controller.databasePath) == false)
        {
            errorMessage = "Error: Database folder not found.";
            return false;
        }
        else
        {
            Debug.Log("Database found.");
            return true;
        }
    }

    // Checks if the Character_Images and Transformation_Images folders exist and stores their paths for easy access.
    bool GetSubfolderPaths()
    {
        // Check for Character_Images folder.
        controller.characterImagesPath = controller.databasePath + "\\Character_Images";
        if (System.IO.Directory.Exists(controller.characterImagesPath) == false)
        {
            errorMessage = "Error: \"Character_Images\" folder not found.";
            return false;
        }

        // Check for Transformation_Icons folder.
        controller.transformationIconsPath = controller.databasePath + "\\Transformation_Icons";
        if (System.IO.Directory.Exists(controller.transformationIconsPath) == false)
            Debug.Log("\"Transformation_Icons\" folder not found. Using default icons.");
        else
            hasIconsFolder = true;

        return true;
    }

    // Gets the character and database author's names from the Names.txt file.
    bool GetNames()
    {
        try
        {
            // Set up stream reader.
            string path = controller.databasePath + "/Names.txt";
            StreamReader reader = new StreamReader(path);

            // Read the names from the file.
            try
            {
                controller.characterName = reader.ReadLine();
                controller.authorName = reader.ReadLine();
            }
            catch
            {
                errorMessage = "Error: Unable to read file \"Names.txt\".";
                return false;
            }

            // Print the names to the console and close the reader.
            Debug.Log("Character name: " + controller.characterName);
            Debug.Log("Author name: " + controller.authorName);
            reader.Close();
            return true;
        }
        catch
        {
            errorMessage = "Error: Could not load file \"Names.txt\". File may be missing from the database.";
            return false;
        }
    }

    // Gets the list of transformations from the Transformations.txt file.
    bool GetTransformations()
    {
        try
        {
            // Set up stream reader.
            string path = controller.databasePath + "/Transformations.txt";
            StreamReader reader = new StreamReader(path);

            try
            {
                // Get the number of transformations listed in the Transformations.txt file.
                string transformationCountString = reader.ReadLine();
                if (int.TryParse(transformationCountString, out int transformationCount) == false) {
                    errorMessage = "Error: Transformation count must be a non-decimal number.";
                    return false;
                }

                // Check if number of transformations is within range.
                if (transformationCount > 10 || transformationCount < 0)
                {
                    errorMessage = "Error: Transformation count cannot be greater than 10 nor less than 0.";
                    return false;
                }

                // Get the transformations from the file and put them into an array.
                controller.transformations = new string[transformationCount];
                for (int i = 0; i < transformationCount; i++)
                {
                    string transformation = reader.ReadLine();
                    if (transformation == null)
                    {
                        errorMessage = "Error: No transformation specified for Transformation #" + i + ".";
                        return false;
                    }
                    controller.transformations[i] = transformation;
                }
            }
            catch
            {
                errorMessage = "Error: Unable to read file \"Transformations.txt\".";
                return false;
            }

            // Print the transformations list to the debug log
            for (int i = 0; i < controller.transformations.Length; i++)
            {
                Debug.Log("Transformation " + (i + 1) + ": " + controller.transformations[i]);
            }

            // Close the reader.
            reader.Close();
            return true;
        }
        catch
        {
            errorMessage = "Error: Could not load file \"Transformations.txt\". File may be missing from the database.";
            return false;
        }
    }

    bool GetColors()
    {
        try
        {
            // Set up stream reader.
            string path = controller.databasePath + "/Colors.txt";
            StreamReader reader = new StreamReader(path);
            float r;
            float g;
            float b;
            string temp;

            try
            {
                // Get the background color.
                temp = reader.ReadLine();
                r = GetColorValue(reader.ReadLine());
                g = GetColorValue(reader.ReadLine());
                b = GetColorValue(reader.ReadLine());
                if (r == -1)
                {
                    errorMessage = "Error: Invalid value for background color r (line 2 of Colors.txt).";
                    return false;
                }
                if (g == -1)
                {
                    errorMessage = "Error: Invalid value for background color g (line 3 of Colors.txt).";
                    return false;
                }
                if (b == -1)
                {
                    errorMessage = "Error: Invalid value for background color b (line 4 of Colors.txt).";
                    return false;
                }
                bgColor = new Color(r, g, b);
                Debug.Log("Background Color: " + bgColor.r + ", " + bgColor.g + ", " + bgColor.b);

                // Get the bottom bar color.
                temp = reader.ReadLine();
                r = GetColorValue(reader.ReadLine());
                g = GetColorValue(reader.ReadLine());
                b = GetColorValue(reader.ReadLine());
                if (r == -1)
                {
                    errorMessage = "Error: Invalid value for bottom bar color r (line 6 of Colors.txt).";
                    return false;
                }
                if (g == -1)
                {
                    errorMessage = "Error: Invalid value for bottom bar color g (line 7 of Colors.txt).";
                    return false;
                }
                if (b == -1)
                {
                    errorMessage = "Error: Invalid value for bottom bar color b (line 8 of Colors.txt).";
                    return false;
                }
                bottomBarColor = new Color(r, g, b);
                Debug.Log("Bottom Bar Color: " + bottomBarColor.r + ", " + bottomBarColor.g + ", " + bottomBarColor.b);

                // Get the text color.
                temp = reader.ReadLine();
                r = GetColorValue(reader.ReadLine());
                g = GetColorValue(reader.ReadLine());
                b = GetColorValue(reader.ReadLine());
                if (r == -1)
                {
                    errorMessage = "Error: Invalid value for text color r (line 10 of Colors.txt).";
                    return false;
                }
                if (g == -1)
                {
                    errorMessage = "Error: Invalid value for text color g (line 11 of Colors.txt).";
                    return false;
                }
                if (b == -1)
                {
                    errorMessage = "Error: Invalid value for text color b (line 12 of Colors.txt).";
                    return false;
                }
                textColor = new Color(r, g, b);
                Debug.Log("Text Color: " + textColor.r + ", " + textColor.g + ", " + textColor.b);

                // Get the tf button text color.
                temp = reader.ReadLine();
                r = GetColorValue(reader.ReadLine());
                g = GetColorValue(reader.ReadLine());
                b = GetColorValue(reader.ReadLine());
                if (r == -1)
                {
                    errorMessage = "Error: Invalid value for transformation button text color r (line 14 of Colors.txt).";
                    return false;
                }
                if (g == -1)
                {
                    errorMessage = "Error: Invalid value for transformation button text color g (line 15 of Colors.txt).";
                    return false;
                }
                if (b == -1)
                {
                    errorMessage = "Error: Invalid value for transformation button text color b (line 16 of Colors.txt).";
                    return false;
                }
                tfButtonTextColor = new Color(r, g, b);
                Debug.Log("TF Button Text Color: " + tfButtonTextColor.r + ", " + tfButtonTextColor.g + ", " + tfButtonTextColor.b);

                // Get the tf text color.
                temp = reader.ReadLine();
                r = GetColorValue(reader.ReadLine());
                g = GetColorValue(reader.ReadLine());
                b = GetColorValue(reader.ReadLine());
                if (r == -1)
                {
                    errorMessage = "Error: Invalid value for transformation text color r (line 18 of Colors.txt).";
                    return false;
                }
                if (g == -1)
                {
                    errorMessage = "Error: Invalid value for transformation text color g (line 19 of Colors.txt).";
                    return false;
                }
                if (b == -1)
                {
                    errorMessage = "Error: Invalid value for transformation text color b (line 20 of Colors.txt).";
                    return false;
                }
                transformationTextColor = new Color(r, g, b);
                Debug.Log("Transformation Text Color: " + tfButtonTextColor.r + ", " + tfButtonTextColor.g + ", " + tfButtonTextColor.b);
            }
            catch
            {
                Debug.Log("Unable to read file \"Colors.txt\". Using default colors.");
                reader.Close();
                return true;
            }

            // Close the reader.
            reader.Close();
            return true;
        }
        catch
        {
            Debug.Log("Could not load file \"Colors.txt\". Using default colors.");
            return true;
        }
    }

    // Check if a read value is a valid color value. Return it as an int if so, return -1 if not.
    float GetColorValue(string readVal)
    {
        if (float.TryParse(readVal, out float readInt) == false)
        {
            return -1;
        }
        if (readInt > 255 || readInt < 0)
        {
            return -1;
        }
        return readInt / 255;
    }

    // Sets the gameplay colors.
    void SetColors()
    {
        camera.backgroundColor = bgColor;
        bottomBar.color = bottomBarColor;
        characterNameText.color = textColor;
        authorNameText.color = textColor;
        versionText.color = tfButtonTextColor;
        for (int i = 0; i < controller.transformationTextElements.Length; i++)
        {
            controller.transformationTextElements[i].color = transformationTextColor;
        }
    }
    
    // Get the overides of various gameplay images.
    void GetImageOverrides()
    {
        string path;
        Sprite image;

        // Image Missing Image
        path = controller.databasePath + "\\Image_Missing.png";
        image = GetImage(path);
        if (image != null)
            controller.imageMissing = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Default Transformation Icon Image
        path = controller.databasePath + "\\Transformation_Button_Background.png";
        image = GetImage(path);
        if (image != null)
            controller.tfButtonBackground = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Undo Icon Image
        path = controller.databasePath + "\\Undo_Icon.png";
        image = GetImage(path);
        if (image != null)
            controller.undoIcon = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Exit Icon Image
        path = controller.databasePath + "\\Exit_Icon.png";
        image = GetImage(path);
        if (image != null)
            controller.exitIcon = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Mute Icon Image
        path = controller.databasePath + "\\Mute_Icon.png";
        image = GetImage(path);
        if (image != null)
            controller.muteIcon = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Unmute Icon Image
        path = controller.databasePath + "\\Unmute_Icon.png";
        image = GetImage(path);
        if (image != null)
            controller.unmuteIcon = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Left Arrow Image
        path = controller.databasePath + "\\Left_Arrow.png";
        image = GetImage(path);
        if (image != null)
            controller.leftArrow = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");

        // Right Arrow Image
        path = controller.databasePath + "\\Right_Arrow.png";
        image = GetImage(path);
        if (image != null)
            controller.rightArrow = image;
        else
            Debug.Log("Image \"" + path + "\" could not be found, using default.");
    }

    // Return an Sprite from an image at the given path, or null if the image doesn't exist.
    Sprite GetImage(string path)
    {
        try
        {
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData = File.ReadAllBytes(path);
            texture.LoadImage(fileData);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        catch
        {
            return null;

        }
    }

    void SetupImageOverrides()
    {
        undoButton.gameObject.GetComponent<Image>().sprite = controller.undoIcon;
        exitButton.gameObject.GetComponent<Image>().sprite = controller.exitIcon;
        muteButton.gameObject.GetComponent<Image>().sprite = controller.muteIcon;
        leftArrowButton.gameObject.GetComponent<Image>().sprite = controller.leftArrow;
        rightArrowButton.gameObject.GetComponent<Image>().sprite = controller.rightArrow;
    }

    void GetBackgrounds()
    {
        // Check for Backgrounds folder.
        controller.backgroundsPath = controller.databasePath + "\\Backgrounds";
        if (System.IO.Directory.Exists(controller.backgroundsPath) == false)
        {
            Debug.Log("\"Backgrounds\" folder not found.");
            return;
        }

        // Check for Backgrounds.txt file.
        try
        {
            string path = controller.backgroundsPath + "/Backgrounds.txt";
            StreamReader reader = new StreamReader(path);
            try
            {
                // Get the number of backgrounds listed in the Backgrounds.txt file.
                string backgroundCountText = reader.ReadLine();
                if (int.TryParse(backgroundCountText, out int backgroundCount) == false)
                    Debug.Log("Background count must be a non-decimal number!");

                // If the background count isn't greater than zero, don't do backgrounds.
                if (backgroundCount < 1) return;

                controller.backgroundCount = backgroundCount;
                hasBackgrounds = true;
            }
            catch
            {
                Debug.Log("Unable to read file \"Backgrounds.txt\".");
            }
        }
        catch
        {
            Debug.Log("Could not load file \"Backgrounds.txt\". File may be missing from the database.");
        }
        
    }
    
    // Gets the music if it exists in the database. Supports .wav, .mp3, and .ogg files.
    void GetMusic()
    {
        string musicPath = controller.databasePath + "\\Music.";
        WWW www;
        AudioClip clip;

        // WAV
        try
        {
            // Check if the file exists.
            byte[] fileData = File.ReadAllBytes(musicPath + "wav");

            // Read from the file.
            www = new WWW(musicPath + "wav");
            clip = www.GetAudioClip();
            clip.LoadAudioData();

            // Wait until the clip is loaded.
            while (clip.loadState != AudioDataLoadState.Loaded && clip.loadState != AudioDataLoadState.Failed) ;
            
            // Print and return on load failed.
            if (clip.loadState == AudioDataLoadState.Failed)
            {
                Debug.Log("Failed to load \"Music.wav\".");
                return;
            }

            // Print on successful load.
            Debug.Log("Loaded \"Music.wav\".");
        }
        catch
        {
            // MP3
            Debug.Log("Unable to load \"Music.wav\", trying \"Music.mp3\"...");
            try
            {
                // Check if the file exists.
                byte[] fileData = File.ReadAllBytes(musicPath + "mp3");

                // Read from the file.
                www = new WWW(musicPath + "mp3");
                clip = www.GetAudioClip();
                clip.LoadAudioData();

                // Wait until the clip is loaded.
                while (clip.loadState != AudioDataLoadState.Loaded && clip.loadState != AudioDataLoadState.Failed) ;

                // Print and return on load failed.
                if (clip.loadState == AudioDataLoadState.Failed)
                {
                    Debug.Log("Failed to load \"Music.mp3\".");
                    return;
                }

                // Print on successful load.
                Debug.Log("Loaded \"Music.mp3\".");
            }
            catch
            {
                // OGG
                Debug.Log("Unable to load \"Music.mp3\", trying \"Music.ogg\"...");
                try
                {
                    // Check if the file exists.
                    byte[] fileData = File.ReadAllBytes(musicPath + "ogg");

                    // Read from the file.
                    www = new WWW(musicPath + "ogg");
                    clip = www.GetAudioClip();
                    clip.LoadAudioData();

                    // Wait until the clip is loaded.
                    while (clip.loadState != AudioDataLoadState.Loaded && clip.loadState != AudioDataLoadState.Failed) ;

                    // Print and return on load failed.
                    if (clip.loadState == AudioDataLoadState.Failed)
                    {
                        Debug.Log("Failed to load \"Music.ogg\".");
                        return;
                    }

                    // Print on successful load.
                    Debug.Log("Loaded \"Music.ogg\".");
                }
                catch
                {
                    Debug.Log("Unable to load \"Music.ogg\".");
                    return;
                }
            }
        }

        clip.name = "Background Music";
        controller.backgroundMusic = clip;
        hasMusic = true;
    }

    // Sets the starting image for the character, and also saves the path to it in the Controller for easy access.
    void SetupDefaultImage()
    {
        controller.imagePath = controller.characterImagesPath + "\\" + controller.characterName;
        controller.UpdateCharacterImage(controller.imagePath + ".png");
    }

    // Sets up the transformations and icons for each button and enables them per the Transformations.txt file.
    void SetupTransformationButtons()
    {
        // TO DO:
        // Add a check for if the transformation icons folder doesn't exist.
        // Fix icon indicator being overlapped by other buttons.
        for (int i = 0; i < controller.transformations.Length; i++)
        {
            // Set the button's transformation.
            string tf = controller.transformations[i];
            Button transformationButton = controller.transformationButtons[i];
            TransformationButton tfButtonScript = transformationButton.GetComponent<TransformationButton>();
            tfButtonScript.transformation = tf;
            tfButtonScript.transformationText.text = tf;
            tfButtonScript.transformationText.color = tfButtonTextColor;
            controller.transformationButtons[i].image.sprite = controller.tfButtonBackground;

            // Set the path to the button's icon.
            string iconPath = controller.transformationIconsPath + "\\" + tf + ".png";

            // Attempt to set the button's icon and set it to a placeholder if failed.
            if (hasIconsFolder)
            {
                try
                {
                    Texture2D tex = new Texture2D(2, 2);
                    byte[] fileData = File.ReadAllBytes(iconPath);
                    tex.LoadImage(fileData);
                    Sprite icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    tfButtonScript.icon.sprite = icon;
                    tfButtonScript.icon.gameObject.SetActive(true);
                    tfButtonScript.transformationText.gameObject.SetActive(false);
                }
                catch
                {
                    Debug.Log("Icon \"" + iconPath + "\" could not be found.");
                }
            }

            // Enable the button.
            transformationButton.gameObject.SetActive(true);
        }

        // Update the image indicators.
        controller.UpdateImageIndicators();
    }

}
