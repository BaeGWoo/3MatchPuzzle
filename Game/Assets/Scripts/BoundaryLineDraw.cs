using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryLineDraw : MonoBehaviour
{
    public RectTransform targetImage;  // �ܰ����� �׸� UI �̹����� RectTransform
    private LineRenderer lineRenderer; // �ܰ����� �׸� LineRenderer

    void Start()
    {
        // LineRenderer ������Ʈ ��������
        lineRenderer = GetComponent<LineRenderer>();

        // LineRenderer�� �Ӽ� ����
        lineRenderer.positionCount = 5;  // �簢�� 4�� �𼭸� + 1
        lineRenderer.loop = true;         // �������� ����
        lineRenderer.startWidth = 5f;     // �ܰ��� �β�
        lineRenderer.endWidth = 5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green; // �ܰ��� ����
        lineRenderer.endColor = Color.green;

        // �ܰ��� �׸���
        DrawOutline();
    }

    void DrawOutline()
    {
        // RectTransform�� �������� �̹��� ũ�� ���
        Vector3[] corners = new Vector3[4];
        targetImage.GetWorldCorners(corners);

        // �ܰ��� �׸��� (���ϴ�, �»��, ����, ���ϴ� ������)
        lineRenderer.SetPosition(0, corners[0]); // ���ϴ�
        lineRenderer.SetPosition(1, corners[1]); // �»��
        lineRenderer.SetPosition(2, corners[2]); // ����
        lineRenderer.SetPosition(3, corners[3]); // ���ϴ�
        lineRenderer.SetPosition(4, corners[0]); // �ٽ� ���ϴ����� ���ư���
    }
}
