using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageMessage : MonoBehaviour
{
    public Text stageText; // �Ǵ� TMP_Text
    public RectTransform rectTransform;
    float moveDistance = 300f; // ȭ�� �ۿ��� ���� �Ÿ�(px)
    float moveDuration = 0.5f;
    float showDuration = 1f;

    // �޽��� ǥ�� �Լ�
    public void ShowStageMessage(int stageNumber)
    {
        stageText.text = $"Stage {stageNumber}";
        gameObject.SetActive(true);

        // �߾� ��ġ ���
        Vector2 centerPos = Vector2.zero;

        // ���� ��ġ (������ ȭ�� ��)
        Vector2 startPos = centerPos + Vector2.right * moveDistance;
        // �� ��ġ (���� ȭ�� ��)
        Vector2 endPos = centerPos + Vector2.left * moveDistance;

        // �ʱ� ����
        rectTransform.anchoredPosition = startPos;
        // stageText.color = new Color(1, 1, 1, 1);

        // DOTween ������
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(centerPos, moveDuration).SetEase(Ease.OutExpo)) // �����ʡ��߾�
           .AppendInterval(showDuration) // �߾ӿ��� ���
           .Append(rectTransform.DOAnchorPos(endPos, moveDuration).SetEase(Ease.InExpo)) // �߾ӡ����
           .OnComplete(() => gameObject.SetActive(false));
    }
}