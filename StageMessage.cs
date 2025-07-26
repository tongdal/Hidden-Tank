using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageMessage : MonoBehaviour
{
    public Text stageText; // 또는 TMP_Text
    public RectTransform rectTransform;
    float moveDistance = 300f; // 화면 밖에서 들어올 거리(px)
    float moveDuration = 0.5f;
    float showDuration = 1f;

    // 메시지 표시 함수
    public void ShowStageMessage(int stageNumber)
    {
        stageText.text = $"Stage {stageNumber}";
        gameObject.SetActive(true);

        // 중앙 위치 계산
        Vector2 centerPos = Vector2.zero;

        // 시작 위치 (오른쪽 화면 밖)
        Vector2 startPos = centerPos + Vector2.right * moveDistance;
        // 끝 위치 (왼쪽 화면 밖)
        Vector2 endPos = centerPos + Vector2.left * moveDistance;

        // 초기 세팅
        rectTransform.anchoredPosition = startPos;
        // stageText.color = new Color(1, 1, 1, 1);

        // DOTween 시퀀스
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(centerPos, moveDuration).SetEase(Ease.OutExpo)) // 오른쪽→중앙
           .AppendInterval(showDuration) // 중앙에서 대기
           .Append(rectTransform.DOAnchorPos(endPos, moveDuration).SetEase(Ease.InExpo)) // 중앙→왼쪽
           .OnComplete(() => gameObject.SetActive(false));
    }
}