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
    public MineSweeperGridManager GridManager;
    public LayerMask TileLayerMask;
    public LayerMask MineLayerMask;
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

    private float DetectionRadius = 1.05f;

    [Serializable]
    public class MineSweeperTileEvent : UnityEvent { }

    #region Unity APIs

    private void OnDrawGizmos()
    {
        if (Mine)
        {
            Gizmos.color = new Color(0.2f, 0.1f, 0.1f, 0.1f);
            Gizmos.DrawCube(transform.position, transform.localScale * DetectionRadius);
        }
    }

    private void OnMouse()
    {
        TriggerTile();
        PlateRigidbody.transform.position = new Vector3(0f, 0.2f, 0f);
    }

    private void OnMouseDrag()
    {
        TriggerTile();
    }

    private void OnEnable()
    {
        ResetTile();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        ResetTile();
    }

    #endregion

    #region Main Tile Methods

    private Coroutine PeekRoutine;

    [Button("Peek At Result", ButtonSizes.Large)]
    public void PeekAtResult()
    {
        StartCoroutine(_PeekAtResult());
    }

    public IEnumerator _PeekAtResult()
    {
        PlateRigidbody.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        PlateRigidbody.gameObject.SetActive(true);
    }

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

    [Button("List Surrounding Colliders", ButtonSizes.Large)]
    public void ListSurroundingColliders()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * DetectionRadius, Quaternion.identity, MineLayerMask);
        int count = 0;
        if(hitColliders.Length > 0)
        {
            Debug.Log("--- LISTING SURROUNDING COLLIDERS of " + gameObject.name + " ---");
        }

        foreach (Collider col in hitColliders)
        {
            count++;
            Debug.Log("Object #1: " + col.gameObject.name);
        }

        if (hitColliders.Length > 0)
        {
            Debug.Log("--- DONE with " + gameObject.name + " ---");
        }
    }

    public void UpdateSurroundingNumbers()
    {
        if(Mine)
        {
            TileLabel.text = "";
            return;
        }

        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * DetectionRadius, Quaternion.identity, MineLayerMask);

        int count = 0;
        foreach(Collider col in hitColliders)
        {
            count++;
        }

        TileLabel.text = count.ToString();

        if (hitColliders.Length == 0)
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
