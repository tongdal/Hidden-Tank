using UnityEngine;

public class Border : MonoBehaviour
{
    float newPosX;
    float maxPosX = 35.3f;
    public void ResizeMap(StageConfig cfg)
    {
        
        transform.position = new Vector3(maxPosX, transform.position.y, transform.position.z);
        switch (cfg.mapSize) {
            case (SizeOption.Small):
                newPosX = transform.position.x - 15f;
                break;
            case (SizeOption.Medium):
                newPosX = transform.position.x - 7.5f;
                break;
            case (SizeOption.Large):
                newPosX = transform.position.x;
                break;
        }
        transform.position = new Vector3( newPosX, transform.position.y, transform.position.z);
    }
}