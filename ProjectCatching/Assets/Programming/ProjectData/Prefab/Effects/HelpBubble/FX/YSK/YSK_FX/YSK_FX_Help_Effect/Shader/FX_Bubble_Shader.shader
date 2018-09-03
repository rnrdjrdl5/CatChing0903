Shader "Custom/FX_Bubble_Shader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo (RGB)", 2D) = "white" {}
		_ref("Refraction" , float) = 0
		_rimpow("Rim Power" , float) = 0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		GrabPass{}

		CGPROGRAM
		#pragma surface surf Lambert 

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _GrabTexture;

		float _ref;
		float _rimpow;
		float _spec;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;

			float4 screenPos;
			float3 viewDir;
		};

	
		fixed4 _Color;

	

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			float4 d = tex2D (_MainTex2, IN.uv_MainTex2) * _Color;
			float3 screenUV = IN.screenPos.rgb/IN.screenPos.a;
			//프레넬
			float3 rim = saturate(dot(IN.viewDir, o.Normal));
			rim = pow(1-rim,_rimpow);
			
			//굴절
			o.Emission = tex2D(_GrabTexture,screenUV + c.r * _ref).rgb;
			o.Emission = lerp (o.Emission , _Color ,rim) + d ; 
			o.Alpha = c.a;
		}

		

		ENDCG
	}
	FallBack "Diffuse"
}
