using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;           // UI�� �ҼӵǾ� �ִ� �ֻ�� UI Transform
    private Transform previousParent;   // �ش� ������Ʈ�� ������ �����ִ� �θ� Transform
    private RectTransform rect;         // UI ��ġ ��� ���� RectTransform
    private CanvasGroup canvasGroup;    // UI�� ���İ��� ��ȣ�ۿ� ��� ���� CanvasGroup

    /*
     * Canvas�� ������ ����ϰų� ������ ������Ʈ�� �ڵ�� ������ ���
     * Awake() ��� Setup() ���� public �޼ҵ带 �̿��� �Ű������� canvas ���� ���� �����Ѵ�.
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
    /// �巡�� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ����
        previousParent = transform.parent;

        // ���� �巡�� ���� UI�� ȭ���� �ֻ�ܿ� ��µǱ� ����
        transform.SetParent(canvas);    // �θ� ������Ʈ�� canvas�� ����
        transform.SetAsLastSibling();   // ���� �տ� ���̵��� Hierachy �� ������ �ڽ� ������ ����

        // �巡�� ������ ������Ʈ�� �ϳ��� �ƴ� �ڽĵ��� ������ ���� ���� �ֱ� ������ CanvasGroup���� ����
        // ���İ��� 0.6���� �����ϰ� ���� �浹ó���� ���� �ʵ��� �Ѵ�.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    ///  �巡�� �ϴ� ���� �� ������ ȣ��
    /// </summary>
    // PointerEventData => ���콺/��ġ�� ��ġ ��, ��ġ ���� ��, �巡�� ����, �巡�� ���� ���, Ŭ�� Ƚ�� ���� ������Ƽ�� ����Ǿ� ����
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    /// <summary>
    /// �巡�� ���� �� 1ȸ ȣ��
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�׸� �����ϸ� �θ� canvas�� �����Ǳ� ������
        // �巡�׸� ������ �� �θ� canvas�̸� ������ ������ �ƴ� ������ ����
        // ����� �ߴٴ� ���̱� ������ �巡�� ������ �ҼӵǾ� �ִ� ������ �������� ������ �̵�
        if(transform.parent == canvas)
        {
            // �������� �ҼӵǾ� �ִ� previousParent�� �ڽ����� �����ϰ� �ش� ��ġ�� �̵�
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }

        // ���İ��� 1�� ����, ���� �浹ó���� �ǵ��� ��
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
