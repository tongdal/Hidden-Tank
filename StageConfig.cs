using UnityEngine;

public enum HintType { DirectionOnly, DirectionAndDistance }
public enum SizeOption { Small, Medium, Large }

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObject/StageConfig")]
public class StageConfig : ScriptableObject
{
    public HintType hintType;
    public SizeOption mapSize;
    public SizeOption tankSize;
    public SizeOption wallHeight;
}