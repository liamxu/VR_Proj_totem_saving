using UnityEngine;


namespace CompleteProject
{
    public class WeaponAttack: MonoBehaviour
    {
        public int damagePerShot = 10;                  // The damage inflicted by each bullet.
        public string startStr = "";                    //

        void Awake ()
        {
        
		}


        void Update ()
        {

        }


        public void DisableEffects ()
        {
            // Disable the line renderer and the light.

        }

		void OnCollisionEnter( Collision collision ) {
			GameObject collideObj = collision.gameObject;
			ContactPoint contact = collision.contacts[0];

            if (collideObj.tag == "Enemy"){
                
                // Try and find an EnemyHealth script on the gameobject hit.
				EnemyHealth enemyHealth = collideObj.GetComponent <EnemyHealth> ();

                // If the EnemyHealth component exist...
                if(enemyHealth != null)
                {
					Vector3 contactPos = contact.point;
                    if (collideObj.name.StartsWith(startStr)) {
                        // ... the enemy should take damage.
                        enemyHealth.TakeDamage(damagePerShot * 2, contactPos);
                    }else {
                        enemyHealth.TakeDamage(damagePerShot, contactPos);
                    }
                }
            }
            if ((collideObj.tag != "Building")          && 
                (collideObj.tag != "HandL")             && 
                (collideObj.tag != "HandR")             && 
                (collideObj.tag != "handComponents")    && 
                (collideObj.tag != "HandLPalm")         && 
                (collideObj.tag != "HandRPalm")         &&
                (collideObj.tag != "HandRIndex")        &&
                (collideObj.name != "BrushHand_L")      && (collideObj.name != "BrushHand_R")      &&
                (collideObj.tag != "weapon")            && (collideObj.name != "createObj") 
                ) {

                //Debug.Log("-----------collision ------" + collideObj.name + "=============" + collideObj.tag + "-------");

                //collideObj.GetComponentInParent();
                
                // After 3 seconds destory the enemy.
                Destroy(gameObject, 3.0f);
            }
	
        }


    }
}
