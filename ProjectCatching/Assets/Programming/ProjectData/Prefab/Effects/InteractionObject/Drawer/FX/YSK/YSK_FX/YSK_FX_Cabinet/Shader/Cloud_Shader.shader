Shader "Custom/Cloud_Shader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Cartoon 

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		float4 LightingCartoon (SurfaceOutput s , float3 viewDir , float3 lightDir,  float atten) 
		{
		
		float NdotL ;
		float NdotV ;
		NdotL = saturate(dot(s.Normal, lightDir));
		NdotV = dot(s.Normal , viewDir);
		float4 ramp = tex2D(_MainTex, float2 (NdotL,NdotV));
		
		
		float4 final;
		final.rgb = ramp ;
		final.a = 1;

		return final;
		}


		ENDCG
	}
	FallBack "Diffuse"
}
