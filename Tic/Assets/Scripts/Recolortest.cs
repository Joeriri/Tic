using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Recolortest : MonoBehaviour
{
    //public Material groundMat;
    public TilemapRenderer groundTilemapR;

    public Color startColor;
    public Color targetColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().zKey.wasPressedThisFrame)
        {
            RecolorGround();
        }
    }

    void RecolorGround()
    {
        StartCoroutine(RecolorRoutine());
    }

    IEnumerator RecolorRoutine()
    {
        Material groundMat = groundTilemapR.material;

        float duration = 1f;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            groundMat.SetColor("_BlackColor", Color.Lerp(startColor, targetColor, timer/duration));

            yield return null;
        }
    }
}
