using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;           // UI가 소속되어 있는 최상단 UI Transform
    private Transform previousParent;   // 해당 오브젝트가 직전에 속해있던 부모 Transform
    private RectTransform rect;         // UI 위치 제어를 위한 RectTransform
    private CanvasGroup canvasGroup;    // UI의 알파값과 상호작용 제어를 위한 CanvasGroup

    /*
     * Canvas를 여러개 사용하거나 아이템 오브젝트를 코드로 생성할 경우
     * Awake() 대신 Setup() 등의 public 메소드를 이용해 매개변수로 canvas 변수 값을 설정한다.
     */
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvas = FindObjectOfType<Canvas>().transform;
    }

    /// <summary>
    /// 드래그 시작할 때 1회 호출
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 직전에 소속되어 있던 부모 Transform 정보
        previousParent = transform.parent;

        // 현재 드래그 중인 UI가 화면의 최상단에 출력되기 위해
        transform.SetParent(canvas);    // 부모 오브젝트를 canvas로 설정
        transform.SetAsLastSibling();   // 가장 앞에 보이도록 Hierachy 상 마지막 자식 순서로 설정

        // 드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 있기 때문에 CanvasGroup으로 통제
        // 알파값을 0.6으로 설정하고 광선 충돌처리가 되지 않도록 한다.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    ///  드래그 하는 동안 매 프레임 호출
    /// </summary>
    // PointerEventData => 마우스/터치의 위치 값, 위치 변위 값, 드래그 여부, 드래그 중인 대상, 클릭 횟수 등의 프로퍼티가 선언되어 있음
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    /// <summary>
    /// 드래그 끝날 때 1회 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그를 시작하면 부모가 canvas로 설정되기 때문에
        // 드래그를 종료할 때 부모가 canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
        // 드롭을 했다는 뜻이기 때문에 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
        if(transform.parent == canvas)
        {
            // 마지막에 소속되어 있던 previousParent의 자식으로 설정하고 해당 위치로 이동
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }

        // 알파값은 1로 설정, 광선 충돌처리가 되도록 함
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
