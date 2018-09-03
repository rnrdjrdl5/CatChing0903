Shader "JJ_Shader/Run_dust" {
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RimGlow("RimGlow", Range(0,1)) = 0.1
		_Emission("Emission ", Range(0,1)) = 0.1
	}
	SubShader {
		Tags { "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert keepalpha noambient  //nolightmap  
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		fixed4 _Color;
		float _RimGlow;
		float _Emission;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 color = tex2D(_MainTex, IN.uv_MainTex);
			fixed clipValue = IN.color.a - color.a;
			//clip(clipValue);
		
		

			o.Albedo = IN.color;
			o.Emission = _Emission;
			//o.Albedo = (clipValue * color.rgb) + (emission * IN.color.rgb);
			//o.Emission = (emission * IN.color.rgb);
			o.Alpha = color.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}