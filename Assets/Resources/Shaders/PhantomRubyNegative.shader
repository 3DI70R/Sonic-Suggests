Shader "Unlit/PhantomRubyNegative"
{
	Properties
	{
		_Color ("Texture", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent+100"}
        Cull Front
        Lighting Off
        ZWrite Off
        ZTest Always
        Fog { Mode Off }
        Blend OneMinusDstColor OneMinusSrcAlpha
        BlendOp Add

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			half4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
