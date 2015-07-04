Shader "Codetta/DualColorShader" {
    Properties {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        Cull Back
		ZWrite On

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				half2 texcoord : TEXCOORD0;
			};

			struct f2s {
				fixed4 col0 : COLOR0;
				fixed4 col1 : COLOR1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal = v.normal;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 _BaseColor;
			fixed4 _OutlineColor;

			fixed4 frag (v2f i) : SV_Target
			{
				if (i.normal.x < 0.1 || i.normal.y < 0.1 || i.normal.z < 0.1) {
					return _OutlineColor;
				}
				else {
					return _BaseColor;
				}
			}
			ENDCG
		}
    }
}
