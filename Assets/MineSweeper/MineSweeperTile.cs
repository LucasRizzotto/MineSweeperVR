using LucasUtilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MineSweeperTile : MonoBehaviour
{
    public LayerMask TileLayerMask;
    [Space(5)]
    public bool Mine = false;
    public int NearbyMines = 0;
    [Space(5)]
    public GameObject MinePrefab;
    public Rigidbody PlateRigidbody;
    public TextMeshPro TileLabel;
    public FloatToVector3Behavior FloatScript;
    [Space(5)]
    public MineSweeperTileEvent OnPress;

    [Serializable]
    public class MineSweeperTileEvent : UnityEvent { }

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
        CheckMine();
    }

    [Button("Step", ButtonSizes.Large)]
    public void Step()
    {
        Reveal();
    }

    void CheckMine()
    {
        if(Mine)
        {
            MinePrefab.SetActive(true);
        }
        else
        {
            MinePrefab.SetActive(false);
        }
    }

    [Button("Check Surroundings", ButtonSizes.Large)]
    public void CheckSurroundings()
    {
        if(Mine)
        {
            TileLabel.text = "";
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

        if(NearbyMines == 0)
        {
            TileLabel.text = "";
        }
    }
    
    public void Reveal()
    {
        PlateRigidbody.isKinematic = true;
        FloatScript.enabled = false;
        OnPress.Invoke();

        if (Mine)
        {
            Explode();
        }
        else
        {
            TileLabel.enabled = true;
        }
    }

    public void Explode()
    {

    }

    public void Hide()
    {
        TileLabel.enabled = false;
    }

}
