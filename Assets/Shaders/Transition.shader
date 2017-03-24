     Shader "Custom/Transition" {
         Properties {
             _MainTex ("Base (RGB)", 2D) = "white" {}
                     _DetailTex ("Detail (RGB)", 2D) = "white" {}
                     //_DetailTex ("Detail (RGB)", 2D) = "white"{}
                    //_DetailTex3 ("Detail (RGB)", 2D) = "white"{}
                     //_DetailTex4 ("Detail (RGB)", 2D) = "white"{}
                     //_DetailTex5 ("Detail (RGB)", 2D) = "white"{}
                     //_DetailTex6 ("Detail (RGB)", 2D) = "white"{}
                     //_DetailTex7 ("Detail (RGB)", 2D) = "white"{}
                     //_DetailTex8 ("Detail (RGB)", 2D) = "white"{}
                     _Guide ("Guide (RGB)", 2D) = "white" {}
                     _Threshold("Threshold",Range(0,1))=0
         }
         SubShader {
             Tags { "RenderType"="Opaque" }
             LOD 200
             
             CGPROGRAM
             #pragma surface surf Lambert
     
             sampler2D _MainTex;
                     sampler2D _DetailTex;
                     sampler2D _Guide;
                     float _Threshold;
     
             struct Input {
                 float2 uv_MainTex;
                             float2 uv_DetailTex;
                             float2 uv_Guide;
             };
     
             void surf (Input IN, inout SurfaceOutput o) {
                 half4 c = tex2D (_MainTex, IN.uv_MainTex);
                 half4 d = tex2D (_DetailTex, IN.uv_DetailTex);
                 half4 g = tex2D (_Guide, IN.uv_Guide);
                 if((g.r+g.g+g.b)*0.33333f<_Threshold)
                     o.Albedo = d.rgb;
                         else
                             o.Albedo = c.rgb;
                 o.Alpha = c.a;
             }
             ENDCG
         } 
         FallBack "Diffuse"
     }
