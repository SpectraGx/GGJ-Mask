using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MaskType { None, FireWall, Admin, Guest }

public class PlayerMaskManager : MonoBehaviour
{
    public MaskType currentMask = MaskType.None;

    [Header("Visuals")]
    [SerializeField] private MeshRenderer cubeRenderer;
    [SerializeField] private Material noneMaterial;
    [SerializeField] private Material fireWallMaterial;
    [SerializeField] private Material adminMaterial;
    [SerializeField] private Material guestMaterial;

    [Header("Mask UI")]
    [SerializeField] private Image maskIconUI;
    [SerializeField] private Sprite noneSprite;
    [SerializeField] private Sprite fireWallSprite;
    [SerializeField] private Sprite adminSprite;
    [SerializeField] private Sprite guestSprite;
    [SerializeField] private Color noneColor;
    [SerializeField] private Color fireWallColor;
    [SerializeField] private Color adminColor;
    [SerializeField] private Color guestColor;

    [Header("Mask Bool")]
    public bool hasFireWallMask = false;
    public bool hasAdminMask = false;
    public bool hasGuestMask = false;

    [Header("Audio")]
    [SerializeField] private AudioClip equipMaskAudioClip;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasFireWallMask) EquipMask(MaskType.FireWall);
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasAdminMask) EquipMask(MaskType.Admin);
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasGuestMask) EquipMask(MaskType.Guest);
        if (Input.GetKeyDown(KeyCode.R)) EquipMask(MaskType.None);
    }

    public void EquipMask(MaskType type)
    {
        currentMask = type;
        AudioManager.instance.PlaySFX(equipMaskAudioClip);
        switch (type)
        {
            case MaskType.None: 
                cubeRenderer.material = noneMaterial;
                maskIconUI.sprite = noneSprite;
                maskIconUI.color = noneColor; 
                break;
            case MaskType.FireWall: 
                cubeRenderer.material = fireWallMaterial; 
                maskIconUI.sprite = fireWallSprite;
                maskIconUI.color = fireWallColor;
                break;
            case MaskType.Admin: 
                cubeRenderer.material = adminMaterial; 
                maskIconUI.sprite = adminSprite;
                maskIconUI.color = adminColor;
                break;
            case MaskType.Guest: 
                cubeRenderer.material = guestMaterial; 
                maskIconUI.sprite = guestSprite;
                maskIconUI.color = guestColor;
                break;
        }
        Debug.Log("Equipped Mask: " + type.ToString());
    }
}
