Shader "Zan_Shader/Emoticon/Hi_Emoticon" {
	Properties {
		[HDR]_Color("Color",Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MainTex2("Patton", 2D) = "white" {}
		_MainTex3("Mask", 2D) = "white" {}
		_MainTex4("Icon", 2D) = "white" {}
		_MoveSpeed("Speed",Range(0,5)) = 0
		_UDRange("위아래 범위",Range(0,1)) = 0
		_UDSpeed("위아래 속도",Range(1,20)) = 0
		_LRRange("좌우 범위",Range(0,1)) = 0
		_LRSpeed("좌우 속도",Range(1,10)) = 0
		_Cutoff("Cutoff",float) = 0.5
	}
	SubShader {
		Tags { "RenderType"="TransparentCutout" "Queue" =  "alphatest" }
		LOD 200

		
		CGPROGRAM
		#pragma surface surf nolight noambient alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;
		sampler2D _MainTex4;
		float _MoveSpeed;
		float4 _Color;
		float _UDRange;
		float _UDSpeed;
		float _LRRange;
		float _LRSpeed;


		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
			float2 uv_MainTex4;
		};



		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 d = tex2D(_MainTex2,float2(IN.uv_MainTex2.x,IN.uv_MainTex2.y+_Time.y*_MoveSpeed));
			float4 f = tex2D(_MainTex3,IN.uv_MainTex3);
			float4 g = tex2D(_MainTex4,float2(IN.uv_MainTex4.x+sin(_Time.y*_LRSpeed)*_LRRange,IN.uv_MainTex4.y+sin(_Time.y*_UDSpeed)*_UDRange));
			float lerps = step(0.5,cos(_Time.y*9));
			o.Albedo = c*lerp(0,d,c.a);
			o.Albedo = c*lerp(o.Albedo,f,f.a);
			o.Albedo = lerp(o.Albedo,g,g.a)*_Color;
			o.Alpha = c.a+g.a;
		}

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir,float atten)
		{
			return float4(s.Albedo,1);
		}
		ENDCG
	}
	FallBack "Legacy Shaders/Transparent/VertexLit"
}
