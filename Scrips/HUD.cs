using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { bullet, stage, gold, power };
    public InfoType infoType;

    Text text;
    Slider slider;
    // Update is called once per frame
    private void Awake()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }
    void LateUpdate()
    {
        switch (infoType) {
            case InfoType.bullet:
                text.text = string.Format("{0:F0}", GameManager.instance.curBullet);
                break;
            case InfoType.stage:
                if (GameManager.instance.curStage + 1 < GameManager.instance.stages.Count)
                    text.text = string.Format("Stage {0:F0}", GameManager.instance.curStage+1);
                break;
            case InfoType.gold:
                text.text = string.Format("{0:F0}", GameManager.instance.curGold);
                break;
            case InfoType.power:
                float powerPercent = (GameManager.instance.player.currentPower - GameManager.instance.player.minPower) / (GameManager.instance.player.maxPower - GameManager.instance.player.minPower);
                slider.value = powerPercent;
                break;
        }
    }
}

