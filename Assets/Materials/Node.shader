Shader "Custom/Node" {

    Properties{
        _MainTex("Main Texture", 2D) = "gray" {}

        _Color("Color", Color) = (1,1,1,1)
        _ShadowColor("Shadow Color", Color) = (0,0,1,1)
        _EmissionColor("Emission Color", Color) = (1,1,1,1)

        _Color2("Color 2", Color) = (0,0,0,0)
        _ShadowColor2("Shadow Color 2", Color) = (0,0,0,0)
        _EmissionColor2("Emission Color 2", Color) = (0,0,0,0)

        _Emit( "Emission Rate", Float) = 1
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
        #pragma prefer_hlsl2glsl gles

        #include "UnityCG.cginc"
        #include "rgb2hsv.cginc"
        

        float yama(float zero2two){
            return saturate(zero2two < 1 ? (zero2two * 2 - 1/2) : (7/2 - 2 * zero2two));
        }

        sampler2D _MainTex;
        fixed4 _MainTex_ST;

        half4 _Color;
        half4 _ShadowColor;
        half4 _EmissionColor;
        half4 _Color2;
        half4 _ShadowColor2;
        half4 _EmissionColor2;

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
            o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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

                fixed3 col1hsv = rgb2hsv(_Color.rgb);
                fixed3 col2hsv = rgb2hsv(_Color2.rgb);
                fixed3 shad1hsv = rgb2hsv(_ShadowColor.rgb);
                fixed3 shad2hsv = rgb2hsv(_ShadowColor2.rgb);

                fixed3 actualHSV1 = lerp(col1hsv, shad1hsv, 1 - dif);
                fixed3 actualHSV2 = lerp(col2hsv, shad2hsv, 1 - dif);

                fixed3 actualHSV = ((_Color2.a == 0) ? actualHSV1 : lerp(actualHSV1, actualHSV2, yama(fmod(i.uv.x + i.uv.y + _Time.z, 2))));

                fixed3 actualRGB = hsv2rgb(saturate(actualHSV));

                //ブラウン管風にする
                float vppX3 = floor(i.viewportPos.x / i.viewportPos.w * 500 % 3);
                float3 rgbRate = float3((vppX3==0)*1.5 + 0.5, (vppX3==1)*1.5 + 0.5, (vppX3==2)*1.5 + 0.5);

                actualRGB = lerp(actualRGB * rgbRate, actualRGB, saturate(dif * 2));

                actualColor = fixed4(actualRGB, 1);
            }

            fixed4 actualEmission;
            {
                actualEmission = (_Color2.a == 0) ? _EmissionColor : lerp(_EmissionColor, _EmissionColor2, yama(fmod(i.uv.x + i.uv.y + _Time.z, 2)));
                actualEmission *= _Emit;
            }

            fixed4 texCol;
            {
                fixed2 texUV = i.uv; //(i.uv - fixed2(0, _Time.y)) % fixed2(1,1);
                texCol = tex2D(_MainTex, texUV);
            }

            return saturate(actualColor + actualEmission + texCol - fixed4(0.5,0.5,0.5,0));
        }

        ENDCG
        }
    }
    Fallback "Diffuse"
}
