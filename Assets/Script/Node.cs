using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{
    public Enemy enemy;
    public bool haveEnemyOn;
    public bool havePlayerOn;
    public int enemyIndex;
    public string tileName;

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get 
        { 
            return heapIndex;
        }
        set 
        {
            heapIndex = value;   
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    public void SetStateEnemy()
    {
        this.walkable = false;
        this.haveEnemyOn = true;
        this.havePlayerOn = false;
    }

    public void SetStateNormalTile()
    {
        this.walkable = true;
        this.haveEnemyOn = false;
        this.havePlayerOn = false;
    }
}
