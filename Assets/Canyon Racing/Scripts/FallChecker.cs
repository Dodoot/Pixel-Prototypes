using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallChecker : MonoBehaviour
{
    [SerializeField] bool isLeft = true;
    [SerializeField] Vehicle myVehicle = null;

    Collider2D lastCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("CR_Fall"))
        {
            lastCollider = collision;

            if (isLeft) { myVehicle.SetColliderLeft(collision); }
            else { myVehicle.SetColliderRight(collision); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == lastCollider)
        {
            if (isLeft) { myVehicle.SetColliderLeft(null); }
            else { myVehicle.SetColliderRight(null); }
        }
    }
}
