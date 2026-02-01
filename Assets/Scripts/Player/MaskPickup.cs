using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    public MaskType maskToUnlock;
    public GameObject pickupVFX; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMaskManager manager = other.GetComponent<PlayerMaskManager>();
            
            if (manager != null)
            {
                switch (maskToUnlock)
                {
                    case MaskType.FireWall: manager.hasFireWallMask = true; break;
                    case MaskType.Admin: manager.hasAdminMask = true; break;
                    case MaskType.Guest: manager.hasGuestMask = true; break;
                }
                
                manager.EquipMask(maskToUnlock);
                
                if (pickupVFX) Instantiate(pickupVFX, transform.position, Quaternion.identity);
                
                Destroy(gameObject);
            }
        }
    }
}
