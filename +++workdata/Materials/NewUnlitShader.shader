Shader "Pixelate"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _Radius("_Radius", Float) = 0.4
        _LineWidth("_LineWidth", Float) = 0.07
        _Color("_Color", Color) = (1, 1, 1, 1)
        _Rotation("_Rotation", Float) = 0
        _RemovedSegments("_RemovedSegments", Float) = 0
        _SegmentSpacing("_SegmentSpacing", Float) = 0.03
        _SegmentCount("_SegmentCount", Float) = 5
        _Float("Float", Float) = 16
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = "UniversalSpriteUnlitSubTarget"
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _PixelDensity;
            float2 _AspectRatioMultiplier;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pixelScaling = _PixelDensity * _AspectRatioMultiplier;
                i.uv = round(i.uv * pixelScaling) / pixelScaling;
                return tex2D(_MainTex, i.uv);
            }
            ENDCG


        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>

        // Defines

        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz = input.positionWS;
            output.interp1.xyzw = input.texCoord0;
            output.interp2.xyzw = input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }


        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _Radius;
        float _LineWidth;
        float _Rotation;
        float4 _Color;
        float _RemovedSegments;
        float _SegmentSpacing;
        float _SegmentCount;
        float _Float;
        CBUFFER_END

            // Object and Global properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Graph Includes
            // GraphIncludes: <None>

            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif

            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif

            // Graph Functions

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            void Unity_Flip_float2(float2 In, float2 Flip, out float2 Out)
            {
                Out = (Flip * -2 + 1) * In;
            }

            void Unity_Length_float2(float2 In, out float Out)
            {
                Out = length(In);
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }

            void Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(float In, out float Out)
            {

                        #if defined(SHADER_STAGE_RAY_TRACING)
                        #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                        #endif
                Out = abs(ddx(In)) + abs(ddy(In));
            }

            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }

            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }

            void Unity_Clamp_float(float In, float Min, float Max, out float Out)
            {
                Out = clamp(In, Min, Max);
            }

            void Unity_Multiply_float_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                //rotation matrix
                Rotation = Rotation * (3.1415926f / 180.0f);
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);

                //center rotation matrix
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix * 2 - 1;

                //multiply the UVs by the rotation matrix
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;

                Out = UV;
            }

            void Unity_Arctangent2_float(float A, float B, out float Out)
            {
                Out = atan2(A, B);
            }

            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }

            void Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(float In, out float Out)
            {

                        #if defined(SHADER_STAGE_RAY_TRACING)
                        #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                        #endif
                Out = abs(ddx(In)) + abs(ddy(In));
            }

            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            void Unity_Round_float(float In, out float Out)
            {
                Out = round(In);
            }

            void Unity_Modulo_float(float A, float B, out float Out)
            {
                Out = fmod(A, B);
            }

            void Unity_Sine_float(float In, out float Out)
            {
                Out = sin(In);
            }

            void Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(float In, out float Out)
            {

                        #if defined(SHADER_STAGE_RAY_TRACING)
                        #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                        #endif
                Out = abs(ddx(In)) + abs(ddy(In));
            }

            void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Custom interpolators, pre surface
            #ifdef FEATURES_GRAPH_VERTEX
            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
            {
            return output;
            }
            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
            #endif

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float2 _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3);
                float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1;
                float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip = float2 (1, 0);
                Unity_Flip_float2(_TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3, _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip, _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1);
                float _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1;
                Unity_Length_float2(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1);
                float _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0 = _Radius;
                float _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2;
                Unity_Subtract_float(_Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1, _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0, _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2);
                float _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1;
                Unity_Absolute_float(_Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2, _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1);
                float _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0 = _LineWidth;
                float _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2;
                Unity_Subtract_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0, _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2);
                float _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1;
                Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1);
                float _Divide_e7e5a51becee4e03b442703491284618_Out_2;
                Unity_Divide_float(_Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1, _Divide_e7e5a51becee4e03b442703491284618_Out_2);
                float _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1;
                Unity_OneMinus_float(_Divide_e7e5a51becee4e03b442703491284618_Out_2, _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1);
                float _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3;
                Unity_Clamp_float(_OneMinus_3b21457b95c04895886b601f9db54d49_Out_1, 0, 1, _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3);
                float _Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0 = _RemovedSegments;
                float _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0 = _SegmentCount;
                float _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2;
                Unity_Divide_float(6.28318, _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2);
                float _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2;
                Unity_Multiply_float_float(_Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2);
                float _Property_e3cb19cafc414e85a713764652a69ed8_Out_0 = _Rotation;
                float2 _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3;
                Unity_Rotate_Degrees_float(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, float2 (0, 0), _Property_e3cb19cafc414e85a713764652a69ed8_Out_0, _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3);
                float _Split_5ea11c02fe194da4aac6b02be4863e0c_R_1 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[0];
                float _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[1];
                float _Split_5ea11c02fe194da4aac6b02be4863e0c_B_3 = 0;
                float _Split_5ea11c02fe194da4aac6b02be4863e0c_A_4 = 0;
                float _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2;
                Unity_Arctangent2_float(_Split_5ea11c02fe194da4aac6b02be4863e0c_R_1, _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2, _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2);
                float _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2;
                Unity_Add_float(_Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2, 3.14, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2);
                float _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2;
                Unity_Subtract_float(_Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2);
                float _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1;
                Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1);
                float _Divide_738bf564f833485db6c5045406266298_Out_2;
                Unity_Divide_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1, _Divide_738bf564f833485db6c5045406266298_Out_2);
                float _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3;
                Unity_Clamp_float(_Divide_738bf564f833485db6c5045406266298_Out_2, 0, 1, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3);
                float _Subtract_2246bd99d0524309a85d16f952060561_Out_2;
                Unity_Subtract_float(_Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3, _Subtract_2246bd99d0524309a85d16f952060561_Out_2);
                float _Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0 = 5;
                float _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3;
                Unity_Remap_float(_Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0, float2 (1, 2), float2 (0, 6.28318), _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3);
                float _Clamp_f60e8f5b606447a2b130980737de3653_Out_3;
                Unity_Clamp_float(_Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3, 0, 1, _Clamp_f60e8f5b606447a2b130980737de3653_Out_3);
                float _Round_bd8541e176564e7098c9d726b793334f_Out_1;
                Unity_Round_float(_Clamp_f60e8f5b606447a2b130980737de3653_Out_3, _Round_bd8541e176564e7098c9d726b793334f_Out_1);
                float _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2;
                Unity_Divide_float(_Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, 2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2);
                float _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2;
                Unity_Add_float(_Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2);
                float _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2;
                Unity_Modulo_float(_Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2);
                float _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2;
                Unity_Subtract_float(_Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2);
                float _Sine_1d3599fe40e4420283d29abec36debb0_Out_1;
                Unity_Sine_float(_Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2, _Sine_1d3599fe40e4420283d29abec36debb0_Out_1);
                float _Absolute_422ea26f337640a994966391e9b02e4b_Out_1;
                Unity_Absolute_float(_Sine_1d3599fe40e4420283d29abec36debb0_Out_1, _Absolute_422ea26f337640a994966391e9b02e4b_Out_1);
                float2 _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3);
                float _Length_86d9c8de793348acb41e069d1a478594_Out_1;
                Unity_Length_float2(_TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3, _Length_86d9c8de793348acb41e069d1a478594_Out_1);
                float _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2;
                Unity_Multiply_float_float(_Absolute_422ea26f337640a994966391e9b02e4b_Out_1, _Length_86d9c8de793348acb41e069d1a478594_Out_1, _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2);
                float _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0 = _SegmentSpacing;
                float _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2;
                Unity_Subtract_float(_Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2, _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0, _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2);
                float _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1;
                Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1);
                float _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2;
                Unity_Divide_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1, _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2);
                float _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1;
                Unity_OneMinus_float(_Divide_566aae9fc42540dfabdd79989f4cb650_Out_2, _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1);
                float _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3;
                Unity_Clamp_float(_OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1, 0, 1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3);
                float _Multiply_8221929914b040488b6aedec669142ba_Out_2;
                Unity_Multiply_float_float(_Round_bd8541e176564e7098c9d726b793334f_Out_1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3, _Multiply_8221929914b040488b6aedec669142ba_Out_2);
                float _Subtract_856544981c08425aa186ead2e9b9addc_Out_2;
                Unity_Subtract_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, _Multiply_8221929914b040488b6aedec669142ba_Out_2, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2);
                float _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                Unity_Clamp_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, 0, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2, _Clamp_d80b5ce22844457fa4636209261529e6_Out_3);
                float4 _Property_5b8fee2a7de1427cae5a816a48cc2254_Out_0 = _Color;
                float4 _Multiply_90c17c3f3b5140a2845be88b67cd3d2a_Out_2;
                Unity_Multiply_float4_float4((_Clamp_d80b5ce22844457fa4636209261529e6_Out_3.xxxx), _Property_5b8fee2a7de1427cae5a816a48cc2254_Out_0, _Multiply_90c17c3f3b5140a2845be88b67cd3d2a_Out_2);
                surface.BaseColor = (_Multiply_90c17c3f3b5140a2845be88b67cd3d2a_Out_2.xyz);
                surface.Alpha = _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES Attributes
            #define VFX_SRP_VARYINGS Varyings
            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
            #endif
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS.xyz;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

            #ifdef HAVE_VFX_MODIFICATION
                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

            #endif








                #if UNITY_UV_STARTS_AT_TOP
                #else
                #endif


                output.uv0 = input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
            }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

            // --------------------------------------------------
            // Visual Effect Vertex Invocations
            #ifdef HAVE_VFX_MODIFICATION
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
            #endif

            ENDHLSL
            }
            Pass
            {
                Name "SceneSelectionPass"
                Tags
                {
                    "LightMode" = "SceneSelectionPass"
                }

                // Render State
                Cull Off

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                HLSLPROGRAM

                // Pragmas
                #pragma target 2.0
                #pragma exclude_renderers d3d11_9x
                #pragma vertex vert
                #pragma fragment frag

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines

                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_DEPTHONLY
                #define SCENESELECTIONPASS 1

                #define _ALPHATEST_ON 1
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                // custom interpolator pre-include
                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                // --------------------------------------------------
                // Structs and Packing

                // custom interpolators pre packing
                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                struct Attributes
                {
                     float3 positionOS : POSITION;
                     float3 normalOS : NORMAL;
                     float4 tangentOS : TANGENT;
                     float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                     float4 positionCS : SV_POSITION;
                     float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                     float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                     float3 ObjectSpaceNormal;
                     float3 ObjectSpaceTangent;
                     float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                     float4 positionCS : SV_POSITION;
                     float4 interp0 : INTERP0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output;
                    ZERO_INITIALIZE(PackedVaryings, output);
                    output.positionCS = input.positionCS;
                    output.interp0.xyzw = input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.interp0.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }


                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_TexelSize;
                float _Radius;
                float _LineWidth;
                float _Rotation;
                float4 _Color;
                float _RemovedSegments;
                float _SegmentSpacing;
                float _SegmentCount;
                float _Float;
                CBUFFER_END

                    // Object and Global properties
                    TEXTURE2D(_MainTex);
                    SAMPLER(sampler_MainTex);

                    // Graph Includes
                    // GraphIncludes: <None>

                    // -- Property used by ScenePickingPass
                    #ifdef SCENEPICKINGPASS
                    float4 _SelectionID;
                    #endif

                    // -- Properties used by SceneSelectionPass
                    #ifdef SCENESELECTIONPASS
                    int _ObjectId;
                    int _PassValue;
                    #endif

                    // Graph Functions

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }

                    void Unity_Flip_float2(float2 In, float2 Flip, out float2 Out)
                    {
                        Out = (Flip * -2 + 1) * In;
                    }

                    void Unity_Length_float2(float2 In, out float Out)
                    {
                        Out = length(In);
                    }

                    void Unity_Subtract_float(float A, float B, out float Out)
                    {
                        Out = A - B;
                    }

                    void Unity_Absolute_float(float In, out float Out)
                    {
                        Out = abs(In);
                    }

                    void Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(float In, out float Out)
                    {

                                #if defined(SHADER_STAGE_RAY_TRACING)
                                #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                #endif
                        Out = abs(ddx(In)) + abs(ddy(In));
                    }

                    void Unity_Divide_float(float A, float B, out float Out)
                    {
                        Out = A / B;
                    }

                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }

                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }

                    void Unity_Multiply_float_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
                    {
                        //rotation matrix
                        Rotation = Rotation * (3.1415926f / 180.0f);
                        UV -= Center;
                        float s = sin(Rotation);
                        float c = cos(Rotation);

                        //center rotation matrix
                        float2x2 rMatrix = float2x2(c, -s, s, c);
                        rMatrix *= 0.5;
                        rMatrix += 0.5;
                        rMatrix = rMatrix * 2 - 1;

                        //multiply the UVs by the rotation matrix
                        UV.xy = mul(UV.xy, rMatrix);
                        UV += Center;

                        Out = UV;
                    }

                    void Unity_Arctangent2_float(float A, float B, out float Out)
                    {
                        Out = atan2(A, B);
                    }

                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }

                    void Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(float In, out float Out)
                    {

                                #if defined(SHADER_STAGE_RAY_TRACING)
                                #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                #endif
                        Out = abs(ddx(In)) + abs(ddy(In));
                    }

                    void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                    {
                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                    }

                    void Unity_Round_float(float In, out float Out)
                    {
                        Out = round(In);
                    }

                    void Unity_Modulo_float(float A, float B, out float Out)
                    {
                        Out = fmod(A, B);
                    }

                    void Unity_Sine_float(float In, out float Out)
                    {
                        Out = sin(In);
                    }

                    void Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(float In, out float Out)
                    {

                                #if defined(SHADER_STAGE_RAY_TRACING)
                                #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                #endif
                        Out = abs(ddx(In)) + abs(ddy(In));
                    }

                    // Custom interpolators pre vertex
                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                    // Graph Vertex
                    struct VertexDescription
                    {
                        float3 Position;
                        float3 Normal;
                        float3 Tangent;
                    };

                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        description.Position = IN.ObjectSpacePosition;
                        description.Normal = IN.ObjectSpaceNormal;
                        description.Tangent = IN.ObjectSpaceTangent;
                        return description;
                    }

                    // Custom interpolators, pre surface
                    #ifdef FEATURES_GRAPH_VERTEX
                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                    {
                    return output;
                    }
                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                    #endif

                    // Graph Pixel
                    struct SurfaceDescription
                    {
                        float Alpha;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float2 _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3);
                        float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1;
                        float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip = float2 (1, 0);
                        Unity_Flip_float2(_TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3, _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip, _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1);
                        float _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1;
                        Unity_Length_float2(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1);
                        float _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0 = _Radius;
                        float _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2;
                        Unity_Subtract_float(_Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1, _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0, _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2);
                        float _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1;
                        Unity_Absolute_float(_Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2, _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1);
                        float _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0 = _LineWidth;
                        float _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2;
                        Unity_Subtract_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0, _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2);
                        float _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1;
                        Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1);
                        float _Divide_e7e5a51becee4e03b442703491284618_Out_2;
                        Unity_Divide_float(_Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1, _Divide_e7e5a51becee4e03b442703491284618_Out_2);
                        float _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1;
                        Unity_OneMinus_float(_Divide_e7e5a51becee4e03b442703491284618_Out_2, _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1);
                        float _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3;
                        Unity_Clamp_float(_OneMinus_3b21457b95c04895886b601f9db54d49_Out_1, 0, 1, _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3);
                        float _Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0 = _RemovedSegments;
                        float _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0 = _SegmentCount;
                        float _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2;
                        Unity_Divide_float(6.28318, _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2);
                        float _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2;
                        Unity_Multiply_float_float(_Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2);
                        float _Property_e3cb19cafc414e85a713764652a69ed8_Out_0 = _Rotation;
                        float2 _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3;
                        Unity_Rotate_Degrees_float(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, float2 (0, 0), _Property_e3cb19cafc414e85a713764652a69ed8_Out_0, _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3);
                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_R_1 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[0];
                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[1];
                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_B_3 = 0;
                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_A_4 = 0;
                        float _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2;
                        Unity_Arctangent2_float(_Split_5ea11c02fe194da4aac6b02be4863e0c_R_1, _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2, _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2);
                        float _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2;
                        Unity_Add_float(_Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2, 3.14, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2);
                        float _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2;
                        Unity_Subtract_float(_Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2);
                        float _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1;
                        Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1);
                        float _Divide_738bf564f833485db6c5045406266298_Out_2;
                        Unity_Divide_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1, _Divide_738bf564f833485db6c5045406266298_Out_2);
                        float _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3;
                        Unity_Clamp_float(_Divide_738bf564f833485db6c5045406266298_Out_2, 0, 1, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3);
                        float _Subtract_2246bd99d0524309a85d16f952060561_Out_2;
                        Unity_Subtract_float(_Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3, _Subtract_2246bd99d0524309a85d16f952060561_Out_2);
                        float _Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0 = 5;
                        float _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3;
                        Unity_Remap_float(_Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0, float2 (1, 2), float2 (0, 6.28318), _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3);
                        float _Clamp_f60e8f5b606447a2b130980737de3653_Out_3;
                        Unity_Clamp_float(_Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3, 0, 1, _Clamp_f60e8f5b606447a2b130980737de3653_Out_3);
                        float _Round_bd8541e176564e7098c9d726b793334f_Out_1;
                        Unity_Round_float(_Clamp_f60e8f5b606447a2b130980737de3653_Out_3, _Round_bd8541e176564e7098c9d726b793334f_Out_1);
                        float _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2;
                        Unity_Divide_float(_Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, 2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2);
                        float _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2;
                        Unity_Add_float(_Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2);
                        float _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2;
                        Unity_Modulo_float(_Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2);
                        float _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2;
                        Unity_Subtract_float(_Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2);
                        float _Sine_1d3599fe40e4420283d29abec36debb0_Out_1;
                        Unity_Sine_float(_Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2, _Sine_1d3599fe40e4420283d29abec36debb0_Out_1);
                        float _Absolute_422ea26f337640a994966391e9b02e4b_Out_1;
                        Unity_Absolute_float(_Sine_1d3599fe40e4420283d29abec36debb0_Out_1, _Absolute_422ea26f337640a994966391e9b02e4b_Out_1);
                        float2 _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3);
                        float _Length_86d9c8de793348acb41e069d1a478594_Out_1;
                        Unity_Length_float2(_TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3, _Length_86d9c8de793348acb41e069d1a478594_Out_1);
                        float _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2;
                        Unity_Multiply_float_float(_Absolute_422ea26f337640a994966391e9b02e4b_Out_1, _Length_86d9c8de793348acb41e069d1a478594_Out_1, _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2);
                        float _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0 = _SegmentSpacing;
                        float _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2;
                        Unity_Subtract_float(_Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2, _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0, _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2);
                        float _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1;
                        Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1);
                        float _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2;
                        Unity_Divide_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1, _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2);
                        float _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1;
                        Unity_OneMinus_float(_Divide_566aae9fc42540dfabdd79989f4cb650_Out_2, _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1);
                        float _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3;
                        Unity_Clamp_float(_OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1, 0, 1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3);
                        float _Multiply_8221929914b040488b6aedec669142ba_Out_2;
                        Unity_Multiply_float_float(_Round_bd8541e176564e7098c9d726b793334f_Out_1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3, _Multiply_8221929914b040488b6aedec669142ba_Out_2);
                        float _Subtract_856544981c08425aa186ead2e9b9addc_Out_2;
                        Unity_Subtract_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, _Multiply_8221929914b040488b6aedec669142ba_Out_2, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2);
                        float _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                        Unity_Clamp_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, 0, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2, _Clamp_d80b5ce22844457fa4636209261529e6_Out_3);
                        surface.Alpha = _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs
                    #ifdef HAVE_VFX_MODIFICATION
                    #define VFX_SRP_ATTRIBUTES Attributes
                    #define VFX_SRP_VARYINGS Varyings
                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                    #endif
                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                    {
                        VertexDescriptionInputs output;
                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                        output.ObjectSpaceNormal = input.normalOS;
                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                        output.ObjectSpacePosition = input.positionOS;

                        return output;
                    }
                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                    #ifdef HAVE_VFX_MODIFICATION
                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                    #endif








                        #if UNITY_UV_STARTS_AT_TOP
                        #else
                        #endif


                        output.uv0 = input.texCoord0;
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                    }

                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

                    // --------------------------------------------------
                    // Visual Effect Vertex Invocations
                    #ifdef HAVE_VFX_MODIFICATION
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                    #endif

                    ENDHLSL
                    }
                    Pass
                    {
                        Name "ScenePickingPass"
                        Tags
                        {
                            "LightMode" = "Picking"
                        }

                        // Render State
                        Cull Off

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        HLSLPROGRAM

                        // Pragmas
                        #pragma target 2.0
                        #pragma exclude_renderers d3d11_9x
                        #pragma vertex vert
                        #pragma fragment frag

                        // Keywords
                        // PassKeywords: <None>
                        // GraphKeywords: <None>

                        // Defines

                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define ATTRIBUTES_NEED_TEXCOORD0
                        #define VARYINGS_NEED_TEXCOORD0
                        #define FEATURES_GRAPH_VERTEX
                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                        #define SHADERPASS SHADERPASS_DEPTHONLY
                        #define SCENEPICKINGPASS 1

                        #define _ALPHATEST_ON 1
                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                        // custom interpolator pre-include
                        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                        // --------------------------------------------------
                        // Structs and Packing

                        // custom interpolators pre packing
                        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                        struct Attributes
                        {
                             float3 positionOS : POSITION;
                             float3 normalOS : NORMAL;
                             float4 tangentOS : TANGENT;
                             float4 uv0 : TEXCOORD0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                             uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };
                        struct Varyings
                        {
                             float4 positionCS : SV_POSITION;
                             float4 texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                             uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };
                        struct SurfaceDescriptionInputs
                        {
                             float4 uv0;
                        };
                        struct VertexDescriptionInputs
                        {
                             float3 ObjectSpaceNormal;
                             float3 ObjectSpaceTangent;
                             float3 ObjectSpacePosition;
                        };
                        struct PackedVaryings
                        {
                             float4 positionCS : SV_POSITION;
                             float4 interp0 : INTERP0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                             uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output;
                            ZERO_INITIALIZE(PackedVaryings, output);
                            output.positionCS = input.positionCS;
                            output.interp0.xyzw = input.texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output;
                            output.positionCS = input.positionCS;
                            output.texCoord0 = input.interp0.xyzw;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }


                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float4 _MainTex_TexelSize;
                        float _Radius;
                        float _LineWidth;
                        float _Rotation;
                        float4 _Color;
                        float _RemovedSegments;
                        float _SegmentSpacing;
                        float _SegmentCount;
                        float _Float;
                        CBUFFER_END

                            // Object and Global properties
                            TEXTURE2D(_MainTex);
                            SAMPLER(sampler_MainTex);

                            // Graph Includes
                            // GraphIncludes: <None>

                            // -- Property used by ScenePickingPass
                            #ifdef SCENEPICKINGPASS
                            float4 _SelectionID;
                            #endif

                            // -- Properties used by SceneSelectionPass
                            #ifdef SCENESELECTIONPASS
                            int _ObjectId;
                            int _PassValue;
                            #endif

                            // Graph Functions

                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                            {
                                Out = UV * Tiling + Offset;
                            }

                            void Unity_Flip_float2(float2 In, float2 Flip, out float2 Out)
                            {
                                Out = (Flip * -2 + 1) * In;
                            }

                            void Unity_Length_float2(float2 In, out float Out)
                            {
                                Out = length(In);
                            }

                            void Unity_Subtract_float(float A, float B, out float Out)
                            {
                                Out = A - B;
                            }

                            void Unity_Absolute_float(float In, out float Out)
                            {
                                Out = abs(In);
                            }

                            void Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(float In, out float Out)
                            {

                                        #if defined(SHADER_STAGE_RAY_TRACING)
                                        #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                        #endif
                                Out = abs(ddx(In)) + abs(ddy(In));
                            }

                            void Unity_Divide_float(float A, float B, out float Out)
                            {
                                Out = A / B;
                            }

                            void Unity_OneMinus_float(float In, out float Out)
                            {
                                Out = 1 - In;
                            }

                            void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                            {
                                Out = clamp(In, Min, Max);
                            }

                            void Unity_Multiply_float_float(float A, float B, out float Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
                            {
                                //rotation matrix
                                Rotation = Rotation * (3.1415926f / 180.0f);
                                UV -= Center;
                                float s = sin(Rotation);
                                float c = cos(Rotation);

                                //center rotation matrix
                                float2x2 rMatrix = float2x2(c, -s, s, c);
                                rMatrix *= 0.5;
                                rMatrix += 0.5;
                                rMatrix = rMatrix * 2 - 1;

                                //multiply the UVs by the rotation matrix
                                UV.xy = mul(UV.xy, rMatrix);
                                UV += Center;

                                Out = UV;
                            }

                            void Unity_Arctangent2_float(float A, float B, out float Out)
                            {
                                Out = atan2(A, B);
                            }

                            void Unity_Add_float(float A, float B, out float Out)
                            {
                                Out = A + B;
                            }

                            void Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(float In, out float Out)
                            {

                                        #if defined(SHADER_STAGE_RAY_TRACING)
                                        #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                        #endif
                                Out = abs(ddx(In)) + abs(ddy(In));
                            }

                            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                            {
                                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                            }

                            void Unity_Round_float(float In, out float Out)
                            {
                                Out = round(In);
                            }

                            void Unity_Modulo_float(float A, float B, out float Out)
                            {
                                Out = fmod(A, B);
                            }

                            void Unity_Sine_float(float In, out float Out)
                            {
                                Out = sin(In);
                            }

                            void Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(float In, out float Out)
                            {

                                        #if defined(SHADER_STAGE_RAY_TRACING)
                                        #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                        #endif
                                Out = abs(ddx(In)) + abs(ddy(In));
                            }

                            // Custom interpolators pre vertex
                            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                            // Graph Vertex
                            struct VertexDescription
                            {
                                float3 Position;
                                float3 Normal;
                                float3 Tangent;
                            };

                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                            {
                                VertexDescription description = (VertexDescription)0;
                                description.Position = IN.ObjectSpacePosition;
                                description.Normal = IN.ObjectSpaceNormal;
                                description.Tangent = IN.ObjectSpaceTangent;
                                return description;
                            }

                            // Custom interpolators, pre surface
                            #ifdef FEATURES_GRAPH_VERTEX
                            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                            {
                            return output;
                            }
                            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                            #endif

                            // Graph Pixel
                            struct SurfaceDescription
                            {
                                float Alpha;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                float2 _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3);
                                float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1;
                                float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip = float2 (1, 0);
                                Unity_Flip_float2(_TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3, _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip, _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1);
                                float _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1;
                                Unity_Length_float2(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1);
                                float _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0 = _Radius;
                                float _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2;
                                Unity_Subtract_float(_Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1, _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0, _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2);
                                float _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1;
                                Unity_Absolute_float(_Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2, _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1);
                                float _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0 = _LineWidth;
                                float _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2;
                                Unity_Subtract_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0, _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2);
                                float _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1;
                                Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1);
                                float _Divide_e7e5a51becee4e03b442703491284618_Out_2;
                                Unity_Divide_float(_Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1, _Divide_e7e5a51becee4e03b442703491284618_Out_2);
                                float _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1;
                                Unity_OneMinus_float(_Divide_e7e5a51becee4e03b442703491284618_Out_2, _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1);
                                float _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3;
                                Unity_Clamp_float(_OneMinus_3b21457b95c04895886b601f9db54d49_Out_1, 0, 1, _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3);
                                float _Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0 = _RemovedSegments;
                                float _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0 = _SegmentCount;
                                float _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2;
                                Unity_Divide_float(6.28318, _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2);
                                float _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2;
                                Unity_Multiply_float_float(_Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2);
                                float _Property_e3cb19cafc414e85a713764652a69ed8_Out_0 = _Rotation;
                                float2 _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3;
                                Unity_Rotate_Degrees_float(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, float2 (0, 0), _Property_e3cb19cafc414e85a713764652a69ed8_Out_0, _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3);
                                float _Split_5ea11c02fe194da4aac6b02be4863e0c_R_1 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[0];
                                float _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[1];
                                float _Split_5ea11c02fe194da4aac6b02be4863e0c_B_3 = 0;
                                float _Split_5ea11c02fe194da4aac6b02be4863e0c_A_4 = 0;
                                float _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2;
                                Unity_Arctangent2_float(_Split_5ea11c02fe194da4aac6b02be4863e0c_R_1, _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2, _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2);
                                float _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2;
                                Unity_Add_float(_Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2, 3.14, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2);
                                float _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2;
                                Unity_Subtract_float(_Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2);
                                float _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1;
                                Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1);
                                float _Divide_738bf564f833485db6c5045406266298_Out_2;
                                Unity_Divide_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1, _Divide_738bf564f833485db6c5045406266298_Out_2);
                                float _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3;
                                Unity_Clamp_float(_Divide_738bf564f833485db6c5045406266298_Out_2, 0, 1, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3);
                                float _Subtract_2246bd99d0524309a85d16f952060561_Out_2;
                                Unity_Subtract_float(_Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3, _Subtract_2246bd99d0524309a85d16f952060561_Out_2);
                                float _Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0 = 5;
                                float _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3;
                                Unity_Remap_float(_Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0, float2 (1, 2), float2 (0, 6.28318), _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3);
                                float _Clamp_f60e8f5b606447a2b130980737de3653_Out_3;
                                Unity_Clamp_float(_Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3, 0, 1, _Clamp_f60e8f5b606447a2b130980737de3653_Out_3);
                                float _Round_bd8541e176564e7098c9d726b793334f_Out_1;
                                Unity_Round_float(_Clamp_f60e8f5b606447a2b130980737de3653_Out_3, _Round_bd8541e176564e7098c9d726b793334f_Out_1);
                                float _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2;
                                Unity_Divide_float(_Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, 2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2);
                                float _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2;
                                Unity_Add_float(_Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2);
                                float _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2;
                                Unity_Modulo_float(_Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2);
                                float _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2;
                                Unity_Subtract_float(_Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2);
                                float _Sine_1d3599fe40e4420283d29abec36debb0_Out_1;
                                Unity_Sine_float(_Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2, _Sine_1d3599fe40e4420283d29abec36debb0_Out_1);
                                float _Absolute_422ea26f337640a994966391e9b02e4b_Out_1;
                                Unity_Absolute_float(_Sine_1d3599fe40e4420283d29abec36debb0_Out_1, _Absolute_422ea26f337640a994966391e9b02e4b_Out_1);
                                float2 _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3);
                                float _Length_86d9c8de793348acb41e069d1a478594_Out_1;
                                Unity_Length_float2(_TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3, _Length_86d9c8de793348acb41e069d1a478594_Out_1);
                                float _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2;
                                Unity_Multiply_float_float(_Absolute_422ea26f337640a994966391e9b02e4b_Out_1, _Length_86d9c8de793348acb41e069d1a478594_Out_1, _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2);
                                float _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0 = _SegmentSpacing;
                                float _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2;
                                Unity_Subtract_float(_Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2, _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0, _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2);
                                float _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1;
                                Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1);
                                float _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2;
                                Unity_Divide_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1, _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2);
                                float _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1;
                                Unity_OneMinus_float(_Divide_566aae9fc42540dfabdd79989f4cb650_Out_2, _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1);
                                float _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3;
                                Unity_Clamp_float(_OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1, 0, 1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3);
                                float _Multiply_8221929914b040488b6aedec669142ba_Out_2;
                                Unity_Multiply_float_float(_Round_bd8541e176564e7098c9d726b793334f_Out_1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3, _Multiply_8221929914b040488b6aedec669142ba_Out_2);
                                float _Subtract_856544981c08425aa186ead2e9b9addc_Out_2;
                                Unity_Subtract_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, _Multiply_8221929914b040488b6aedec669142ba_Out_2, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2);
                                float _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                                Unity_Clamp_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, 0, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2, _Clamp_d80b5ce22844457fa4636209261529e6_Out_3);
                                surface.Alpha = _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Build Graph Inputs
                            #ifdef HAVE_VFX_MODIFICATION
                            #define VFX_SRP_ATTRIBUTES Attributes
                            #define VFX_SRP_VARYINGS Varyings
                            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                            #endif
                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                            {
                                VertexDescriptionInputs output;
                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                output.ObjectSpaceNormal = input.normalOS;
                                output.ObjectSpaceTangent = input.tangentOS.xyz;
                                output.ObjectSpacePosition = input.positionOS;

                                return output;
                            }
                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                            #ifdef HAVE_VFX_MODIFICATION
                                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                            #endif








                                #if UNITY_UV_STARTS_AT_TOP
                                #else
                                #endif


                                output.uv0 = input.texCoord0;
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif
                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                    return output;
                            }

                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

                            // --------------------------------------------------
                            // Visual Effect Vertex Invocations
                            #ifdef HAVE_VFX_MODIFICATION
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                            #endif

                            ENDHLSL
                            }
                            Pass
                            {
                                Name "Sprite Unlit"
                                Tags
                                {
                                    "LightMode" = "UniversalForward"
                                }

                                // Render State
                                Cull Off
                                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                ZTest LEqual
                                ZWrite Off

                                // Debug
                                // <None>

                                // --------------------------------------------------
                                // Pass

                                HLSLPROGRAM

                                // Pragmas
                                #pragma target 2.0
                                #pragma exclude_renderers d3d11_9x
                                #pragma vertex vert
                                #pragma fragment frag

                                // Keywords
                                #pragma multi_compile_fragment _ DEBUG_DISPLAY
                                // GraphKeywords: <None>

                                // Defines

                                #define ATTRIBUTES_NEED_NORMAL
                                #define ATTRIBUTES_NEED_TANGENT
                                #define ATTRIBUTES_NEED_TEXCOORD0
                                #define ATTRIBUTES_NEED_COLOR
                                #define VARYINGS_NEED_POSITION_WS
                                #define VARYINGS_NEED_TEXCOORD0
                                #define VARYINGS_NEED_COLOR
                                #define FEATURES_GRAPH_VERTEX
                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                #define SHADERPASS SHADERPASS_SPRITEFORWARD
                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                // custom interpolator pre-include
                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                // Includes
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                // --------------------------------------------------
                                // Structs and Packing

                                // custom interpolators pre packing
                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                struct Attributes
                                {
                                     float3 positionOS : POSITION;
                                     float3 normalOS : NORMAL;
                                     float4 tangentOS : TANGENT;
                                     float4 uv0 : TEXCOORD0;
                                     float4 color : COLOR;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : INSTANCEID_SEMANTIC;
                                    #endif
                                };
                                struct Varyings
                                {
                                     float4 positionCS : SV_POSITION;
                                     float3 positionWS;
                                     float4 texCoord0;
                                     float4 color;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };
                                struct SurfaceDescriptionInputs
                                {
                                     float4 uv0;
                                };
                                struct VertexDescriptionInputs
                                {
                                     float3 ObjectSpaceNormal;
                                     float3 ObjectSpaceTangent;
                                     float3 ObjectSpacePosition;
                                };
                                struct PackedVaryings
                                {
                                     float4 positionCS : SV_POSITION;
                                     float3 interp0 : INTERP0;
                                     float4 interp1 : INTERP1;
                                     float4 interp2 : INTERP2;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };

                                PackedVaryings PackVaryings(Varyings input)
                                {
                                    PackedVaryings output;
                                    ZERO_INITIALIZE(PackedVaryings, output);
                                    output.positionCS = input.positionCS;
                                    output.interp0.xyz = input.positionWS;
                                    output.interp1.xyzw = input.texCoord0;
                                    output.interp2.xyzw = input.color;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }

                                Varyings UnpackVaryings(PackedVaryings input)
                                {
                                    Varyings output;
                                    output.positionCS = input.positionCS;
                                    output.positionWS = input.interp0.xyz;
                                    output.texCoord0 = input.interp1.xyzw;
                                    output.color = input.interp2.xyzw;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }


                                // --------------------------------------------------
                                // Graph

                                // Graph Properties
                                CBUFFER_START(UnityPerMaterial)
                                float4 _MainTex_TexelSize;
                                float _Radius;
                                float _LineWidth;
                                float _Rotation;
                                float4 _Color;
                                float _RemovedSegments;
                                float _SegmentSpacing;
                                float _SegmentCount;
                                float _Float;
                                CBUFFER_END

                                    // Object and Global properties
                                    TEXTURE2D(_MainTex);
                                    SAMPLER(sampler_MainTex);

                                    // Graph Includes
                                    // GraphIncludes: <None>

                                    // -- Property used by ScenePickingPass
                                    #ifdef SCENEPICKINGPASS
                                    float4 _SelectionID;
                                    #endif

                                    // -- Properties used by SceneSelectionPass
                                    #ifdef SCENESELECTIONPASS
                                    int _ObjectId;
                                    int _PassValue;
                                    #endif

                                    // Graph Functions

                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                    {
                                        Out = UV * Tiling + Offset;
                                    }

                                    void Unity_Flip_float2(float2 In, float2 Flip, out float2 Out)
                                    {
                                        Out = (Flip * -2 + 1) * In;
                                    }

                                    void Unity_Length_float2(float2 In, out float Out)
                                    {
                                        Out = length(In);
                                    }

                                    void Unity_Subtract_float(float A, float B, out float Out)
                                    {
                                        Out = A - B;
                                    }

                                    void Unity_Absolute_float(float In, out float Out)
                                    {
                                        Out = abs(In);
                                    }

                                    void Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(float In, out float Out)
                                    {

                                                #if defined(SHADER_STAGE_RAY_TRACING)
                                                #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                                #endif
                                        Out = abs(ddx(In)) + abs(ddy(In));
                                    }

                                    void Unity_Divide_float(float A, float B, out float Out)
                                    {
                                        Out = A / B;
                                    }

                                    void Unity_OneMinus_float(float In, out float Out)
                                    {
                                        Out = 1 - In;
                                    }

                                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                                    {
                                        Out = clamp(In, Min, Max);
                                    }

                                    void Unity_Multiply_float_float(float A, float B, out float Out)
                                    {
                                        Out = A * B;
                                    }

                                    void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
                                    {
                                        //rotation matrix
                                        Rotation = Rotation * (3.1415926f / 180.0f);
                                        UV -= Center;
                                        float s = sin(Rotation);
                                        float c = cos(Rotation);

                                        //center rotation matrix
                                        float2x2 rMatrix = float2x2(c, -s, s, c);
                                        rMatrix *= 0.5;
                                        rMatrix += 0.5;
                                        rMatrix = rMatrix * 2 - 1;

                                        //multiply the UVs by the rotation matrix
                                        UV.xy = mul(UV.xy, rMatrix);
                                        UV += Center;

                                        Out = UV;
                                    }

                                    void Unity_Arctangent2_float(float A, float B, out float Out)
                                    {
                                        Out = atan2(A, B);
                                    }

                                    void Unity_Add_float(float A, float B, out float Out)
                                    {
                                        Out = A + B;
                                    }

                                    void Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(float In, out float Out)
                                    {

                                                #if defined(SHADER_STAGE_RAY_TRACING)
                                                #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                                #endif
                                        Out = abs(ddx(In)) + abs(ddy(In));
                                    }

                                    void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                                    {
                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                    }

                                    void Unity_Round_float(float In, out float Out)
                                    {
                                        Out = round(In);
                                    }

                                    void Unity_Modulo_float(float A, float B, out float Out)
                                    {
                                        Out = fmod(A, B);
                                    }

                                    void Unity_Sine_float(float In, out float Out)
                                    {
                                        Out = sin(In);
                                    }

                                    void Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(float In, out float Out)
                                    {

                                                #if defined(SHADER_STAGE_RAY_TRACING)
                                                #error 'DDXY' node is not supported in ray tracing, please provide an alternate implementation, relying for instance on the 'Raytracing Quality' keyword
                                                #endif
                                        Out = abs(ddx(In)) + abs(ddy(In));
                                    }

                                    void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
                                    {
                                        Out = A * B;
                                    }

                                    // Custom interpolators pre vertex
                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                    // Graph Vertex
                                    struct VertexDescription
                                    {
                                        float3 Position;
                                        float3 Normal;
                                        float3 Tangent;
                                    };

                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                    {
                                        VertexDescription description = (VertexDescription)0;
                                        description.Position = IN.ObjectSpacePosition;
                                        description.Normal = IN.ObjectSpaceNormal;
                                        description.Tangent = IN.ObjectSpaceTangent;
                                        return description;
                                    }

                                    // Custom interpolators, pre surface
                                    #ifdef FEATURES_GRAPH_VERTEX
                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                    {
                                    return output;
                                    }
                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                    #endif

                                    // Graph Pixel
                                    struct SurfaceDescription
                                    {
                                        float3 BaseColor;
                                        float Alpha;
                                    };

                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                    {
                                        SurfaceDescription surface = (SurfaceDescription)0;
                                        float2 _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3;
                                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3);
                                        float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1;
                                        float2 _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip = float2 (1, 0);
                                        Unity_Flip_float2(_TilingAndOffset_e64962709a7147cb9c5ab9f392db8423_Out_3, _Flip_e75d75ac770d4225a165d6e528ff10c2_Flip, _Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1);
                                        float _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1;
                                        Unity_Length_float2(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, _Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1);
                                        float _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0 = _Radius;
                                        float _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2;
                                        Unity_Subtract_float(_Length_dcd46cedf4b74177a37eab2b7146e69c_Out_1, _Property_39ea3bd507bb4228a579dd4ce19896e2_Out_0, _Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2);
                                        float _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1;
                                        Unity_Absolute_float(_Subtract_4a1e8cd0ee9b4d92b8a07ffe68dad2da_Out_2, _Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1);
                                        float _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0 = _LineWidth;
                                        float _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2;
                                        Unity_Subtract_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _Property_62eaf2068ef84659a8a801f66ed1cd47_Out_0, _Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2);
                                        float _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1;
                                        Unity_DDXY_7203497a4a584adcb9ea9af0c90b9a08_float(_Absolute_253f6bba381e4922b86501d6b6c5103c_Out_1, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1);
                                        float _Divide_e7e5a51becee4e03b442703491284618_Out_2;
                                        Unity_Divide_float(_Subtract_688749bfb2984f4f93795ecfe8664f18_Out_2, _DDXY_7203497a4a584adcb9ea9af0c90b9a08_Out_1, _Divide_e7e5a51becee4e03b442703491284618_Out_2);
                                        float _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1;
                                        Unity_OneMinus_float(_Divide_e7e5a51becee4e03b442703491284618_Out_2, _OneMinus_3b21457b95c04895886b601f9db54d49_Out_1);
                                        float _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3;
                                        Unity_Clamp_float(_OneMinus_3b21457b95c04895886b601f9db54d49_Out_1, 0, 1, _Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3);
                                        float _Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0 = _RemovedSegments;
                                        float _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0 = _SegmentCount;
                                        float _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2;
                                        Unity_Divide_float(6.28318, _Property_e8141b98ec4a41f99dadcc0f1e60ba2f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2);
                                        float _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2;
                                        Unity_Multiply_float_float(_Property_22948304f41c4f4eb72c2af9bb6b2a0f_Out_0, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2);
                                        float _Property_e3cb19cafc414e85a713764652a69ed8_Out_0 = _Rotation;
                                        float2 _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3;
                                        Unity_Rotate_Degrees_float(_Flip_e75d75ac770d4225a165d6e528ff10c2_Out_1, float2 (0, 0), _Property_e3cb19cafc414e85a713764652a69ed8_Out_0, _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3);
                                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_R_1 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[0];
                                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2 = _Rotate_570b0b6237af4e7883685e1f4e50115b_Out_3[1];
                                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_B_3 = 0;
                                        float _Split_5ea11c02fe194da4aac6b02be4863e0c_A_4 = 0;
                                        float _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2;
                                        Unity_Arctangent2_float(_Split_5ea11c02fe194da4aac6b02be4863e0c_R_1, _Split_5ea11c02fe194da4aac6b02be4863e0c_G_2, _Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2);
                                        float _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2;
                                        Unity_Add_float(_Arctangent2_5afa1e01f41c4737b59afdbc4c492a0d_Out_2, 3.14, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2);
                                        float _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2;
                                        Unity_Subtract_float(_Multiply_26349e828a7e48fe8110298ba33a31f2_Out_2, _Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2);
                                        float _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1;
                                        Unity_DDXY_5d7e993d0985405fae4442e035d53a2c_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1);
                                        float _Divide_738bf564f833485db6c5045406266298_Out_2;
                                        Unity_Divide_float(_Subtract_5c06f410de324152bdfed2ea4723c4ed_Out_2, _DDXY_5d7e993d0985405fae4442e035d53a2c_Out_1, _Divide_738bf564f833485db6c5045406266298_Out_2);
                                        float _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3;
                                        Unity_Clamp_float(_Divide_738bf564f833485db6c5045406266298_Out_2, 0, 1, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3);
                                        float _Subtract_2246bd99d0524309a85d16f952060561_Out_2;
                                        Unity_Subtract_float(_Clamp_a043a65142684d4cb24e231ffa27c50b_Out_3, _Clamp_af5ee3e5549644a68fe04134a92049a3_Out_3, _Subtract_2246bd99d0524309a85d16f952060561_Out_2);
                                        float _Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0 = 5;
                                        float _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3;
                                        Unity_Remap_float(_Float_4b61f003c41c4e4a824f718a68d0dee0_Out_0, float2 (1, 2), float2 (0, 6.28318), _Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3);
                                        float _Clamp_f60e8f5b606447a2b130980737de3653_Out_3;
                                        Unity_Clamp_float(_Remap_1b9959b147264aa892a85c4bb78f53f8_Out_3, 0, 1, _Clamp_f60e8f5b606447a2b130980737de3653_Out_3);
                                        float _Round_bd8541e176564e7098c9d726b793334f_Out_1;
                                        Unity_Round_float(_Clamp_f60e8f5b606447a2b130980737de3653_Out_3, _Round_bd8541e176564e7098c9d726b793334f_Out_1);
                                        float _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2;
                                        Unity_Divide_float(_Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, 2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2);
                                        float _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2;
                                        Unity_Add_float(_Add_d35ec247818444439b5b3f08cb0e73ee_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2);
                                        float _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2;
                                        Unity_Modulo_float(_Add_1ab5bc3c772f4948a68bc0eac727a0c5_Out_2, _Divide_9a5ea228fb6d445191b7c8df01874fd2_Out_2, _Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2);
                                        float _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2;
                                        Unity_Subtract_float(_Modulo_5d4ea96e9b84403d9f394ea89bc7c7d4_Out_2, _Divide_650e6897204a4bbaa963143f9d99c9df_Out_2, _Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2);
                                        float _Sine_1d3599fe40e4420283d29abec36debb0_Out_1;
                                        Unity_Sine_float(_Subtract_6fda5f4f2484435aafcdbb2ecc881df0_Out_2, _Sine_1d3599fe40e4420283d29abec36debb0_Out_1);
                                        float _Absolute_422ea26f337640a994966391e9b02e4b_Out_1;
                                        Unity_Absolute_float(_Sine_1d3599fe40e4420283d29abec36debb0_Out_1, _Absolute_422ea26f337640a994966391e9b02e4b_Out_1);
                                        float2 _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3;
                                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3);
                                        float _Length_86d9c8de793348acb41e069d1a478594_Out_1;
                                        Unity_Length_float2(_TilingAndOffset_8234599152694f0dbbb07f3e54f6b510_Out_3, _Length_86d9c8de793348acb41e069d1a478594_Out_1);
                                        float _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2;
                                        Unity_Multiply_float_float(_Absolute_422ea26f337640a994966391e9b02e4b_Out_1, _Length_86d9c8de793348acb41e069d1a478594_Out_1, _Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2);
                                        float _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0 = _SegmentSpacing;
                                        float _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2;
                                        Unity_Subtract_float(_Multiply_6e6c1e812bfc4dae8052a8071963a03e_Out_2, _Property_f8a1ee9354e14a9b861f56ff996e1511_Out_0, _Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2);
                                        float _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1;
                                        Unity_DDXY_dd504df98e5548619c2beb268b9829ee_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1);
                                        float _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2;
                                        Unity_Divide_float(_Subtract_4b0d32eb44244b209e125a496ddf5dd8_Out_2, _DDXY_dd504df98e5548619c2beb268b9829ee_Out_1, _Divide_566aae9fc42540dfabdd79989f4cb650_Out_2);
                                        float _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1;
                                        Unity_OneMinus_float(_Divide_566aae9fc42540dfabdd79989f4cb650_Out_2, _OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1);
                                        float _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3;
                                        Unity_Clamp_float(_OneMinus_0a82f9fab80244709039f9d1f2481b85_Out_1, 0, 1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3);
                                        float _Multiply_8221929914b040488b6aedec669142ba_Out_2;
                                        Unity_Multiply_float_float(_Round_bd8541e176564e7098c9d726b793334f_Out_1, _Clamp_b46e739e6ad844a1aefbd29e101cdab5_Out_3, _Multiply_8221929914b040488b6aedec669142ba_Out_2);
                                        float _Subtract_856544981c08425aa186ead2e9b9addc_Out_2;
                                        Unity_Subtract_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, _Multiply_8221929914b040488b6aedec669142ba_Out_2, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2);
                                        float _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                                        Unity_Clamp_float(_Subtract_2246bd99d0524309a85d16f952060561_Out_2, 0, _Subtract_856544981c08425aa186ead2e9b9addc_Out_2, _Clamp_d80b5ce22844457fa4636209261529e6_Out_3);
                                        float4 _Property_5b8fee2a7de1427cae5a816a48cc2254_Out_0 = _Color;
                                        float4 _Multiply_90c17c3f3b5140a2845be88b67cd3d2a_Out_2;
                                        Unity_Multiply_float4_float4((_Clamp_d80b5ce22844457fa4636209261529e6_Out_3.xxxx), _Property_5b8fee2a7de1427cae5a816a48cc2254_Out_0, _Multiply_90c17c3f3b5140a2845be88b67cd3d2a_Out_2);
                                        surface.BaseColor = (_Multiply_90c17c3f3b5140a2845be88b67cd3d2a_Out_2.xyz);
                                        surface.Alpha = _Clamp_d80b5ce22844457fa4636209261529e6_Out_3;
                                        return surface;
                                    }

                                    // --------------------------------------------------
                                    // Build Graph Inputs
                                    #ifdef HAVE_VFX_MODIFICATION
                                    #define VFX_SRP_ATTRIBUTES Attributes
                                    #define VFX_SRP_VARYINGS Varyings
                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                    #endif
                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                    {
                                        VertexDescriptionInputs output;
                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                        output.ObjectSpaceNormal = input.normalOS;
                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                        output.ObjectSpacePosition = input.positionOS;

                                        return output;
                                    }
                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                    {
                                        SurfaceDescriptionInputs output;
                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                    #ifdef HAVE_VFX_MODIFICATION
                                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                    #endif








                                        #if UNITY_UV_STARTS_AT_TOP
                                        #else
                                        #endif


                                        output.uv0 = input.texCoord0;
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                    #else
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                    #endif
                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                            return output;
                                    }

                                    // --------------------------------------------------
                                    // Main

                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

                                    // --------------------------------------------------
                                    // Visual Effect Vertex Invocations
                                    #ifdef HAVE_VFX_MODIFICATION
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                    #endif

                                    ENDHLSL
                                    }
    }
        CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
                                        FallBack "Hidden/Shader Graph/FallbackError"
}