using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;           // ScrollBar
    [SerializeField] private Transform[] circleContents;    // 현재 페이지를 나타내는 원 Image UI들의 Transform
    [SerializeField] private float swipeTime = 0.2f;        // 페이지 swipe 시간
    [SerializeField] private float swipeDistance = 50.0f;   // 페이지가 swipe 되기 위해 움직여야 하는 최소 시간

    private float[] scrollPageValues;           // 각 페이지의 위치 값 [ 0.0 ~ 1.0 ]
    private float valueDistance;                // 각 페이지 사이의 거리
    private int currentPage = 0;                // 현재 Page
    private int maxPage = 0;                    // 최대 Page
    private float startTouchX;                  // Touch 시작 위치
    private float endTouchX;                    // Touch 완료 위치
    private bool isSwipeMode = false;           // 현재 Swipe 상태인지 확인
    private float circleContentScale = 1.6f;    // 현재 페이지의 원 크기 (배율)

    private void Awake()
    {
        // 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
        scrollPageValues = new float[transform.childCount];

        // 스크롤 되는 페이지 사이의 거리
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤 되는 페이지의 각 value 위치 설정 [ 0 <= value <= 1]
        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        maxPage = transform.childCount;
    }

    private void Start()
    {
        // 최초 시작시 0번 페이지
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
        // 현재 Swipe 중이면 반환
        if (isSwipeMode) return;

        #if UNITY_EDITOR
        // 마우스 왼쪽 버튼을 눌렀을 때 한번
        if (Input.GetMouseButtonDown(0))
        {
            // 터치 시작 지점 (Swipe 방향 구분)
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 터치 종료 지점
            endTouchX = Input.mousePosition.x;

            UpdateSwipe();
        }
        #endif

        #if UNITY_ANDROID

        #endif
    }

    private void UpdateSwipe()
    {
        // swipe 거리를 최소 swipeDistance 만큼 하지 않을 경우
        if(Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // 원래 페이지로 Swipe해서 돌아간다
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        // Swipe 방향. endTouchX 가 더 클 경우 (오른쪽에서 왼쪽으로 InputPoint 가 이동)
        bool isLeft = startTouchX < endTouchX ? true : false;

        // 왼쪽으로 이동할 때
        if (isLeft)
        {
            // 현재 페이지가 왼쪽 끝이면 종료
            if (currentPage == 0) return;

            // 왼쪽으로 이동을 위해 현재 페이지 1 감소
            currentPage--;
        }
        // 오른쪽으로 이동할 때
        else
        {
            // 현재 페이지가 오른쪽 끝이면 종료
            if (currentPage == maxPage - 1) return;
        
            // 오른쪽으로 이동을 위해 현재 페이지를 1 증가
            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    /// <summary>
    /// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
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
        // 아래에 배치된 페이지 버튼 크기, 색상 제어 (현재 머물고 있는 페이지의 버튼만 수정)
        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.white;

            // 페이지의 절반을 넘어가면 현재 페이지 원을 바꾸도록
            if( scrollBar.value < scrollPageValues[i] + (valueDistance / 2) &&
                scrollBar.value > scrollPageValues[i] - (valueDistance / 2))
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.black;
            }
        }
    }
}
