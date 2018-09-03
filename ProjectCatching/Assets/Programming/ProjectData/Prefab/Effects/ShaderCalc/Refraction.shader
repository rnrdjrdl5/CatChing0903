Shader "TEST/Refraction" {
	Properties {
		_ColorTex ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo ", 2D) = "white" {}
		_RefStrength("Reflection Strength", Range(0,1)) = 0.05
		_FlowSpeedX ("FlowSpeedX", float) = 0
		_FlowSpeedY ("FlowSpeedY", float) = 0
	}

	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		zwrite off
		cull off
		GrabPass{}
		
		CGPROGRAM
		#pragma surface surf nolight keepalpha noforwardadd nolightmap noambient novertexlights noshadow

		sampler2D _GrabTexture;
		sampler2D _MainTex;
		float _RefStrength;
		float _FlowSpeedX;
     	float _FlowSpeedY;
		float4 _ColorTex;

		struct Input {
			float4 color:COLOR;
			float4 screenPos;
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		float4 ref = tex2D (_MainTex, float2(IN.uv_MainTex.x - ( _FlowSpeedX * _Time.y) , IN.uv_MainTex.y - ( _FlowSpeedY * _Time.y) ));
			float3 screenUV = IN.screenPos.rgb / IN.screenPos.a;
		//	o.Emission = tex2D(_GrabTexture,(screenUV.xy + ref.x * _RefStrength)) * _ColorTex;
		o.Emission = tex2D(_GrabTexture,(screenUV.xy + ref.x * _RefStrength * _ColorTex * IN.color.a));
		}

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten){
			return float4(0,0,0,1);
		}
		ENDCG
	}
	FallBack "Regacy Shaders/Transparent/Vertexlit"
}
