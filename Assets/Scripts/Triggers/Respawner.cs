using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Respawner : MonoBehaviour
{
    public Transform respawnPos;
    public Color gizmoColor;

    //FOR THIS TO WORK ON THE PLAYER I HAD TO TURN ON "AUTO SYNC TRANSFORMS" IN THE PROJECT SETTINGS'S PHYSICS TAB
    //if this causes any problems just turn it off and check if the collider has a character controller
    //and if it has one disable it set the pos and then reenable it
    void OnTriggerEnter(Collider other)
    {
        print(other.name);
        other.transform.position = respawnPos.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "RotateTool@2x", false, gizmoColor);
        Gizmos.DrawIcon(respawnPos.position, "PreTexR@2x", false, gizmoColor);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
