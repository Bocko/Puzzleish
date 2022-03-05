using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Image pointer;
    public TextMeshProUGUI itemName;
    public Color highlightedPointerColor;

    PlayerItemPickUper playerItemPickUper;

    Color defaultPointerColor;

    void Start()
    {
        playerItemPickUper = FindObjectOfType<PlayerItemPickUper>();
        itemName.text = "";
        defaultPointerColor = pointer.color;
    }

    void Update()
    {
        itemName.text = playerItemPickUper.currentItemName;
        if (playerItemPickUper.moveable)
        {
            pointer.color = highlightedPointerColor;
        }
        else
        {
            pointer.color = defaultPointerColor;
        }
    }
}
