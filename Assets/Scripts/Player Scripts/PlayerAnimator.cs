using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator handAnimator;
    PlayerItemPickUper pickuper;
    PlayerCauseAndEffect playerCnE;

    void Start()
    {
        handAnimator = GetComponentInChildren<Animator>();
        pickuper = GetComponent<PlayerItemPickUper>();
        playerCnE = GetComponent<PlayerCauseAndEffect>();

        pickuper.HandGrabbed += CloseHand;
        pickuper.HandReleased += OpenHand;
        playerCnE.onTeleport += Snap;
    }

    void CloseHand()
    {
        handAnimator.SetTrigger("TriggerClose");
    }

    void OpenHand()
    {
        handAnimator.SetTrigger("TriggerOpen");
    }

    void Snap()
    {
        handAnimator.SetTrigger("TTDSnap");
    }
}
