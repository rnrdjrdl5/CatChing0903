Shader "Custom/Star_Shader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_Rim ("Rim" , float ) = 0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0
		
		sampler2D _MainTex;


		struct Input {
			float3 viewDir;
			float2 uv_MainTex;

		};

		
		fixed4 _Color;
		float4 _Color2;
		float _Rim;

		

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			float3 rim ;
			float3 NdotV = saturate(dot(o.Normal, IN.viewDir));
			NdotV = 1 - NdotV;
			NdotV = pow(NdotV, _Rim);
			rim = NdotV * _Color2 ; 
			o.Emission = rim + _Color + c.r ;
			o.Alpha = 1;
		}

		


		ENDCG
	}
	FallBack "Diffuse"
}
