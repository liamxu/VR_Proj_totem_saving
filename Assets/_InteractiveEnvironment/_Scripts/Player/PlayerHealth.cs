using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int startHealth = 100;                               // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    //PlayerMovement playerMovement;                              // Reference to the player's movement.
    //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool isDamaged;                                             // True when the player gets damaged.



	//初始化函数，设置玩家当前血量
	void Awake () {
        playerAudio = GetComponent<AudioSource>();

        // Set the initial health of the player.
        currentHealth = startHealth;
	}
	
	//每帧执行一次，检测玩家是否存活
	void Update () {
		if (currentHealth <= 0) {
            isDead = true;
		}

        //To-Do：牛角增加被攻击动画
        //if (isDamaged) {
        //    // ... set the colour of the damageImage to the flash colour.
        //    damageImage.color = Color.red;
        //} else {
        //    // ... transition the colour back to clear.
        //    damageImage.color = Color.Lerp(Color.red, Color.white, 1.5f);
        //}
        isDamaged = false;

	}

	//玩家扣血函数，在GameManager脚本中调用
	public void TakeDamage(int damageAmount) {
        isDamaged = true;
		currentHealth -= damageAmount;
        healthSlider.value = currentHealth;
        playerAudio.Play();

        Debug.Log("---------game -------" + currentHealth);

        if (currentHealth <= 0 && isDead) {
            currentHealth = 0;
            Death();
            Debug.Log("---------game -------" + currentHealth);

        }

	}

    void Death() {

        isDead = true;
        Debug.Log("---------game -------" + currentHealth + "--isDead---" + isDead);

        //anim.SetTrigger("Die");
        playerAudio.Play();
        GameManager.gm.gameState = GameManager.GameState.GameOver;

    }

    //玩家加血函数，在GameManager脚本中调用
    public void AddHealth(int value){
		currentHealth += value;
		if (currentHealth > startHealth)	//加血后当前生命值不能超过初始生命值
			currentHealth = startHealth;
	}
}
