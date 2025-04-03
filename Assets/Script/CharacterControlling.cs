using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterControlling : MonoBehaviour
{
    [SerializeField] Vector3 enemyCord;
    [SerializeField] PathFinding pf;
    [SerializeField] Transform playerCord;
    [SerializeField] Transform tileCord;

    private bool allowClick = true;
    private bool secondMove = false;
    private bool attackAction = false;
    private bool moveAction = false;
    private Node firstPointedNode;
    private Node pointedNode;
    List<Node> oldPath = new List<Node>();

    private Vector3 playerPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private float jumpDuration = 0.6f;
    [SerializeField] public Animator animator;
    [SerializeField] GridManager gridManager;
    List<Node> path;
    [SerializeField] EnemyControlling enemyControlling;
    PlayerStats playerStats;

    SuggestTiles suggestTiles;

    private void Awake()
    {
        pf = GameObject.Find("Grid").GetComponent<PathFinding>();
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        enemyControlling = GetComponent<EnemyControlling>();
        suggestTiles = GameObject.Find("SuggestPathManager").GetComponent<SuggestTiles>();
        playerStats = PlayerStats.Instance;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GameObject.Find("MainCharacter").GetComponent<Animator>();
    }
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                pointedNode = gridManager.NodeFromWorldPoint(hit.collider.gameObject.transform.position);

                if (hasHit && pointedNode.havePlayerOn && !secondMove && allowClick)
                {
                    Debug.Log("hitplayer");
                    playerPosition = hit.collider.gameObject.transform.position;
                    secondMove = true;
                    suggestTiles.PlayerTileOnClick(pointedNode);
                    firstPointedNode = null;
                }
                else if (hasHit && pointedNode.havePlayerOn && secondMove && allowClick)
                {
                    Debug.Log("Cancel Movement");
                    secondMove = false;
                    suggestTiles.CancelPlayerTileOnClick(pointedNode);
                    if (path != null)
                    {
                        suggestTiles.CancelSuggestMovePath(path);
                    }
                }


                //Moving
                else if (hasHit && secondMove && pointedNode.walkable && !pointedNode.haveEnemyOn)
                {
                    suggestTiles.CancelSuggestMovePath(oldPath);
                    if (pointedNode == firstPointedNode)
                    {
                        int index = pointedNode.enemyIndex;
                        suggestTiles.CancelSuggestMovePath(path);
                        suggestTiles.CancelPlayerTileOnClick(gridManager.NodeFromWorldPoint(playerPosition));
                        moveAction = true;
                        StartCoroutine(DoAction(index));
                        gridManager.NodeFromWorldPoint(playerPosition).havePlayerOn = false;
                        
                        gridManager.NodeFromWorldPoint(playerPosition).walkable = true;
                        firstPointedNode.havePlayerOn = true;
                        firstPointedNode.walkable = false;
                    }
                    else
                    {
                        Debug.Log("Click On Tile");
                        firstPointedNode = pointedNode;
                        pf.FindPath(playerPosition, pointedNode.worldPosition);
                        path = gridManager.path;
                        oldPath = path;
                        suggestTiles.SuggestMovePath(path);
                    }
                }


                //Attack
                else if (hasHit && pointedNode.haveEnemyOn && secondMove)
                {
                    suggestTiles.CancelSuggestMovePath(oldPath);
                    if (pointedNode == firstPointedNode)
                    {
                        int index = pointedNode.enemyIndex;
                        suggestTiles.CancelSuggestMovePath(path);
                        suggestTiles.CancelPlayerTileOnClick(gridManager.NodeFromWorldPoint(playerPosition));
                        path.RemoveAt(path.Count - 1);
                        attackAction = true;
                        Debug.Log("tan cong xa");
                        Debug.Log(path.Count);
                        StartCoroutine(DoAction(index));
                    }

                    else
                    {
                        Debug.Log("Click On Enemy");
                        firstPointedNode = pointedNode;
                        pointedNode.walkable = true;
                        pf.FindPath(playerPosition, pointedNode.worldPosition);
                        pointedNode.walkable = false;
                        path = gridManager.path;
                        oldPath = path;
                        suggestTiles.SuggestAttackPath(path);
                    }
                }
            }
            else
            {
                return;
            }
        }
    }


    IEnumerator DoAction(int index)
    {
        secondMove = false;
        allowClick = false;
        //Move
        if (moveAction)
        {
            yield return StartCoroutine(Moving());
            Debug.Log("Player Moved");
            moveAction = false;
        }

        //Attack
        else if (attackAction && path.Count > 0)
        {
            yield return StartCoroutine(Moving());
            yield return StartCoroutine(Attack(index)); 
            Debug.Log("Player Attack");
            gridManager.NodeFromWorldPoint(playerPosition).havePlayerOn = false;
            gridManager.NodeFromWorldPoint(playerPosition).walkable = true;
            path[path.Count - 1].havePlayerOn = true;
            path[path.Count - 1].walkable = false;
        }
        else if (attackAction && path.Count <= 0)
        {
            yield return StartCoroutine(Attack(index));
            Debug.Log("Player Attack");
        }
        allowClick = true;
    }



    IEnumerator Moving()
    {
        foreach (Node n in path)
        {
            Vector3 directionToTarget = n.worldPosition - player.transform.position;
            Vector3 objectForward = player.transform.forward;
            float angle = Vector3.Angle(objectForward, directionToTarget);
            Vector3 rotationAxis = Vector3.Cross(objectForward, directionToTarget);
            if(rotationAxis == Vector3.zero)
            {
                rotationAxis = Vector3.Cross(player.transform.right, directionToTarget);
            }
            Quaternion angle2 = Quaternion.AngleAxis(angle, rotationAxis) * player.transform.rotation;
            
            float timer = 0f;
            Vector3 currentPosition = player.transform.position;
            animator.SetBool("Moving", true);
            while (timer < jumpDuration)
            {
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, angle2, angle/(jumpDuration / 2) * Time.deltaTime);
                player.transform.position = Vector3.Lerp(currentPosition, n.worldPosition, timer/jumpDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("Moving", false);
            yield return new WaitForSeconds(0.1f);
            player.transform.position = n.worldPosition;
        }
        secondMove = false;
        pointedNode = null;
    }
    
    IEnumerator Attack(int enemyIndex)
    {
        Debug.Log("test");
        animator.SetBool("Attack", true);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Attack", false);
        int dmg = playerStats.GetDamage();
        // Execute from EnemyControlling
        enemyControlling.BeDamaged(enemyIndex , dmg);
        attackAction = false;
        secondMove = false;
    }

    public void HandleRotate()
    {

    }






    IEnumerator Moving2(List<Node> path)
    {
        foreach (Node n in path)
        {
            Vector3 directionToTarget = n.worldPosition - player.transform.position;
            Vector3 objectForward = player.transform.forward;
            float angle = Vector3.Angle(objectForward, directionToTarget);
            Vector3 rotationAxis = Vector3.Cross(objectForward, directionToTarget);
            if (rotationAxis == Vector3.zero)
            {
                rotationAxis = Vector3.Cross(player.transform.right, directionToTarget);
            }
            Quaternion angle2 = Quaternion.AngleAxis(angle, rotationAxis) * player.transform.rotation;

            float timer = 0f;
            Vector3 currentPosition = player.transform.position;
            animator.SetBool("Moving", true);
            while (timer < jumpDuration)
            {
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, angle2, angle / (jumpDuration / 2) * Time.deltaTime);
                player.transform.position = Vector3.Lerp(currentPosition, n.worldPosition, timer / jumpDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("Moving", false);
            yield return new WaitForSeconds(0.1f);
            player.transform.position = n.worldPosition;
        }
        secondMove = false;
        pointedNode = null;
    }
}



