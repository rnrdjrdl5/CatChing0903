Shader "Toon/Rim Outline" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_Brightness("Outline Brightness", Range(1,3)) = 2//
		_Pow("Pow",Range(1,500)) = 20
		_BumpMap("Normal",2D) = "bump"{}
		_OutlineZ("Outline Z", Range(0.01,0.25)) = 0.2//
		_Outline("Outline width", Range(.002, 0.03)) = .005
		_MainTex("Base (RGB)", 2D) = "white" { }
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;//
	};

	struct v2f {
		float4 pos : SV_POSITION;
		UNITY_FOG_COORDS(0)
			fixed4 color : COLOR;
		float4 screenPos : TEXCOORD0;//
		float3 lightDir : TEXCOORD1;//
		float2 uv :TEXCOORD2;
		float3 viewDir : TEXCOORD3;

	};

	uniform float _Outline, _Brightness, _OutlineZ;//

	sampler2D _MainTex;
	float4 _LightColor0;

	v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.screenPos = ComputeScreenPos(o.pos);//
		float3 outline = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.vertex));
		o.lightDir = normalize(WorldSpaceLightDir(v.vertex));//
		o.viewDir = WorldSpaceViewDir(v.vertex);

		float2 offset = TransformViewToProjection(outline) + (float2(o.lightDir.x,o.lightDir.y));// add the light direction to the offset for rimlight only on the light side
		float4 tex = tex2Dlod(_MainTex, float4(v.uv.xy, 0, 0));//

#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
		o.pos.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Outline;//
		o.pos.z -= _OutlineZ + (o.screenPos.z * 0.04);// push back the outline, instead of culling the front

#else
		o.pos.xy += offset * o.pos.z * _Outline;
		o.pos.z -= _OutlineZ + (o.screenPos.z * 0.04);
#endif
		o.color = tex * _Brightness;// textured bright outline
		return o;
	}
	ENDCG

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		UsePass "Toon/Lit/FORWARD" // replace with your own shader if you dont want the toon shader
		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" }
		//Cull Front
		ZWrite On
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#include "UnityCG.cginc"

		float4 _Color;

		fixed4 frag(v2f i) : SV_Target
	{
		float4 c = tex2D(_MainTex,i.uv);
		return saturate(float4(c.rgb+i.color.rgb*_LightColor0.rgb,c.a));
	}
		ENDCG
	}
	}

		Fallback "Diffuse"
}