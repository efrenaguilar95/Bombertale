using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mapper : MonoBehaviour
{
    public GameObject coneBlock, hardBlock, softBlock;
    public List<List<char>> grid = new List<List<char>>();
    public List<List<GameObject>> gameObjectGrid = new List<List<GameObject>>();

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
    string mapString = "CCCCCCCCCCCCCCC\n" +
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
        this.grid = StringToMap(mapString);

        for (int col = 0; col < grid.Count; col++)
        {
            List<GameObject> goList = new List<GameObject>();
            for (int row = 0; row < grid[col].Count; row++)
            {
                goList.Add(CreateBlock(grid[col][row], col, row));
            }
            gameObjectGrid.Add(goList);
        }

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

    GameObject CreateBlock(char cellID, int x, int y)
    {
        GameObject block;

        if (cellID == CellID.ConeBlock)
        {
            block = (GameObject)Instantiate(coneBlock, new Vector2(x, y), Quaternion.identity);
            //gameObjectGrid[x][y] = block;
        }
        else if (cellID == CellID.HardBlock)
        {
            block = (GameObject)Instantiate(hardBlock, new Vector2(x, y), Quaternion.identity);
            //gameObjectGrid[x][y] = block;
        }
        else if (cellID == CellID.SoftBlock)
        {
            block = (GameObject)Instantiate(softBlock, new Vector2(x, y), Quaternion.identity);
            //gameObjectGrid[x][y] = block;
        }
        else
        {
            block = null;
        }

        return block;
    }

    public static string MapToString(List<List<char>> grid)
    {
        string result = "";

        for (int row = 0; row < grid[0].Count; row++)
        {
            for (int col = 0; col < grid.Count; col++)
            {
                result += grid[col][row];
            }
            if (row != grid[0].Count - 1)
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
