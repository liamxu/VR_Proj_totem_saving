using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 20;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.

    public AudioClip enemyDeathClip;                // The sound to play when the enemy dies.
    public AudioClip enemyHurtClip;		            // The sound to play when the enemy hurts.

    ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    CapsuleCollider capsuleCollider;            // Reference to the capsule collider.

    private Animator anim;          //敌人的Animator组件
    private Collider colli;         //敌人的Collider组件
    private Rigidbody rigid;        //敌人的rigidbody组件

	bool isDamage;
    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.

    private GameObject hpBar;       // 测试，处理enemy血条
	float timer = 0.0f;                                // Timer for counting up to change back to move.



    //初始化，获取敌人的组件
    void Awake() {
        // Setting up the references.
		hitParticles = GetComponentInChildren<ParticleSystem>(true);
        capsuleCollider = GetComponent<CapsuleCollider>();

        anim = GetComponent<Animator>();    //获取敌人的Animator组件
        colli = GetComponent<Collider>();   //获取敌人的Collider组件
        rigid = GetComponent<Rigidbody>();	//获取敌人的Rigidbody组件

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }

	void Update(){
		timer += Time.deltaTime;

		if (timer > 0.3f) {
			anim.SetBool ("isDamaged", false);
			anim.SetBool ("isMove", true);
		}
    }

    //敌人受伤函数，用于PlayerAttack脚本中调用
	public void TakeDamage(int damage, Vector3 hitPoint) {

        // If the enemy is dead, no need to take damage so exit the function.
        if (isDead)
            return;

	    // Play the hitParticles. First need to set the position of the particle system to where the hit was sustained.
        //hitParticles.transform.position = hitPoint;
        hitParticles.Play();
   
        currentHealth -= damage;                        //敌人受伤扣血
    
		if (enemyHurtClip != null) {              //在敌人位置处播放敌人受伤音效
			AudioSource.PlayClipAtPoint (enemyHurtClip, transform.position);
			anim.SetBool ("isDamaged", true);
			timer = 0.0f;
		}

        if (currentHealth <= 0) {
            Death();
        }
        
    }


    void Death() {
        // The enemy is dead.
        isDead = true;

        anim.applyRootMotion = true;    //设置Animator组件的ApplyRootMotion属性，使敌人的移动与位移受动画的影响
        // Tell the animator that the enemy is dead. Play the dead animation
        anim.SetTrigger("isDie");
        


        // Increase the score by the enemy's score value.
        if (GameManager.gm != null) {
            GameManager.gm.AddScore(scoreValue);
        }
        if (EnemyManager.em != null) {
            EnemyManager.em.AdjustEnemyCount(-1);
        } 


        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        AudioSource.PlayClipAtPoint (enemyHurtClip, transform.position);

        // Find and disable the Nav Mesh Agent.

        
        // Destroy enemy with some process
        // Turn the collider off. so shots can pass through it. the enemy won't collide with others
        colli.enabled = false;          //禁用敌人的collider组件，使其不会与其他物体发生碰撞
        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        rigid.useGravity = true;       //因为敌人的collider组件被禁用，敌人会因重力穿过地形系统下落

		isSinking = true;

        // After 3 seconds destory the enemy.
        Destroy(gameObject, 3.0f);
    }

}
