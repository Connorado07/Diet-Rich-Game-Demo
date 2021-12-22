//using UnityEngine;
//using System.Collections;
//
//#if UNITY_EDITOR
//using UnityEditor;
//
//[CustomEditor (typeof(weaponSystem))]
//[CanEditMultipleObjects]
//public class weaponSystemEditor : Editor
//{
//	SerializedProperty reloading;
//	SerializedProperty carryingWeaponInThirdPerson;
//	SerializedProperty carryingWeaponInFirstPerson;
//	SerializedProperty aimingInThirdPerson;
//	SerializedProperty aimingInFirstPerson;
//	SerializedProperty playerControllerGameObject;
//	SerializedProperty playerCameraGameObject;
//	SerializedProperty IKWeaponManager;
//	SerializedProperty mainCameraTransform;
//	SerializedProperty layer;
//	SerializedProperty showSettings;
//	SerializedProperty weaponSettings;
//	SerializedProperty weaponProjectile;
//	SerializedProperty outOfAmmo;
//
//	SerializedProperty impactDecalList;
//	SerializedProperty impactDecalIndex;
//	SerializedProperty impactDecalName;
//
//	SerializedProperty mainDecalManagerName;
//	SerializedProperty mainNoiseMeshManagerName;
//
//	SerializedProperty getImpactListEveryFrame;
//	SerializedProperty canMarkTargets;
//	SerializedProperty tagListToMarkTargets;
//	SerializedProperty markTargetName;
//	SerializedProperty maxDistanceToMarkTargets;
//	SerializedProperty markTargetsLayer;
//	SerializedProperty canMarkTargetsOnFirstPerson;
//	SerializedProperty canMarkTargetsOnThirdPerson;
//	SerializedProperty aimOnFirstPersonToMarkTarget;
//	SerializedProperty useMarkTargetSound;
//	SerializedProperty markTargetSound;
//
//	SerializedProperty useRemoteEventOnObjectsFound;
//	SerializedProperty remoteEventNameList;
//
//	SerializedProperty useRemoteEventOnObjectsFoundOnExplosion;
//	SerializedProperty remoteEventNameOnExplosion;
//
//	SerializedProperty useAmmoFromMainPlayerWeaponSystem;
//
//	SerializedProperty mainPlayerWeaponSystem;
//
//	weaponSystem weapon;
//	bool showDrawAimFunctionSettings;
//	Color buttonColor;
//
//	void OnEnable ()
//	{
//		reloading = serializedObject.FindProperty ("reloading");
//		carryingWeaponInThirdPerson = serializedObject.FindProperty ("carryingWeaponInThirdPerson");
//		carryingWeaponInFirstPerson = serializedObject.FindProperty ("carryingWeaponInFirstPerson");
//		aimingInThirdPerson = serializedObject.FindProperty ("aimingInThirdPerson");
//		aimingInFirstPerson = serializedObject.FindProperty ("aimingInFirstPerson");
//		playerControllerGameObject = serializedObject.FindProperty ("playerControllerGameObject");
//		playerCameraGameObject = serializedObject.FindProperty ("playerCameraGameObject");
//		IKWeaponManager = serializedObject.FindProperty ("IKWeaponManager");
//		mainCameraTransform = serializedObject.FindProperty ("mainCameraTransform");
//		layer = serializedObject.FindProperty ("layer");
//		showSettings = serializedObject.FindProperty ("showSettings");
//		weaponSettings = serializedObject.FindProperty ("weaponSettings");
//		weaponProjectile = serializedObject.FindProperty ("weaponProjectile");
//		outOfAmmo = serializedObject.FindProperty ("outOfAmmo");
//
//		impactDecalList = serializedObject.FindProperty ("impactDecalList");
//		impactDecalIndex = serializedObject.FindProperty ("impactDecalIndex");
//		impactDecalName = serializedObject.FindProperty ("impactDecalName");
//
//		mainDecalManagerName = serializedObject.FindProperty ("mainDecalManagerName");
//		mainNoiseMeshManagerName = serializedObject.FindProperty ("mainNoiseMeshManagerName");
//
//		getImpactListEveryFrame = serializedObject.FindProperty ("getImpactListEveryFrame");
//		canMarkTargets = serializedObject.FindProperty ("canMarkTargets");
//		tagListToMarkTargets = serializedObject.FindProperty ("tagListToMarkTargets");
//		markTargetName = serializedObject.FindProperty ("markTargetName");
//		maxDistanceToMarkTargets = serializedObject.FindProperty ("maxDistanceToMarkTargets");
//		markTargetsLayer = serializedObject.FindProperty ("markTargetsLayer");
//		canMarkTargetsOnFirstPerson = serializedObject.FindProperty ("canMarkTargetsOnFirstPerson");
//		canMarkTargetsOnThirdPerson = serializedObject.FindProperty ("canMarkTargetsOnThirdPerson");
//		aimOnFirstPersonToMarkTarget = serializedObject.FindProperty ("aimOnFirstPersonToMarkTarget");
//		useMarkTargetSound = serializedObject.FindProperty ("useMarkTargetSound");
//		markTargetSound = serializedObject.FindProperty ("markTargetSound");
//
//		useRemoteEventOnObjectsFound = serializedObject.FindProperty ("useRemoteEventOnObjectsFound");
//		remoteEventNameList = serializedObject.FindProperty ("remoteEventNameList");
//
//		useRemoteEventOnObjectsFoundOnExplosion = serializedObject.FindProperty ("useRemoteEventOnObjectsFoundOnExplosion");
//		remoteEventNameOnExplosion = serializedObject.FindProperty ("remoteEventNameOnExplosion");
//
//		useAmmoFromMainPlayerWeaponSystem = serializedObject.FindProperty ("useAmmoFromMainPlayerWeaponSystem");
//
//		mainPlayerWeaponSystem = serializedObject.FindProperty ("mainPlayerWeaponSystem");
//
//		weapon = (weaponSystem)target;
//	}
//
//	public override void OnInspectorGUI ()
//	{
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical (GUILayout.Height (30));
//
//		GUILayout.BeginVertical ("Weapon State", "window");
//		EditorGUILayout.PropertyField (reloading);
//		EditorGUILayout.PropertyField (carryingWeaponInThirdPerson);
//		EditorGUILayout.PropertyField (carryingWeaponInFirstPerson);
//		EditorGUILayout.PropertyField (aimingInThirdPerson);
//		EditorGUILayout.PropertyField (aimingInFirstPerson);
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		EditorGUILayout.PropertyField (playerControllerGameObject);
//		EditorGUILayout.PropertyField (playerCameraGameObject);
//		EditorGUILayout.PropertyField (IKWeaponManager);
//		EditorGUILayout.PropertyField (mainCameraTransform);
//		EditorGUILayout.PropertyField (layer);
//
//		EditorGUILayout.PropertyField (mainDecalManagerName);
//		EditorGUILayout.PropertyField (mainNoiseMeshManagerName);
//
//		EditorGUILayout.Space ();
//
//		EditorGUILayout.PropertyField (useAmmoFromMainPlayerWeaponSystem);
//		if (useAmmoFromMainPlayerWeaponSystem.boolValue) {
//			EditorGUILayout.PropertyField (mainPlayerWeaponSystem);
//		}
//
//		EditorGUILayout.Space ();
//
//		buttonColor = GUI.backgroundColor;
//		EditorGUILayout.BeginVertical ();
//		string inputListOpenedText = "";
//		if (showSettings.boolValue) {
//			GUI.backgroundColor = Color.gray;
//			inputListOpenedText = "Hide Weapon Settings";
//		} else {
//			GUI.backgroundColor = buttonColor;
//			inputListOpenedText = "Show Weapon Settings";
//		}
//		if (GUILayout.Button (inputListOpenedText)) {
//			showSettings.boolValue = !showSettings.boolValue;
//		}
//		GUI.backgroundColor = buttonColor;
//		EditorGUILayout.EndVertical ();
//
//		if (showSettings.boolValue) {
//
//			EditorGUILayout.Space ();
//
//			GUI.color = Color.cyan;
//			EditorGUILayout.HelpBox ("Configure the shot settings of this weapon", MessageType.None);
//			GUI.color = Color.white;
//
//			EditorGUILayout.Space ();
//
//			showWeaponSettings (weaponSettings);
//		}
//
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		if (GUI.changed) {
//			serializedObject.ApplyModifiedProperties ();
//		}
//	}
//
//	void showWeaponSettings (SerializedProperty list)
//	{
//		GUILayout.BeginVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Weapon Info", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("HUD Elements Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("weaponIconHUD"));	
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("showWeaponNameInHUD"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("showWeaponIconInHUD"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("showWeaponAmmoSliderInHUD"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("showWeaponAmmoTextInHUD"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Fire Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("automatic"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("fireRate"));
//		if (list.FindPropertyRelative ("automatic").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useBurst"));
//			if (list.FindPropertyRelative ("useBurst").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("burstAmount"));
//			}
//		}
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("reloadTime"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Projectile Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("shootAProjectile"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("launchProjectile"));
//		EditorGUILayout.PropertyField (weaponProjectile);
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("projectilesPerShoot"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("projectileDamage"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("killInOneShot"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("projectileWithAbility"));
//
//		if (list.FindPropertyRelative ("launchProjectile").boolValue) {
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Launch Projectile Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("activateLaunchParableThirdPerson"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("activateLaunchParableFirstPerson"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useParableSpeed"));
//			if (!list.FindPropertyRelative ("useParableSpeed").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("projectileSpeed"));
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("parableDirectionTransform"));
//			} else {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("useMaxDistanceWhenNoSurfaceFound"));
//				if (list.FindPropertyRelative ("useMaxDistanceWhenNoSurfaceFound").boolValue) {
//					EditorGUILayout.PropertyField (list.FindPropertyRelative ("maxDistanceWhenNoSurfaceFound"));
//				}
//			}
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//		} else {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("fireWeaponForward"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useRayCastShoot"));
//			if (!list.FindPropertyRelative ("useRayCastShoot").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("projectileSpeed"));
//
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("useRaycastCheckingOnRigidbody"));
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("customRaycastCheckingRate"));
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("customRaycastCheckingDistance"));
//			}
//		}
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Search Target Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("isHommingProjectile"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("isSeeker"));
//		if (list.FindPropertyRelative ("isHommingProjectile").boolValue || list.FindPropertyRelative ("isSeeker").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("waitTimeToSearchTarget"));
//			showTagToLocateList (list.FindPropertyRelative ("tagToLocate"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("targetOnScreenForSeeker"));
//		}
//		if (list.FindPropertyRelative ("isHommingProjectile").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("locatedEnemyIconName"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Ammo Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("clipSize"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("infiniteAmmo"));
//		if (!list.FindPropertyRelative ("infiniteAmmo").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("remainAmmo"));
//		}
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("removePreviousAmmoOnClip"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("dropClipWhenReload"));
//		if (list.FindPropertyRelative ("dropClipWhenReload").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("positionToDropClip"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("clipModel"));
//		}
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("startWithEmptyClip"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("autoReloadWhenClipEmpty"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useAmmoLimit"));
//		if (list.FindPropertyRelative ("useAmmoLimit").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("ammoLimit"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Force Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("impactForceApplied"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("forceMode"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("applyImpactForceToVehicles"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Explosion Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("isExplosive"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("isImplosive"));
//		if (list.FindPropertyRelative ("isExplosive").boolValue || list.FindPropertyRelative ("isImplosive").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("explosionForce"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("explosionRadius"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useExplosionDelay"));
//			if (list.FindPropertyRelative ("useExplosionDelay").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("explosionDelay"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("explosionDamage"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("pushCharacters"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("canDamageProjectileOwner"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Disable Projectile Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useDisableTimer"));
//		if (list.FindPropertyRelative ("useDisableTimer").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("noImpactDisableTimer"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("impactDisableTimer"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Sound Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("shootSoundEffect"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("impactSoundEffect"));
//		EditorGUILayout.PropertyField (outOfAmmo);
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("reloadSoundEffect"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("cockSound"));
//
//		EditorGUILayout.Space ();
//
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Particle Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("shootParticles"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("projectileParticles"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("impactParticles"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Ability Weapon Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("weaponWithAbility"));
//		if (list.FindPropertyRelative ("weaponWithAbility").boolValue) {
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Button Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useDownButton"));
//			if (list.FindPropertyRelative ("useDownButton").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("downButtonAction"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useHoldButton"));
//			if (list.FindPropertyRelative ("useHoldButton").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("holdButtonAction"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useUpButton"));
//			if (list.FindPropertyRelative ("useUpButton").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("upButtonAction"));
//			}
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//		}
//		GUILayout.EndVertical ();
//	
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Draw/Aim Settings", "window");
//		buttonColor = GUI.backgroundColor;
//		showDrawAimFunctionSettings = list.FindPropertyRelative ("showDrawAimFunctionSettings").boolValue;
//
//		EditorGUILayout.BeginVertical ();
//		string inputListOpenedText = "";
//		if (showDrawAimFunctionSettings) {
//			GUI.backgroundColor = Color.gray;
//			inputListOpenedText = "Hide Draw/Aim Function Settings";
//		} else {
//			GUI.backgroundColor = buttonColor;
//			inputListOpenedText = "Show Draw/Aim Function Settings";
//		}
//		if (GUILayout.Button (inputListOpenedText)) {
//			showDrawAimFunctionSettings = !showDrawAimFunctionSettings;
//		}
//		GUI.backgroundColor = buttonColor;
//		EditorGUILayout.EndVertical ();
//
//		list.FindPropertyRelative ("showDrawAimFunctionSettings").boolValue = showDrawAimFunctionSettings;
//
//		if (showDrawAimFunctionSettings) {
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Draw Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStartDrawAction"));
//			if (list.FindPropertyRelative ("useStartDrawAction").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("startDrawAction"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStopDrawAction"));
//			if (list.FindPropertyRelative ("useStopDrawAction").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("stopDrawAction"));
//			}
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Third Person Draw Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStartDrawActionThirdPerson"));
//			if (list.FindPropertyRelative ("useStartDrawActionThirdPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("startDrawActionThirdPerson"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStopDrawActionThirdPerson"));
//			if (list.FindPropertyRelative ("useStopDrawActionThirdPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("stopDrawActionThirdPerson"));
//			}
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("First Person Draw Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStartDrawActionFirstPerson"));
//			if (list.FindPropertyRelative ("useStartDrawActionFirstPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("startDrawActionFirstPerson"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStopDrawActionFirstPerson"));
//			if (list.FindPropertyRelative ("useStopDrawActionFirstPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("stopDrawActionFirstPerson"));
//			}
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Third Person Aim Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStartAimActionThirdPerson"));
//			if (list.FindPropertyRelative ("useStartAimActionThirdPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("startAimActionThirdPerson"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStopAimActionThirdPerson"));
//			if (list.FindPropertyRelative ("useStopAimActionThirdPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("stopAimActionThirdPerson"));
//			}
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("First Person Aim Settings", "window");
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStartAimActionFirstPerson"));
//			if (list.FindPropertyRelative ("useStartAimActionFirstPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("startAimActionFirstPerson"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStopAimActionFirstPerson"));
//			if (list.FindPropertyRelative ("useStopAimActionFirstPerson").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("stopAimActionFirstPerson"));
//			}
//			GUILayout.EndVertical ();
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Spread Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useProjectileSpread"));
//		if (list.FindPropertyRelative ("useProjectileSpread").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("spreadAmount"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("sameSpreadInThirdPerson"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("thirdPersonSpreadAmount"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useSpreadAming"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useLowerSpreadAiming"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("lowerSpreadAmount"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Projectile Position Settings", "window");
//		showSimpleList (list.FindPropertyRelative ("projectilePosition"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Shell Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("shell"));
//		if (list.FindPropertyRelative ("shell").objectReferenceValue) {
//			showSimpleList (list.FindPropertyRelative ("shellPosition"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("shellEjectionForce"));
//			showSimpleList (list.FindPropertyRelative ("shellDropSoundList"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Weapon Components", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("weaponMesh"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("animation"));
//		if (list.FindPropertyRelative ("animation").stringValue != "") {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("animationSpeed"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("playAnimationBackward"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Scorch Settings", "window");
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Scorch from Decal Manager", "window");
//		if (impactDecalList.arraySize > 0) {
//			impactDecalIndex.intValue = EditorGUILayout.Popup ("Default Decal Type",
//				impactDecalIndex.intValue, weapon.impactDecalList);
//			impactDecalName.stringValue = weapon.impactDecalList [impactDecalIndex.intValue];
//		}
//
//		EditorGUILayout.PropertyField (getImpactListEveryFrame);
//		if (!getImpactListEveryFrame.boolValue) {
//
//			EditorGUILayout.Space ();
//
//			if (GUILayout.Button ("Update Decal Impact List")) {
//				weapon.getImpactListInfo ();					
//			}
//
//			EditorGUILayout.Space ();
//
//		}
//
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Regular Scorch", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("scorch"));
//		if (list.FindPropertyRelative ("scorch").objectReferenceValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("scorchRayCastDistance"));
//		}
//		GUILayout.EndVertical ();
//
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Auto Shoot Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("autoShootOnTag"));
//		if (list.FindPropertyRelative ("autoShootOnTag").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("layerToAutoShoot"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("maxDistanceToRaycast"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("shootAtLayerToo"));
//			GUILayout.BeginVertical ("Auto Shoot Tag List", "window");
//			showSimpleList (list.FindPropertyRelative ("autoShootTagList"));
//			GUILayout.EndVertical ();
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//	
//		GUILayout.BeginVertical ("Player Force Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("applyForceAtShoot"));
//		if (list.FindPropertyRelative ("applyForceAtShoot").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("forceDirection"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("forceAmount"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Projectile Adherence Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useGravityOnLaunch"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useGraivtyOnImpact"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("adhereToSurface"));
//		if (list.FindPropertyRelative ("adhereToSurface").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("adhereToLimbs"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Projectile Pierce Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("breakThroughObjects"));
//		if (list.FindPropertyRelative ("breakThroughObjects").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("infiniteNumberOfImpacts"));
//			if (!list.FindPropertyRelative ("infiniteNumberOfImpacts").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("numberOfImpacts"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("canDamageSameObjectMultipleTimes"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Upper Body Shake Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("shakeUpperBodyWhileShooting"));
//		if (list.FindPropertyRelative ("shakeUpperBodyWhileShooting").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("shakeAmount"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("shakeSpeed"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Muzzle Flash Light Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useMuzzleFlash"));
//		if (list.FindPropertyRelative ("useMuzzleFlash").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("muzzleFlahsLight"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("muzzleFlahsDuration"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Remote Event Settings", "window");
//		EditorGUILayout.PropertyField (useRemoteEventOnObjectsFound);
//		if (useRemoteEventOnObjectsFound.boolValue) {
//			EditorGUILayout.Space ();
//
//			showSimpleList (remoteEventNameList);
//		}
//
//		EditorGUILayout.Space ();
//
//		EditorGUILayout.PropertyField (useRemoteEventOnObjectsFoundOnExplosion);
//		if (useRemoteEventOnObjectsFoundOnExplosion.boolValue) {
//			EditorGUILayout.Space ();
//
//			EditorGUILayout.PropertyField (remoteEventNameOnExplosion);
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Shield And Reaction Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("ignoreShield"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("canActivateReactionSystemTemporally"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageReactionID"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageTypeID"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Damage Target Over Time Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageTargetOverTime"));
//		if (list.FindPropertyRelative ("damageTargetOverTime").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageOverTimeDelay"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageOverTimeDuration"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageOverTimeAmount"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageOverTimeRate"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("damageOverTimeToDeath"));
//		}
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("removeDamageOverTimeState"));
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Sedate Characters Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("sedateCharacters"));
//		if (list.FindPropertyRelative ("sedateCharacters").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("sedateDelay"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useWeakSpotToReduceDelay"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("sedateUntilReceiveDamage"));
//			if (!list.FindPropertyRelative ("sedateUntilReceiveDamage").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("sedateDuration"));
//			}
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Push Characters Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("pushCharacter"));
//		if (list.FindPropertyRelative ("pushCharacter").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("pushCharacterForce"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("pushCharacterRagdollForce"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Shoot Noise Settings", "window");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useNoise"));
//		if (list.FindPropertyRelative ("useNoise").boolValue) {
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("noiseRadius"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("noiseExpandSpeed"));
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useNoiseDetection"));
//			if (list.FindPropertyRelative ("useNoiseDetection").boolValue) {
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("noiseDetectionLayer"));
//				EditorGUILayout.PropertyField (list.FindPropertyRelative ("showNoiseDetectionGizmo"));
//			}
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("noiseDecibels"));
//
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useNoiseMesh"));
//
//			EditorGUILayout.PropertyField (list.FindPropertyRelative ("forceNoiseDetection"));
//		}
//		GUILayout.EndVertical ();
//
//		EditorGUILayout.Space ();
//
//		GUILayout.BeginVertical ("Mark Targets Settings", "window");
//		EditorGUILayout.PropertyField (canMarkTargets);
//		if (canMarkTargets.boolValue) {
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Tag List To Mark Targets", "window");
//			showSimpleList (tagListToMarkTargets);
//			GUILayout.EndVertical ();
//
//			EditorGUILayout.Space ();
//
//			EditorGUILayout.PropertyField (markTargetName);
//			EditorGUILayout.PropertyField (maxDistanceToMarkTargets);
//			EditorGUILayout.PropertyField (markTargetsLayer);
//
//			EditorGUILayout.PropertyField (canMarkTargetsOnFirstPerson);
//			EditorGUILayout.PropertyField (canMarkTargetsOnThirdPerson);
//			EditorGUILayout.PropertyField (aimOnFirstPersonToMarkTarget);
//			EditorGUILayout.PropertyField (useMarkTargetSound);
//			if (useMarkTargetSound.boolValue) {
//				EditorGUILayout.PropertyField (markTargetSound);
//			}
//		}
//		GUILayout.EndVertical ();
//
//		GUILayout.EndVertical ();
//	}
//
//	void showSimpleList (SerializedProperty list)
//	{
//		EditorGUILayout.PropertyField (list, false);
//		if (list.isExpanded) {
//			GUILayout.BeginVertical ("box");
//
//			EditorGUILayout.Space ();
//
//			GUILayout.Label ("Amount: \t" + list.arraySize);
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginHorizontal ();
//			if (GUILayout.Button ("Add")) {
//				list.arraySize++;
//			}
//			if (GUILayout.Button ("Clear")) {
//				list.arraySize = 0;
//			}
//			GUILayout.EndHorizontal ();
//
//			EditorGUILayout.Space ();
//
//			for (int i = 0; i < list.arraySize; i++) {
//				GUILayout.BeginHorizontal ();
//				if (i < list.arraySize && i >= 0) {
//					EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent ("", null, ""), false);
//				}
//				GUILayout.BeginHorizontal ();
//				if (GUILayout.Button ("x")) {
//					list.DeleteArrayElementAtIndex (i);
//					list.DeleteArrayElementAtIndex (i);
//				}
//				if (GUILayout.Button ("v")) {
//					if (i >= 0) {
//						list.MoveArrayElement (i, i + 1);
//					}
//				}
//				if (GUILayout.Button ("^")) {
//					if (i < list.arraySize) {
//						list.MoveArrayElement (i, i - 1);
//					}
//				}
//				GUILayout.EndHorizontal ();
//				GUILayout.EndHorizontal ();
//
//				EditorGUILayout.Space ();
//
//			}
//			GUILayout.EndVertical ();
//		}       
//	}
//
//	void showTagToLocateList (SerializedProperty list)
//	{
//		EditorGUILayout.PropertyField (list, false);
//		if (list.isExpanded) {
//			GUILayout.BeginVertical ("box");
//
//			EditorGUILayout.Space ();
//
//			GUILayout.Label ("Number of Tags: \t" + list.arraySize);
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginHorizontal ();
//			if (GUILayout.Button ("Add")) {
//				list.arraySize++;
//			}
//			if (GUILayout.Button ("Clear")) {
//				list.arraySize = 0;
//			}
//			GUILayout.EndHorizontal ();
//
//			EditorGUILayout.Space ();
//
//			for (int i = 0; i < list.arraySize; i++) {
//				GUILayout.BeginHorizontal ();
//				if (i < list.arraySize && i >= 0) {
//					EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent ("", null, ""), false);
//				}
//				GUILayout.BeginHorizontal ();
//				if (GUILayout.Button ("x")) {
//					list.DeleteArrayElementAtIndex (i);
//				}
//				GUILayout.EndHorizontal ();
//				GUILayout.EndHorizontal ();
//
//				EditorGUILayout.Space ();
//			}
//			GUILayout.EndVertical ();
//		}       
//	}
//}
//#endif