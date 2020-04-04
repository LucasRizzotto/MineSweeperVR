using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MineSweeper Grid", menuName = "MineSweeper/Grid", order = 1)]
public class MineSweeperGrid : ScriptableObject
{
    public int Xsize = 10;
    public int Ysize = 10;
    public GameObject TilePrefab;
    public float TileSize = 1f;
    public float MineRatio = 0.1f;
    public int MaxMines = 4;
}
