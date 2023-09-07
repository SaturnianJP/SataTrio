// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "TopazDuo/RealtimeEmissive" {
    Properties{
      _MainTex("テクスチャ", 2D) = "white" {}
      _Emission("エミッション", Float) = 1

      [Toggle(_)] _IsMask("マスクを行うか", Int) = 1
      _MaskTex("マスクテクスチャ", 2D) = "white" {}
      [Toggle(_)] _ApplyGamma("ガンマ補正", Float) = 1
       _Dotscale("ドットのサイズ", Range(0.0, 1.0)) = 2
    }
        SubShader{
          Tags { "RenderType" = "Opaque" }
          LOD 200

            CGPROGRAM
          // Physically based Standard lighting model, and enable shadows on all light types
    #pragma surface surf Standard fullforwardshadows

          // Use shader model 3.0 target, to get nicer looking lighting
    #pragma target 3.0
    #pragma shader_feature _EMISSION

        fixed _Emission;
        sampler2D _MainTex;
        sampler2D _MaskTex;
        bool _IsMask;
        bool _ApplyGamma;
        float _Dotscale = 1.0f;
        struct Input {
          float2 uv_MainTex;
          float2 uv_MaskTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o) {
            _Dotscale = clamp(_Dotscale, 0.0f, 1.0f); // 値を0.0fから1.0fの範囲にクランプ
            float value = 1.0f - _Dotscale; // 値を反転

            fixed4 e = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 mask = tex2D(_MaskTex, IN.uv_MaskTex * value);
            o.Albedo = fixed4(0,0,0,0);
            o.Alpha = e.a;

            if (_IsMask)
            {
                e.r *= (1 - mask.a);
                e.g *= (1 - mask.a);
                e.b *= (1 - mask.a);
            }

            if (_ApplyGamma)
            {
                e.rgb = pow(e.rgb, 2.2);
            }
            o.Emission = e * _Emission;
            o.Metallic = 0;
            o.Smoothness = 0;
          }
        ENDCG
      }
          FallBack "Diffuse"
           
}
