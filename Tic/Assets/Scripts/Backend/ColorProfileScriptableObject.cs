using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorProfileScriptableObject", order = 1)]
public class ColorProfileScriptableObject : ScriptableObject
{
    [Header("Ground")]
    public Color groundColorA = Color.black;
    public Color groundColorB = Color.white;
    [Header("Walls")]
    public Color wallColorA = Color.black;
    public Color wallColorB = Color.white;
}
