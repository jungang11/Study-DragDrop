using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHUDText : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 100; // transform ���� �̵��Ÿ�

    [SerializeField]
    private float moveTime = 1.5f;    // �̵� �ð�

    private RectTransform   rectTransform;  // Text UI �̵��� ���� RectTransform
    private TextMeshProUGUI textHUD;        // �ؽ�Ʈ�� ����ϴ� TextMeshProUGUI

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textHUD = GetComponent<TextMeshProUGUI>();
    }

    // ��Ȱ��ȭ �� �� ������Ʈ�� �������� ������ �ٽ� HUD Text�� ���ƿ��� �� ������ ��µƴ� HUD Text���� ��������.
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void Play(string text, Color color, Bounds bounds, float gap = 0.1f)
    {
        textHUD.text = text;
        textHUD.color = color;

        StartCoroutine(OnHUDText(bounds, gap));
    }

    private IEnumerator OnHUDText(Bounds bounds, float gap)
    {
        // WorldToScreenPoint() method�� �̿��� �Ű������� �ִ� ���� ��ǥ�� �������� ȭ�� ���� ��ǥ�� ����.
        Vector2 start = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x, bounds.max.y + gap, bounds.center.z));
        Vector2 end = start + Vector2.up * moveDistance;

        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            // Text UI�� ��ġ ����
            rectTransform.position = Vector2.Lerp(start, end, percent);
            // Text UI�� ���� �� ����
            Color color     =  textHUD.color;
            color.a         =  Mathf.Lerp(1, 0, percent);
            textHUD.color   =  color;

            yield return null;
        }

        Destroy(gameObject);
    }
}
