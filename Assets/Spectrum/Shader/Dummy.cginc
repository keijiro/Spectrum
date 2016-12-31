#include "UnityCG.cginc"

// Basic material
half4 _Albedo;
half _Smoothness;
half _Metallic;

// Custom effects
half3 _Emission;
float _Voxelize;
half _Cutoff;

// Motion vector
float4x4 _NonJitteredVP;
float4x4 _PreviousVP;
float4x4 _PreviousM;
float _MotionVectorDepthBias;

// PRNG
float UVRandom(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// Voxelization effect
float3 Voxelize(float3 localPos)
{
    float4 wp = mul(unity_ObjectToWorld, float4(localPos, 1));
    float3 vp = floor(wp.xyz / max(_Voxelize, 0.001)) * _Voxelize;
    vp = lerp(wp.xyz, vp, saturate(_Voxelize / 0.01));
    return mul(unity_WorldToObject, float4(vp, 1));
}

// Calculate alpha value for cutout effect
half Alpha(float3 worldPos)
{
    float3 vp = floor(worldPos / max(_Voxelize, 0.001));
    float phase = floor(_Time.y * 8);
    float cutkey = UVRandom(vp.xy * 0.12324 - float2(vp.z * 0.72249, phase));
    return _Cutoff * 1.02 - cutkey - 0.01;
}

//
// Motion vector rendering
//

#if defined(DUMMY_MOTION)

struct MotionVertexInput
{
    float4 vertex : POSITION;
    float3 oldPos : NORMAL;
};

struct MotionVectorData
{
    float4 vertex : SV_POSITION;
    float4 transfer0 : TEXCOORD0;
    float4 transfer1 : TEXCOORD1;
    float3 worldPos : TEXCOORD2;
};

MotionVectorData VertMotionVectors(MotionVertexInput v)
{
    MotionVectorData o;
    float4 vp0 = float4(Voxelize(v.oldPos), 1);
    float4 vp1 = float4(Voxelize(v.vertex.xyz), 1);
    o.vertex = UnityObjectToClipPos(vp1);
    o.worldPos = mul(unity_ObjectToWorld, vp1).xyz;
    o.transfer0 = mul(_PreviousVP, mul(_PreviousM, vp0));
    o.transfer1 = mul(_NonJitteredVP, float4(o.worldPos, 1));
    return o;
}

half4 FragMotionVectors(MotionVectorData i) : SV_Target
{
    clip(Alpha(i.worldPos));

    float3 hp0 = i.transfer0.xyz / i.transfer0.w;
    float3 hp1 = i.transfer1.xyz / i.transfer1.w;

    float2 vp0 = (hp0.xy + 1) / 2;
    float2 vp1 = (hp1.xy + 1) / 2;

#if UNITY_UV_STARTS_AT_TOP
    vp0.y = 1 - vp0.y;
    vp1.y = 1 - vp1.y;
#endif

    return half4(vp1 - vp0, 0, 1);
}

#endif

//
// Custom surface shader
//

#if defined(DUMMY_SURFACE)

struct Input
{
    float3 worldPos;
    float facing : VFACE;
};

void vert(inout appdata_full data)
{
    data.vertex.xyz = Voxelize(data.vertex.xyz);
}

void surf(Input IN, inout SurfaceOutputStandard o)
{
    clip(Alpha(IN.worldPos.xyz));
    o.Albedo = _Albedo;
    o.Smoothness = _Smoothness;
    o.Metallic = _Metallic;
    o.Normal = float3(0, 0, IN.facing > 0 ? 1 : -1);
    o.Emission = _Emission;
}

#endif
