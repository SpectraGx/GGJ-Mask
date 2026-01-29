using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaskType { None, FireWall, Admin, Guest}

public class PlayerMaskManager : MonoBehaviour
{
    public MaskType currentMask = MaskType.None;

    [Header("Visuals")]
    [SerializeField] private MeshRenderer cubeRenderer;
    [SerializeField] private Material noneMaterial;
    [SerializeField] private Material fireWallMaterial;
    [SerializeField] private Material adminMaterial;
    [SerializeField] private Material guestMaterial;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipMask(MaskType.FireWall);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipMask(MaskType.Admin);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipMask(MaskType.Guest);
        if (Input.GetKeyDown(KeyCode.Alpha0)) EquipMask(MaskType.None);
    }

    void EquipMask(MaskType type)
    {
        currentMask = type;
        switch (type){ case MaskType.None: cubeRenderer.material = noneMaterial; break;
            case MaskType.FireWall: cubeRenderer.material = fireWallMaterial; break;
            case MaskType.Admin: cubeRenderer.material = adminMaterial; break;
            case MaskType.Guest: cubeRenderer.material = guestMaterial; break;
        }
        Debug.Log("Equipped Mask: " + type.ToString());
    }
}
