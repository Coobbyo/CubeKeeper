using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconGenerator : MonoBehaviour
{
    [SerializeField] private string Prefix = "Test";
    [SerializeField] private string pathFoulder = "Icons";

    private Camera cam;
    private string fullPath = ""; //DO NOT forget to fill this out

    private void Start()
    {
        fullPath = fullPath + pathFoulder + Prefix;
    }

    private void TakeScreenshot()
    {
        if(cam == null)
            cam = GetComponent<Camera>();
        
        RenderTexture rt = new RenderTexture(256, 256, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, true);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        cam.targetTexture = null;

        if(Application.isEditor)
            DestroyImmediate(rt);
        else
            Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
#if UNITY_EDITOR
    //AssetDatabase.Refresh();
#endif
    }
}
