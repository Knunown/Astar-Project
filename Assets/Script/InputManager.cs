using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    private BattleState state;

    [SerializeField] private GameObject enemyStatsDisplay;
    private Node node;
    private GridManager gridManager;
    HashSet<Node> neighbor;
    private SuggestTiles suggestTiles;
    private EnemyControlling enemyControlling;
    private UIScript ui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        suggestTiles = GameObject.Find("SuggestPathManager").GetComponent<SuggestTiles>();
        enemyControlling = GameObject.Find("InteractionManager").GetComponent<EnemyControlling>();
        ui = GameObject.Find("InformationCanvas").GetComponent<UIScript>();
    }


    private void Start()
    {
        state = BattleState.PLAYERTURN;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                node = gridManager.NodeFromWorldPoint(hit.collider.gameObject.transform.position);
                if (node.haveEnemyOn)
                {
                    ui.DisplayEnemyStats(enemyControlling.GetEnemy(node.enemyIndex));
                    //neighbor = enemyControlling.GetEnemyRange(node, enemyControlling.GetMovementRange(node.enemyIndex));
                    //suggestTiles.SuggestMovingRange(neighbor);
                }
                else
                {
                    if (neighbor != null)
                    {
                        enemyStatsDisplay.SetActive(false);
                        //suggestTiles.CancelSuggestMovingRange(neighbor);
                        //neighbor = null;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                if (neighbor != null)
                {
                    enemyStatsDisplay.SetActive(false);
                    //suggestTiles.CancelSuggestMovingRange(neighbor);
                    //neighbor = null;
                }
                else
                {
                    return;
                }
            }



        }

            //bool hasHit = Physics.Raycast(ray, out hit);
            //if (hasHit)
            //{
            //    pointedNode = gridManager.NodeFromWorldPoint(hit.collider.gameObject.transform.position);

            //    if (hasHit && pointedNode.havePlayerOn && !secondMove && allowClick)
            //    {
            //        Debug.Log("hitplayer");
            //        playerPosition = hit.collider.gameObject.transform.position;
            //        secondMove = true;
            //        suggestTiles.PlayerTileOnClick(pointedNode);
            //        firstPointedNode = null;
            //    }
            //    else if (hasHit && pointedNode.havePlayerOn && secondMove && allowClick)
            //    {
            //        Debug.Log("Cancel Movement");
            //        secondMove = false;
            //        suggestTiles.CancelPlayerTileOnClick(pointedNode);
            //        if (path != null)
            //        {
            //            suggestTiles.CancelSuggestMovePath(path);
            //        }
            //    }


            //    //Moving
            //    else if (hasHit && secondMove && pointedNode.walkable && !pointedNode.haveEnemyOn)
            //    {
            //        suggestTiles.CancelSuggestMovePath(oldPath);
            //        if (pointedNode == firstPointedNode)
            //        {
            //            int index = pointedNode.enemyIndex;
            //            suggestTiles.CancelSuggestMovePath(path);
            //            suggestTiles.CancelPlayerTileOnClick(gridManager.NodeFromWorldPoint(playerPosition));
            //            moveAction = true;
            //            StartCoroutine(DoAction(index));
            //            gridManager.NodeFromWorldPoint(playerPosition).havePlayerOn = false;
            //            gridManager.NodeFromWorldPoint(playerPosition).walkable = true;
            //            firstPointedNode.havePlayerOn = true;
            //            firstPointedNode.walkable = false;
            //        }
            //        else
            //        {
            //            Debug.Log("Click On Tile");
            //            firstPointedNode = pointedNode;
            //            pf.FindPath(playerPosition, pointedNode.worldPosition);
            //            path = gridManager.path;
            //            oldPath = path;
            //            suggestTiles.SuggestMovePath(path);
            //        }
            //    }


            //    //Attack
            //    else if (hasHit && pointedNode.haveEnemyOn && secondMove)
            //    {
            //        suggestTiles.CancelSuggestMovePath(oldPath);
            //        if (pointedNode == firstPointedNode)
            //        {
            //            int index = pointedNode.enemyIndex;
            //            suggestTiles.CancelSuggestMovePath(path);
            //            suggestTiles.CancelPlayerTileOnClick(gridManager.NodeFromWorldPoint(playerPosition));
            //            path.RemoveAt(path.Count - 1);
            //            attackAction = true;
            //            Debug.Log("tan cong xa");
            //            Debug.Log(path.Count);
            //            StartCoroutine(DoAction(index));
            //        }

            //        else
            //        {
            //            Debug.Log("Click On Enemy");
            //            firstPointedNode = pointedNode;
            //            pointedNode.walkable = true;
            //            pf.FindPath(playerPosition, pointedNode.worldPosition);
            //            pointedNode.walkable = false;
            //            path = gridManager.path;
            //            oldPath = path;
            //            suggestTiles.SuggestAttackPath(path);
            //        }
            //    }
            //}
            //else
            //{
            //    return;
            //}
        
    }

    

}
