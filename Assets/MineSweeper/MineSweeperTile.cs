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

    #region Unity APIs

    private void OnDrawGizmos()
    {
        if (Mine)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }

    private void OnEnable()
    {
        ResetTile();
    }

    #endregion

    #region Main Tile Methods

    [Button("Trigger Tile", ButtonSizes.Large)]
    public void TriggerTile()
    {
        PlateRigidbody.isKinematic = true;
        FloatScript.enabled = false;
        OnPress.Invoke();

        if (Mine)
        {
            ExplodeMine();
        }
        else
        {
            TileLabel.enabled = true;
        }
    }

    [Button("Reset Tile", ButtonSizes.Large)]
    public void ResetTile()
    {
        PlateRigidbody.transform.localPosition = Vector3.zero;
        PlateRigidbody.isKinematic = false;
        FloatScript.enabled = true;

        if (Mine)
        {
            MinePrefab.SetActive(true);
        }
        else
        {
            MinePrefab.SetActive(false);
        }

        UpdateSurroundingNumbers();
    }

    #endregion

    #region Helpers

    public void UpdateSurroundingNumbers()
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

    public void ExplodeMine()
    {

    }

    public void HidePlate(bool status)
    {
        if(status)
        {
            PlateRigidbody.gameObject.SetActive(false);
        }
        else
        {
            PlateRigidbody.gameObject.SetActive(true);
        }
    }

    #endregion

}
