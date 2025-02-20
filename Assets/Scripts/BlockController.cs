using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{

    public static BlockController instance;
    [field: SerializeField] public CreateBlock createBlockInstance { get; private set; }
    [field: SerializeField] public GameObject ActiveBlock { get; set; }
    [field: SerializeField] public GameOverScript gameOverScriptInstance;
    [field: SerializeField] public Score scoreInstance { get; set; }
    [SerializeField] private GameInput gameInput;
    private Vector3 CurrentPosition { get; set; } = new Vector3();
    private Vector3 TargetPosition { get; set; } = new Vector3();
    [SerializeField] private AudioClip lineClearSound;
    private AudioSource audioSource;
    private Vector3 MoveVector { get; set; } = new Vector3();

    private float currentTime = 0.0f;
    public float fallTime = 0.9f;
    private bool fallAcceleration = false;

    private int boardheight = 20;
    private int boardwidth = 10;

    public List<Vector3> RotationPositions { get; private set; } = new List<Vector3>();
    public Transform BlockTransform { get; set; }
    public int CurrentRotationIndex;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        CreateListOfPositions();
    }
    void Start()
    {
        gameInput.OnBlockRotationAction += GameInput_OnBlockRotationAction;
        gameInput.OnMoveLeftAction += GameInput_OnMoveLeftAction;
        gameInput.OnMoveRightAction += GameInput_OnMoveRightAction;
        gameInput.OnMoveDownActionActive += OnMoveDownActionActive;
        gameInput.OnMoveDownActionCanceled += GameInput_OnMoveDownActionCanceled;

        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        gameInput.OnBlockRotationAction -= GameInput_OnBlockRotationAction;
        gameInput.OnMoveLeftAction -= GameInput_OnMoveLeftAction;
        gameInput.OnMoveRightAction -= GameInput_OnMoveRightAction;
        gameInput.OnMoveDownActionActive -= OnMoveDownActionActive;
        gameInput.OnMoveDownActionCanceled -= GameInput_OnMoveDownActionCanceled;
    }

    private void GameInput_OnMoveDownActionCanceled(object sender, System.EventArgs e)
    {
        fallAcceleration = false;
    }

    private void OnMoveDownActionActive(object sender, System.EventArgs e)
    {
        fallAcceleration = true;
    }

    private void GameInput_OnMoveRightAction(object sender, System.EventArgs e)
    {
        TryMoveActiveBlock(Vector3.right);

    }

    private void GameInput_OnMoveLeftAction(object sender, System.EventArgs e)
    {
        TryMoveActiveBlock(Vector3.left);
    }

    private void GameInput_OnBlockRotationAction(object sender, System.EventArgs e)
    {
        TryRotateBlock();
    }

    protected virtual void Update()
    {
        MakeFall();

    }

    private void TryRotateBlock()
    {

        if (CurrentRotationIndex == 3)
        {
            CurrentRotationIndex = 0;

            BlockTransform.GetChild(0).eulerAngles = RotationPositions[CurrentRotationIndex];
            if (IsRotationValid(BlockTransform) == false)
            {
                CurrentRotationIndex = 3;
                BlockTransform.GetChild(0).eulerAngles = RotationPositions[CurrentRotationIndex];

            }
            return;
        }

        CurrentRotationIndex = CurrentRotationIndex + 1;
        BlockTransform.GetChild(0).eulerAngles = RotationPositions[CurrentRotationIndex];
        if (IsRotationValid(BlockTransform) == false)
        {
            CurrentRotationIndex = CurrentRotationIndex - 1;
            BlockTransform.GetChild(0).eulerAngles = RotationPositions[CurrentRotationIndex];
        }
        return;
    }
    private bool IsRotationValid(Transform activeBlock)
    {
        Transform pivot = activeBlock.GetChild(0);
        for (int childIndex = 0; childIndex < pivot.childCount; childIndex++)
        {
            Transform child = pivot.GetChild(childIndex);

            if (IsPositionInsideBoundaries(child.transform.position) == false)
            {
                return false;
            }
            if (IsPositionColidingWithBlocks(child.transform.position) == true)
            {
                return false;
            }
        }
        return true;
    }


    //private Vector3 SetMoveVector()
    //{
    //    if (Input.GetKeyUp(KeyCode.LeftArrow) == true)
    //    {
    //        MoveVector = Vector3.left;
    //    }
    //    else if (Input.GetKeyUp(KeyCode.RightArrow) == true)
    //    {
    //        MoveVector = Vector3.right;
    //    }
    //    else
    //    {
    //        MoveVector = Vector3.zero;
    //    }

    //    return MoveVector;
    //}


    private bool TryMoveActiveBlock(Vector3 moveVector)
    {
        if (IsMoveValid(moveVector) == true)
        {
            ActiveBlock.transform.position += moveVector;
            return true;
        }

        return false;

    }

    private bool IsPositionInsideBoundaries(Vector3 targetPosition)
    {
        if (targetPosition.x < 0 || targetPosition.x > boardwidth || targetPosition.y < 0)
        {
            return false;
        }
        return true;

    }
    private void MakeFall()
    {
        float currentFallTime;
        if (fallAcceleration)
        {
            currentFallTime = fallTime / 10;
        }
        else
        {
            currentFallTime = fallTime;
        }
        if (Time.time - currentTime > currentFallTime)
        {
            Vector3 targetPosition = ActiveBlock.transform.position + Vector3.down;

            if (IsMoveValid(Vector3.down) == true)
            {
                ActiveBlock.transform.position = targetPosition;
            }
            else
            {
                CheckLineClear();
                RemoveEmptyBlocks();
                createBlockInstance.MakeBlock();
            }

            currentTime = Time.time;
        }
    }

    private bool IsMoveValid(Vector3 moveVector)
    {
        Transform pivot = ActiveBlock.transform.GetChild(0);
        for (int childIndex = 0; childIndex < pivot.childCount; childIndex++)
        {
            Transform child = pivot.GetChild(childIndex);
            Vector3 targetPosition = child.transform.position + moveVector; //new Vector3(child.transform.position.x - 1, child.transform.position.y, child.transform.position.z);
            if (IsPositionInsideBoundaries(targetPosition) == false)
            {
                return false;
            }
            if (IsPositionColidingWithBlocks(targetPosition) == true)
            {
                return false;
            }
        }

        return true;
    }
    public bool IsPositionColidingWithBlocks(Vector3 targetPosition)
    {
        for (int blockNumber = 0; blockNumber < createBlockInstance.allBlocks.Count - 1; blockNumber++)
        {

            Transform pivot = createBlockInstance.allBlocks[blockNumber].transform.GetChild(0);
            for (int cubeIndex = 0; cubeIndex < pivot.childCount; cubeIndex++)
            {
                Transform cube = pivot.GetChild(cubeIndex);
                if (cube.position == targetPosition)
                {

                    return true;
                }
            }

        }
        return false;
    }

    private void CheckLineClear()
    {
        Dictionary<int, List<GameObject>> rowsCollection = new Dictionary<int, List<GameObject>>();
        for (int i = 0; i < boardheight; i++)
        {
            rowsCollection.Add(i, new List<GameObject>());
        }
        for (int blockNumber = 0; blockNumber < createBlockInstance.allBlocks.Count; blockNumber++)
        {

            Transform pivot = createBlockInstance.allBlocks[blockNumber].transform.GetChild(0);
            for (int cubeIndex = 0; cubeIndex < pivot.childCount; cubeIndex++)
            {
                GameObject cube = pivot.GetChild(cubeIndex).gameObject;

                int x = Mathf.FloorToInt(cube.transform.position.y);
                if (x >= boardheight - 2)
                {
                    gameOverScriptInstance.GameOver();
                }
                else
                {
                    rowsCollection[x].Add(cube);
                }
            }

        }

        List<int> rowsToMove = new List<int>();
        foreach (KeyValuePair<int, List<GameObject>> row in rowsCollection)
        {
            if (row.Value.Count == 10)
            {
                audioSource.clip = lineClearSound;
                audioSource.Play();

                fallTime -= 0.05f;
                foreach (GameObject cube in row.Value)
                {
                    Destroy(cube);
                }
                rowsToMove.Add(row.Key);
                /*CheckLineClear();
                return;*/
            }
        }

        //Debug.Log(item.Key);
        scoreInstance.UpdateScore(rowsToMove.Count);

        for (int i = rowsToMove.Count - 1; i >= 0; i--)
        {
            MoveRowsAboveDown(rowsToMove[i]);
        }

    }

    private void MoveRowsAboveDown(int rowNumber)
    {
        List<Transform> cubesAbove = new List<Transform>();
        for (int blockNumber = 0; blockNumber < createBlockInstance.allBlocks.Count; blockNumber++)
        {

            Transform pivot = createBlockInstance.allBlocks[blockNumber].transform.GetChild(0);
            for (int cubeIndex = 0; cubeIndex < pivot.childCount; cubeIndex++)
            {
                Transform cube = pivot.GetChild(cubeIndex);
                if (Mathf.FloorToInt(cube.position.y) > rowNumber)
                {
                    cubesAbove.Add(cube);
                }

            }
        }
        for (int i = 0; i < cubesAbove.Count; i++)
        {
            cubesAbove[i].position += Vector3.down;
        }
    }
    private void RemoveEmptyBlocks()
    {
        for (int blockNumber = 0; blockNumber < createBlockInstance.allBlocks.Count - 1; blockNumber++)
        {

            Transform pivot = createBlockInstance.allBlocks[blockNumber].transform.GetChild(0);
            if (pivot.childCount == 0)
            {
                Destroy(createBlockInstance.allBlocks[blockNumber]);
                createBlockInstance.allBlocks.RemoveAt(blockNumber);
                blockNumber -= 1;
            }



        }
    }
    private void CreateListOfPositions()
    {
        Vector3 rotationOne = new Vector3(0.0f, 0.0f, -90.0f);
        Vector3 rotationTwo = new Vector3(0.0f, 0.0f, -180.0f);
        Vector3 rotationThree = new Vector3(0.0f, 0.0f, -270.0f);
        Vector3 rotationFour = new Vector3(0.0f, 0.0f, 0.0f);

        RotationPositions.Clear();
        RotationPositions.Add(rotationOne);
        RotationPositions.Add(rotationTwo);
        RotationPositions.Add(rotationThree);
        RotationPositions.Add(rotationFour);
    }
}
