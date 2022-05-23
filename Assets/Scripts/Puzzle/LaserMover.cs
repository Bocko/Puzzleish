using System.Collections;
using UnityEngine;

public class LaserMover : MonoBehaviour
{
    public float offset = 17.2f;
    public float laserTime = 5;
    public Transform rayOriginAndDirection;
    public Transform respawnPoint;
    public LayerMask laserMask;
    public float laserDistance;
    public ParticleSystem redJuice;

    void Start()
    {
        StartCoroutine(LaserMoving());
    }

    IEnumerator LaserMoving()//add raycast for collision check
    {
        float percent = 0;
        float laserSpeed = 1 / laserTime;
        while (true)
        {
            percent += Time.deltaTime * laserSpeed;
            float z = Mathf.PingPong(percent, offset * 2) - offset;
            CheckLaserCollision();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);

            yield return null;
        }
    }

    void CheckLaserCollision()
    {
        if(Physics.Raycast(rayOriginAndDirection.position,rayOriginAndDirection.forward,out RaycastHit hitinfo, laserDistance, laserMask))
        {
            Instantiate(redJuice, hitinfo.point,Quaternion.identity);
            hitinfo.transform.position = respawnPoint.position;
        }
    }
}
