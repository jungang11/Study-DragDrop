using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHUDText : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 100; // transform 기준 이동거리

    [SerializeField]
    private float moveTime = 1.5f;    // 이동 시간

    private RectTransform   rectTransform;  // Text UI 이동을 위한 RectTransform
    private TextMeshProUGUI textHUD;        // 텍스트를 출력하는 TextMeshProUGUI

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textHUD = GetComponent<TextMeshProUGUI>();
    }

    // 비활성화 될 때 오브젝트가 삭제되지 않으면 다시 HUD Text로 돌아왔을 때 기존에 출력됐던 HUD Text들이 남아있음.
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
        // WorldToScreenPoint() method를 이용해 매개변수에 있는 월드 좌표를 바탕으로 화면 상의 좌표를 구함.
        Vector2 start = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x, bounds.max.y + gap, bounds.center.z));
        Vector2 end = start + Vector2.up * moveDistance;

        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            // Text UI의 위치 제어
            rectTransform.position = Vector2.Lerp(start, end, percent);
            // Text UI의 알파 값 제어
            Color color     =  textHUD.color;
            color.a         =  Mathf.Lerp(1, 0, percent);
            textHUD.color   =  color;

            yield return null;
        }

        Destroy(gameObject);
    }
}
