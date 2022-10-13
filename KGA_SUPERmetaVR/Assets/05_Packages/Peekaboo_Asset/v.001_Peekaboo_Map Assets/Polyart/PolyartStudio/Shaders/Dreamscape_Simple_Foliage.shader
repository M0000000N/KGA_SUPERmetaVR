// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Polyart/Dreamscape Simple Foliage"
{
	Properties
	{
		_AlphaCutoff("Alpha Cutoff", Range( 0 , 1)) = 0.35
		_ColorTint("Color Tint", Color) = (1,1,1,0)
		_BaseColorTint("Base Color Tint", Color) = (1,1,1,0)
		_FoliageTexture("Foliage Texture", 2D) = "white" {}
		_ColorMask("Color Mask", 2D) = "white" {}
		_Smoothness("Smoothness", Range( -2 , 2)) = 1
		[Header(WIND)]_WindSpeed("Wind Speed", Range( 0 , 1)) = 0.5
		_WindScale("Wind Scale", Range( 0 , 2)) = 0.2588236
		_WindIntensity("Wind Intensity", Range( 0 , 50)) = 5
		_DitherBottomLevel("Dither Bottom Level", Range( -10 , 10)) = 0
		_DitherFade("Dither Fade", Range( 0 , 10)) = 0
		[Toggle(_DITHERINGON_ON)] _DitheringON("Dithering ON", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True"}
		Cull Off
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.0
		#pragma multi_compile_instancing
		#pragma shader_feature _DITHERINGON_ON
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPosition;
			float eyeDepth;
		};

		uniform float _WindSpeed;
		uniform float _WindScale;
		uniform float _WindIntensity;
		uniform float4 _BaseColorTint;
		uniform sampler2D _FoliageTexture;
		uniform float4 _FoliageTexture_ST;
		uniform float4 _ColorTint;
		uniform sampler2D _ColorMask;
		uniform float4 _ColorMask_ST;
		uniform float _Smoothness;
		uniform float FoliageRenderDistance;
		uniform float _DitherBottomLevel;
		uniform float _DitherFade;
		uniform float _AlphaCutoff;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		inline float Dither8x8Bayer( int x, int y )
		{
			const float dither[ 64 ] = {
				 1, 49, 13, 61,  4, 52, 16, 64,
				33, 17, 45, 29, 36, 20, 48, 32,
				 9, 57,  5, 53, 12, 60,  8, 56,
				41, 25, 37, 21, 44, 28, 40, 24,
				 3, 51, 15, 63,  2, 50, 14, 62,
				35, 19, 47, 31, 34, 18, 46, 30,
				11, 59,  7, 55, 10, 58,  6, 54,
				43, 27, 39, 23, 42, 26, 38, 22};
			int r = y * 8 + x;
			return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float mulTime45 = _Time.y * ( _WindSpeed * 3 );
			float simplePerlin3D47 = snoise( ( ase_worldPos + mulTime45 )*_WindScale );
			v.vertex.xyz += ( v.color * ( ( simplePerlin3D47 * 0.015 ) * _WindIntensity ) ).rgb;
			v.vertex.w = 1;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_FoliageTexture = i.uv_texcoord * _FoliageTexture_ST.xy + _FoliageTexture_ST.zw;
			float4 tex2DNode10 = tex2D( _FoliageTexture, uv_FoliageTexture );
			float2 uv_ColorMask = i.uv_texcoord * _ColorMask_ST.xy + _ColorMask_ST.zw;
			float4 lerpResult58 = lerp( ( _BaseColorTint * tex2DNode10 ) , ( tex2DNode10 * _ColorTint ) , tex2D( _ColorMask, uv_ColorMask ));
			float4 vColor60 = lerpResult58;
			o.Albedo = vColor60.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			float vAlpha96 = tex2DNode10.a;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen87 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither87 = Dither8x8Bayer( fmod(clipScreen87.x, 8), fmod(clipScreen87.y, 8) );
			float cameraDepthFade81 = (( i.eyeDepth -_ProjectionParams.y - FoliageRenderDistance ) / FoliageRenderDistance);
			dither87 = step( dither87, ( 1.0 - cameraDepthFade81 ) );
			float vGrassDistance90 = dither87;
			float2 clipScreen89 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither89 = Dither8x8Bayer( fmod(clipScreen89.x, 8), fmod(clipScreen89.y, 8) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			dither89 = step( dither89, saturate( ( ( ase_vertex3Pos.y + _DitherBottomLevel ) * ( _DitherFade * 2 ) ) ) );
			float vTerrainDither91 = dither89;
			#ifdef _DITHERINGON_ON
				float staticSwitch108 = ( ( vAlpha96 * vGrassDistance90 ) * vTerrainDither91 );
			#else
				float staticSwitch108 = vAlpha96;
			#endif
			clip( staticSwitch108 - _AlphaCutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster"  "NatureRendererInstancing"="True" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma multi_compile_instancing

		#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.screenPosition;
				o.customPack1.z = customInputData.eyeDepth;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.screenPosition = IN.customPack2.xyzw;
				surfIN.eyeDepth = IN.customPack1.z;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18912
1038;114;2305;1679;1467.184;678.9503;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;79;-3084.924,-1028.846;Inherit;False;2026.637;461.4008;Dithering;14;91;89;90;107;87;105;86;104;81;102;80;103;101;106;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;53;-3122.395,261.2668;Inherit;False;1713.722;584.5107;;12;57;56;49;50;51;47;48;43;45;42;46;44;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-3016.774,-778.0253;Inherit;False;Property;_DitherBottomLevel;Dither Bottom Level;9;0;Create;True;0;0;0;False;0;False;0;0.69;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2297.287,-953.8452;Inherit;False;Global;FoliageRenderDistance;FoliageRenderDistance;9;0;Create;True;0;0;0;True;0;False;35;90.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-3018.157,-697.3948;Inherit;False;Property;_DitherFade;Dither Fade;10;0;Create;True;0;0;0;False;0;False;0;0.57;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;106;-2992.69,-924.613;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;104;-2718.58,-828.1163;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;102;-2750.693,-695.5176;Inherit;False;2;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;81;-1909.284,-973.8453;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-3093.157,634.5677;Inherit;False;Property;_WindSpeed;Wind Speed;6;0;Create;True;0;0;0;False;1;Header(WIND);False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;65;-3112.49,-451.7797;Inherit;False;1702.997;576.199;Comment;10;64;55;63;54;60;58;59;10;12;96;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;12;-2878.581,-297.7398;Inherit;True;Property;_FoliageTexture;Foliage Texture;3;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-2559.415,-828.2041;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;46;-2791.157,640.5677;Inherit;False;3;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;86;-1638.28,-974.8453;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;107;-2263.735,-831.4385;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;42;-2660.741,448.2984;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;45;-2652.157,641.5677;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;87;-1470.281,-978.8453;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-2658.103,-298.9787;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;12;None;None;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-2388.156,542.5677;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;-1751.819,-200.8229;Inherit;False;vAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;-2578.789,-101.8611;Inherit;False;Property;_ColorTint;Color Tint;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DitheringNode;89;-2065.981,-836.5086;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;64;-3087.081,-371.1855;Inherit;False;Property;_BaseColorTint;Base Color Tint;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;-1285.284,-978.8453;Inherit;False;vGrassDistance;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2650.676,717.2456;Inherit;False;Property;_WindScale;Wind Scale;7;0;Create;True;0;0;0;False;0;False;0.2588236;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-869.6192,123.2771;Inherit;False;96;vAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-2281.362,-375.9384;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;59;-2292.036,-95.81062;Inherit;True;Property;_ColorMask;Color Mask;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2294.79,-208.0502;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;47;-2219.612,537.2402;Inherit;False;Simplex3D;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-901.3788,197.7955;Inherit;False;90;vGrassDistance;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;-1878.983,-837.5086;Inherit;False;vTerrainDither;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2218.673,681.2464;Inherit;False;Property;_WindIntensity;Wind Intensity;8;0;Create;True;0;0;0;False;0;False;5;0;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;50;-2016.675,541.2471;Inherit;False;0.015;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-894.3788,270.7952;Inherit;False;91;vTerrainDither;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-1930.007,-293.4413;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-644.9665,208.7229;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1859.674,541.2471;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-1754.657,-300.3033;Inherit;False;vColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;56;-1892.777,378.6124;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-461.6665,246.7231;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1631.273,477.8916;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-534.5779,-270.3036;Inherit;False;Property;_AlphaCutoff;Alpha Cutoff;0;0;Create;True;0;0;0;False;0;False;0.35;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;108;-306.6837,178.0497;Inherit;False;Property;_DitheringON;Dithering ON;11;0;Create;True;0;0;0;False;0;False;0;0;0;True;_DITHERINGON_ON;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-279.0808,-6.267761;Inherit;False;60;vColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-305.9848,87.3504;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;0;False;0;False;1;0;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;4;;0;0;Standard;Polyart/Dreamscape Simple Foliage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.05;True;True;0;True;Opaque;;AlphaTest;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;True;0;0;False;-1;-1;0;True;1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;104;0;106;2
WireConnection;104;1;103;0
WireConnection;102;0;101;0
WireConnection;81;0;80;0
WireConnection;81;1;80;0
WireConnection;105;0;104;0
WireConnection;105;1;102;0
WireConnection;46;0;44;0
WireConnection;86;0;81;0
WireConnection;107;0;105;0
WireConnection;45;0;46;0
WireConnection;87;0;86;0
WireConnection;10;0;12;0
WireConnection;43;0;42;0
WireConnection;43;1;45;0
WireConnection;96;0;10;4
WireConnection;89;0;107;0
WireConnection;90;0;87;0
WireConnection;63;0;64;0
WireConnection;63;1;10;0
WireConnection;54;0;10;0
WireConnection;54;1;55;0
WireConnection;47;0;43;0
WireConnection;47;1;48;0
WireConnection;91;0;89;0
WireConnection;50;0;47;0
WireConnection;58;0;63;0
WireConnection;58;1;54;0
WireConnection;58;2;59;0
WireConnection;92;0;97;0
WireConnection;92;1;94;0
WireConnection;49;0;50;0
WireConnection;49;1;51;0
WireConnection;60;0;58;0
WireConnection;93;0;92;0
WireConnection;93;1;95;0
WireConnection;57;0;56;0
WireConnection;57;1;49;0
WireConnection;108;1;97;0
WireConnection;108;0;93;0
WireConnection;0;0;61;0
WireConnection;0;4;36;0
WireConnection;0;10;108;0
WireConnection;0;11;57;0
ASEEND*/
//CHKSM=4187E40E8AB7E8DEB22AA2986529D35317461E57