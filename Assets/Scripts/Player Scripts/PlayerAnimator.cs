using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator handAnimator;
    PlayerItemPickUper pickuper;
    PlayerCauseAndEffect playerCnE;

    private const string handCloseTriggerName = "TriggerClose";
    private const string handOpenTriggerName = "TriggerOpen";
    private const string snapTriggerName = "TTDSnap";

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
        handAnimator.SetTrigger(handCloseTriggerName);
    }

    void OpenHand()
    {
        handAnimator.SetTrigger(handOpenTriggerName);
    }

    void Snap()
    {
        handAnimator.SetTrigger(snapTriggerName);
    }
}
