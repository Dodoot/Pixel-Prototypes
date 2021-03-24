using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PP_Player player = FindObjectOfType<PP_Player>();
        if (player)
        {
            player.ReloadAmmo();
            Destroy(gameObject);
        }
    }
}
