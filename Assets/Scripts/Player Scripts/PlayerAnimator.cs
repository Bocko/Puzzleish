using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator handAnimator;
    PlayerItemPickUper pickuper;

    void Start()
    {
        handAnimator = GetComponentInChildren<Animator>();
        pickuper = GetComponent<PlayerItemPickUper>();

        pickuper.HandGrabbed += CloseHand;
        pickuper.HandReleased += OpenHand;
    }

    void CloseHand()
    {
        handAnimator.SetTrigger("TriggerClose");
    }

    void OpenHand()
    {
        handAnimator.SetTrigger("TriggerOpen");
    }
}
