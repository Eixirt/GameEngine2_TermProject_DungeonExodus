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
        Reflected_Laser,
    }

    public class LaserReflectionManager : MonoBehaviour
    {
        public LaserType laserColor;
        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents;
        public GameObject endPointLaserObject;
    
        void Awake()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (laserColor == LaserType.Reflected_Laser)
                return;
            
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            int i = 0;
            while (i < numCollisionEvents)
            {
                if (rb) 
                {
                    switch (other.tag)
                    {
                        case "mirror":
                        {
                            MirrorReflectionManager m = other.transform.Find("Monitor")
                                .GetComponent<MirrorReflectionManager>();
                            
                            Vector3 collisionPos = collisionEvents[i].intersection;

                            RaycastHit hit;
                            Physics.Raycast(transform.position, transform.right, out hit);
                            Debug.DrawRay(transform.position, transform.right * hit.distance, Color.red);

                            m.CallCollisionWithLaser(hit.point, transform.localPosition, laserColor);
                            
                            Vector3 mirrorCameraDirection = other.transform.Find("Monitor").Find("Camera").GetComponent<Camera>().transform.rotation * Vector3.right;

                            var go = this.gameObject;
                            // GameObject reflectLaserObject = Instantiate(go, collisionPos, Quaternion.LookRotation(mirrorCameraDirection), other.transform);
                            //reflectLaserObject.transform.localScale = new Vector3(10, 3, 3);
                            //Destroy(reflectLaserObject, 0.1f);
                        }
                            break;
                        case "Untagged":
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