Shader "CustomPost/ColorfullEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RanderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass {
            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #pragma shader_feature MULTIPLY COLORBURN LINEARBURN SCREEN COLORDODGE LINEARDODGE

            #pragma vertex vert
            #pragma fragment frag

            //sampler2D _MainTex;
            TEXTURE2D(_MainTex);
            SAMPLER (sampler_MainTex);

            float _intensity;
            

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                //UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v) {
                v2f o = (v2f)0;
                //UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                //VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                //o.vertex = vertexInput.positionCS;
                o.vertex = TransformWorldToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f input) : SV_Target {

                //UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 background = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
               // return tex2D(_MainTex,input.uv);
                //return background;

                //background.r = 0;
                //background.g = 0;
                //background.b = 0;
                return background;

                float gray = 0.33 * (background.r + background.g + background.b);

                
                if(gray  > _intensity)
                {
                 // white
                 background = float4(1.0,1.0,1.0,background.w);   
                }
                else
                {
                 // black
                 background = float4(0.0,0.0,0.0,background.w);   
                }



               #if defined(BlackWhite)

               
               
                
                
                #endif

               return (background);


            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
