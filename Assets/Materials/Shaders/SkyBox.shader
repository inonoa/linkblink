Shader "CustomSkybox/Vertical"
{

    properties
    {
        _ColorUp ("Color Up", Color) = (1, 1, 1, 1)
        _ColorDown ("Color Down", Color) = (0, 0, 0, 1)
        _StarTex ("Stars Texture", 2D) = "black" {}
        _LightRingAngle("Light Ring Angle", Vector) = (0, 0, 1, 0)
        _LightDistance("Light Distance", Float) = 0.1
        _LightWidth("Light Width", Float) = 0.1
        _LightColor("Light Color", Color) = (0, 0.3, 0.3, 1)
    }

    SubShader
    {

        Tags
        {
            "RenderType"="Background"
            "Queue"="Background"
            "PreviewType"="SkyBox"
        }

        Pass
        {
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            fixed4 _ColorUp;
            fixed4 _ColorDown;
            sampler2D _StarTex;
            float4 _LightRingAngle;
            float _LightDistance;
            float _LightWidth;
            float4 _LightColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            float random (fixed2 p) { 
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed tan = i.texcoord.y / i.texcoord.z;
                fixed rate = 0.2;
                tan = (tan >= -rate ? tan + rate : - tan - rate * 5);
                fixed4 baseCol = fixed4(lerp(_ColorDown.rgb, _ColorUp.rgb, tan), 1.0);

                float3 dir = normalize(i.texcoord);
                float3 lightDir = normalize(_LightRingAngle.xyz);
                float distanceFromLight = sqrt(dot(dir - lightDir, dir - lightDir));

                float lightIntensity = 
                    saturate(_LightWidth - abs(pow(distanceFromLight - _LightDistance, 2)))
                    / _LightWidth * saturate(1 - 0.7 * distanceFromLight * distanceFromLight);
                fixed4 lightNoise = 0.2 * fixed4(1,1,1,1) - 0.4 * fixed4(
                    random((_Time.xy + i.texcoord.xy) % float2(1,1)),
                    random((_Time.yz + i.texcoord.xy) % float2(1,1)),
                    random((_Time.zw + i.texcoord.xy) % float2(1,1)),
                    random((_Time.wx + i.texcoord.xy) % float2(1,1))
                );
                fixed4 lightCol = lightIntensity * (_LightColor + lightNoise);

                float starsIntensity = distanceFromLight > _LightDistance ? 0 : 1.5 * saturate(2 + 0.5 * (distanceFromLight - _LightDistance));
                fixed4 starsCol = tex2D(_StarTex, (i.texcoord.xy - (_Time.xy / 200) % fixed2(1, 1))) 
                                  * (random((_Time.xy + i.texcoord.xy) % float2(1,1)) + 0.5) * (starsIntensity + 0.5);


                return baseCol + lightCol + starsCol;
            }
            ENDCG
        }
    }
}
