using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mapper : MonoBehaviour {
    public GameObject coneBlock, hardBlock, softBlock;
    public List<List<char>> grid = new List<List<char>>();
    public List<List<GameObject>> gameObjectGrid = new List<List<GameObject>>();

    string mapString = "C C C C C C C C C C C C C C C\n" +
                       "C . . . S . S . S S S . . . C\n" +
                       "C . H S H S H S H S H S H . C\n" +
                       "C . S S S S S S S S . S S . C\n" +
                       "C S H S H S H S H S H S H S C\n" +
                       "C S S S S S S S S S . S S S C\n" +
                       "C . H S H . H S H S H S H . C\n" +
                       "C S S S S S S S . S . S S S C\n" +
                       "C S H S H S H S H S H S H S C\n" +
                       "C . S S S S S S S S S . S . C\n" +
                       "C . H S H S H S H S H S H . C\n" +
                       "C . . . S S . S . S S . . . C\n" +
                       "C C C C C C C C C C C C C C C";
	
	void Awake () {        
        string[] rowList = mapString.Split('\n');

        int ROW_NUMS = rowList.Length;
        int COL_NUMS = rowList[0].Split(' ').Length;        

        //Initialize the grid and gameObjectGrid
        for (int col = 0; col < COL_NUMS; col++)
        {
            List<char> colList = new List<char>();
            List<GameObject> goList = new List<GameObject>();
            for (int row = 0; row < ROW_NUMS; row++)
            {
                colList.Add('.');
                goList.Add(null);
            }
            grid.Add(colList);
            gameObjectGrid.Add(goList);
        }

        int y = ROW_NUMS-1;
        foreach (string row in rowList)
        {
            int x = 0;

            foreach (string cellString in row.Split(' '))
            {
                char cell = cellString[0];
                grid[x][y] = cell;
                CreateBlock(cell, x, y);
                x++;
            }
            y--;
        }	    
	}

    void CreateBlock(char cellID, int x, int y)
    {
        if (cellID == CellID.ConeBlock)
        {
            GameObject block = (GameObject)Instantiate(coneBlock, new Vector2(x, y), Quaternion.identity);
            gameObjectGrid[x][y] = block;
        }
        else if (cellID == CellID.HardBlock)
        {
            GameObject block = (GameObject)Instantiate(hardBlock, new Vector2(x, y), Quaternion.identity);
            gameObjectGrid[x][y] = block;
        }
        else if (cellID == CellID.SoftBlock)
        {
            GameObject block = (GameObject)Instantiate(softBlock, new Vector2(x, y), Quaternion.identity);
            gameObjectGrid[x][y] = block;
        }
    }
}
