using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuggestTiles : MonoBehaviour
{
    [SerializeField] private Material playerTileOnClick;
    [SerializeField] private Material suggesTile;
    [SerializeField] private Material normalTiles;
    [SerializeField] private Material enemyTile;
    [SerializeField] MeshRenderer mesh;
    
    public void SuggestMovePath(List<Node> nodes) 
    {
        int i = 0;
        foreach (Node n in nodes)
        {
            
            mesh = GameObject.Find(n.tileName).GetComponentInChildren<MeshRenderer>();
            Material[] mats = mesh.materials;
            if (i< nodes.Count-1)
            {
                mats[1] = suggesTile;
            }
            else
            {
                mats[1] = enemyTile;
            }
            mesh.materials = mats;
            i++;
        }
    }

    public void CancelSuggestMovePath(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if(!n.havePlayerOn)
            { 
                mesh = GameObject.Find(n.tileName).GetComponentInChildren<MeshRenderer>();
                Material[] mats = mesh.materials;
                mats[1] = normalTiles;
                mesh.materials = mats;
            }
        }
    }

    public void PlayerTileOnClick(Node nodes)
    {
            mesh = GameObject.Find(nodes.tileName).GetComponentInChildren<MeshRenderer>();
            Material[] mats = mesh.materials;
            mats[1] = playerTileOnClick;
            mesh.materials = mats;
    }

    public void CancelPlayerTileOnClick(Node nodes)
    {
        mesh = GameObject.Find(nodes.tileName).GetComponentInChildren<MeshRenderer>();
        Material[] mats = mesh.materials;
        mats[1] = normalTiles;
        mesh.materials = mats;
    }

    public void SuggestAttackPath(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
                mesh = GameObject.Find(n.tileName).GetComponentInChildren<MeshRenderer>();
                Material[] mats = mesh.materials;
                mats[1] = suggesTile;
                mesh.materials = mats;
            if (n == nodes[nodes.Count - 1])
            {
                mesh = GameObject.Find(n.tileName).GetComponentInChildren<MeshRenderer>();
                mats = mesh.materials;
                mats[1] = enemyTile;
                mesh.materials = mats;
            }
        }
    }

    public void SuggestMovingRange(HashSet<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            mesh = GameObject.Find(n.tileName).GetComponentInChildren<MeshRenderer>();
            Material[] mats = mesh.materials;
            mats[1] = enemyTile;
            mesh.materials = mats;
        }
    }

    public void CancelSuggestMovingRange(HashSet<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            mesh = GameObject.Find(n.tileName).GetComponentInChildren<MeshRenderer>();
            Material[] mats = mesh.materials;
            mats[1] = normalTiles;
            mesh.materials = mats;
        }
    }
}
