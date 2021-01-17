using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public GameObject block_GO;
    public Boolean worldNeedsRandomize = false;
    public Vector3 gridSize;
    private IsoHelper isoHelper;

    [SerializeField]
    private GameObject[,,] blockGO = new GameObject[100,100,100];

    private void Start()
    {
        isoHelper = GetComponent<IsoHelper>();
        createMatrix();
    }

    private void Update()
    {
        if (worldNeedsRandomize)
        {
            RandomizeWorld();
            worldNeedsRandomize = false;
        }
    }

    public void createMatrix()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    Vector3 input = new Vector3(x, y, z);
                    Vector3 pos = isoHelper.XYToIso(input);
                    blockGO[x, y, z] = Instantiate(block_GO, pos, Quaternion.identity);
                    blockGO[x, y, z].GetComponent<BlockBehaviour>().blockType = BlockBehaviour.BlockTypes.AIR;
                    blockGO[x, y, z].GetComponent<BlockBehaviour>().posOnGrid = input;
                }
            }
        }
    }
    
    public void RandomizeWorld()
    {
        for (int z = 0; z < gridSize.z; z++)
        {
            //Sets available Blocks for the Layers
            List<BlockBehaviour.BlockTypes> availableBlocks = new List<BlockBehaviour.BlockTypes>();
            switch (z)
            {
                /* RANDOM BLOCKS GENERATOR
                 * foreach (BlockBehaviour.BlockTypes blockType in Enum.GetValues(typeof(BlockBehaviour.BlockTypes)))
                 *
                    {
                        availableBlocks.Add(blockType);
                    }
                    break;
                    */
                case 0:
                    //Stonelayer
                    availableBlocks.Add(BlockBehaviour.BlockTypes.STONE);
                    break;
                case 1:
                    //Dirt and Sandlayer
                    availableBlocks.Add(BlockBehaviour.BlockTypes.GRASS);
                    availableBlocks.Add(BlockBehaviour.BlockTypes.STONE);
                    break;
                case 2:
                    //Grasslayer
                    availableBlocks.Add(BlockBehaviour.BlockTypes.GRASS);
                    availableBlocks.Add(BlockBehaviour.BlockTypes.AIR);
                    break;
                case 3:
                    //Grass and Air layer
                    availableBlocks.Add(BlockBehaviour.BlockTypes.AIR);
                    break;
                default:
                    availableBlocks.Add(BlockBehaviour.BlockTypes.STONE);
                    availableBlocks.Add(BlockBehaviour.BlockTypes.AIR);
                    availableBlocks.Add(BlockBehaviour.BlockTypes.DIRT);
                    availableBlocks.Add(BlockBehaviour.BlockTypes.AIR);
                    availableBlocks.Add(BlockBehaviour.BlockTypes.AIR);
                    break;
            }
            
            //Generates World
            for (int y = 0; y < (int)gridSize.y; y++)
            {
                for (int x = 0; x < (int)gridSize.x; x++)
                {
                    BlockBehaviour bb = blockGO[x, y, z].GetComponent<BlockBehaviour>();

                    BlockBehaviour.BlockTypes b = (BlockBehaviour.BlockTypes) availableBlocks[Random.Range(0, availableBlocks.Count)];
                    bb.blockType = b;
                }
            }
        }
        
        //Let Blocks Fall
        for (int z = 0; z < (int)gridSize.z; z++)
        {
            for (int y = 0; y < (int)gridSize.y; y++)
            {
                for (int x = 0; x < (int)gridSize.x; x++)
                {
                    BlockBehaviour bb = blockGO[x, y, z].GetComponent<BlockBehaviour>();
                    bb.Fall();
                }
            }
        }
    }

    public BlockBehaviour.BlockTypes getBlockType(Vector3 v)
    {
        BlockBehaviour bb = blockGO[(int)v.x, (int)v.y, (int)v.z].GetComponent<BlockBehaviour>();
        return bb.blockType;
    }

    public void setBlockType(Vector3 v, BlockBehaviour.BlockTypes b)
    {
        BlockBehaviour bb = blockGO[(int)v.x, (int)v.y, (int)v.z].GetComponent<BlockBehaviour>();
        bb.blockType = b;
    }
}