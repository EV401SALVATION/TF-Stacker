using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MuteButton : MonoBehaviour
{
    public Button button;
    public Controller controller;

    private void Start()
    {
        button.onClick.AddListener(Mute);
    }

    void Mute()
    {
        controller.ChangeMute();
    }
}
