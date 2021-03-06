﻿using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GunBarrelEnd : MonoBehaviour
{
	public float timeBetweenBullets = 0.15f;   
	public float effectsDisplayTime = 0.2f;           
	float timer;           

	ParticleSystem gunParticles;                    
	LineRenderer gunLine;                           
	AudioSource gunAudio;                           
	Light gunLight;          
	PhotonView photonView;


	void Awake ()
	{
		photonView = transform.parent.GetComponent<PhotonView> ();
		SetShotEffects (false);
	}
	
	void Update ()
	{
		timer += Time.deltaTime;
			
		if (timer >= timeBetweenBullets * effectsDisplayTime) {
			DisableEffects ();
		}
	}

	public void DrawShot (Vector3 hitPositon)
	{
		timer = 0f;
		gunAudio.Play ();
		gunLight.enabled = true;
		gunParticles.Stop ();
		gunParticles.Play ();
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, hitPositon);
	}

	void DisableEffects ()
	{
		if (gunLine != null) {
			gunLine.enabled = false;
		}
		if (gunLight != null) {
			gunLight.enabled = false;
		}
	}

	void SetShotEffects (bool enhanced)
	{
		DisableEffects ();
		Transform effectsObject = transform.GetChild (enhanced ? 1 : 0);
		gunParticles = effectsObject.GetComponent<ParticleSystem> ();
		gunLine = effectsObject.GetComponent<LineRenderer> ();
		gunAudio = effectsObject.GetComponent<AudioSource> ();
		gunLight = effectsObject.GetComponent<Light> ();
	}

	void OnPhotonPlayerPropertiesChanged (object[] playerAndUpdatedProps)
	{
		PhotonPlayer player = playerAndUpdatedProps [0] as PhotonPlayer;
		Hashtable props = playerAndUpdatedProps [1] as Hashtable;
		if (photonView.owner.ID == player.ID && props.ContainsKey (PhotonPlayerExtensions.increasedDamageProp)) {
			SetShotEffects (player.HasIncreasedDamage ());
		}
	}
}
