Shader "Custom/Bomb" {

    Properties{
        _Ramp("Ramp Texture", 2D) = "white" {}
    }

    SubShader{
        Pass{

        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        LOD 200
        Blend SrcAlpha One
        ZWrite Off

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float4 normal : NORMAL;
        };

        struct v2f
        {
            //float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            // ワールド空間の法線ベクトル
            float3 worldNormal : TEXCOORD1;
            // ワールド空間の頂点座標
            float3 worldPos : TEXCOORD2;
            //float4 viewportPos : TEXCOORD3;
        };

        v2f vert (appdata v)
        {
            v2f o;
            // 頂点をクリップ空間座標に変換
            o.vertex = UnityObjectToClipPos(v.vertex);
            // 法線ベクトルをワールド空間座標に変換
            float3 worldNormal = UnityObjectToWorldNormal(v.normal);
            o.worldNormal = worldNormal;
            // 頂点をワールド空間座標に変換
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
            o.worldPos = worldPos;
            //o.viewportPos = ComputeScreenPos(o.vertex);
            return o;
        }

        sampler2D _Ramp;

        fixed4 frag (v2f i) : SV_Target
        {
            // 法線ベクトル
            float3 normal = normalize(i.worldNormal);
            // 光源方向ベクトル
            float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
            // 法線 - ライトの角度量
            float NdotL = dot(normal, lightDir);
            float diff = NdotL * 0.5 + 0.5;

            fixed4 col = tex2D(_Ramp, fixed2(diff * diff + (_Time.y % 1), 0.5));
            col.a *= 0.5;

            return col;
        }

        ENDCG
        }
    }
    Fallback "Diffuse"
}
