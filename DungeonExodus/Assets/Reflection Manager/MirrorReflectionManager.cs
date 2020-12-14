using System;
using UnityEngine;

namespace Reflection_Manager
{
    public class MirrorReflectionManager : MonoBehaviour
    {
        public GameObject mirrorLaserPrefab;
        private Camera _mirrorCamera;

        void Awake()
        {
            _mirrorCamera = transform.Find("Camera").GetComponent<Camera>();
        }

        private void Start()
        {
            
        }

        public void CallCollisionWithLaser(Vector3 collisionPos, Vector3 shootPos, LaserType laserColor)
        {
            Vector3 relativePos = shootPos - collisionPos;

            var transform1 = _mirrorCamera.transform;
            Vector3 mirrorCameraDirection = transform1.localRotation * Vector3.right;
            //Vector3 reflectedVector = Vector3.Reflect(shootVector, mirrorCameraDirection);
            //Debug.Log(mirrorCameraDirection + " " + reflectedVector);
            //Debug.Log(_mirrorCamera.transform.rotation);
            //Debug.Log(relativePos);
            GameObject reflectLaserObject = Instantiate(mirrorLaserPrefab, collisionPos, 
                Quaternion.LookRotation(Vector3.Reflect(relativePos, transform1.forward.normalized)), this.transform);

            Destroy(reflectLaserObject, 0.1f);
        }
    }
}
