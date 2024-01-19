Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "white" {}
        _NormalIntensity ("Normal Intensity", Range(-10,10)) = 0
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0
        _Metallic ("Metallic", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeLine" = "UniversalRenderPipeline"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 4);
                float4 tangent : TEXCOORD5;
                float3 bitangent : TEXCOORD6;

            };

            sampler2D _MainTex, _NormalMap;
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float _NormalIntensity;

            float4 _BaseColor;
            float _Smoothness,_Metallic;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normal.xyz);
                o.tangent.xyz = TransformObjectToWorldDir(v.tangent.xyz);
                o.tangent.w = v.tangent.w;
                o.bitangent = cross(o.normalWS,o.tangent.xyz) * o.tangent.w;

                o.viewDir = normalize(_WorldSpaceCameraPos - o.positionWS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = TransformWorldToHClip(o.positionWS);

                OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
                OUTPUT_SH(o.normalWS.xyz,o.vertexSH);

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 normals = UnpackNormalScale( tex2D(_NormalMap, i.uv),_NormalIntensity);
                float3 finalNormals = normals.r * i.tangent + normals.g * i.bitangent + normals.b * i.normalWS;

                half4 mainTex = tex2D(_MainTex, i.uv);
                InputData inputData = (InputData)0;
                inputData.positionWS = i.positionWS;
                inputData.normalWS = normalize(finalNormals);
                inputData.viewDirectionWS = i.viewDir;
                inputData.bakedGI = SAMPLE_GI(i.lightmapUV, i.vertexSH, inputData.normalWS);


                SurfaceData surfaceData;
               
                surfaceData.albedo = mainTex * _BaseColor;
                surfaceData.specular = 0;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = 0;
                surfaceData.emission = 0;
                surfaceData.occlusion = 1;
                surfaceData.alpha = 0;
                surfaceData.clearCoatMask = 0;
                surfaceData.clearCoatSmoothness = 0;
                


                return UniversalFragmentPBR(inputData,surfaceData);
            }
            ENDHLSL
        }
    }
}
