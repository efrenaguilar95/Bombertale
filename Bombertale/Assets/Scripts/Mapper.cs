using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mapper : MonoBehaviour
{
    //This won't work for local game anymore. have to put a check on whether you're playing a local game and instantiate a non-networked softblock
    public static GameObject coneBlock, hardBlock, softBlock, speedUp, bombUp, explosionUp, determination;

    //public List<List<char>> grid = new List<List<char>>();
    //public List<List<GameObject>> gameObjectGrid = new List<List<GameObject>>();

    //string mapString = "C C C C C C C C C C C C C C C\n" +
    //                   "C . . . S . S . S S S . . . C\n" +
    //                   "C . H S H S H S H S H S H . C\n" +
    //                   "C . S S S S S S S S . S S . C\n" +
    //                   "C S H S H S H S H S H S H S C\n" +
    //                   "C S S S S S S S S S . S S S C\n" +
    //                   "C . H S H . H S H S H S H . C\n" +
    //                   "C S S S S S S S . S . S S S C\n" +
    //                   "C S H S H S H S H S H S H S C\n" +
    //                   "C . S S S S S S S S S . S . C\n" +
    //                   "C . H S H S H S H S H S H . C\n" +
    //                   "C . . . S S . S . S S . . . C\n" +
    //                   "C C C C C C C C C C C C C C C";
    public static string mapString = "CCCCCCCCCCCCCCC\n" +
                                       "C...S.S.SSS...C\n" +
                                       "C.HSHSHSHSHSH.C\n" +
                                       "C.SSSSSSSS.SS.C\n" +
                                       "CSHSHSHSHSHSHSC\n" +
                                       "CSSSSSSSSS.SSSC\n" +
                                       "C.HSH.HSHSHSH.C\n" +
                                       "CSSSSSSS.S.SSSC\n" +
                                       "CSHSHSHSHSHSHSC\n" +
                                       "C.SSSSSSSSS.S.C\n" +
                                       "C.HSHSHSHSHSH.C\n" +
                                       "C...SS.S.SS...C\n" +
                                       "CCCCCCCCCCCCCCC";

    void Awake()
    {
        coneBlock = Resources.Load("ConeBlock") as GameObject;
        hardBlock = Resources.Load("HardBlock") as GameObject;
        softBlock = Resources.Load("NetworkSoftBlock") as GameObject;
        speedUp = Resources.Load("Speed+1") as GameObject;
        bombUp = Resources.Load("Bomb+1") as GameObject;
        explosionUp = Resources.Load("Explosion+1") as GameObject;
        determination = Resources.Load("Determination") as GameObject;
        //this.grid = StringToMap(mapString);



        //int y = grid[0].Count - 1;
        //foreach (List<char> row in grid)
        //{
        //    int x = 0;
        //    foreach (char cell in row)
        //    {
        //        CreateBlock(cell, x, y);
        //        x++;
        //    }
        //    y--;

        //}
        //string[] rowList = mapString.Split('\n');

        //int ROW_NUMS = rowList.Length;
        //int COL_NUMS = rowList[0].Length;
        ////int COL_NUMS = rowList[0].Split(' ').Length;        

        ////Initialize the grid and gameObjectGrid
        //for (int col = 0; col < COL_NUMS; col++)
        //{
        //    List<char> colList = new List<char>();
        //    List<GameObject> goList = new List<GameObject>();
        //    for (int row = 0; row < ROW_NUMS; row++)
        //    {
        //        colList.Add('.');
        //        goList.Add(null);
        //    }
        //    grid.Add(colList);
        //    gameObjectGrid.Add(goList);
        //}

        //int y = ROW_NUMS - 1;

        //foreach (string row in rowList)
        //{
        //    int x = 0;

        //    //foreach (string cellString in row.Split(' '))
        //    foreach (char cell in row)
        //    {
        //        //char cell = cellString[0];
        //        grid[x][y] = cell;
        //        CreateBlock(cell, x, y);
        //        x++;
        //    }
        //    y--;
        //}
    }

    public static GameObject CreateCell(char cellID, int x, int y)
    {
        GameObject cell;

        if (cellID == CellID.ConeBlock)
        {
            cell = (GameObject)Instantiate(coneBlock, new Vector2(x, y), Quaternion.identity);
        }
        else if (cellID == CellID.HardBlock)
        {
            cell = (GameObject)Instantiate(hardBlock, new Vector2(x, y), Quaternion.identity);
        }
        else if (cellID == CellID.SoftBlock)
        {
            cell = (GameObject)Instantiate(softBlock, new Vector2(x, y), Quaternion.identity);
        }
        else if (cellID == CellID.SpeedUp)
        {
            cell = (GameObject)Instantiate(speedUp, new Vector2(x, y), Quaternion.identity);
        }
        else if (cellID == CellID.BombUp)
        {
            cell = (GameObject)Instantiate(bombUp, new Vector2(x, y), Quaternion.identity);
        }
        else if (cellID == CellID.ExplosionUp)
        {
            cell = (GameObject)Instantiate(explosionUp, new Vector2(x, y), Quaternion.identity);
        }
        else if (cellID == CellID.Determination)
        {
            cell = (GameObject)Instantiate(determination, new Vector2(x, y), Quaternion.identity);
        }
        else
        {
            cell = null;
        }

        return cell;
    }

    public static List<List<GameObject>> CreateGameObjectMap(List<List<char>> charMap)
    {
        List<List<GameObject>> result = new List<List<GameObject>>();
        for (int col = 0; col < charMap.Count; col++)
        {
            List<GameObject> goList = new List<GameObject>();
            for (int row = 0; row < charMap[col].Count; row++)
            {
                goList.Add(CreateCell(charMap[col][row], col, row));
            }
            result.Add(goList);
        }
        return result;
    }

    public static string MapToString(List<List<char>> grid)
    {
        string result = "";

        //for (int row = 0; row < grid[0].Count; row++)
        for (int row = grid[0].Count - 1; row >= 0; row--)
        {
            for (int col = 0; col < grid.Count; col++)
            {
                result += grid[col][row];
            }
            //if (row != grid[0].Count - 1)
            if (row != 0)
                result += '\n';
        }

        return result;
    }

    public static List<List<char>> StringToMap(string mapString)
    {
        List<List<char>> result = new List<List<char>>();

        string[] rowList = mapString.Split('\n');

        int ROW_NUMS = rowList.Length;
        int COL_NUMS = rowList[0].Length;

        for (int col = 0; col < COL_NUMS; col++)
        {
            List<char> colList = new List<char>();
            for (int row = 0; row < ROW_NUMS; row++)
            {
                colList.Add('.');
            }
            result.Add(colList);
        }

        int y = ROW_NUMS - 1;

        foreach (string row in rowList)
        {
            int x = 0;

            foreach (char cell in row)
            {
                result[x][y] = cell;
                x++;
            }
            y--;
        }

        return result;
    }
}
