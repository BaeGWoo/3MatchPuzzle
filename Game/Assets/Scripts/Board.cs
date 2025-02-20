using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject[] BackgroundBlock;
    [SerializeField] int[,] boardMap=new int[9,7];
    [SerializeField] int rowPadding;
    [SerializeField] int colPadding;
    void Start()
    {
        boardMap[2, 5] = 1;
        boardMap[6, 2] = 2;
        boardMap[0, 4] = 3;
        rowPadding = 120;
        colPadding = -125;
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                int temp = boardMap[i, j];

                GameObject tempBlock = Instantiate(BackgroundBlock[temp]);
                tempBlock.transform.SetParent(transform);
                tempBlock.transform.localPosition = new Vector2(j*rowPadding,i*colPadding);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
