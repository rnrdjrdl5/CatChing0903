Shader "Zan-Shader/Standard_Fixed" {
	Properties {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("BumpMap", 2D) = "bump" {}
		_Color("Color",Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MetalicTexture("MetalTexture",2D) = ""{}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200
		Zwrite on
		ColorMask 0

		CGPROGRAM
		#pragma surface surf nolight noambient nolightmap novertexlights noshadow

		struct Input {
			float4 color :COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		}

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir,float atten)
		{
			return float4(0,0,0,1);
		}
		ENDCG

	
		Zwrite off
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _MetalicTexture;
		half _Glossiness;
		half _Metallic;
		float4 _Color;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_MetalicTexture;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*_Color;
			fixed4 d = tex2D (_MetalicTexture, IN.uv_MetalicTexture);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
			o.Metallic = d.r;
			o.Smoothness = d.a*_Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}


