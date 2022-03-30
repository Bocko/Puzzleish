using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnCToPlayer : MonoBehaviour
{
    void Start()
    {
        GameObject p = FindObjectOfType<CharacterController>().gameObject;
        p.AddComponent<PlayerCauseAndEffect>();
    }
}
