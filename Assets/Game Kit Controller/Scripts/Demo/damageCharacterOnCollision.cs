using UnityEngine;
using System.Collections;

public class damageCharacterOnCollision : MonoBehaviour
{
	public bool addForceToRigidbodies = true;
	public float forceAmountToRigidbodies = 1000;
	public bool killCharacterOnCollision = true;
	public bool pushCharacterOnCollision;
	public bool applyDamageWhenPushCharacter;
	public float extraForceOnCollision;

	public bool ignoreShield;

	public int damageTypeID = -1;

	public bool canActivateReactionSystemTemporally;
	public int damageReactionID = -1;

	public bool damageEnabled = true;

	ContactPoint currentContact;
	GameObject collisionObject;

	void OnCollisionEnter (Collision collision)
	{
		if (!damageEnabled) {
			return;
		}

		currentContact = collision.contacts [0];

		collisionObject = collision.gameObject;

		if (addForceToRigidbodies) {
			if (applyDamage.canApplyForce (collisionObject)) {
				collision.rigidbody.AddExplosionForce (forceAmountToRigidbodies, collision.transform.position, 100);
			}
		}

		if (killCharacterOnCollision) {
			//applyDamage.killCharacter (collisionObject);
			float damage = applyDamage.getCurrentHealthAmount (collisionObject);

			applyDamage.checkHealth (gameObject, collisionObject, damage, transform.forward, currentContact.point, gameObject, 
				false, true, ignoreShield, false, canActivateReactionSystemTemporally, damageReactionID, damageTypeID);
		} else {
			if (pushCharacterOnCollision) {
				Vector3 pushDirection = (currentContact.point - transform.position).normalized;

				if (extraForceOnCollision > 0) {
					pushDirection *= extraForceOnCollision;
				}

				applyDamage.pushCharacter (collisionObject, pushDirection);

				if (applyDamageWhenPushCharacter) {
					float damage = collision.relativeVelocity.magnitude;

					applyDamage.checkHealth (gameObject, collisionObject, damage, transform.forward, currentContact.point, gameObject, 
						false, true, ignoreShield, false, canActivateReactionSystemTemporally, damageReactionID, damageTypeID);
				}
			}
		}
	}

	public void setDamageEnabledState (bool state)
	{
		damageEnabled = state;
	}

	public void disableDamage ()
	{
		setDamageEnabledState (false);
	}

	public void enableDamage ()
	{
		setDamageEnabledState (true);
	}
}
