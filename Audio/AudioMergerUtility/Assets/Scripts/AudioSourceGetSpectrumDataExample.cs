using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceGetSpectrumDataExample : MonoBehaviour
{
    [SerializeField] private float beatBias;
    [SerializeField] private float spectrumMultiplier = 1000;
    
    private float _lowSpectrumFloat;
    
    void Update()
    {
        float[] spectrum = new float[512];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        var spec = spectrum[0] * spectrumMultiplier;

        if (spec >= beatBias)
        {
            _lowSpectrumFloat = spec;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            fontSize = 50,
            normal =
            {
                textColor = Color.white,
            }
        };

        GUIStyle boxStyle = new GUIStyle
        {
            normal =
            {
                background = MakeTex(2,2,Color.white)
            }
        };
        
        GUI.Label(new Rect(50,50,100,40), "" + _lowSpectrumFloat, style);
        GUI.Box(new Rect(50,150,_lowSpectrumFloat * 5,10), "", boxStyle);
    }
    
    private Texture2D MakeTex( int width, int height, Color col )
    {
        Color[] pix = new Color[width * height];
        for( int i = 0; i < pix.Length; ++i )
        {
            pix[ i ] = col;
        }
        Texture2D result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply();
        return result;
    }
}
