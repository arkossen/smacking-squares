Shader "Custom/DissolveForceField" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//This is the Main Texture of the object.
		_SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
		//This is the Texture that is being used to calculate which part of the normal texture to cut out.
		_Reflection ("Reflection Cubemap", CUBE) = "black" {}
		//This is the reflective cubemap
		
		_HoloColor("Holo Color", Color ) = (0.19,0.48,0.725,1)
		//This is the holographic color of the object. This color is being transmitted.
		_RimColor ("Rim Color", Color ) = (0.075,0.22,0.34,1)
		//This is the color on the outside edges of an object.
		_RimPower ("Rim Power", Range(1,20 )) = 1
		//This is the strength of the Rim Color.
		
		_ReflectPower( "Reflect Power", Range(0,1) ) = .1
		//This is the strength of the reflection of the cubemap onto an object.
      	_SliceAmount ("Slice Amount", Range(0.0, 1.0)) = 0
      	//This is the amount that is being cut off from the texture.
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		//Depth of the shader in comparison to other shaders. (Which shader comes before another)
		
		Cull off 
		
		CGPROGRAM
		#pragma surface surf Lambert
		//End the default ShaderLab language and start programming in CGPROGRAM language.
		//Create a function called surf with the lighting model Lambert.
		
		sampler2D		_MainTex;
		samplerCUBE		_Reflection;
		half4			_RimColor;
		half			_RimPower;
		half			_ReflectPower;
		
		sampler2D _SliceGuide;
      	float _SliceAmount;
      	//Define all variables that are going to be used in the Input / Surf functions.

		struct Input 
		{
			float2 uv_MainTex;
			//Main texture skin.
			float3 viewDir;
			//Direction from where the player is looking.
			float4 screenPos;
			//Screen Position.
			float3 worldPos;
			//Position of an object in world coordinates.
			float3 worldRefl;
			//Reflection from the world onto an object.
			
			float2 uv_SliceGuide;
			//Texture skin that is being used to determine what part is being cut off out of the _MainTex
            float _SliceAmount;
            //The amount that is being cut out.
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D ( _MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			//These pixels have the same color as the pixels on the texture (So actually just the default texture colors)
			
			half rimFactor = pow( 1 - dot( normalize( IN.viewDir ), o.Normal ), _RimPower );
			//Calculating where the rimPower takes place. (From the direction of the Camera).
			
			half3 reflect = texCUBE( _Reflection, IN.worldRefl );
			//Reflection from the world.
			o.Emission = reflect * _ReflectPower + rimFactor * _RimColor;
			//Emission by the object of the reflectiveness from the world onto the object.
			//Cubemap/World -> Object -> World emission.
			
			clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgba - _SliceAmount);
			//Cut out a pattern from the uv_MainTex depending on the texture of the _SliceGuide. (Editable in the editor)
		}
		ENDCG
		//Stop writing in CGPROGRAM and switch back to ShaderLab Language.
		
		Blend One One
		//Blend the color of the pixels.
		//For more options check out: http://docs.unity3d.com/Documentation/Components/SL-Blend.html
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		samplerCUBE		_Reflection;
		half4			_RimColor;
		half			_RimPower;
		half			_ReflectPower;
		half4			_HoloColor;
		
		sampler2D _SliceGuide;
      	float _SliceAmount;

		struct Input 
		{
			float3 viewDir;
			float4 screenPos;
			float3 worldPos;
			float3 worldRefl;
			
			float2 uv_SliceGuide;
            float _SliceAmount;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half clipValue = sin( IN.worldPos.y * 50 + _Time * 100 );
			clip( clipValue );
			//Cut out a part of the y-axis of the texture every x-amount of time with a certain amount of speed.
		
			half4 c = tex2D ( _SliceGuide, IN.uv_SliceGuide);
			o.Albedo = c.rgb * _HoloColor;
			o.Alpha = c.a;
			
			half rimFactor = pow( 1 - dot( normalize( IN.viewDir ), o.Normal ), _RimPower );
			
			half3 reflect = texCUBE( _Reflection, IN.worldRefl );
			o.Emission = reflect * _ReflectPower + rimFactor * _RimColor;
			    
			clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgba - _SliceAmount);
			//Cut out a pattern from the uv_SliceGuide depending on the texture of the _SliceGuide. (Editable in the editor)
			//The clip will cut out itself, so that the "forcefield" effect will dissapear as well.
		}
		ENDCG
	} 	
	FallBack "Diffuse"
	//If the shader cannot be displayed by somebody elses computer, then fallback onto the default Diffuse Shader.
}