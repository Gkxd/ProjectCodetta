Shader "ProjectCodetta/Horizontal Skew Texture Shader" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Color ("Tint", Color) = (0, 0, 0, 0)
    _ColorBlend ("Tint Blending", int) = 0
	_Skew ("Horizontal Skew", float) = 0
}

SubShader {
    Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane"}
    LOD 100
     
    Cull Off
    ZWrite On
    Blend SrcAlpha OneMinusSrcAlpha
 
    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct appdata_t {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
        };

        struct v2f {
            float4 vertex : SV_POSITION;
            half2 texcoord : TEXCOORD0;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
		float _Skew;

        v2f vert (appdata_t v)
        {
            v2f o;

			float4x4 skewMatrix = float4x4(
				1, _Skew, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1
			);

            o.vertex = mul(UNITY_MATRIX_MVP, mul(skewMatrix, v.vertex));
            o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }

        fixed4 _Color;
        half _ColorBlend; 

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.texcoord);

            switch(_ColorBlend) {
                case 1:
                    col.rgb += _Color.rgb;
                    break;
                case 0: default:
                    col.rgb *= _Color.rgb;
                    break;
            }
            col.a *= _Color.a;
            return col;
        }
        ENDCG
    }
}

}
