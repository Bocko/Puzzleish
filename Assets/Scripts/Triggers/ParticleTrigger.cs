using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem particle;
    public Transform particlePos;

    ParticleSystem spawnedParticle;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(spawnedParticle == null)
            {
                spawnedParticle = Instantiate(particle, particlePos);
            }
            if (!particle.isPlaying)
            {
                spawnedParticle.Play();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
