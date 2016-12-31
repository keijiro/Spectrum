Shader "Spectrum/Floor"
{
    Properties
    {
        _Color("Base Color", Color) = (1, 1, 1)
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
        [Space]
        _MainTex("Pattern Texture", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (1, 1, 1)
        [Space]
        _Origin("Origin", Vector) = (0, 0, 0, 0)
        _Frequency("Frequency", Float) = 1
        _Repeat("Repeat", Int) = 1
        _Speed("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard nolightmap
        #pragma target 3.0

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        fixed3 _Color;
        half _Smoothness;
        half _Metallic;

        sampler2D _MainTex;
        half3 _EmissionColor;

        float3 _Origin;
        float _Frequency;
        float _Repeat;
        float _Speed;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;

            float dist = length(IN.worldPos - _Origin);
            float phase = dist * _Frequency - _Time.y * _Speed;
            half theta = saturate(frac(phase / _Repeat) * _Repeat);
            half br = 0.5 - cos(theta * UNITY_PI * 2) * 0.5;

            half4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Emission = tex.rgb * _EmissionColor * br;
        }

        ENDCG
    }
}
