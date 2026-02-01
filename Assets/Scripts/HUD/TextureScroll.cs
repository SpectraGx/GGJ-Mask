using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public float scrollSpeed = 2f;
    private LineRenderer lineRenderer;
    private Material lineMaterial;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineMaterial = lineRenderer.material;
        }
    }

    void Update()
    {
        if (lineMaterial != null)
        {
            float offset = Time.time * scrollSpeed;
            lineMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }

}
