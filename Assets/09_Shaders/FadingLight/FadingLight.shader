Shader "Unlit/FadingLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (0, 1, 1, 1)
        _Alpha ("Alpha", Range(0,1)) = 0.0
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 0.1
        _MinY ("Fade Bottom (World Y)", Float) = 0.96
        _MaxY ("Fade Top (World Y)", Float) = 1.73
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend One One
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _TintColor;
            float _Alpha;
            float _FresnelPower;
            float _MinY;
            float _MaxY;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float worldY : TEXCOORD3;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                #if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
                    float3 cameraPos = unity_StereoWorldSpaceCameraPos[unity_StereoEyeIndex];
                #else
                    float3 cameraPos = _WorldSpaceCameraPos;
                #endif

                o.viewDir = normalize(cameraPos - worldPos);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldY = worldPos.y;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);
                float fresnel = pow(1.0 - saturate(dot(i.viewDir, i.normal)), _FresnelPower);
                float verticalFade = saturate(1.0 - ((i.worldY - _MinY) / (_MaxY - _MinY)));
                fixed4 finalCol = texCol * _TintColor;
                finalCol.a = (_Alpha + fresnel * 0.3) * verticalFade;
                finalCol.rgb *= finalCol.a;
                return finalCol;
            }
            ENDCG
        }
    }
}
