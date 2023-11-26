using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftArrowButton : MonoBehaviour
{
    
    public Button button;
    public Controller controller;
    public bool active;

    private void Start()
    {
        button.onClick.AddListener(Decrement);
    }

    void Decrement()
    {
        controller.DecrementBackground();
    }
}
