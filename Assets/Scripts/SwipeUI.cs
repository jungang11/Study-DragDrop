using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;           // ScrollBar
    [SerializeField] private Transform[] circleContents;    // ���� �������� ��Ÿ���� �� Image UI���� Transform
    [SerializeField] private float swipeTime = 0.2f;        // ������ swipe �ð�
    [SerializeField] private float swipeDistance = 50.0f;   // �������� swipe �Ǳ� ���� �������� �ϴ� �ּ� �ð�

    private float[] scrollPageValues;           // �� �������� ��ġ �� [ 0.0 ~ 1.0 ]
    private float valueDistance;                // �� ������ ������ �Ÿ�
    private int currentPage = 0;                // ���� Page
    private int maxPage = 0;                    // �ִ� Page
    private float startTouchX;                  // Touch ���� ��ġ
    private float endTouchX;                    // Touch �Ϸ� ��ġ
    private bool isSwipeMode = false;           // ���� Swipe �������� Ȯ��
    private float circleContentScale = 1.6f;    // ���� �������� �� ũ�� (����)

    private void Awake()
    {
        // ��ũ�� �Ǵ� �������� �� value ���� �����ϴ� �迭 �޸� �Ҵ�
        scrollPageValues = new float[transform.childCount];

        // ��ũ�� �Ǵ� ������ ������ �Ÿ�
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // ��ũ�� �Ǵ� �������� �� value ��ġ ���� [ 0 <= value <= 1]
        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        maxPage = transform.childCount;
    }

    private void Start()
    {
        // ���� ���۽� 0�� ������
        SetScrollBarValue(0);
    }

    private void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[currentPage];
    }

    private void Update()
    {
        UpdateInput();

        UpdateCircleContent();
    }

    private void UpdateInput()
    {
        // ���� Swipe ���̸� ��ȯ
        if (isSwipeMode) return;

        #if UNITY_EDITOR
        // ���콺 ���� ��ư�� ������ �� �ѹ�
        if (Input.GetMouseButtonDown(0))
        {
            // ��ġ ���� ���� (Swipe ���� ����)
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // ��ġ ���� ����
            endTouchX = Input.mousePosition.x;

            UpdateSwipe();
        }
        #endif

        #if UNITY_ANDROID

        #endif
    }

    private void UpdateSwipe()
    {
        // swipe �Ÿ��� �ּ� swipeDistance ��ŭ ���� ���� ���
        if(Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // ���� �������� Swipe�ؼ� ���ư���
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        // Swipe ����. endTouchX �� �� Ŭ ��� (�����ʿ��� �������� InputPoint �� �̵�)
        bool isLeft = startTouchX < endTouchX ? true : false;

        // �������� �̵��� ��
        if (isLeft)
        {
            // ���� �������� ���� ���̸� ����
            if (currentPage == 0) return;

            // �������� �̵��� ���� ���� ������ 1 ����
            currentPage--;
        }
        // ���������� �̵��� ��
        else
        {
            // ���� �������� ������ ���̸� ����
            if (currentPage == maxPage - 1) return;
        
            // ���������� �̵��� ���� ���� �������� 1 ����
            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    /// <summary>
    /// �������� �� �� ������ �ѱ�� Swipe ȿ�� ���
    /// </summary>
    private IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollBar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }
        isSwipeMode = false;
    }

    private void UpdateCircleContent()
    {
        // �Ʒ��� ��ġ�� ������ ��ư ũ��, ���� ���� (���� �ӹ��� �ִ� �������� ��ư�� ����)
        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.white;

            // �������� ������ �Ѿ�� ���� ������ ���� �ٲٵ���
            if( scrollBar.value < scrollPageValues[i] + (valueDistance / 2) &&
                scrollBar.value > scrollPageValues[i] - (valueDistance / 2))
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.black;
            }
        }
    }
}
