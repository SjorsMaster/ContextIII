﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "PortalsVR/Portal"
{
    Properties
    {
        _InactiveColour("Inactive Colour", Color) = (1, 1, 1, 1)
        _Eye ("Active For Eye", Integer) = 0
        _LerpCol ("Lerp Color", Color) = (0,0,0,1)
        _LerpValue ("Max Lerp", Range(0,1)) = 1
        _StartLerp ("Start Depth", Float) = 0.005
        _EndLerp("End Depth", Float) = 0.015
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100
        Cull Off

        Blend One Zero //OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            uniform float3 _CenterEyePosition;

            sampler2D _MainTex;
            float4 _InactiveColour;
            int displayMask; // set to 1 to display texture, otherwise will draw test colour
            uniform int currentEye = 0;
            int _Eye;

            float4 _LerpCol;
            float _LerpValue;
            float _StartLerp, _EndLerp;

            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert
                
                if (unity_StereoEyeIndex != _Eye) discard;

                float2 uv = i.screenPos.xy / i.screenPos.w;
                fixed4 portalCol = tex2D(_MainTex, uv);

                // TODO: TEST THIS
                float dist = distance(i.worldPos, _CenterEyePosition);// i.screenPos.z;// 1 - (i.screenPos.z);// / i.screenPos.w);
                float t = max(min( ( dist - _StartLerp) / (_EndLerp - _StartLerp), 1), 0);
                t = pow(t, 0.5);
                float lVal = max(min(_LerpValue, t), 0);

                if (_LerpCol.a < 0.01f && t > 0.99f ) discard;

                return lerp(portalCol * displayMask + _InactiveColour * (1 - displayMask), _LerpCol, lVal);
            }
            ENDCG
        }
    }
    Fallback "Standard" // for shadows
}

