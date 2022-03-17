using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator handAnimator;

    private void Start()
    {
        PlayerItemPickUper.HandGrabbed += CloseHand;
        PlayerItemPickUper.HandReleased += OpenHand;
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
