/// This doesn't work, but the idea is to write the existing depth texture directly to the depth buffer
///  Might not be possible in Unity directly...
Shader "Unlit/SceneDepthWrite"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderQueue" = "Geometry-100" }
        LOD 100

        ColorMask 0
        ZWrite On
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // DepthAPI Environment Occlusion
            #pragma multi_compile _ HARD_OCCLUSION SOFT_OCCLUSION

            #include "UnityCG.cginc"
            #include "Packages/com.meta.xr.depthapi/Runtime/BiRP/EnvironmentOcclusionBiRP.cginc"

            uniform sampler2D _SceneDepth;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;

                META_DEPTH_VERTEX_OUTPUT(1) // the number should stand for the previous TEXCOORD# + 1

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO // required for stereo
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                    
                o.vertex = UnityObjectToClipPos(v.vertex);

                META_DEPTH_INITIALIZE_VERTEX_OUTPUT(o, v.vertex);

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, vert);

                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); // required to support stereo

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                float4 col = float4(0, 0, 0, 1);

                // Third field is for environment depth bias. 0.0 means the occlusion will be calculated with depths as they are.
                META_DEPTH_OCCLUDE_OUTPUT_PREMULTIPLY(i, col, 0.0);

                return col;
            }
            ENDCG
        }
    }
}

