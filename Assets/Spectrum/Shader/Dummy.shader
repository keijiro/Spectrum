Shader "Spectrum/Dummy"
{
    Properties
    {
        _Albedo("Albedo", Color) = (1, 1, 1, 1)
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
        [Space]
        [HDR] _Emission("Emission", Color) = (1, 1, 1)
        _Voxelize("Voxelize", Range(0, 0.2)) = 0.1
        _Cutoff("Cutoff", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags{ "LightMode" = "MotionVectors" }
            ZTest LEqual
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex VertMotionVectors
            #pragma fragment FragMotionVectors
            #define DUMMY_MOTION
            #include "Dummy.cginc"
            ENDCG
        }

        Cull Off

        CGPROGRAM
        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0
        #define DUMMY_SURFACE
        #include "Dummy.cginc"
        ENDCG
    }
}
