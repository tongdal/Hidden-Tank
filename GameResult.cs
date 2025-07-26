using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject[] resultImgs;
    public void Lose()
    {
        resultImgs[0].SetActive(true);
    }

    public void Win()
    {
        resultImgs[1].SetActive(true);
    }
}
