using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryLineDraw : MonoBehaviour
{
    public RectTransform targetImage;  // 외곽선을 그릴 UI 이미지의 RectTransform
    private LineRenderer lineRenderer; // 외곽선을 그릴 LineRenderer

    void Start()
    {
        // LineRenderer 컴포넌트 가져오기
        lineRenderer = GetComponent<LineRenderer>();

        // LineRenderer의 속성 설정
        lineRenderer.positionCount = 5;  // 사각형 4개 모서리 + 1
        lineRenderer.loop = true;         // 닫히도록 설정
        lineRenderer.startWidth = 5f;     // 외곽선 두께
        lineRenderer.endWidth = 5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green; // 외곽선 색상
        lineRenderer.endColor = Color.green;

        // 외곽선 그리기
        DrawOutline();
    }

    void DrawOutline()
    {
        // RectTransform을 기준으로 이미지 크기 계산
        Vector3[] corners = new Vector3[4];
        targetImage.GetWorldCorners(corners);

        // 외곽선 그리기 (좌하단, 좌상단, 우상단, 우하단 순서로)
        lineRenderer.SetPosition(0, corners[0]); // 좌하단
        lineRenderer.SetPosition(1, corners[1]); // 좌상단
        lineRenderer.SetPosition(2, corners[2]); // 우상단
        lineRenderer.SetPosition(3, corners[3]); // 우하단
        lineRenderer.SetPosition(4, corners[0]); // 다시 좌하단으로 돌아가기
    }
}
