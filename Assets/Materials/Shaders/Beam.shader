Shader "Unlit/Beam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Color", Color) = (0,1,1,1)
        _TintColor2 ("Color 2 (optional)", Color) = (0,0,0,0)
        _Alpha ("Alpha", Range(0, 1)) = 1
        _ScrollX ("X Offset", Float) = 0
        _EmitColor("Emit Color", Color) = (0,0,0,0)
        _Emit ("Emit", Float) = 0
    }
    SubShader
    {
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float4 color:   COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                //float4 color:   COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float4 _TintColor2;
            float _Alpha;
            float _ScrollX;
            float4 _EmitColor;
            float _Emit;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.color = v.color;
                return o;
            }

            float updown(float x){
                float y = x % 0.5;
                return saturate((y < 0.25) ? (y * 20 - 2) : (- y * 20 + 8));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float4 actualColor;
                {
                    float alpha1 = updown(i.uv.x + (1 - _ScrollX % 1));
                    float4 blended = lerp(_TintColor2, _TintColor, alpha1);
                    
                    float twoColor = _TintColor2 == float4(0,0,0,0);
                    actualColor = twoColor ? _TintColor : blended;
                    actualColor.a = _Alpha;
                }

                return col * (actualColor + _EmitColor * _Emit);
            }
            ENDCG
        }
    }
}
