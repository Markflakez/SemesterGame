using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurScript : MonoBehaviour
{
    public int blurSize = 3;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture rt = RenderTexture.GetTemporary(src.width, src.height);
        Graphics.Blit(src, rt);
        RenderTexture.ReleaseTemporary(rt);

        Texture2D tex = new Texture2D(src.width, src.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
        tex.Apply();

        Color[] pixels = tex.GetPixels();
        Color[] result = new Color[pixels.Length];

        for (int y = 0; y < src.height; y++)
        {
            for (int x = 0; x < src.width; x++)
            {
                int index = y * src.width + x;
                Color pixel = pixels[index];

                float r = 0;
                float g = 0;
                float b = 0;

                for (int i = -blurSize; i <= blurSize; i++)
                {
                    for (int j = -blurSize; j <= blurSize; j++)
                    {
                        int xCoord = x + i;
                        int yCoord = y + j;

                        if (xCoord >= 0 && xCoord < src.width && yCoord >= 0 && yCoord < src.height)
                        {
                            int newIndex = yCoord * src.width + xCoord;
                            Color newPixel = pixels[newIndex];
                            r += newPixel.r;
                            g += newPixel.g;
                            b += newPixel.b;
                        }
                    }
                }

                int numPixels = (blurSize * 2 + 1) * (blurSize * 2 + 1);
                r /= numPixels;
                g /= numPixels;
                b /= numPixels;

                result[index] = new Color(r, g, b, pixel.a);
            }
        }

        tex.SetPixels(result);
        tex.Apply();

        Graphics.Blit(tex, dest);
    }
}
