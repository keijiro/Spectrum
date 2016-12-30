Shader "Test/Dummy"
{
    Properties
    {
        _Albedo("Albedo", Color) = (1, 1, 1, 1)
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0

        [Space]
        [HDR] _Emission("Emission", Color) = (1, 1, 1)

        [Space]
        _Voxelize("Voxelize", Range(0, 0.2)) = 0.1
        _Cutoff("Cutoff", Range(0, 1)) = 0.1
    }

    CGINCLUDE

    half4 _Albedo;
    half _Smoothness;
    half _Metallic;
    half3 _Emission;
    float _Voxelize;
    half _Cutoff;

    struct Input
    {
        float3 worldPos;
        float facing : VFACE;
    };

    float UVRandom(float2 uv)
    {
        return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
    }

    void vert(inout appdata_full data)
    {
        float4 wp = mul(unity_ObjectToWorld, data.vertex);
        float3 vp = floor(wp.xyz / max(_Voxelize, 0.001)) * _Voxelize;
        vp = lerp(wp.xyz, vp, saturate(_Voxelize / 0.01));
        data.vertex.xyz = mul(unity_WorldToObject, float4(vp, 1));
    }

    void surf(Input IN, inout SurfaceOutputStandard o)
    {
        float3 vp = floor(IN.worldPos.xyz / _Voxelize);
        float phase = floor(_Time.y * 8);
        float cutkey = UVRandom(vp.xy * 0.12324 - float2(vp.z * 0.72249, phase));
        
        clip(_Cutoff * 1.02 - cutkey - 0.01);

        o.Albedo = _Albedo;
        o.Smoothness = _Smoothness;
        o.Metallic = _Metallic;
        o.Normal = float3(0, 0, IN.facing > 0 ? 1 : -1);
        o.Emission = _Emission;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0
        ENDCG
    }
}
