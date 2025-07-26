using UnityEngine;


public class Enemy : MonoBehaviour
{
    public void ChangeTankSize(StageConfig cfg)
    {
        switch (cfg.tankSize) {
            case (SizeOption.Small):
                transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                break;
            case (SizeOption.Medium):
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case (SizeOption.Large):
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                break;
        }
    }
}
