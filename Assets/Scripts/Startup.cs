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

    [Header("Primary Game Controller")]
    public Controller controller;

    [Header("Start Menu UI Elements")]
    public Button startButton;
    public TMP_InputField pathInput;
    public TMP_Text errorText;

    [Header("Database/Author Name Elements")]
    public TMP_Text characterNameText;
    public TMP_Text authorNameText;

    [Header("Placeholder Sprite")]
    public Sprite imageMissing;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    // Prepares everything from the database and ensures the database is properly set up, then starts the game.
    void StartGame()
    {
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

        // Hide start screen elements.
        errorText.enabled = false;
        startButton.gameObject.SetActive(false);
        pathInput.gameObject.SetActive(false);

        // Show gameplay elements.
        characterNameText.text = controller.characterName;
        characterNameText.gameObject.SetActive(true);
        authorNameText.text = controller.authorName;
        authorNameText.gameObject.SetActive(true);
        SetupDefaultImage();
        SetupTransformationButtons();
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
        {
            errorMessage = "Error: \"Transformation_Icons\" folder not found.";
            return false;
        }

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

    // Sets the starting image for the character, and also saves the path to it in the Controller for easy access.
    void SetupDefaultImage()
    {
        controller.imagePath = controller.characterImagesPath + "\\" + controller.characterName;
        controller.UpdateCharacterImage(controller.imagePath + ".png");
    }

    // Sets up the transformations and icons for each button and enables them per the Transformations.txt file.
    void SetupTransformationButtons()
    {
        for (int i = 0; i < controller.transformations.Length; i++)
        {
            // Set the button's transformation.
            string tf = controller.transformations[i];
            Button transformationButton = controller.transformationButtons[i];
            transformationButton.GetComponent<TransformationButton>().transformation = tf;

            // Set the path to the button's icon.
            string iconPath = controller.transformationIconsPath + "\\" + tf + ".png";

            // Attempt to set the button's icon and set it to a placeholder if failed.
            try
            {
                Texture2D tex = new Texture2D(2, 2);
                byte[] fileData = File.ReadAllBytes(iconPath);
                tex.LoadImage(fileData);
                Sprite icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                controller.transformationButtons[i].image.sprite = icon;
            }
            catch
            {
                controller.transformationButtons[i].image.sprite = imageMissing;
                Debug.Log("Error: Icon \"" + iconPath + "\" could not be found.");
            }

            // Enable the button.
            transformationButton.gameObject.SetActive(true);

            // Update the image indicators.
            controller.UpdateImageIndicators();
        }
    }

}
