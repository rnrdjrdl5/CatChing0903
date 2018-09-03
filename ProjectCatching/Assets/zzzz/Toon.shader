Shader "Toon/Lit" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_BumpMap("Normal",2D) = "bump"{}
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	_Pow("power",Range(1,100)) = 10
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf ToonRamp

		sampler2D _Ramp;
		float _Pow;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, float3 lightDir,float3 viewDir,float atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif	
		float ndotl = saturate(dot(s.Normal, lightDir));
		float ldotv = 1;//saturate(-dot(lightDir, viewDir));
		float vdotn = saturate(dot(viewDir,s.Normal));
		vdotn = pow(1-vdotn, _Pow);
		vdotn = vdotn * ldotv;

		vdotn *= 3;
		vdotn = ceil(vdotn);
		vdotn /= 3;	

		float3 DiffColor = ndotl * _LightColor0.rgb * atten;
		DiffColor *= vdotn;
		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return float4(DiffColor, 1);
	}


	sampler2D _MainTex;
	sampler2D _BumpMap;
	float4 _Color;

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float2 uv_BumpMap;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		half4 c = tex2D(_MainTex, IN.uv_MainTex) ;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

	}

		Fallback "Diffuse"
}