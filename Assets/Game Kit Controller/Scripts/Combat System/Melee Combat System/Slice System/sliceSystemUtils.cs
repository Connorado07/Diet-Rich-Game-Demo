using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using EzySlice;

//using NobleMuffins.LimbHacker.Guts;
//using NobleMuffins.LimbHacker;

public class sliceSystemUtils : MonoBehaviour
{
	
	public static void sliceObject (Vector3 slicePosition, GameObject objectToSlice, Vector3 cutDirection, Material crossSectionMaterial, ref bool objectSliced, ref GameObject object1, ref GameObject object2)
	{
		//SlicedHull hull = objectToSlice.SliceObject (slicePosition, cutDirection, crossSectionMaterial);

		//if (hull != null) {

		//	objectSliced = true;

		//	object1 = hull.CreateLowerHull (objectToSlice, crossSectionMaterial);
		//	object2 = hull.CreateUpperHull (objectToSlice, crossSectionMaterial);
		//}
	}

	public static surfaceToSlice getSurfaceToSlice (GameObject currentSurface)
	{
		//ChildOfHackable currentChildOfHackable = currentSurface.GetComponent<ChildOfHackable> ();

		//if (currentChildOfHackable != null) {
		//	return currentChildOfHackable.parentHackable.mainSurfaceToSlice;
		//}

		return null;
	}

	public static void initializeValuesOnHackableComponent (GameObject objectToUse, simpleSliceSystem currentSimpleSliceSystem)
	{
		//Hackable currentHackable = objectToUse.GetComponent<Hackable> ();

		//if (currentHackable == null) {
		//	currentHackable = objectToUse.AddComponent<Hackable> ();
		//}

		//currentHackable.mainSurfaceToSlice = currentSimpleSliceSystem.mainSurfaceToSlice;

		//currentHackable.alternatePrefab = currentSimpleSliceSystem.getMainAlternatePrefab ();

		//currentHackable.objectToSlice = currentSimpleSliceSystem.objectToSlice;

		//currentHackable.infillMaterial = currentSimpleSliceSystem.infillMaterial;

		//currentHackable.severables = currentSimpleSliceSystem.getSeverables ();

		//currentHackable.initializeValues ();
	}

	public static void sliceCharacter (GameObject objectToSlice, Vector3 point, Vector3 newNormalInWorldSpaceValue)
	{
		//if (objectToSlice == null) {
		//	return;
		//}

		//Hackable currentHackable = objectToSlice.GetComponent<Hackable> ();

		//currentHackable.activateSlice (objectToSlice, point, newNormalInWorldSpaceValue);
	}

	public static bool shatterObject (GameObject obj, int shatterAmount, Material crossSectionMaterial, float forceToApplyToCutPart, ForceMode forceMode, 
	                                  Vector3 cutPosition, Vector3 objectPosition, Quaternion objectRotation, Transform objectParent)
	{
		//if (shatterAmount > 0) {
		//	GameObject[] slices = obj.SliceInstantiate (GetRandomPlane (objectPosition, obj.transform.localScale), new TextureRegion (0.0f, 0.0f, 1.0f, 1.0f), crossSectionMaterial);

		//	if (slices != null) {

		//		shatterAmount--;

		//		for (int i = 0; i < slices.Length; i++) {
		//			if (shatterObject (slices [i], shatterAmount, crossSectionMaterial, forceToApplyToCutPart, forceMode, cutPosition, objectPosition, objectRotation, objectParent)) {
		//				GameObject.Destroy (slices [i]);
		//			} else {
		//				GameObject object1 = slices [i];

		//				object1.transform.position = objectPosition;
		//				object1.transform.rotation = objectRotation;

		//				if (objectParent != null) {
		//					object1.transform.SetParent (objectParent);
		//				}
		//			}
		//		}

		//		if (shatterAmount <= 0) {
		//			GameObject.Destroy (obj);

		//			for (int i = 0; i < slices.Length; i++) {
		//				GameObject object1 = slices [i];

		//				object1.transform.position = objectPosition;
		//				object1.transform.rotation = objectRotation;

		//				if (objectParent != null) {
		//					object1.transform.SetParent (objectParent);
		//				}

		//				MeshCollider object1Collider = object1.AddComponent<MeshCollider> ();

		//				object1Collider.convex = true;

		//				surfaceToSlice newSurfaceToSlice = object1.AddComponent<surfaceToSlice> ();

		//				newSurfaceToSlice.crossSectionMaterial = crossSectionMaterial;

		//				Rigidbody object1Rigidbody = object1.AddComponent<Rigidbody> ();

		//				object1Rigidbody.AddExplosionForce (forceToApplyToCutPart / 2, cutPosition, 10, 1, forceMode);
		//			}
		//		} else {
		//			return true;
		//		}
		//	}

		//	return shatterObject (obj, shatterAmount, crossSectionMaterial, forceToApplyToCutPart, forceMode, cutPosition, objectPosition, objectRotation, objectParent);

		//}


		return false;
	}

	//public static EzySlice.Plane GetRandomPlane (Vector3 positionOffset, Vector3 scaleOffset)
	//{
	//	Vector3 randomPosition = Random.insideUnitSphere;

	//	randomPosition += positionOffset;

	//	Vector3 randomDirection = Random.insideUnitSphere.normalized;

	//	return new EzySlice.Plane (randomPosition, randomDirection);
	//}
}