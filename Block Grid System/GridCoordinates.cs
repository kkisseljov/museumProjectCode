using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for working with grid coordinates with different useful getters and checks
/// </summary>
[Serializable]
public class GridCoordinates {
    public int x;
    public int y;

    public GridCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator == (GridCoordinates obj1, GridCoordinates obj2)
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return true;
        }

        if (ReferenceEquals(obj1, null))
        {
            return false;
        }
        if (ReferenceEquals(obj2, null))
        {
            return false;
        }

        return (obj1.x == obj2.x && obj1.y == obj2.y);
    }

    public static bool operator != (GridCoordinates obj1, GridCoordinates obj2)
    {
        return !(obj1 == obj2);
    }

    public override bool Equals(object obj)
    {
        var coordinates = obj as GridCoordinates;
        return coordinates != null &&
               x == coordinates.x &&
               y == coordinates.y;
    }

    public override int GetHashCode()
    {
        var hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }

    //TODO overload with input Vector3
    //TODO bad naming
    public Vector3 GetPositionOffset(float yOffset = 0f)
    {
        return new Vector3(x * 1.5f, yOffset, y * 1.5f);
    }

    public GridCoordinates GetFrontLeft(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x - 1, y + 1);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetFront(BlockGrid grid)
    {

        GridCoordinates coords = new GridCoordinates(x, y + 1);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetFrontRight(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x + 1, y + 1);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetRight(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x + 1, y);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetRearRight(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x + 1, y - 1);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetRear(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x, y - 1);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetRearLeft(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x - 1, y - 1);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public GridCoordinates GetLeft(BlockGrid grid)
    {
        GridCoordinates coords = new GridCoordinates(x - 1, y);
        return coords.OutOfBounds(grid) ? null : coords;
    }

    public List<GridCoordinates> GetNearestCoords(BlockGrid grid)
    {
        List<GridCoordinates> resultList = new List<GridCoordinates>();
        GridCoordinates frontLeft = GetFrontLeft(grid);
        GridCoordinates front = GetFront(grid);
        GridCoordinates frontRight = GetFrontRight(grid);
        GridCoordinates right = GetRight(grid);
        GridCoordinates rearRight = GetRearRight(grid);
        GridCoordinates rear = GetRear(grid);
        GridCoordinates rearLeft = GetRearLeft(grid);
        GridCoordinates left = GetLeft(grid);

        if(frontLeft != null)
        {
            resultList.Add(frontLeft);
        }
        if (front != null)
        {
            resultList.Add(front);
        }
        if (frontRight != null)
        {
            resultList.Add(frontRight);
        }
        if (right != null)
        {
            resultList.Add(right);
        }
        if (rearRight != null)
        {
            resultList.Add(rearRight);
        }
        if (rear != null)
        {
            resultList.Add(rear);
        }
        if (rearLeft != null)
        {
            resultList.Add(rearLeft);
        }
        if (left != null)
        {
            resultList.Add(left);
        }

        return resultList;
    }

    /*
    public List<GridCoordinates> GetAdjacentCoords(BlockGrid grid)
    {
        List<GridCoordinates> resultList = new List<GridCoordinates>();
        GridCoordinates front = GetFront(grid);
        GridCoordinates right = GetRight(grid);
        GridCoordinates rear = GetRear(grid);
        GridCoordinates left = GetLeft(grid);

        if (front != null)
        {
            resultList.Add(front);
        }
        if (right != null)
        {
            resultList.Add(right);
        }
        if (rear != null)
        {
            resultList.Add(rear);
        }
        if (left != null)
        {
            resultList.Add(left);
        }

        return resultList;
    }
    */

    public bool OutOfBounds(BlockGrid grid)
    {
        return x < 0 || y < 0 || x >= grid.Width || y >= grid.Length;
    }

    public bool OnGridEdge(BlockGrid grid)
    {
        return OutOfBounds(grid) || x == 0 || y == 0 || x == grid.Width - 1 || y == grid.Length - 1;
    }

    public bool IsAdjacentTo(GridCoordinates coordinate)
    {
        return (coordinate.x - x == 0 || coordinate.x - x == -1 || coordinate.x - x == 1) 
            && (coordinate.y - y == 0 || coordinate.y - y == -1 || coordinate.y - y == 1)
            && (coordinate.x == x || coordinate.y == y);
    }

    public override string ToString()
    {
        return x + " : " + y;
    }
}
