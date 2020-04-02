using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineSweeperTile : MonoBehaviour
{
    public LayerMask TileLayerMask;
    [Space(5)]
    public bool Mine = false;
    public int NearbyMines = 0;
    public bool Flagged = false;
    [Space(5)]
    public TextMeshPro TileLabel;

    private void OnDrawGizmos()
    {
        if(Mine)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }

    private void OnEnable()
    {
        CheckSurroundings();
    }

    [Button("Step", ButtonSizes.Large)]
    public void Step()
    {
        if(Mine)
        {
            Explode();
        }
    }

    [Button("Check Surroundings", ButtonSizes.Large)]
    public void CheckSurroundings()
    {
        if(Mine)
        {
            return;
        }

        NearbyMines = 0;
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * 1f, Quaternion.identity, TileLayerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            var tempCollider = hitColliders[i].gameObject.GetComponent<MineSweeperTile>();
            if (tempCollider.Mine)
            {
                NearbyMines++;
            }
        }
        TileLabel.text = NearbyMines.ToString();
    }
    
    public void Explode()
    {

    }

}
