using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;  // 생성한 Text UI가 배치되는 부모의 Transform

    [SerializeField]
    private GameObject hudTextPrefab;   // 월드에 Text UI를 출력하는 프리팹

    private void OnEnable()
    {
        StartCoroutine("UpdateHPLoop");
    }

    private void DisEnable()
    {
        StopCoroutine("UpdateHPLoop");
    }

    private IEnumerator UpdateHPLoop()
    {
        while (true)
        {
            // 0.1초 ~ 1초 사이의 시간동안 대기 후 ChangeHPDate Method 실행
            float time = Random.Range(0.1f, 1.0f);
            yield return new WaitForSeconds(time);

            // 0 : 체력 감소, 1 : 체력 증가
            int type = Random.Range(0, 2);

            // 증가, 감소되는 체력 수치
            string text = Random.Range(10, 1000).ToString();

            // Text Color. 체력 감소 : red, 체력 증가 : green
            Color color = type == 0 ? Color.red : Color.green;

            SpawnHUDText(text, color);
        }
    }

    private void SpawnHUDText(string text, Color color)
    {
        // hudTextPrefab 복사본 생성 후 parentTransform의 하위자식으로 변경
        GameObject clone = Instantiate(hudTextPrefab);
        clone.transform.SetParent(parentTransform);

        // 실제 텍스트가 출력될 오브젝트의 Bounds 정보
        Bounds bounds  = GetComponent<Collider2D>().bounds;
        // 텍스트 출력 및 애니메이션 재생
        clone.GetComponent<UIHUDText>().Play(text, color, bounds);
    }
}
