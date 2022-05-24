using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Trigger names")]
    public string handCloseTriggerName = "TriggerClose";
    public string handOpenTriggerName = "TriggerOpen";
    public string snapTriggerName = "TTDSnap";

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
