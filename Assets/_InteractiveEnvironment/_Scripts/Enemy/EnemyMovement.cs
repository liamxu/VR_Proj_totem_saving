using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        public GameObject targetBuild;   // Reference to the target attacking building
        Transform player;               // Reference to the player's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        Animator anim;

        void Awake ()
        {
            // Set up the references.
            player = GameObject.FindGameObjectWithTag ("Player").transform;
            playerHealth = player.GetComponent <PlayerHealth> ();
            targetBuild = player.gameObject;

            enemyHealth = GetComponent <EnemyHealth> ();
            anim = GetComponent<Animator>();
        }


        void Update ()
        {
            //Debug.Log("Rabbit stop move,start Attack=========" + enemyHealth.currentHealth.ToString() + ", Player current health" + playerHealth.currentHealth.ToString());
            // If the enemy and the player have health left...
            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                // ... set the destination of the nav mesh agent to the player.
                // nav.SetDestination (player.position);
                //nav.SetDestination(targetBuild.transform.position);
                anim.SetBool("isMove", true);
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.


            }
        }
    }
}
