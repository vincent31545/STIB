// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/VoxelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color("Color", Color) = (1, 1, 1, 1)
        [Space]
        _OnTex ("On Texture", 2D) = "white" {}
        [HDR] _OnColor("On Color", Color) = (1, 0, 0, 1)
        [Space]
        _BlendStart("Blend Start", Range(0, 1)) = 0
        _BlendEnd("Blend End", Range(0, 1)) = 1
        _BlendOffset("Blend Offset", Range(-2, 1)) = 0
        _ShadowStrength("Shadow Strength", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members poralNormal)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 normal : TEXCOORD1;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            sampler2D _OnTex;
            float4 _OnColor;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _InstanceColor)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _VoxelData)
            UNITY_INSTANCING_BUFFER_END(Props)

            float _BlendStart;
            float _BlendEnd;
            float _BlendOffset;
            float _ShadowStrength;

            v2f vert (appdata v) {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                float4 norm = v.vertex;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = mul(unity_ObjectToWorld, v.vertex).xyz;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = norm;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(i);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                fixed4 onCol = tex2D(_OnTex, i.uv) * _OnColor;

                float dir = dot(i.normal, -_WorldSpaceLightPos0.xyz);
                float strength = min(1, dir * 3);
                float step = smoothstep(_BlendStart + _BlendOffset, _BlendEnd + _BlendOffset, strength);

                float4 data = UNITY_ACCESS_INSTANCED_PROP(Props, _VoxelData);

                float lightResult = step + (1 - step) * _ShadowStrength;
                float4 colorResult = col * lightResult * UNITY_ACCESS_INSTANCED_PROP(Props, _InstanceColor);

                return (colorResult) + (onCol * data[0]);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}
