using System;
using UnityEngine;

namespace Reflection_Manager
{
    public class MirrorReflectionManager : MonoBehaviour
    {
        public GameObject whiteMirrorLaserPrefab;
        public GameObject redMirrorLaserPrefab;
        public GameObject blueMirrorLaserPrefab;

        private GameObject _laser;
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
            switch (laserColor)
            {
                case LaserType.Blue_Laser:
                    _laser = blueMirrorLaserPrefab;
                    break;
                case LaserType.White_Laser:
                    _laser = whiteMirrorLaserPrefab;
                    break;
                case LaserType.Red_Laser:
                    _laser = redMirrorLaserPrefab;
                    break;
            }
            
            Vector3 relativePos = collisionPos - shootPos;
            relativePos = relativePos.normalized;
            
            Vector3 collisionVector = gameObject.transform.right;

            //Debug.DrawLine(transform.position, transform.position + collisionVector, Color.blue, 1.0f);
            //Debug.DrawLine(transform.position,transform.position + gameObject.transform.forward, Color.red, 1.0f);
            //Debug.DrawLine(transform.position,transform.position + gameObject.transform.up, Color.yellow, 1.0f);
            
            Vector3 reflectedVector = Vector3.Reflect(relativePos, collisionVector);
            
            //Debug.DrawLine(collisionPos, shootPos, Color.red, 1.0f);
            Debug.DrawLine(transform.position, transform.position + 2 * reflectedVector, Color.white, 1.0f);
            
            //Vector3 reflectedVector = Vector3.Reflect(shootVector, mirrorCameraDirection);
            //Debug.Log(mirrorCameraDirection + " " + reflectedVector);
            //Debug.Log(_mirrorCamera.transform.rotation);
            //Debug.Log(relativePos);
            GameObject reflectLaserObject = Instantiate(_laser, collisionPos, 
                Quaternion.LookRotation(reflectedVector), this.transform);

            Debug.DrawLine(transform.position, transform.position + 2 * reflectLaserObject.transform.forward, Color.black, 1.0f);

            Destroy(reflectLaserObject, 0.1f);
        }
    }
}
