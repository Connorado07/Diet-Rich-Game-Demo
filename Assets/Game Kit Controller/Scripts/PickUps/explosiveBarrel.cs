﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explosiveBarrel : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public AudioClip explosionSound;
	public float explosionDamage;
	public bool ignoreShield;
	public float damageRadius;
	public float minVelocityToExplode;
	public float explosionDelay;
	public float explosionForce = 300;
	public bool breakInPieces;
	public bool canDamageToExplosiveBarrelOwner = true;

	public int damageTypeID = -1;

	public float explosionForceToBarrelPieces = 5;
	public float explosionRadiusToBarrelPieces = 30;
	public ForceMode forceModeToBarrelPieces = ForceMode.Impulse;

	[Space]
	[Header ("Explosion Damage Settings")]
	[Space]

	public bool pushCharacters = true;

	public bool killObjectsInRadius;

	public ForceMode explosionForceMode;

	public bool userLayerMask;
	public LayerMask layer;

	public bool applyExplosionForceToVehicles = true;
	public float explosionForceToVehiclesMultiplier = 0.2f;

	[Space]
	[Header ("Remote Events Settings")]
	[Space]

	public bool useRemoteEventOnObjectsFound;
	public string remoteEventName;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool canExplode = true;

	[Space]
	[Header ("Gizmo Settings")]
	[Space]

	public bool showGizmo;

	[Space]
	[Header ("Components")]
	[Space]

	public GameObject brokenBarrel;
	public GameObject explosionParticles;
	public Shader transparentShader;

	//	public bool firstShootAddForce;
	//	public float firstShootForce;
	//	public ForceMode forceMode;
	//	public bool useCustomForceDirection;
	//	public Vector3 customDirection;

	bool exploded;

	List<Material> rendererParts = new List<Material> ();
	int i, j;
	float timeToRemove = 3;
	GameObject barrelOwner;
	Rigidbody mainRigidbody;
	bool isDamaged;

	Vector3 damageDirection;
	Vector3 damagePosition;

	int rendererPartsCount;

	Material currentMaterial;

	void Start ()
	{
		getBarrelRigidbody ();
	}

	void Update ()
	{
		//if the barrel has exploded, wait a seconds and then 
		if (exploded) {
			if (timeToRemove > 0) {
				timeToRemove -= Time.deltaTime;
			} else {
				//change the alpha of the color in every renderer component in the fragments of the barrel
				rendererPartsCount = rendererParts.Count;

				for (i = 0; i < rendererPartsCount; i++) {
					currentMaterial = rendererParts [i];

					Color alpha = currentMaterial.color;
					alpha.a -= Time.deltaTime / 5;
					currentMaterial.color = alpha;

					//once the alpha is 0, remove the gameObject
					if (currentMaterial.color.a <= 0) {
						Destroy (gameObject);
					}
				}
			}
		}

//		if (isDamaged) {
//			if (useCustomForceDirection) {
//				mainRigidbody.AddForce (transform.TransformDirection (customDirection) * firstShootForce, forceMode);
//			} else {
//				mainRigidbody.AddForceAtPosition (-transform.InverseTransformDirection (damageDirection) * firstShootForce,
//					transform.position + transform.TransformDirection (damagePosition), forceMode);
//			}
//		}
	}

	//explode this barrel
	public void explodeBarrel ()
	{
		if (exploded) {
			return;
		}

		//if the barrel has not been throwing by the player, the barrel owner is the barrel itself
		if (barrelOwner == null) {
			barrelOwner = gameObject;
		}

		//disable the main mesh of the barrel and create the copy with the fragments of the barrel
		GetComponent<Collider> ().enabled = false;

		GetComponent<MeshRenderer> ().enabled = false;

		if (mainRigidbody != null) {
			mainRigidbody.isKinematic = true;
		}

		if (transparentShader == null) {
			transparentShader = Shader.Find ("Legacy Shaders/Transparent/Diffuse");
		}

		Vector3 currentPosition = transform.position;

		//check all the colliders inside the damage radius
		applyDamage.setExplosion (currentPosition, damageRadius, userLayerMask, layer, barrelOwner, canDamageToExplosiveBarrelOwner, 
			gameObject, killObjectsInRadius, true, false, explosionDamage, pushCharacters, applyExplosionForceToVehicles,
			explosionForceToVehiclesMultiplier, explosionForce, explosionForceMode, true, barrelOwner.transform, ignoreShield, 
			useRemoteEventOnObjectsFound, remoteEventName, damageTypeID);

		//create the explosion particles
		GameObject explosionParticlesClone = (GameObject)Instantiate (explosionParticles, transform.position, transform.rotation);
		explosionParticlesClone.transform.SetParent (transform);

		//if the option break in pieces is enabled, create the barrel broken
		if (breakInPieces) {
			GameObject brokenBarrelClone = (GameObject)Instantiate (brokenBarrel, transform.position, transform.rotation);
			brokenBarrelClone.transform.localScale = transform.localScale;
			brokenBarrelClone.transform.SetParent (transform);

			brokenBarrelClone.GetComponent<AudioSource> ().PlayOneShot (explosionSound);

			Component[] components = brokenBarrelClone.GetComponentsInChildren (typeof(MeshRenderer));
			foreach (MeshRenderer child in components) {
				//add force to every piece of the barrel and add a box collider
				Rigidbody currentPartRigidbody = child.gameObject.AddComponent<Rigidbody> ();

				child.gameObject.AddComponent<BoxCollider> ();

				currentPartRigidbody.AddExplosionForce (explosionForceToBarrelPieces, child.transform.position, explosionRadiusToBarrelPieces, 1, forceModeToBarrelPieces);

				//change the shader of the fragments to fade them
				int materialsLength = child.materials.Length;

				for (j = 0; j < materialsLength; j++) {
					Material temporalMaterial = child.materials [j];

					temporalMaterial.shader = transparentShader;
					rendererParts.Add (temporalMaterial);
				}
			}
		}

		//kill the health component, to call the functions when the object health is 0
		if (!applyDamage.checkIfDead (gameObject)) {
			applyDamage.killCharacter (gameObject);
		}

		//search the player in case he had grabbed the barrel when it exploded
		exploded = true;

		//if the object is being carried by the player, make him drop it
		GKC_Utils.checkDropObject (gameObject);
	}

	//if the player grabs this barrel, disable its explosion by collisions
	public void canExplodeState (bool state)
	{
		canExplode = state;
	}

	public void setExplosiveBarrelOwner (GameObject newBarrelOwner)
	{
		barrelOwner = newBarrelOwner;
	}

	//if the barrel collides at enough speed, explode it
	void OnCollisionEnter (Collision col)
	{
		if (mainRigidbody != null) {
			if (mainRigidbody.velocity.magnitude > minVelocityToExplode && canExplode && !exploded) {
				//if (!firstShootAddForce) {
				explodeBarrel ();
				//}
			}
		}
	}

	public void getBarrelRigidbody ()
	{
		mainRigidbody = GetComponent<Rigidbody> ();
	}

	public void setBarrelRigidbody (Rigidbody rigidbodyToUse)
	{
		mainRigidbody = rigidbodyToUse;
	}

	public void waitToExplode ()
	{
		if (explosionDelay > 0) {
			StartCoroutine (waitToExplodeCorutine ());
		} else {
			explodeBarrel ();
		}
	}

	//delay to explode the barrel
	IEnumerator waitToExplodeCorutine ()
	{
		yield return new WaitForSeconds (explosionDelay);

		explodeBarrel ();
	}

	//set the explosion values from other component
	public void setExplosionValues (float force, float radius)
	{
		explosionForce = force;
		damageRadius = radius;
	}

	public void damageDetected ()
	{
//		if (firstShootAddForce && !isDamaged) {
//			isDamaged = true;
//		}
	}

	//	public void setDamagePosition (Vector3 position)
	//	{
	//		damagePosition = position;
	//	}
	//
	//	public void setDamageDirection (Vector3 direcion)
	//	{
	//		damageDirection = direcion;
	//	}

	//draw the lines of the pivot camera in the editor
	void OnDrawGizmos ()
	{
		if (!showGizmo) {
			return;
		}

		if (GKC_Utils.isCurrentSelectionActiveGameObject (gameObject)) {
			DrawGizmos ();
		}
	}

	void OnDrawGizmosSelected ()
	{
		DrawGizmos ();
	}

	void DrawGizmos ()
	{
		if (showGizmo) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (transform.position, damageRadius);
		}
	}
}