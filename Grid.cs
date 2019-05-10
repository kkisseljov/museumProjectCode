using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Grid {

    public int width_x = 300;
    public int length_y = 300;

    //TODO make as array of objects
    public int[,] _value;

    public Grid(int width, int length)
    {
        width_x = width;
        length_y = length;
    }

    public void GenerateTestGrid(int startPosX, int startPosY)
    {
        GenerateFlatGrid();

        //Debug.Log("FLAT: \n" + this.toString());

        GenerateMines(startPosX, startPosY);

        //Debug.Log("WITH MINES: \n" + this.toString());
    }

    public int GetTileCode(int coordinateX, int coordinateY)
    {
        //TODO add checks
        return _value[coordinateX, coordinateY];
    }

    private void GenerateFlatGrid()
    {
        _value = new int[width_x, length_y];

        for (int _wi = 0; _wi < width_x; _wi++)
        {
            for (int _li = 0; _li < length_y; _li++)
            {
                _value[_wi, _li] = 100;
            }
        }
    }

    private void GenerateMines(int startingPositionX = 0, int startingPositionY = 0)
    {
        int numberOfMines = 30;
        int generatedMines = 0;

        int currentPosX = startingPositionX;
        int currentPosY = startingPositionY;

        do
        {
            int newPosX;
            int newPosY;
            GenerateMine(currentPosX, currentPosY, out newPosX, out newPosY);
            currentPosX = newPosX;
            currentPosY = newPosY;
            generatedMines++;
        } while (generatedMines < numberOfMines);

        Debug.Log("Target number of mines: " + numberOfMines + "\ngenerated: " + generatedMines);
    }

    private void GenerateMine(int startX, int startY, out int targetX, out int targetY)
    {
        bool ok = false;
        int maxIterations = 20;
        int iterationsAmount = 0;

        targetX = startX;
        targetY = startY;

        do
        {
            int direction = UnityEngine.Random.Range(0, 4);
            int length = UnityEngine.Random.Range(5, 15);

            Debug.Log("PASS " + iterationsAmount + ": Try to create mine with direction = " + direction + ", length = " + length);

            switch(direction)
            {
                case 0:
                    targetX += length;
                    break;
                case 1:
                    targetX -= length;
                    break;
                case 2:
                    targetY += length;
                    break;
                case 3:
                    targetY -= length;
                    break;
            }

            if(!IsCloseToEdge(targetX, targetY))
            {
                ok = TilesBeforeIntersection(startX, startY, targetX, targetY) > 3;
            }

            iterationsAmount++;
        } while (!ok && iterationsAmount <= maxIterations);

        if(ok)
        {
            Debug.Log("\tChecks passed -> create from " + startX + "," + startY + " to " + targetX + "," + targetY + "\n close to edge = "
                + IsCloseToEdge(targetX, targetY) + "\t tiles before intersect = " + TilesBeforeIntersection(startX, startY, targetX, targetY));

            if (startX < targetX)
            {
                for (int x = startX; x < targetX; x++)
                {
                    _value[x, startY] = 1;
                }
            }

            if (startX > targetX)
            {
                for (int x = targetX; x < startX; x++)
                {
                    _value[x, startY] = 1;
                }
            }

            if (startY < targetY)
            {
                for (int y = startY; y < targetX; y++)
                {
                    _value[startX, y] = 1;
                }
            }

            if (startY > targetY)
            {
                for (int y = targetY; y < startY; y++)
                {
                    _value[startX, y] = 1;
                }
            }
        } else
        {
            targetX = startX;
            targetY = startY;
        }
    }

    public bool IsCloseToEdge(int xPos, int yPos, int margin = 3)
    {
        return xPos < margin || xPos > width_x - margin
            || yPos < margin || yPos > width_x - margin;
    }

    public int TilesBeforeIntersection(int startX, int startY, int targetX, int targetY)
    {
        Debug.Log("TilesBeforeIntersection start(" + startX + "," + startY + ") -> target (" + targetX + "," + targetY + ")");
        int length = 0;

        //Mine goes beyond grid
        if(targetX < 0 || targetY < 0)
        {
            return 0;
        }

        if (startX < targetX)
        {
            for (int x = startX; x < targetX; x++)
            {
                if(_value[x, startY] == 1)
                {
                    return length;
                } else
                {
                    length++;
                }
            }
        }

        if (startX > targetX)
        {
            for (int x = targetX; x < startX; x++)
            {
                if (_value[x, startY] == 1)
                {
                    return length;
                }
                else
                {
                    length++;
                }
            }
        }

        if (startY < targetY)
        {
            for (int y = startY; y < targetX; y++)
            {
                if (_value[startX, y] == 1)
                {
                    return length;
                }
                else
                {
                    length++;
                }
            }
        }

        if (startY > targetY)
        {
            for (int y = targetY; y < startY; y++)
            {
                if (_value[startX, y] == 1)
                {
                    return length;
                }
                else
                {
                    length++;
                }
            }
        }

        return length;
    }

    string toString()
    {
        string result = "";

        for (int _wi = 0; _wi < width_x; _wi++)
        {
            for (int _li = 0; _li < length_y; _li++)
            {
                result += _value[_wi, _li] + "|";
            }

            result += "\n";
        }

        return result;
    }
}
