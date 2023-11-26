using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Controller : MonoBehaviour
{
    // Transformation Stack
    private TransformationNode head = null;
    private int transformationCount;
    private int currentBackgroundID = 0;
    private bool musicMuted = false;

    [Header("Folder Paths")]
    public string databasePath;
    public string characterImagesPath;
    public string transformationIconsPath;
    public string backgroundsPath;

    [Header("Image Path")]
    public string imagePath;

    [Header("Names")]
    public string characterName;
    public string authorName;

    [Header("Object References")]
    public Image characterImage;
    public Image backgroundImage;
    public Button undoButton;
    public Button muteButton;
    public AudioSource audioSource;

    [Header("Image References")]
    public Texture2D defaultTexture;
    public Sprite imageMissing;
    public Sprite tfButtonBackground;
    public Sprite undoIcon;
    public Sprite exitIcon;
    public Sprite muteIcon;
    public Sprite unmuteIcon;
    public Sprite leftArrow;
    public Sprite rightArrow;

    [Header("Audio References")]
    public AudioClip backgroundMusic;

    [Header("Lists")]
    public string[] transformations;
    public Button[] transformationButtons;
    public TMP_Text[] transformationTextElements;

    [Header("Other Variables")]
    public int backgroundCount;

    // Transforms the character.
    public void TransformCharacter(string transformation)
    {
        // Push a transformation onto the stack.
        PushTransformation(transformation);

        // Update the image indicators.
        UpdateImageIndicators();

        // Update the transformation text stack
        transformationTextElements[transformationCount - 1].text = transformation;
        transformationTextElements[transformationCount - 1].gameObject.SetActive(true);

        // Update the character's image.
        UpdateCharacterImage(imagePath + ".png");
    }

    // Undoes the most recent transformation.
    public void UndoTransformation()
    {
        // Update the transformation text stack
        transformationTextElements[transformationCount - 1].gameObject.SetActive(false);

        // Pop the most recent transformation from the stack.
        string tf = PopTransformation();

        // Re-enable that transformation's button.
        for (int i = 0; i < transformationButtons.Length; i++)
        {
            if (transformationButtons[i].GetComponent<TransformationButton>().transformation == tf)
            {
                transformationButtons[i].gameObject.SetActive(true);
            }
        }

        // Update the image indicators.
        UpdateImageIndicators();

        // Update the character's image.
        UpdateCharacterImage(imagePath + ".png");
    }

    // Pushes a transformation onto the stack.
    public void PushTransformation(string transformation)
    {
        // If this is the first transformation in the stack, show the undo button.
        if (head == null)
            undoButton.gameObject.SetActive(true);

        // Update the image path
        imagePath = imagePath + "%" + transformation;

        // Add a new TransformationNode to the stack.
        TransformationNode newNode = new TransformationNode(transformation);
        newNode.next = head;
        head = newNode;
        transformationCount += 1;
        if (transformationCount > 10)
            transformationCount = 10;

        // Print debug information to the console.
        Debug.Log("Pushed transformation: " + head.transformation);
    }

    // Pops a transformation from the stack.
    public string PopTransformation()
    {
        // Pop off the head transformation.
        string returnTF = head.transformation;
        head = head.next;

        // Update the image path.
        imagePath = imagePath.Remove((imagePath.Length - (returnTF.Length + 1)), (returnTF.Length + 1));

        // If stack is emptied, hide the undo button.
        if (head == null)
            undoButton.gameObject.SetActive(false);

        transformationCount -= 1;
        if (transformationCount < 0)
            transformationCount = 0;

        // Print debug information to the console.
        Debug.Log("Popped transformation: " + returnTF);
        return returnTF;
    }

    // Update the character's image to that stored at the specified path, or to the placeholder if that image doesn't exist.
    public void UpdateCharacterImage(string newImagePath)
    {
        try
        {
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData = File.ReadAllBytes(newImagePath);
            texture.LoadImage(fileData);

            characterImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        catch
        {
            characterImage.sprite = imageMissing;
            Debug.Log("Error: Image \"" + newImagePath + "\" could not be found.");
        }
    }

    // Updates the image indicators for the different transformation buttons.
    public void UpdateImageIndicators()
    {
        for (int i = 0; i < transformationButtons.Length; i++)
        {
            if (transformationButtons[i].enabled == true)
            {
                // Get the transformation button component.
                TransformationButton tfButton = transformationButtons[i].gameObject.GetComponent<TransformationButton>();

                // Check whether to enable the transformation button's image indicator.
                string nextImagePath = imagePath + "%" + tfButton.transformation + ".png";
                if (System.IO.File.Exists(nextImagePath))
                    tfButton.imageIndicator.enabled = true;
                else
                    tfButton.imageIndicator.enabled = false;
            }
        }
    }

    // Increment the background image when the right arrow is pressed.
    public void IncrementBackground()
    {
        currentBackgroundID++;
        if (currentBackgroundID > backgroundCount)
        {
            currentBackgroundID = 0;
        }

        // If the ID is 0 (default no background), disable the background image.
        if (currentBackgroundID == 0)
        {
            backgroundImage.gameObject.SetActive(false);
            return;
        }
        else
        {
            backgroundImage.sprite = GetBackgroundImage(currentBackgroundID);
            backgroundImage.gameObject.SetActive(true);
        }
    }

    // Decrement the background image when the left arrow is pressed.
    public void DecrementBackground()
    {
        currentBackgroundID--;
        if (currentBackgroundID < 0)
        {
            currentBackgroundID = backgroundCount;
        }

        // If the ID is 0 (default no background), disable the background image.
        if (currentBackgroundID == 0)
        {
            backgroundImage.gameObject.SetActive(false);
            return;
        }
        else
        {
            backgroundImage.sprite = GetBackgroundImage(currentBackgroundID);
            backgroundImage.gameObject.SetActive(true);
        }
    }

    // Tries to get the background image corresponding to the given ID from the database.
    public Sprite GetBackgroundImage(int ID)
    {
        // Set the path for the background image.
        string backgroundImagePath = backgroundsPath + "\\Background_" + ID.ToString() + ".png";

        // Try to get the image, and return the placeholder if it can't be found.
        try
        {
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData = File.ReadAllBytes(backgroundImagePath);
            texture.LoadImage(fileData);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        catch
        {
            Debug.Log("Error: Image \"" + backgroundImagePath + "\" could not be found.");
            return imageMissing;
        }
    }

    // Toggle music mute.
    public void ChangeMute()
    {
        if (musicMuted == false)
        {
            audioSource.volume = 0;
            muteButton.image.sprite = unmuteIcon;
            musicMuted = true;
        }
        else
        {
            audioSource.volume = 1;
            muteButton.image.sprite = muteIcon;
            musicMuted = false;
        }
    }
}

// Basic node class for the stack.
public class TransformationNode
{
    public string transformation;
    public TransformationNode next;

    public TransformationNode(string tf)
    {
        transformation = tf;
        next = null;
    }
}
