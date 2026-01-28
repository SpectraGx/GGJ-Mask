using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;
    public NodeType nodeType;
    public MaskType maskType;

    private void OnDrawGizmos()
    {
        if (neighbors == null) return;
        Gizmos.color = Color.cyan;
        foreach (var node in neighbors)
        {
            if (node != null)
            {
                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }
    }

}

public enum NodeType { Normal, Firewall, Start, End}

public enum MaskType { None, Red, Blue, Green}
