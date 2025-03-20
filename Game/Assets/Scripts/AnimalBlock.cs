using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimalBlock : MonoBehaviour
{
   [SerializeField] float curRow;
    [SerializeField] float curCol;
    [SerializeField] bool DownMoving = false;
    float speed = 150.0f;


    public void SetPosition(float row, float col)
    {
        curRow = row;
        curCol = col;
    }


    public void Move(float rowPadding, float colPadding, float animalRowPadding, float animalColPadding)
    {
        DownMoving = true;
        //StartCoroutine(BlockMove(rowPadding, colPadding, animalRowPadding, animalColPadding));
        
    }

    public void MoveTo(int row, int col)
    {
        StartCoroutine(BlockMoveTo(row, col));
    }

    IEnumerator BlockMove(float rowPadding, float colPadding, float animalRowPadding, float animalColPadding)
    {
        Vector2 targetPosition = new Vector2(curCol*rowPadding+animalRowPadding, curRow*colPadding+animalColPadding);
        while(Vector2.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);          
            yield return null;
            targetPosition = new Vector2(curCol * rowPadding + animalRowPadding, curRow * colPadding + animalColPadding);

        }

        transform.localPosition = targetPosition;
        DownMoving = false;
    }

    IEnumerator BlockMoveTo(int row, int col)
    {
        Vector2 targetPosition = new Vector2(row,col);
        while (Vector2.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = targetPosition;
        //DownMoving = false;
    }

    public bool DownMoveCheck()
    {
        return DownMoving;
    }
}
