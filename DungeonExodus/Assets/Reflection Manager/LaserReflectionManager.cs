using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflectionManager : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject endPointLaserObject;
    
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;
        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                //Vector3 force = collisionEvents[i].velocity * 10;
                var go = endPointLaserObject;
                Instantiate(go, pos, Quaternion.LookRotation(-part.transform.localPosition.normalized));
                //rb.AddForce(force);
            }

            i++;
        }
    }
}
