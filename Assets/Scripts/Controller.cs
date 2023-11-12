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

    [Header("Folder Paths")]
    public string databasePath;
    public string characterImagesPath;
    public string transformationIconsPath;

    [Header("Image Path")]
    public string imagePath;

    [Header("Names")]
    public string characterName;
    public string authorName;

    [Header("Object References")]
    public SpriteRenderer characterSprite;
    public Button undoButton;

    [Header("Useful Image Storage")]
    public Texture2D defaultTexture;
    public Sprite imageMissing;

    [Header("Lists")]
    public string[] transformations;
    public Button[] transformationButtons;
    public TMP_Text[] transformationTextElements;

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

            characterSprite.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        catch
        {
            characterSprite.sprite = imageMissing;
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
