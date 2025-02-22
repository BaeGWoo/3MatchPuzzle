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

    [SerializeField] int rowPadding;
    [SerializeField] int colPadding;
    [SerializeField] int animalRowPadding;
    [SerializeField] int animalColPadding;
    [SerializeField] List<int> animalNumbers = new List<int>();
    public int[,] boardTable = new int[10, 7];
    public GameObject[,] dynamicBoard=new GameObject[10, 7];
    void Start()
    {
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

            if (boardTable[1, i] == 0 && boardMap[1, i] <= 2)
            {
                CreateNewBlock(0 , i,animalNum);
                BlockMoveCheck(0, i);
            }
        }
    }
    private void Update()
    {
        int count = 0;
        for (int i = 0; i < 7; i++)
        {
            int animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
            if (i > 0 && boardMap[0, i - 1] == animalNum)
                count++;
            else count = 0;

            if (count >= 3)
            {
                int temp = animalNum;
                animalNumbers.Remove(animalNum);
                animalNum = animalNumbers[Random.Range(0, animalNumbers.Count)];
                animalNumbers.Add(temp);
                count = 0;
            }

            if (boardTable[1, i] == 0 && boardMap[1,i]<=2)
            {
                CreateNewBlock(0, i, animalNum);
                BlockMoveCheck(0, i);
            }
        }

        
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
        BlockLeftCheck(row, col);
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
    public bool BlockDownCheck(int row, int col)
    {
        if (row + 1 < 10)
        {
            if (boardMap[row + 1, col] <= 2)
            {
                if (boardTable[row + 1, col] == 0)
                {
                    return true;
                }
            }

        }
        return false;      
    }

    public bool BlockLeftCheck(int row, int col)
    {
        if (col - 1 > 0)
        {
            if (boardMap[row, col - 1] <= 2)
            {
                if (row + 1 < 10)
                {
                    if (boardTable[row + 1, col - 1] == 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
       
    }

    public bool BlockRightCheck(int row, int col)
    {
        if (col + 1 < 7)
        {
            if (boardMap[row, col + 1] <= 2)
            {
                if (row + 1 < 10)
                {
                    if (boardTable[row + 1, col + 1] == 0)
                    {
                        return true;
                    }
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
        dynamicBoard[row, col] = newBlock;

    }

    public void MoveBlock(int row,int col, int x,int y)
    {
        dynamicBoard[row, col].GetComponent<AnimalBlock>().SetPosition(x, y);
        dynamicBoard[row,col].GetComponent<AnimalBlock>().MoveTo
            (rowPadding , colPadding, animalRowPadding,animalColPadding);
        dynamicBoard[row, col] = null;
    }
}
