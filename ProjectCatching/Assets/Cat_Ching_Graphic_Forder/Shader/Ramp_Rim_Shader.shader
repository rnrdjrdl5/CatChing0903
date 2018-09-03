Shader "Custom/Ramp_Rim_Shader" {
	Properties {
		//[HDR]_EmissionColor("Emission Color",Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_MainTex2("Ramp Texture", 2D) = "white" {}
		_OutlineWidth("Outline Width",Range(0,0.01)) = 0.001
		[HDR]_OutlineColor("Outline Color",Color) = (1,1,1,1)
		_BumpMap("Normal Map",2D) = "bump" {}
		_MainTex3("Occlusion",2D) = "white"{}
		_LamPower("Lambert Power",Range(1,500)) = 3
		_RimPower("Rim Power",Range(0,20)) = 10
		_Cutoff("Cutoff",float) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
	
			ZWrite Off
			ZTest LEqual
			//offset (-1;-1)
		
			CGPROGRAM
			#pragma surface surf Lambert2 vertex:vert exclude_path:deferred//랜더링 순서를 디퍼드이후까지 미룬다.
			#pragma target 3.0


			float _OutlineWidth;
			float4 _OutlineColor;
			sampler2D _MainTex;

			struct Input{
				float2 uv_MainTex;
				float4 color:COLOR;
			};

			void vert(inout appdata_full v)
			{
				v.vertex.xyz += v.normal.xyz*_OutlineWidth;
			}

			void surf(Input IN,inout SurfaceOutput s)
			{
				float4 c = tex2D(_MainTex,IN.uv_MainTex)*_OutlineColor;
				s.Albedo = c.rgb;
				s.Alpha = 1;
			}

			float4 LightingLambert2(SurfaceOutput o,float3 lightDir, float3 viewDir, float atten)
			{
				float ndotl = saturate(dot(o.Normal,lightDir))*0.5+0.5;
				ndotl = pow(ndotl,3);
				return float4(o.Albedo,1);
			}


			ENDCG
	
	
		Zwrite on
		ZTest LEqual

			CGPROGRAM
			#pragma surface surf DKLambert exclude_path:deferred
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _MainTex2;
			sampler2D _MainTex3;
			float _RimPower;
			//float3 _EmissionColor;
			float _LamPower;

			struct Input 
			{
			float2 uv_MainTex;
			float2 uv_MainTex3;
			float2 uv_BumpMap;
			};

			fixed4 _Color;


			void surf (Input IN, inout SurfaceOutput o) {
			o.Normal = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
			float4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 d = tex2D (_MainTex3, IN.uv_MainTex3);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
			}






			float4 LightingDKLambert(SurfaceOutput o,float3 lightDir, float3 viewDir, float atten)
			{
			float ndotl = saturate(dot(o.Normal,lightDir))*0.5+0.5;
			float4 FinalColor;
////Lambert:
			ndotl = pow(ndotl,_LamPower);
			float3 DiffuseColor = ndotl * o.Albedo * _LightColor0.rgb* atten;
///////Ramp:
			float4 RampTexture = tex2D(_MainTex2,float2(ndotl,0.5));
////////Rim: 안씀
			float3 RimColor;
			float vdotn = (dot(normalize(viewDir),normalize(o.Normal)));
			float invdotn = 1- vdotn;
			RimColor = pow(invdotn,_RimPower)*o.Albedo;
//////Final:
			FinalColor.rgb = (DiffuseColor.rgb * RampTexture.rgb);//+RimColor * 0.1;
			FinalColor.a = o.Alpha;
			return FinalColor;

			}
			ENDCG

			
	}
	FallBack "Legacy Shaders/Diffuse"
}


