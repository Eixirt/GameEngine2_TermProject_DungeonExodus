using System;
using UnityEngine;

namespace Reflection_Manager
{
    public class MirrorReflectionManager : MonoBehaviour
    {
        private Camera _mirrorCamera;
        void Awake()
        {
            _mirrorCamera = GetComponent<Camera>();
        }
        // Scene의 카메라 culling이 이루어지기 전 호출
        public void OnPreCull()
        {
            _mirrorCamera.ResetProjectionMatrix();
            _mirrorCamera.projectionMatrix = _mirrorCamera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }
        public void OnPreRender()
        {
            GL.invertCulling = true;
        }

        public void OnPostRender()
        {
            GL.invertCulling = false;
        }
    }
}
