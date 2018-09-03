Shader "Custom/NewSurfaceShader" {
	Properties {
		[HDR]_Color ("색상1", Color) = (1,1,1,1)
		[HDR]_Color2 ("색상2", Color) = (1,1,1,1)
		_Speed ("속도", Range(0,5)) = 1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex1 ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo (RGB)", 2D) = "white" {}
		_MainTex3 ("Albedo (RGB)", 2D) = "white" {}


	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard noambient
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex1;
		sampler2D _MainTex2;
		sampler2D _MainTex3;



		struct Input {
			float3 Color:COLOR;
			float2 uv_MainTex1;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
			float2 uv_MainTex;

			
		};

		fixed4 _Color;
		fixed4 _Color2;
		float _Speed;


		void surf (Input IN, inout SurfaceOutputStandard o) {
			float4 Mask = tex2D(_MainTex1 , IN.uv_MainTex1);
			float4 c = tex2D(_MainTex , IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Metallic = tex2D(_MainTex2 , IN.uv_MainTex2);
			o.Normal = UnpackNormal(tex2D (_MainTex3 , IN.uv_MainTex3));
			o.Emission = lerp(_Color,_Color2,sin(_Time.y*_Speed)*0.5+0.5).rgb * Mask.r;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
