using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject[] BackgroundBlock;
    [SerializeField] Sprite[] AnimalBlock;
    [SerializeField] GameObject AnimalPrefab;
    [SerializeField] int[,] boardMap=new int[10,7];

    [SerializeField] int row;
    [SerializeField] int col;

    [SerializeField] int rowPadding;
    [SerializeField] int colPadding;
    [SerializeField] int animalRowPadding;
    [SerializeField] int animalColPadding;
    [SerializeField] List<int> animalNumbers = new List<int>();
    public int[,] boardTable = new int[10, 7];
    public GameObject[,] dynamicBoard=new GameObject[10, 7];

    public enum BlockType
    {
        BASIC,
        MISSION1,
        MISSION2,
        BREAKABLE,
        NONE
    }


    void Start()
    {
        row = 10;
        col = 7;

        for(int i = 0; i < AnimalBlock.Length; i++)
        {
            animalNumbers.Add(i);
        }

        boardMap[2, 5] = 1;
        boardMap[6, 2] = 2;
        boardMap[1, 4] = 3;

        rowPadding = 120;
        colPadding = -125;

        animalRowPadding = 15;
        animalColPadding = -10;
        

        for (int i = 1; i < 10; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                int temp = boardMap[i, j];

                GameObject tempBlock = Instantiate(BackgroundBlock[temp]);
                tempBlock.transform.SetParent(transform);
                tempBlock.transform.localPosition = new Vector2(j*rowPadding,i*colPadding);
            }
        }


        int count = 0;

        for (int i = 0; i < 7; i++)
        {
            int animalNum=animalNumbers[Random.Range(0, animalNumbers.Count)];
            if (i > 0 && boardMap[0, i - 1] == animalNum)
                count++;
            else count = 0;

            if (count >= 3)
            {
                int temp = animalNum;
                animalNumbers.Remove(animalNum);
                animalNum= animalNumbers[Random.Range(0, animalNumbers.Count)];
                animalNumbers.Add(temp);
                count = 0;
            }

            //if (boardTable[1, i] == 0 && boardMap[1, i] <= 2)
            //{
                CreateNewBlock(0 , i,animalNum);
                //BlockMoveCheck(0, i);
            //}
        }

        boardFulling();
    }
    private void Update()
    {
       // int count = 0;
       // for (int i = 0; i < 7; i++)
       // {
       //     int animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
       //     if (i > 0 && boardMap[0, i - 1] == animalNum)
       //         count++;
       //     else count = 0;
       //
       //     if (count >= 3)
       //     {
       //         int temp = animalNum;
       //         animalNumbers.Remove(animalNum);
       //         animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
       //         animalNumbers.Add(temp);
       //         count = 0;
       //     }
       //
       //     if (boardTable[1, i] == 0 && boardMap[1,i]<=2)
       //     {
       //         CreateNewBlock(0, i, animalNum);
       //         BlockMoveCheck(0, i);
       //     }
       // }

        
    }

   

    public bool CheckTable(int row, int col)
    {
        bool result=false;

        if (boardTable[row, col] == 0)
            result = true;


        return result;
    }

    public void BlockMoveCheck(int row, int col)
    {
       bool isMoving = false;
       int x=row;
       int y=col;

        if(BlockDownCheck(row, col))
        {
            isMoving = true;
            x = row + 1;
            y = col;
        }

        else if(BlockLeftCheck(row, col))
        {
            isMoving = true;
            x = row + 1;
            y = col - 1;
        }

        else if(BlockRightCheck(row, col))
        {
            isMoving = true;
            x = row + 1;
            y = col + 1;
        }

       

        if (isMoving)
        {
            boardTable[row, col] = 0;
            boardTable[x, y] = 1;
            dynamicBoard[x, y] = dynamicBoard[row, col];
        
            MoveBlock(row, col, x, y);
            BlockMoveCheck(x, y);
        }
    }


    public void boardFulling()
    {
        
        bool result = false;
        int pivotrow = -1;
        int pivotcol = -1;

        do
        {
            for (int i = row - 1; i > 0; i--)
            {
                for (int j = 0; j < col; j++)
                {
                    if (dynamicBoard[i, j] != null)
                        continue;

                    if (BlockDownCheck(i, j))
                    {
                        Debug.Log("현재 (" + i + " , " + j + " ) => Down");
                        result = true;
                        pivotrow = i - 1;
                        pivotcol = j;
                    }

                   
                    if (pivotrow == -1)
                        continue;

                    //pivotrow,pivotcol 위치의 블록 i,j로 이동
                    if (pivotrow == 0)
                        dynamicBoard[pivotrow, pivotcol].SetActive(true);

                    dynamicBoard[pivotrow, pivotcol].GetComponent<AnimalBlock>().MoveTo
                        (j * rowPadding + animalRowPadding, i * colPadding + animalColPadding);
                    dynamicBoard[pivotrow, pivotcol] = dynamicBoard[i, j];
                    dynamicBoard[i, j] = null;

                    if (pivotrow == 0)
                    {
                        int animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
                        CreateNewBlock(0, j, animalNum);
                    }
                }
            }
        } while (result);


        do
        {
            for (int i = row - 1; i > 0; i--)
            {
                for (int j = 0; j < col; j++)
                {
                    if (dynamicBoard[i, j] != null)
                        continue;

                   if (BlockLeftCheck(i, j))
                    {
                        Debug.Log("현재 (" + i + " , " + j + " ) => Left");
                        result = true;
                        pivotrow = i - 1;
                        pivotcol = j - 1;
                    }

                   
                    if (pivotrow == -1)
                        continue;

                    //pivotrow,pivotcol 위치의 블록 i,j로 이동
                    if (pivotrow == 0)
                        dynamicBoard[pivotrow, pivotcol].SetActive(true);

                    dynamicBoard[pivotrow, pivotcol].GetComponent<AnimalBlock>().MoveTo
                        (j * rowPadding + animalRowPadding, i * colPadding + animalColPadding);
                    dynamicBoard[pivotrow, pivotcol] = dynamicBoard[i, j];
                    dynamicBoard[i, j] = null;

                    if (pivotrow == 0)
                    {
                        int animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
                        CreateNewBlock(0, j, animalNum);
                    }
                }
            }
        } while (result);

        do
        {
            for (int i = row - 1; i > 0; i--)
            {
                for (int j = 0; j < col; j++)
                {
                    if (dynamicBoard[i, j] != null)
                        continue;

                   if (BlockRightCheck(i, j))
                    {
                        Debug.Log("현재 (" + i + " , " + j + " ) => Right");
                        result = true;
                        pivotrow = i - 1;
                        pivotcol = j + 1;
                    }
                    if (pivotrow == -1)
                        continue;

                    //pivotrow,pivotcol 위치의 블록 i,j로 이동
                    if (pivotrow == 0)
                        dynamicBoard[pivotrow, pivotcol].SetActive(true);

                    dynamicBoard[pivotrow, pivotcol].GetComponent<AnimalBlock>().MoveTo
                        (j * rowPadding + animalRowPadding, i * colPadding + animalColPadding);
                    dynamicBoard[pivotrow, pivotcol] = dynamicBoard[i, j];
                    dynamicBoard[i, j] = null;

                    if (pivotrow == 0)
                    {
                        int animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
                        CreateNewBlock(0, j, animalNum);
                    }
                }
            }
        } while (result);

    }


    public bool BlockDownCheck(int row, int col)
    {
        if (row - 1 >= 0)
        {
            if (boardMap[row - 1, col] <= 2)
            {
                if (dynamicBoard[row - 1, col] != null)
                {
                    return true;
                }
            }
        }

        return false;      
    }

    public bool BlockLeftCheck(int row, int col)
    {
        if (row - 1 >= 0&&col-1>=0)
        {
            if (boardMap[row - 1, col-1] <= 2)
            {
                if (dynamicBoard[row - 1, col-1] != null)
                {
                    return true;
                }
            }
        }

        return false;

    }

    public bool BlockRightCheck(int row, int col)
    {
        if (row - 1 >= 0 && col + 1 <this.col)
        {
            if (boardMap[row - 1, col + 1] <= 2)
            {
                if (dynamicBoard[row - 1, col + 1] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }



        public void CreateNewBlock(int row, int col, int animalNum)
    {
        GameObject newBlock = Instantiate(AnimalPrefab);
        newBlock.transform.SetParent(transform);
        newBlock.transform.localPosition= new Vector2(col * rowPadding + animalRowPadding, row * colPadding + animalColPadding);
        newBlock.transform.GetComponent<Image>().sprite = AnimalBlock[animalNum];
        newBlock.GetComponent<AnimalBlock>().SetPosition(row,col);
        newBlock.SetActive(false);
        dynamicBoard[row, col] = newBlock;

    }

    public void MoveBlock(int row,int col, int x,int y)
    {
        dynamicBoard[row, col].GetComponent<AnimalBlock>().SetPosition(x, y);
        //dynamicBoard[row,col].GetComponent<AnimalBlock>().MoveTo
            //(rowPadding , colPadding, animalRowPadding,animalColPadding);
        dynamicBoard[row, col] = null;
    }
}
