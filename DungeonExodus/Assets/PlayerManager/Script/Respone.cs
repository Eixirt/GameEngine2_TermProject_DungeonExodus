using System.Collections;
using System.Collections.Generic;
using Assets.PlayerManager.Script;
using UnityEngine;

public class Respone : MonoBehaviour
{
    [SerializeField] private Transform pos;

    private PlayerInputManager target;

    void OnTriggerStay(Collider other) 
    {
        if (other.tag == "player")
        {
            target = other.gameObject.GetComponent<PlayerInputManager>();
            target.moveFlag = false;
            target._characterController.Move(pos.position - target.transform.position);
            target.transform.localRotation = Quaternion.identity;
         

            Invoke("SetTrueMoveFlag",Time.deltaTime * 5);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "player")
        {
            collision.gameObject.transform.position = pos.position;
        }
    }

    void SetTrueMoveFlag()
    {
        if (target != null)
        {
            target.moveFlag = true;
            target = null;
        }
    }
}
