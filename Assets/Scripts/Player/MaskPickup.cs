using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaskPickup : MonoBehaviour
{
    public MaskType maskToUnlock;
    public GameObject pickupVFX;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupAudioClip;

    void Start()
    {
        transform.DOLocalMoveY(transform.position.y + 0.5f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        transform.DORotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

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
                
                //manager.EquipMask(maskToUnlock);
                
                if (pickupVFX) Instantiate(pickupVFX, transform.position, Quaternion.identity);
                
                //Destroy(gameObject);
                Collect();
            }
        }
    }

    public void Collect()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        AudioManager.instance.PlaySFX(pickupAudioClip);
        Destroy(gameObject,2f);
    }
}
