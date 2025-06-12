Shader "Unlit/FadingLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Alpha ("Alpha", Range(0,1)) = 0.3
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 2.0
        _MinY ("Fade Bottom (World Y)", Float) = 0.0
        _MaxY ("Fade Top (World Y)", Float) = 2.0
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float worldY : TEXCOORD3;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
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
