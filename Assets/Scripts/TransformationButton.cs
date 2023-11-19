using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransformationButton : MonoBehaviour
{
    
    public Button button;
    public Controller controller;
    public string transformation;
    public Texture2D texture;
    public bool active;
    public Image icon;
    public Image imageIndicator;
    public TMP_Text transformationText;

    private void Start()
    {
        button.onClick.AddListener(TransformCharacter);
    }

    void TransformCharacter()
    {
        controller.TransformCharacter(transformation);
        gameObject.SetActive(false);
    }
}
