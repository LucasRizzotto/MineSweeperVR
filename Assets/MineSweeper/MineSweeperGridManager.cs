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
}
