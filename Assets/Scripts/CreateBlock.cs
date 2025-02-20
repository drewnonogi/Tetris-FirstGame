using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    [field: SerializeField]
    public BlockController blockControllerInstance { get; private set; }
    public GameObject[] blocksList;
    public List<GameObject> allBlocks = new List<GameObject>();
    private int BlockNumber { get; set; }

    private void Awake()
    {
        MakeBlock();
    }
    private void Start()
    {
    }
    
    public void MakeBlock()
    {
        //int i = 0;
        int i = Random.Range(0, blocksList.Length);
        GameObject go = Instantiate(blocksList[i], new Vector3(4.5f, 18.5f, 0), Quaternion.identity);
        go.transform.name = BlockNumber.ToString();

        blockControllerInstance.ActiveBlock = go;
        blockControllerInstance.BlockTransform = go.transform;
        blockControllerInstance.CurrentRotationIndex = 3; //3 to avoid first rotation jumpover position
        allBlocks.Add(go);

        if (blockControllerInstance.IsPositionColidingWithBlocks(go.transform.position) == true)
        {
            //Destroy(CreateBlock);                    CZEMU TO NIE DZIA£A?
        }
        if ((BlockNumber == 0) == false)
        {
            //GameObject previousGameObject = GameObject.Find($"{BlockNumber - 1}");
            //Destroy(previousGameObject);
        }
        BlockNumber++;
        
    }

}
