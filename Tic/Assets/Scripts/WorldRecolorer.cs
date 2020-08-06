using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRecolorer : MonoBehaviour
{
    [Header("Active Materials")]
    public Material wallMat;
    public Material floorMat;

    [Header("Reference Materials")]
    public Material[] wallMaterials;
    public Material[] floorMaterials;

    public void BlendWorldColor(int startWorld, int targetWorld, float lerp)
    {
        startWorld -= 1;
        targetWorld -= 1;
        // walls
        wallMat.SetColor("_BlackColor", Color.Lerp(wallMaterials[startWorld].GetColor("_BlackColor"), wallMaterials[targetWorld].GetColor("_BlackColor"), lerp));
        wallMat.SetColor("_WhiteColor", Color.Lerp(wallMaterials[startWorld].GetColor("_WhiteColor"), wallMaterials[targetWorld].GetColor("_WhiteColor"), lerp));
        // floor
        floorMat.SetColor("_BlackColor", Color.Lerp(floorMaterials[startWorld].GetColor("_BlackColor"), floorMaterials[targetWorld].GetColor("_BlackColor"), lerp));
        floorMat.SetColor("_WhiteColor", Color.Lerp(floorMaterials[startWorld].GetColor("_WhiteColor"), floorMaterials[targetWorld].GetColor("_WhiteColor"), lerp));

        Debug.Log("blend color done");
    }
}
