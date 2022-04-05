using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public enum item { JETPACK, TTDEVICE }
    public enum itemStatus { ON, OFF }

    public item itemToSwitch;
    public itemStatus statusToSetItem;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (itemToSwitch)
            {
                case item.JETPACK:
                {
                    PlayerJetpack pjp = other.GetComponent<PlayerJetpack>();
                    if (pjp.IsOn == (statusToSetItem == itemStatus.ON)) return;

                    pjp.IsOn = statusToSetItem == itemStatus.ON;
                    if (NotificationManager.instance != null)
                    {
                        NotificationManager.instance.ShowNotification(pjp.IsOn ? "JETPACK ENABLED!" : "JETPACK DISABLED!", 1);
                    }
                    break;
                }
                case item.TTDEVICE:
                {
                    PlayerCauseAndEffect playerCnE = other.GetComponent<PlayerCauseAndEffect>();
                    if (playerCnE.isOn == (statusToSetItem == itemStatus.ON)) return;

                    playerCnE.isOn = statusToSetItem == itemStatus.ON;
                    if (NotificationManager.instance != null)
                    {
                        NotificationManager.instance.ShowNotification(playerCnE.isOn ? "TIME DEVICE ENABLED!" : "TIME DEVICE DISABLED!", 1);
                    }
                    break;
                }
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, itemToSwitch == item.JETPACK ? "d_Spotlight Icon" : "UnityEditor.ProfilerWindow@2x", false, statusToSetItem == itemStatus.ON ? Color.green : Color.red);
    }
}
