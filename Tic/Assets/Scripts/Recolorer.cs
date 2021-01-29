using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class Recolorer : MonoBehaviour
{
    // tilemap references
    public TilemapRenderer groundTilemapR;
    public TilemapRenderer wallTilemapR;

    public ColorProfileScriptableObject[] worldColorProfiles;
    
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().zKey.wasPressedThisFrame)
        {
            StartWorldRecolor(worldColorProfiles[1]);
        }
    }

    public void SetWorldColors(ColorProfileScriptableObject targetWorldColorProfile)
    {
        // get tilemap materials
        Material groundMat = groundTilemapR.material;
        Material wallMat = wallTilemapR.material;
        // set material colors
        groundMat.SetColor("_BlackColor", targetWorldColorProfile.groundColorA);
        groundMat.SetColor("_WhiteColor", targetWorldColorProfile.groundColorB);
        wallMat.SetColor("_BlackColor", targetWorldColorProfile.wallColorA);
        wallMat.SetColor("_WhiteColor", targetWorldColorProfile.wallColorB);
    }

    public void StartWorldRecolor(ColorProfileScriptableObject targetWorldColorProfile)
    {
        StartCoroutine(RecolorWorldRoutine(targetWorldColorProfile));
    }

    IEnumerator RecolorWorldRoutine(ColorProfileScriptableObject targetWorldColorProfile)
    {
        // get tilemap materials
        Material groundMat = groundTilemapR.material;
        Material wallMat = wallTilemapR.material;

        // get current colors
        Color startGroundColorA = groundMat.GetColor("_BlackColor");
        Color startGroundColorB = groundMat.GetColor("_WhiteColor");
        Color startWallColorA = wallMat.GetColor("_BlackColor");
        Color startWallColorB = wallMat.GetColor("_WhiteColor");

        float duration = 0.5f;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float prc = timer / duration;

            groundMat.SetColor("_BlackColor", Color.Lerp(startGroundColorA, targetWorldColorProfile.groundColorA, prc));
            groundMat.SetColor("_WhiteColor", Color.Lerp(startGroundColorB, targetWorldColorProfile.groundColorB, prc));
            wallMat.SetColor("_BlackColor", Color.Lerp(startWallColorA, targetWorldColorProfile.wallColorA, prc));
            wallMat.SetColor("_WhiteColor", Color.Lerp(startWallColorB, targetWorldColorProfile.wallColorB, prc));

            yield return null;
        }
    }
}
