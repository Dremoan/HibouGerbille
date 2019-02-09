Shader "Unlit/S_Distort"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Displacement", 2D) = "white" {}
		_ScrollSpeedDisp ("Speed (X,Y) / Intensity (Z,W)", Vector) = (0.2, 0.2, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float4 _ScrollSpeedDisp;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 scrollNoise = i.uv + (_Time.y * float2(_ScrollSpeedDisp.x, _ScrollSpeedDisp.y));
				fixed4 noise = tex2D(_NoiseTex, scrollNoise);
				fixed4 col = tex2D(_MainTex, i.uv + float2(noise.r * _ScrollSpeedDisp.z, noise.g * _ScrollSpeedDisp.w));
				return col;
			}
			ENDCG
		}
	}
}
