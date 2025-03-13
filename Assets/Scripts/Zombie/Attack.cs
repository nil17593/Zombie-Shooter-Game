using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [SerializeField] private Zombie zombie;
    [SerializeField] private LayerMask whatIsPlayer;
    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
    //    if (playerController != null)
    //    {
    //        Debug.Log("AYAYAYAYAYAYA");
    //        playerController.TakeDamage(zombie.GetDamage());
    //    }
    //}

    private void Update()
    {
        //Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(transform.position, .5f, whatIsPlayer);
        //for(int i = 0; i < playerToDamage.Length; i++)
        //{
        //    playerToDamage[i].GetComponent<PlayerController>().TakeDamage(zombie.GetDamage());
        //}
    }
}
