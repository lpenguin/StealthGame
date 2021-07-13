    Shader "My Shaders/No Lighting 100% colour on top" {
       Properties {
          _Color ("Main Color", Color) = (1, 1, 1, 1)
          _MainTex ("Base (RGB)", 2D) = "white" {}
       }
       SubShader {
          Tags { "Queue"="Overlay+1" }
          ZTest Always
          Blend SrcAlpha OneMinusSrcAlpha 
//          Blend [_SrcBlend] [_DstBlend]
          Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f_in
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 uv : TEXCOORD0;
            };
                
            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (v2f_in v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
          }
       }
    }
