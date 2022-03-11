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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FUHand();
        }
    }

    void CloseHand()
    {
        handAnimator.SetTrigger("TriggerClose");
    }

    void OpenHand()
    {
        handAnimator.SetTrigger("TriggerOpen");
    }

    void FUHand()
    {
        handAnimator.SetTrigger("TriggerFUClose");
    }

}
