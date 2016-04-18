Shader "Custom/BlackMask" {
    Properties {
        _Alpha ("Alpha", Float) = 0.5
    }

    SubShader {
        Tags  { "Queue"="Overlay" "RenderType"="Transparent" }

        Pass {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Alpha;

            struct v2f {
                float4  pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                half4 c = half4(0.0, 0.0, 0.0, 0.0);
                c.a = _Alpha;
                return c;
            }

            ENDCG
        }
    }
     
    FallBack "Diffuse"
}
