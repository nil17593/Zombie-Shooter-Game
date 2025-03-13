using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>()!=null)
        {
            //Zombie.isAttacking = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>() != null)
        {
            //Zombie.isAttacking = false;
        }
    }
}
