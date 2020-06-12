Shader "Custom/NodeTransparent" {

    Properties{
        _MainTex("Main Texture", 2D) = "gray" {}
        _Color("Color", Color) = (1,1,1,1)
        _ShadowColor("Shadow Color", Color) = (0,0,1,1)
        _EmissionColor("Emission Color", Color) = (1,1,1,1)

        _Color2("Color 2", Color) = (0,0,0,0)
        _ShadowColor2("Shadow Color 2", Color) = (0,0,0,0)
        _EmissionColor2("Emission Color 2", Color) = (0,0,0,0)

        _Emit( "Emission Rate", Float) = 1
        _Alpha("Alpha", Range(0, 1)) = 1
    }

    SubShader{
        Pass{

        Tags{
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"
        #include "rgb2hsv.cginc"
        

        float yama(float zero2two){
            return saturate(zero2two < 1 ? (zero2two * 4 - 3/2) : (13/2 - 4 * zero2two));
        }

        sampler2D _MainTex;

        half4 _Color;
        half4 _ShadowColor;
        half4 _EmissionColor;
        half4 _Color2;
        half4 _ShadowColor2;
        half4 _EmissionColor2;

        float _Emit;
        float _Alpha;


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
        };

        v2f vert (appdata v)
        {
            v2f o;
            // 頂点をクリップ空間座標に変換
            o.vertex = UnityObjectToClipPos(v.vertex + v.normal * (1 - 0.5 * _Alpha*_Alpha - 0.5 * _Alpha));
            //uv 座標
            o.uv = v.uv;
            // 法線ベクトルをワールド空間座標に変換
            float3 worldNormal = UnityObjectToWorldNormal(v.normal);
            o.worldNormal = worldNormal;
            // 頂点をワールド空間座標に変換(ずらす)
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
            o.worldPos = worldPos;
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
                fixed4 col1hsv = fixed4(rgb2hsv(_Color.rgb), 1);
                fixed4 col2hsv = fixed4(rgb2hsv(_Color2.rgb), 1);
                fixed4 shad1hsv = fixed4(rgb2hsv(_ShadowColor.rgb), 1);
                fixed4 shad2hsv = fixed4(rgb2hsv(_ShadowColor2.rgb), 1);

                fixed4 actualHSV1 = lerp(col1hsv, shad1hsv, 1 - dif);
                fixed4 actualHSV2 = lerp(col2hsv, shad2hsv, 1 - dif);

                fixed4 actualHSV = ((_Color2.a == 0) ? actualHSV1 : lerp(actualHSV1, actualHSV2, yama((i.uv.x + i.uv.y + _Time.y) % 2)));

                actualColor = fixed4(hsv2rgb(actualHSV), 1);
            }

            fixed4 actualEmission;
            {
                actualEmission = (_Color2.a == 0) ? _EmissionColor : lerp(_EmissionColor, _EmissionColor2, yama((i.uv.x + i.uv.y + _Time.y) % 2));
                actualEmission *= _Emit;
            }

            fixed4 texCol;
            {
                fixed2 texUV = (i.uv - fixed2(0, _Time.y)) % fixed2(1,1);
                texCol = tex2D(_MainTex, texUV);
            }

            fixed4 col;
            {
                col = actualColor + actualEmission + texCol - fixed4(0.5,0.5,0.5,0);
                col.a = _Alpha;
            }

            return saturate(col);
        }

        ENDCG
        }
    }
    Fallback "Diffuse"
}
