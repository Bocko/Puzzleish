using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public enum item { JETPACK, TTDEVICE }
    public enum itemStatus { ON, OFF }

    [Header("Settings")]
    public item itemToSwitch;
    public itemStatus statusToSetItem;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag))
        {
            switch (itemToSwitch)
            {
                case item.JETPACK:
                {
                    SwitchJetpack(other.GetComponent<PlayerJetpack>());
                    break;
                }
                case item.TTDEVICE:
                {
                    SwitchTTD(other.GetComponent<PlayerCauseAndEffect>());
                    break;
                }
            }
        }
    }

    void SwitchJetpack(PlayerJetpack pjp)
    {
        if (pjp.IsOn == (statusToSetItem == itemStatus.ON)) return;

        pjp.IsOn = statusToSetItem == itemStatus.ON;
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowNotification(pjp.IsOn ? "JETPACK ENABLED!" : "JETPACK DISABLED!", 1);
        }
    }

    void SwitchTTD(PlayerCauseAndEffect playerCnE)
    {
        if (playerCnE.IsOn == (statusToSetItem == itemStatus.ON)) return;

        playerCnE.IsOn = statusToSetItem == itemStatus.ON;
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowNotification(playerCnE.IsOn ? "TIME DEVICE ENABLED!" : "TIME DEVICE DISABLED!", 1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, itemToSwitch == item.JETPACK ? UnityIcons.JetpackIcon : UnityIcons.TTDIcon, false, statusToSetItem == itemStatus.ON ? Color.green : Color.red);
    }
}
