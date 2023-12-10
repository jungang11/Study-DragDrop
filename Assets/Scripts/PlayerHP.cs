using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;  // ������ Text UI�� ��ġ�Ǵ� �θ��� Transform

    [SerializeField]
    private GameObject hudTextPrefab;   // ���忡 Text UI�� ����ϴ� ������

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
            // 0.1�� ~ 1�� ������ �ð����� ��� �� ChangeHPDate Method ����
            float time = Random.Range(0.1f, 1.0f);
            yield return new WaitForSeconds(time);

            // 0 : ü�� ����, 1 : ü�� ����
            int type = Random.Range(0, 2);

            // ����, ���ҵǴ� ü�� ��ġ
            string text = Random.Range(10, 1000).ToString();

            // Text Color. ü�� ���� : red, ü�� ���� : green
            Color color = type == 0 ? Color.red : Color.green;

            SpawnHUDText(text, color);
        }
    }

    private void SpawnHUDText(string text, Color color)
    {
        // hudTextPrefab ���纻 ���� �� parentTransform�� �����ڽ����� ����
        GameObject clone = Instantiate(hudTextPrefab);
        clone.transform.SetParent(parentTransform);

        // ���� �ؽ�Ʈ�� ��µ� ������Ʈ�� Bounds ����
        Bounds bounds  = GetComponent<Collider2D>().bounds;
        // �ؽ�Ʈ ��� �� �ִϸ��̼� ���
        clone.GetComponent<UIHUDText>().Play(text, color, bounds);
    }
}
