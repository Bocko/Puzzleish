using System.Collections;
using UnityEngine;

public class PlayerCauseAndEffect : MonoBehaviour
{
    public event System.Action onTeleport;

    [Header("TTD objects")]
    public GameObject handWrap;
    public GameObject ttd;

    [Header("Settings")]
    public bool isOnAtStart;
    public float offset = 45;
    public Vector3 direction = Vector3.right;
    public float effectTime = 0.05f;

    public bool inPresent { get; private set; } = true;
    CanvasGroup effect;

    bool isOn;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;
            handWrap.SetActive(isOn);
            ttd.SetActive(isOn);
        }
    }

    void Start()
    {
        effect = GameObject.Find("teleportEffect").GetComponent<CanvasGroup>();
        isOn = isOnAtStart;
        handWrap.SetActive(isOn);
        ttd.SetActive(isOn);
    }

    void Update()
    {
        if (isOn)
        {
            if (Input.GetButtonDown("TimeTravel"))
            {
                onTeleport?.Invoke();
                StartCoroutine(Fade(effectTime));
            }
        }
    }

    void Teleport()
    {
        if (inPresent)
        {
            transform.Translate(offset * 2 * direction, Space.World);
        }
        else
        {
            transform.Translate(offset * 2 * -direction, Space.World);
        }
        inPresent = !inPresent;
    }

    IEnumerator Fade(float effectTime)
    {
        float percent = 0;
        float effectSpeed = 1 / effectTime;
        float dir = 1;

        while (percent >= 0)
        {
            percent += Time.deltaTime * effectSpeed * dir;

            if (percent >= 1)
            {
                percent = 1;
                dir = -1;
                Teleport();
            }

            effect.alpha = percent;

            yield return null;
        }
    }

    public IEnumerator ExternalTeleport(float effectTime)
    {
        yield return StartCoroutine(Fade(effectTime));
    }
}
