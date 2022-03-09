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

    [Header("Stance Indicator")]
    public Image stanceHolder;
    public Sprite standing;
    public Sprite crouching;
    bool crouched;

    PlayerItemPickUper playerItemPickUper;
    PlayerMovement playerMovement;

    Color defaultPointerColor;

    void Start()
    {
        playerItemPickUper = FindObjectOfType<PlayerItemPickUper>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        itemName.text = "";
        defaultPointerColor = pointer.color;
        crouched = playerMovement.isCrouched;
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

        if(crouched != playerMovement.isCrouched)
        {
            crouched = playerMovement.isCrouched;
            if (crouched)
            {
                stanceHolder.sprite = crouching;
            }
            else
            {
                stanceHolder.sprite = standing;
            }
        }
    }
}
