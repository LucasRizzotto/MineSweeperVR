using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperTileTrigger : MonoBehaviour
{
    public MineSweeperTile Tile;
    public LayerMask PlateLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if(Helpers.IsInLayerMask(other.gameObject.layer, PlateLayerMask))
        {
            Tile.Step();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

}
