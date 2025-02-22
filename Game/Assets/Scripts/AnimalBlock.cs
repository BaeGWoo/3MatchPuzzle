using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimalBlock : MonoBehaviour
{
   [SerializeField] float curRow;
    [SerializeField] float curCol;
    float speed = 50.0f;


    public void SetPosition(float row, float col)
    {
        curRow = row;
        curCol = col;
    }


    public void MoveTo(float rowPadding, float colPadding, float animalRowPadding, float animalColPadding)
    {
        StartCoroutine(BlockMove(rowPadding, colPadding, animalRowPadding, animalColPadding));
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
    }
}
