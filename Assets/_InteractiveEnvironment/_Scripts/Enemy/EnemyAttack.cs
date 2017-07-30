using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 1.0f;     // The time in seconds between each attack.（敌人攻击动画约为1.0秒，为了使得动画正常播放，该值至少设为1.0秒）
    public int attackDamage = 10;               // The amount of health taken away per attack.
    public AudioClip enemyAttackAudio;		    //敌人的攻击音效
    public float attackAnimTime = 1.0f;

    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    Follow pathFollow; 
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.


    // Use this for initialization
    void Awake() {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        pathFollow = GetComponent<Follow>();
    }

    void OnTriggerEnter(Collider other) {

        // If the entering collider is the player...
        if (other.gameObject == player) {
            // ... the player is in range.
            playerInRange = true;
            //Start attack, stop walk
            pathFollow.enabled = false;
        }
        
    }


    void OnTriggerExit(Collider other) {
        // If the exiting collider is the player...
        if (other.gameObject == player) {
            // ... the player is no longer in range.
            playerInRange = false;
            //Stop attack, start walk
            pathFollow.enabled = true;
        }
    }

    void Attack() {

        // Reset the timer.
        timer = 0f;
        // If the player has health to lose...
        if (playerHealth.currentHealth >= 0) {
            // ... damage the player.
            GameManager.gm.PlayerTakeDamage(attackDamage);
            //playerHealth.TakeDamage(attackDamage);
        }


    }

    //Update the attacking interval, called once per frame
    void Update() {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0) {
            // ... attack.
            anim.SetBool("isAttack", true);
            Attack();
        }else if (timer >= attackAnimTime) {
            anim.SetBool("isAttack", false);
        }

        // If the player has zero or less health...
        if (playerHealth.currentHealth <= 0) {
            // ... tell the animator the player is dead.
            anim.SetBool("isMove", true);
        }

        // If the enemy has die
        if (enemyHealth.currentHealth <= 0) {
            anim.SetTrigger("isDie");
            pathFollow.enabled = false;
        }
    }
}
