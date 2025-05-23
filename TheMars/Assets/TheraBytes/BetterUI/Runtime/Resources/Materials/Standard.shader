Shader "BetterUI/Standard"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
		
		SrcBlendMode ("SrcBlendMode", Float) = 5
		DstBlendMode ("DstBlendMode", Float) = 10
		
		[Toggle(COMBINE_ALPHA)] CombineAlpha("Combine Alpha", Float) = 0
		[Toggle(FORCE_CLIP)] ForceClip("Force Clip", Float) = 0
		ClipThreshold("Alpha Clip Threshold", Float) = 0.5
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend [SrcBlendMode] [DstBlendMode]
		ColorMask[_ColorMask]

		Pass
	{
		Name "Standard"
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP
#pragma multi_compile __ COMBINE_ALPHA
#pragma multi_compile __ FORCE_CLIP

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0; 

#if UNITY_VERSION >= 550
			UNITY_VERTEX_INPUT_INSTANCE_ID
#endif

	};

	struct v2_f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float4 worldPosition	: TEXCOORD1;

#if UNITY_VERSION >= 550
		UNITY_VERTEX_OUTPUT_STEREO
#endif
	};

	fixed4 _TextureSampleAdd;
	float4 _ClipRect;
	
#if UNITY_VERSION < 550
	bool _UseClipRect;
	bool _UseAlphaClip;
	bool CombineAlpha;
	bool ForceClip;
#endif

	// VERTEX SHADER
	v2_f vert(appdata_t IN)
	{
		v2_f OUT;
#if UNITY_VERSION >= 550
		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
#endif
		OUT.worldPosition = IN.vertex;

#if UNITY_VERSION >= 550
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
#else
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

		#ifdef UNITY_HALF_TEXEL_OFFSET
		OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1, 1);
		#endif
#endif

		OUT.texcoord = IN.texcoord;

		OUT.color = IN.color;
		return OUT;
	}

	sampler2D _MainTex;
	float ClipThreshold;


	// FRAGMENT SHADER
	fixed4 frag(v2_f IN) : SV_Target
	{
		half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
		
		
#if UNITY_VERSION >= 550
#ifdef COMBINE_ALPHA
		color.rgb *= color.a;
#endif
#ifdef FORCE_CLIP
		clip(color.a - ClipThreshold);
#endif
#else
		if (CombineAlpha)
			color.rgb *= color.a;
			
		if (ForceClip)
			clip(color.a - ClipThreshold);
#endif


#if UNITY_VERSION >= 550
		color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
#ifdef UNITY_UI_ALPHACLIP
		clip(color.a - ClipThreshold);
#endif
#else
		if (_UseClipRect)
			color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

		if (_UseAlphaClip)
			clip(color.a - ClipThreshold);
#endif

		return color;
	}


		ENDCG
	}
	}
}
