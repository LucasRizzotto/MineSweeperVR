using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MineSweeperGridManager : MonoBehaviour
{
    public MineSweeperGrid Grid;
    [Space(5)]
    public bool GenerateOnStart = false;
    public List<MineSweeperTile> Tiles = new List<MineSweeperTile>();

    private float GridXOffset = 0f;
    private float GridYOffset = 0f;
    private int CurrentMineNumber = 0;
  
    private GameObject TempGameObject;

    private void OnEnable()
    {
        if(GenerateOnStart)
        {
            GenerateGrid();
        }
    }

    [Button("Generate Grid", ButtonSizes.Large)]
    public void GenerateGrid()
    {
        ClearLists();
        for (int i = 0; i < Grid.Xsize; i++)
        {
            for (int j = 0; j < Grid.Ysize; j++)
            {
                Vector3 pos = new Vector3(GridXOffset, 0f, GridYOffset);

                if (Application.isPlaying)
                {
                    TempGameObject = Instantiate(Grid.TilePrefab);
                }
                else
                {
#if UNITY_EDITOR
                    TempGameObject = PrefabUtility.InstantiatePrefab(Grid.TilePrefab) as GameObject;
#endif
                }

                TempGameObject.name = "Tile #" + (Tiles.Count + 1);
                TempGameObject.transform.position = pos;
                TempGameObject.transform.parent = transform;

                GridXOffset += TempGameObject.transform.localScale.x;
                Tiles.Add(TempGameObject.GetComponent<MineSweeperTile>());
            }
            GridXOffset = 0;
            GridYOffset += TempGameObject.transform.localScale.z;
        }
    }

    [Button("Reset Grid", ButtonSizes.Large)]
    public void ResetGrid()
    {
        CurrentMineNumber = 0;

        for (int i = 0; i < Tiles.Count; i++)
        {
            if(CurrentMineNumber >= Grid.MaxMines)
            {
                return;
            }
            Tiles[i].Mine = ShouldGetMine();
            Tiles[i].ResetTile();
        }

        // If we have no mines
        if(CurrentMineNumber == 0)
        {
            int num = Random.Range(0, Tiles.Count);
            Tiles[num].Mine = true;
            Tiles[num].ResetTile();
        }
    }

    [Button("Clear Grid", ButtonSizes.Large)]
    public void ClearLists()
    {
        foreach(MineSweeperTile tile in Tiles)
        {
            if(Application.isPlaying)
            {
                Destroy(tile.gameObject);
            }
            else
            {   
                DestroyImmediate(tile.gameObject, true);
            }
        }
        GridXOffset = 0f;
        GridYOffset = 0f;
        Tiles.Clear();
    }

    [Button("Hide Tiles", ButtonSizes.Large)]
    public void HideTiles()
    {
        StartCoroutine(_HideTiles());
    }

    #region Helpers

    public bool ShouldGetMine()
    {
        if (CurrentMineNumber >= Grid.MaxMines)
        {
            return false;
        }

        if (Random.Range(0, 1f) < Grid.MineRatio)
        {
            return true;
        }
        return false;
    }

    public IEnumerator _HideTiles()
    {
        for(int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].HidePlate(true);
        }
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].HidePlate(false);
        }
    }

    #endregion
}
