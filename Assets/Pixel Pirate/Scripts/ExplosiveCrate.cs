using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExplosiveCrate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Explode();
    }

    private void Explode()
    {
        MusicAndSoundManager.PlaySound("Explosion", transform.position);
        FindObjectOfType<GameMaster>().CrateDestroyed();
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        Destroy(gameObject);
    }
}
