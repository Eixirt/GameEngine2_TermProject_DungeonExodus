using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reflection_Manager
{
    public enum LaserType
    {
        Red_Laser,
        White_Laser,
        Blue_Laser,
    }

    public class LaserReflectionManager : MonoBehaviour
    {
        public bool isReflected = false;
        public LaserType laserColor;
        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents;
        public GameObject endPointLaserObject;
    
        void Awake()
        {
            part = GetComponent<ParticleSystem>();
            if(collisionEvents == null)
                collisionEvents = new List<ParticleCollisionEvent>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (isReflected)
                return;

            if (collisionEvents == null)
            {
                collisionEvents = new List<ParticleCollisionEvent>();
            }
            
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            int i = 0;
            while (i < numCollisionEvents)
            {
                if (rb) 
                {
                    //Debug.Log(other.gameObject + " " + other.tag);
                    switch (other.tag)
                    {
                        case "mirror_monitor":
                        {
                            //MirrorReflectionManager m = other.transform.Find("Monitor").GetComponent<MirrorReflectionManager>();
                            MirrorReflectionManager m = other.transform.parent.GetComponent<MirrorReflectionManager>();
                            
                            //Debug.DrawRay(transform.position, transform.right * 55f, Color.red, 1f);
                            //Debug.Log("hit: " + hit.transform.gameObject + " " + laserColor);
                            
                            //m.CallCollisionWithLaser(hit.point, transform.localPosition, laserColor);
                            m.CallCollisionWithLaser(collisionEvents[i].intersection, transform.position, laserColor);
                        }
                            break;
                        case "block":
                        {
                            //Vector3 force = collisionEvents[i].velocity * 10;
                        
                            Vector3 pos = collisionEvents[i].intersection;
                            var go = endPointLaserObject;
                            Instantiate(go, pos, Quaternion.LookRotation(-part.transform.localPosition.normalized));
                        }
                            break;
                    }
                }

                i++;
            }
        }
    
    }
}