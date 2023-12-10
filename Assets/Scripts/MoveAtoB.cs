using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAtoB : MonoBehaviour
{
    [SerializeField]
    private Vector2[]   wayPoints;      // �̵� ���� �迭
    private int         wayIndex = 0;   // ���� �̵� ���� �ε���

    // Toggle�� Ȱ��ȭ�Ǹ� ����
    private void OnEnable()
    {
        StartCoroutine("OnMoveLoop");
    }

    private void OnDisable()
    {
        StopCoroutine("OnMoveLoop");
    }

    private IEnumerator OnMoveLoop()
    {
        while(true)
        {
            yield return StartCoroutine("OnMoveTo");
        }
    }

    private IEnumerator OnMoveTo()
    {
        float current = 0;
        float percent = 0;

        // ���� ��ġ�� �׻� ���� ��ġ, ��ǥ ��ġ�� wayPoints[]
        Vector2 start = transform.position;
        Vector2 end   = wayPoints[wayIndex];

        // �̵� �ð��� �Ÿ��� ����ؼ� ������ �� �ֵ��� ����( 1 �̵��� 1�� �ҿ� )
        float time = Vector2.Distance(start, end);

        // time �ð����� �÷��̾ start���� end�� �̵�
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        wayIndex = wayIndex < wayPoints.Length - 1 ? wayIndex + 1 : 0;
    }
}
