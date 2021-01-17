using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject blockTypes_go;
    [SerializeField]
    public List<GameObject> blockGO;
    public BlockTypes blockType;
    public Vector3 posOnGrid;
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        blockType = BlockTypes.AIR;
        registerBlockTypes();
        ModelSwitch();
    }
    
    // Update is called once per frame
    void Update()
    {
        //if (typeof(MouseInput.MouseActions))
        ModelSwitch();
    }
    

    void ModelSwitch()
    {
        for (int x = 0; x < blockGO.Count; x++)
        {
            if (x == (int)blockType)
            {
                blockGO[x].SetActive(true);
            }
            else
            {
                blockGO[x].SetActive(false);
            }
        }
    }

    public void registerBlockTypes()
    {
        for (int i = 0; i < blockTypes_go.transform.childCount; ++i)
        {
            //adds Block to available Blocks
            blockGO.Add(blockTypes_go.transform.GetChild(i).gameObject);
            
            //Sets sorting order depending on height
            blockGO[i].GetComponent<SpriteRenderer>().sortingOrder = (int)posOnGrid.z;
            
            //Sets color depending on height
            float colorTint = 1f;
            switch ((int)posOnGrid.z)
            {
                case 0: colorTint = 0.5f; break;
                case 1: colorTint = 0.6f; break;
                case 2: colorTint = 0.7f; break;
                case 3: colorTint = 0.8f; break;
                case 4: colorTint = 0.9f; break;
                default: colorTint = 1f; break;
            }
            blockGO[i].GetComponent<SpriteRenderer>().color = new Color(colorTint,colorTint,colorTint);
        }
    }

    public void Fall()
    {
        /*
        if (blockType != BlockTypes.AIR)
        {
            if (posOnGrid.z > 0)
            {
                if (gridManager.getBlockType(new Vector3((int) posOnGrid.x, (int) posOnGrid.y,
                    (int) posOnGrid.z - 1)) == BlockTypes.AIR)
                {
                    gridManager.setBlockType(new Vector3((int) posOnGrid.x, (int) posOnGrid.y, (int) posOnGrid.z - 1),
                        blockType);
                }
            }
        }
        */
        if (posOnGrid.z > 0)
        {
            if (blockType != BlockTypes.AIR)
            {

                Vector3 oldPos = posOnGrid;
                Vector3 targetPos = new Vector3((int)posOnGrid.x, (int)posOnGrid.y, (int)posOnGrid.z - 1);
                if (targetPos.z > 0)
                {
                    for (int z = (int)posOnGrid.z; z > 0; z--)
                    {
                        if (gridManager.getBlockType(targetPos) == BlockTypes.AIR)
                        {
                            targetPos = new Vector3(posOnGrid.x, posOnGrid.y, z);
                        }
                        else
                        {
                            targetPos += new Vector3(0, 0, -1);
                            break;
                        }
                    }
                }

                if (oldPos != targetPos)
                {
                    gridManager.setBlockType(targetPos, blockType);
                    gridManager.setBlockType(oldPos, BlockTypes.AIR);
                }
            }
        }
    }
    
    public enum BlockTypes
    {
        AIR = 0,
        GRASS = 1,
        STONE = 2,
        SAND = 3,
        DIRT = 4,
    }
}