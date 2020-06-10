Shader "Custom/NodeAllColor" {

    Properties{
        _MainTex("Main Texture", 2D) = "gray" {}

        _Saturation("Saturation", Range(0, 1)) = 1
        _ShadowSaturation("Saturation in Shadow", Range(0, 1)) = 0.5
        _Value("Value (Brightness)", Range(0, 1)) = 0.5
        _ShadowValue("Value in Shadow", Range(0, 1)) = 0.8

        _Emission("Emission", Float) = 0.2
        _Emit("Emission Rate", Float) = 1
    }

    SubShader{
        Pass{

        Tags {
            "RenderType" = "Opaque"
        }
        LOD 200
        Blend One Zero

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"
        #include "rgb2hsv.cginc"

        float yama(float zero2two){
            return saturate(zero2two < 1 ? (zero2two * 2 - 1/2) : (7/2 - 2 * zero2two));
        }

        sampler2D _MainTex;

        float _Saturation;
        float _ShadowSaturation;
        float _Value;
        float _ShadowValue;

        float _Emission;

        float _Emit;


        // ライトカラー
        float4 _LightColor0;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float4 normal : NORMAL;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            // ワールド空間の法線ベクトル
            float3 worldNormal : TEXCOORD1;
            // ワールド空間の頂点座標
            float3 worldPos : TEXCOORD2;
            float4 viewportPos : TEXCOORD3;
        };

        v2f vert (appdata v)
        {
            v2f o;
            // 頂点をクリップ空間座標に変換
            o.vertex = UnityObjectToClipPos(v.vertex);
            //uv 座標
            o.uv = v.uv;
            // 法線ベクトルをワールド空間座標に変換
            float3 worldNormal = UnityObjectToWorldNormal(v.normal);
            o.worldNormal = worldNormal;
            // 頂点をワールド空間座標に変換
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
            o.worldPos = worldPos;
            o.viewportPos = ComputeScreenPos(o.vertex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            float dif;
            {
                // 法線ベクトル
                float3 normal = normalize(i.worldNormal);
                // 光源方向ベクトル
                float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                // 法線 - ライトの角度量
                float NdotL = dot(normal, lightDir);
                dif = 0.5 * NdotL + 0.5;
            }

            fixed4 actualColor;
            {
                float hue = (i.uv.x / 2 + i.uv.y / 2 + _Time.y) % 1;
                float sat = dif * _Saturation + (1 - dif) * _ShadowSaturation;
                float val = dif * _Value + (1 - dif) * _ShadowValue + _Emission * _Emit;

                fixed3 actualHSV = fixed3(hue, sat, val);
                fixed3 actualRGB = hsv2rgb(actualHSV);
                
                //ブラウン管風にする
                float vppX3 = floor(i.viewportPos.x / i.viewportPos.w * 500 % 3);
                float3 rgbRate = float3((vppX3==0)*1.5 + 0.5, (vppX3==1)*1.5 + 0.5, (vppX3==2)*1.5 + 0.5);

                actualRGB = lerp(actualRGB * rgbRate, actualRGB, saturate(dif * 2));

                actualColor = fixed4(actualRGB, 1);
            }

            fixed4 texCol;
            {
                fixed2 texUV = (i.uv - fixed2(0, _Time.y)) % fixed2(1,1);
                texCol = tex2D(_MainTex, texUV);
            }

            return actualColor + texCol - fixed4(0.5,0.5,0.5,0);
        }

        ENDCG
        }
    }
    Fallback "Diffuse"
}
