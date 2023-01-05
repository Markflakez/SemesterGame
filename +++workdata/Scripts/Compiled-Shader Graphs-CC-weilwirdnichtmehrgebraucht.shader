// Compiled shader for Windows, Mac, Linux

//////////////////////////////////////////////////////////////////////////
// 
// NOTE: This is *not* a valid shader file, the contents are provided just
// for information and for debugging purposes only.
// 
//////////////////////////////////////////////////////////////////////////
// Skipping shader variants that would not be included into build of current scene.

Shader "Shader Graphs/CC" {
Properties {
[NoScaleOffset]  _MainTex ("_MainTex", 2D) = "white" { }
 _Radius ("_Radius", Float) = 0.400000
 _LineWidth ("_LineWidth", Float) = 0.070000
 _Color ("_Color", Color) = (1.000000,1.000000,1.000000,1.000000)
 _Rotation ("_Rotation", Float) = 0.000000
 _RemovedSegments ("_RemovedSegments", Float) = 0.000000
 _SegmentSpacing ("_SegmentSpacing", Float) = 0.030000
 _SegmentCount ("_SegmentCount", Float) = 5.000000
 _Float ("Float", Float) = 16.000000
[HideInInspector] [NoScaleOffset]  unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" { }
[HideInInspector] [NoScaleOffset]  unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" { }
[HideInInspector] [NoScaleOffset]  unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" { }
}
SubShader { 
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "UniversalMaterialType"="Unlit" "ShaderGraphShader"="true" "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget" }


 // Stats for Vertex shader:
 //        d3d11: 9 math
 // Stats for Fragment shader:
 //        d3d11: 68 avg math (68..69), 4 avg branch (0..9)
 Pass {
  Name "Sprite Unlit"
  Tags { "LIGHTMODE"="Universal2D" "QUEUE"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "UniversalMaterialType"="Unlit" "ShaderGraphShader"="true" "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
  //////////////////////////////////
  //                              //
  //      Compiled programs       //
  //                              //
  //////////////////////////////////
//////////////////////////////////////////////////////
Keywords: <none>
-- Hardware tier variant: Tier 1
-- Vertex shader for "d3d11":
// Stats: 9 math, 2 temp registers
Uses vertex data channel "Vertex"
Uses vertex data channel "TexCoord0"
Uses vertex data channel "Color"

Constant Buffer "$Globals" (2000 bytes) on slot 0 {
  Matrix4x4 unity_MatrixVP at 1152
  Vector4 _RendererColor at 1984
}
Constant Buffer "UnityPerDraw" (656 bytes) on slot 1 {
  Matrix4x4 unity_ObjectToWorld at 0
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyz         0     NONE   float   xyz 
// NORMAL                   0   xyz         1     NONE   float       
// TANGENT                  0   xyzw        2     NONE   float       
// TEXCOORD                 0   xyzw        3     NONE   float   xyzw
// COLOR                    0   xyzw        4     NONE   float   xyzw
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// INTERP                   0   xyz         1     NONE   float   xyz 
// INTERP                   1   xyzw        2     NONE   float   xyzw
// INTERP                   2   xyzw        3     NONE   float   xyzw
//
      vs_4_0
      dcl_constantbuffer CB0[125], immediateIndexed
      dcl_constantbuffer CB1[4], immediateIndexed
      dcl_input v0.xyz
      dcl_input v3.xyzw
      dcl_input v4.xyzw
      dcl_output_siv o0.xyzw, position
      dcl_output o1.xyz
      dcl_output o2.xyzw
      dcl_output o3.xyzw
      dcl_temps 2
   0: mul r0.xyz, v0.yyyy, cb1[1].xyzx
   1: mad r0.xyz, cb1[0].xyzx, v0.xxxx, r0.xyzx
   2: mad r0.xyz, cb1[2].xyzx, v0.zzzz, r0.xyzx
   3: add r0.xyz, r0.xyzx, cb1[3].xyzx
   4: mul r1.xyzw, r0.yyyy, cb0[73].xyzw
   5: mad r1.xyzw, cb0[72].xyzw, r0.xxxx, r1.xyzw
   6: mad r1.xyzw, cb0[74].xyzw, r0.zzzz, r1.xyzw
   7: mov o1.xyz, r0.xyzx
   8: add o0.xyzw, r1.xyzw, cb0[75].xyzw
   9: mov o2.xyzw, v3.xyzw
  10: mul o3.xyzw, v4.xyzw, cb0[124].xyzw
  11: ret 
// Approximately 0 instruction slots used


-- Hardware tier variant: Tier 1
-- Fragment shader for "d3d11":
// Stats: 68 math, 4 temp registers
Constant Buffer "UnityPerMaterial" (64 bytes) on slot 0 {
  Float _Radius at 16
  Float _LineWidth at 20
  Float _Rotation at 24
  Vector4 _Color at 32
  Float _RemovedSegments at 48
  Float _SegmentSpacing at 52
  Float _SegmentCount at 56
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// INTERP                   0   xyz         1     NONE   float       
// INTERP                   1   xyzw        2     NONE   float   xy  
// INTERP                   2   xyzw        3     NONE   float   xyzw
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_TARGET                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_constantbuffer CB0[4], immediateIndexed
      dcl_input_ps linear v2.xy
      dcl_input_ps linear v3.xyzw
      dcl_output o0.xyzw
      dcl_temps 4
   0: add r0.xy, v2.xyxx, l(-0.500000, -0.500000, 0.000000, 0.000000)
   1: mul r0.zw, r0.xxxy, l(0.000000, 0.000000, -1.000000, 1.000000)
   2: dp2 r1.x, r0.zwzz, r0.zwzz
   3: sqrt r1.x, r1.x
   4: add r1.x, r1.x, -cb0[1].x
   5: mov r1.x, |r1.x|
   6: add r1.y, r1.x, -cb0[1].y
   7: deriv_rtx r1.z, r1.x
   8: deriv_rty r1.x, r1.x
   9: add r1.x, |r1.x|, |r1.z|
  10: div r1.x, r1.y, r1.x
  11: add_sat r1.x, -r1.x, l(1.000000)
  12: div r1.y, l(6.283180), cb0[3].z
  13: mul r1.z, cb0[1].z, l(0.017453)
  14: sincos r2.x, r3.x, r1.z
  15: mov r3.yz, r2.xxxx
  16: mad r2.xyz, r3.xyzx, l(0.500000, -0.500000, 0.500000, 0.000000), l(0.500000, 0.500000, 0.500000, 0.000000)
  17: mad r2.xyz, r2.xyzx, l(2.000000, 2.000000, 2.000000, 0.000000), l(-1.000000, -1.000000, -1.000000, 0.000000)
  18: dp2 r1.z, r0.zwzz, r2.xzxx
  19: dp2 r0.z, r0.wzww, r2.xyxx
  20: min r0.w, |r0.z|, |r1.z|
  21: max r1.w, |r0.z|, |r1.z|
  22: div r1.w, l(1.000000, 1.000000, 1.000000, 1.000000), r1.w
  23: mul r0.w, r0.w, r1.w
  24: mul r1.w, r0.w, r0.w
  25: mad r2.x, r1.w, l(0.020835), l(-0.085133)
  26: mad r2.x, r1.w, r2.x, l(0.180141)
  27: mad r2.x, r1.w, r2.x, l(-0.330299)
  28: mad r1.w, r1.w, r2.x, l(0.999866)
  29: mul r2.x, r0.w, r1.w
  30: lt r2.y, |r0.z|, |r1.z|
  31: mad r2.x, r2.x, l(-2.000000), l(1.570796)
  32: and r2.x, r2.y, r2.x
  33: mad r0.w, r0.w, r1.w, r2.x
  34: lt r1.w, r0.z, -r0.z
  35: and r1.w, r1.w, l(0xc0490fdb)
  36: add r0.w, r0.w, r1.w
  37: min r1.w, r0.z, r1.z
  38: max r0.z, r0.z, r1.z
  39: lt r1.z, r1.w, -r1.w
  40: ge r0.z, r0.z, -r0.z
  41: and r0.z, r0.z, r1.z
  42: movc r0.z, r0.z, -r0.w, r0.w
  43: add r0.z, r0.z, l(3.141000)
  44: mad r0.w, cb0[3].x, r1.y, -r0.z
  45: deriv_rtx r1.z, r0.w
  46: deriv_rty r1.w, r0.w
  47: add r1.z, |r1.w|, |r1.z|
  48: div_sat r0.w, r0.w, r1.z
  49: add r0.w, -r0.w, r1.x
  50: mul r1.x, r1.y, l(0.500000)
  51: mad r0.z, r1.y, l(0.500000), r0.z
  52: div r0.z, r0.z, r1.y
  53: ge r1.z, r0.z, -r0.z
  54: frc r0.z, |r0.z|
  55: movc r0.z, r1.z, r0.z, -r0.z
  56: mad r0.z, r0.z, r1.y, -r1.x
  57: sincos r0.z, null, r0.z
  58: dp2 r0.x, r0.xyxx, r0.xyxx
  59: sqrt r0.x, r0.x
  60: mad r0.x, |r0.z|, r0.x, -cb0[3].y
  61: deriv_rtx r0.y, r0.x
  62: deriv_rty r0.z, r0.x
  63: add r0.y, |r0.z|, |r0.y|
  64: div r0.x, r0.x, r0.y
  65: add_sat r0.x, -r0.x, l(1.000000)
  66: add r0.x, -r0.x, r0.w
  67: max r0.y, r0.w, l(0.000000)
  68: min r0.x, r0.x, r0.y
  69: round_ni r1.w, r0.x
  70: eq r0.y, r1.w, l(0.000000)
  71: discard_nz r0.y
  72: mul r1.xyz, r0.xxxx, cb0[2].xyzx
  73: mul o0.xyzw, r1.xyzw, v3.xyzw
  74: ret 
// Approximately 0 instruction slots used


//////////////////////////////////////////////////////
Keywords: DEBUG_DISPLAY
-- Vertex shader for "d3d11":
// No shader variant for this keyword set. The closest match will be used instead.

-- Hardware tier variant: Tier 1
-- Fragment shader for "d3d11":
// Stats: 69 math, 5 temp registers, 9 branches
Constant Buffer "$Globals" (2144 bytes) on slot 0 {
  ScalarInt _DebugMaterialMode at 1968
  ScalarInt _DebugSceneOverrideMode at 1984
  Vector4 _DebugColor at 2048
  Vector4 _DebugColorInvalidMode at 2064
}
Constant Buffer "UnityPerMaterial" (64 bytes) on slot 1 {
  Float _Radius at 16
  Float _LineWidth at 20
  Float _Rotation at 24
  Vector4 _Color at 32
  Float _RemovedSegments at 48
  Float _SegmentSpacing at 52
  Float _SegmentCount at 56
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// INTERP                   0   xyz         1     NONE   float       
// INTERP                   1   xyzw        2     NONE   float   xy  
// INTERP                   2   xyzw        3     NONE   float   xyzw
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_TARGET                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_constantbuffer CB0[130], immediateIndexed
      dcl_constantbuffer CB1[4], immediateIndexed
      dcl_input_ps linear v2.xy
      dcl_input_ps linear v3.xyzw
      dcl_output o0.xyzw
      dcl_temps 5
   0: add r0.xy, v2.xyxx, l(-0.500000, -0.500000, 0.000000, 0.000000)
   1: mul r0.zw, r0.xxxy, l(0.000000, 0.000000, -1.000000, 1.000000)
   2: dp2 r1.x, r0.zwzz, r0.zwzz
   3: sqrt r1.x, r1.x
   4: add r1.x, r1.x, -cb1[1].x
   5: mov r1.x, |r1.x|
   6: add r1.y, r1.x, -cb1[1].y
   7: deriv_rtx r1.z, r1.x
   8: deriv_rty r1.x, r1.x
   9: add r1.x, |r1.x|, |r1.z|
  10: div r1.x, r1.y, r1.x
  11: add_sat r1.x, -r1.x, l(1.000000)
  12: div r1.y, l(6.283180), cb1[3].z
  13: mul r1.z, cb1[1].z, l(0.017453)
  14: sincos r2.x, r3.x, r1.z
  15: mov r3.yz, r2.xxxx
  16: mad r2.xyz, r3.xyzx, l(0.500000, -0.500000, 0.500000, 0.000000), l(0.500000, 0.500000, 0.500000, 0.000000)
  17: mad r2.xyz, r2.xyzx, l(2.000000, 2.000000, 2.000000, 0.000000), l(-1.000000, -1.000000, -1.000000, 0.000000)
  18: dp2 r1.z, r0.zwzz, r2.xzxx
  19: dp2 r0.z, r0.wzww, r2.xyxx
  20: min r0.w, |r0.z|, |r1.z|
  21: max r1.w, |r0.z|, |r1.z|
  22: div r1.w, l(1.000000, 1.000000, 1.000000, 1.000000), r1.w
  23: mul r0.w, r0.w, r1.w
  24: mul r1.w, r0.w, r0.w
  25: mad r2.x, r1.w, l(0.020835), l(-0.085133)
  26: mad r2.x, r1.w, r2.x, l(0.180141)
  27: mad r2.x, r1.w, r2.x, l(-0.330299)
  28: mad r1.w, r1.w, r2.x, l(0.999866)
  29: mul r2.x, r0.w, r1.w
  30: lt r2.y, |r0.z|, |r1.z|
  31: mad r2.x, r2.x, l(-2.000000), l(1.570796)
  32: and r2.x, r2.y, r2.x
  33: mad r0.w, r0.w, r1.w, r2.x
  34: lt r1.w, r0.z, -r0.z
  35: and r1.w, r1.w, l(0xc0490fdb)
  36: add r0.w, r0.w, r1.w
  37: min r1.w, r0.z, r1.z
  38: max r0.z, r0.z, r1.z
  39: lt r1.z, r1.w, -r1.w
  40: ge r0.z, r0.z, -r0.z
  41: and r0.z, r0.z, r1.z
  42: movc r0.z, r0.z, -r0.w, r0.w
  43: add r0.z, r0.z, l(3.141000)
  44: mad r0.w, cb1[3].x, r1.y, -r0.z
  45: deriv_rtx r1.z, r0.w
  46: deriv_rty r1.w, r0.w
  47: add r1.z, |r1.w|, |r1.z|
  48: div_sat r0.w, r0.w, r1.z
  49: add r0.w, -r0.w, r1.x
  50: mul r1.x, r1.y, l(0.500000)
  51: mad r0.z, r1.y, l(0.500000), r0.z
  52: div r0.z, r0.z, r1.y
  53: ge r1.z, r0.z, -r0.z
  54: frc r0.z, |r0.z|
  55: movc r0.z, r1.z, r0.z, -r0.z
  56: mad r0.z, r0.z, r1.y, -r1.x
  57: sincos r0.z, null, r0.z
  58: dp2 r0.x, r0.xyxx, r0.xyxx
  59: sqrt r0.x, r0.x
  60: mad r0.x, |r0.z|, r0.x, -cb1[3].y
  61: deriv_rtx r0.y, r0.x
  62: deriv_rty r0.z, r0.x
  63: add r0.y, |r0.z|, |r0.y|
  64: div r0.x, r0.x, r0.y
  65: add_sat r0.x, -r0.x, l(1.000000)
  66: add r0.x, -r0.x, r0.w
  67: max r0.y, r0.w, l(0.000000)
  68: min r0.x, r0.x, r0.y
  69: round_ni r1.xyw, r0.xxxx
  70: eq r0.y, r1.w, l(0.000000)
  71: discard_nz r0.y
  72: mul r0.xyz, r0.xxxx, cb1[2].xyzx
  73: switch cb0[123].x
  74:   case l(0)
  75:   mov r2.xyzw, l(0,0,0,0)
  76:   mov r3.x, l(0)
  77:   break 
  78:   case l(1)
  79:   mov r0.w, l(1.000000)
  80:   mov r2.xyzw, r0.xyzw
  81:   mov r3.x, l(-1)
  82:   break 
  83:   case l(3)
  84:   mov r1.z, l(1.000000)
  85:   mov r2.xyzw, r1.xywz
  86:   mov r3.x, l(-1)
  87:   break 
  88:   case l(11)
  89:   mov r2.xyzw, l(1.000000,1.000000,1.000000,1.000000)
  90:   mov r3.x, l(-1)
  91:   break 
  92:   case l(8)
  93:   case l(7)
  94:   mov r2.xyzw, l(0,0,1.000000,1.000000)
  95:   mov r3.x, l(-1)
  96:   break 
  97:   default 
  98:   mov r2.xyzw, cb0[129].xyzw
  99:   mov r3.x, l(-1)
 100:   break 
 101: endswitch 
 102: ine r0.w, cb0[123].x, l(11)
 103: movc r0.w, r3.x, r0.w, l(-1)
 104: if_nz r0.w
 105:   movc r4.xyzw, cb0[124].xxxx, cb0[128].xyzw, cb0[129].xyzw
 106:   movc o0.xyzw, r3.xxxx, r2.xyzw, r4.xyzw
 107:   ret 
 108: endif 
 109: mov r1.xyz, r0.xyzx
 110: mul o0.xyzw, r1.xyzw, v3.xyzw
 111: ret 
// Approximately 0 instruction slots used


 }


 // Stats for Vertex shader:
 //        d3d11: 9 math
 // Stats for Fragment shader:
 //        d3d11: 68 avg math (68..69), 4 avg branch (0..9)
 Pass {
  Name "Sprite Unlit"
  Tags { "LIGHTMODE"="UniversalForward" "QUEUE"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "UniversalMaterialType"="Unlit" "ShaderGraphShader"="true" "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
  //////////////////////////////////
  //                              //
  //      Compiled programs       //
  //                              //
  //////////////////////////////////
//////////////////////////////////////////////////////
Keywords: <none>
-- Hardware tier variant: Tier 1
-- Vertex shader for "d3d11":
// Stats: 9 math, 2 temp registers
Uses vertex data channel "Vertex"
Uses vertex data channel "TexCoord0"
Uses vertex data channel "Color"

Constant Buffer "$Globals" (2000 bytes) on slot 0 {
  Matrix4x4 unity_MatrixVP at 1152
  Vector4 _RendererColor at 1984
}
Constant Buffer "UnityPerDraw" (656 bytes) on slot 1 {
  Matrix4x4 unity_ObjectToWorld at 0
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyz         0     NONE   float   xyz 
// NORMAL                   0   xyz         1     NONE   float       
// TANGENT                  0   xyzw        2     NONE   float       
// TEXCOORD                 0   xyzw        3     NONE   float   xyzw
// COLOR                    0   xyzw        4     NONE   float   xyzw
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// INTERP                   0   xyz         1     NONE   float   xyz 
// INTERP                   1   xyzw        2     NONE   float   xyzw
// INTERP                   2   xyzw        3     NONE   float   xyzw
//
      vs_4_0
      dcl_constantbuffer CB0[125], immediateIndexed
      dcl_constantbuffer CB1[4], immediateIndexed
      dcl_input v0.xyz
      dcl_input v3.xyzw
      dcl_input v4.xyzw
      dcl_output_siv o0.xyzw, position
      dcl_output o1.xyz
      dcl_output o2.xyzw
      dcl_output o3.xyzw
      dcl_temps 2
   0: mul r0.xyz, v0.yyyy, cb1[1].xyzx
   1: mad r0.xyz, cb1[0].xyzx, v0.xxxx, r0.xyzx
   2: mad r0.xyz, cb1[2].xyzx, v0.zzzz, r0.xyzx
   3: add r0.xyz, r0.xyzx, cb1[3].xyzx
   4: mul r1.xyzw, r0.yyyy, cb0[73].xyzw
   5: mad r1.xyzw, cb0[72].xyzw, r0.xxxx, r1.xyzw
   6: mad r1.xyzw, cb0[74].xyzw, r0.zzzz, r1.xyzw
   7: mov o1.xyz, r0.xyzx
   8: add o0.xyzw, r1.xyzw, cb0[75].xyzw
   9: mov o2.xyzw, v3.xyzw
  10: mul o3.xyzw, v4.xyzw, cb0[124].xyzw
  11: ret 
// Approximately 0 instruction slots used


-- Hardware tier variant: Tier 1
-- Fragment shader for "d3d11":
// Stats: 68 math, 4 temp registers
Constant Buffer "UnityPerMaterial" (64 bytes) on slot 0 {
  Float _Radius at 16
  Float _LineWidth at 20
  Float _Rotation at 24
  Vector4 _Color at 32
  Float _RemovedSegments at 48
  Float _SegmentSpacing at 52
  Float _SegmentCount at 56
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// INTERP                   0   xyz         1     NONE   float       
// INTERP                   1   xyzw        2     NONE   float   xy  
// INTERP                   2   xyzw        3     NONE   float   xyzw
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_TARGET                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_constantbuffer CB0[4], immediateIndexed
      dcl_input_ps linear v2.xy
      dcl_input_ps linear v3.xyzw
      dcl_output o0.xyzw
      dcl_temps 4
   0: add r0.xy, v2.xyxx, l(-0.500000, -0.500000, 0.000000, 0.000000)
   1: mul r0.zw, r0.xxxy, l(0.000000, 0.000000, -1.000000, 1.000000)
   2: dp2 r1.x, r0.zwzz, r0.zwzz
   3: sqrt r1.x, r1.x
   4: add r1.x, r1.x, -cb0[1].x
   5: mov r1.x, |r1.x|
   6: add r1.y, r1.x, -cb0[1].y
   7: deriv_rtx r1.z, r1.x
   8: deriv_rty r1.x, r1.x
   9: add r1.x, |r1.x|, |r1.z|
  10: div r1.x, r1.y, r1.x
  11: add_sat r1.x, -r1.x, l(1.000000)
  12: div r1.y, l(6.283180), cb0[3].z
  13: mul r1.z, cb0[1].z, l(0.017453)
  14: sincos r2.x, r3.x, r1.z
  15: mov r3.yz, r2.xxxx
  16: mad r2.xyz, r3.xyzx, l(0.500000, -0.500000, 0.500000, 0.000000), l(0.500000, 0.500000, 0.500000, 0.000000)
  17: mad r2.xyz, r2.xyzx, l(2.000000, 2.000000, 2.000000, 0.000000), l(-1.000000, -1.000000, -1.000000, 0.000000)
  18: dp2 r1.z, r0.zwzz, r2.xzxx
  19: dp2 r0.z, r0.wzww, r2.xyxx
  20: min r0.w, |r0.z|, |r1.z|
  21: max r1.w, |r0.z|, |r1.z|
  22: div r1.w, l(1.000000, 1.000000, 1.000000, 1.000000), r1.w
  23: mul r0.w, r0.w, r1.w
  24: mul r1.w, r0.w, r0.w
  25: mad r2.x, r1.w, l(0.020835), l(-0.085133)
  26: mad r2.x, r1.w, r2.x, l(0.180141)
  27: mad r2.x, r1.w, r2.x, l(-0.330299)
  28: mad r1.w, r1.w, r2.x, l(0.999866)
  29: mul r2.x, r0.w, r1.w
  30: lt r2.y, |r0.z|, |r1.z|
  31: mad r2.x, r2.x, l(-2.000000), l(1.570796)
  32: and r2.x, r2.y, r2.x
  33: mad r0.w, r0.w, r1.w, r2.x
  34: lt r1.w, r0.z, -r0.z
  35: and r1.w, r1.w, l(0xc0490fdb)
  36: add r0.w, r0.w, r1.w
  37: min r1.w, r0.z, r1.z
  38: max r0.z, r0.z, r1.z
  39: lt r1.z, r1.w, -r1.w
  40: ge r0.z, r0.z, -r0.z
  41: and r0.z, r0.z, r1.z
  42: movc r0.z, r0.z, -r0.w, r0.w
  43: add r0.z, r0.z, l(3.141000)
  44: mad r0.w, cb0[3].x, r1.y, -r0.z
  45: deriv_rtx r1.z, r0.w
  46: deriv_rty r1.w, r0.w
  47: add r1.z, |r1.w|, |r1.z|
  48: div_sat r0.w, r0.w, r1.z
  49: add r0.w, -r0.w, r1.x
  50: mul r1.x, r1.y, l(0.500000)
  51: mad r0.z, r1.y, l(0.500000), r0.z
  52: div r0.z, r0.z, r1.y
  53: ge r1.z, r0.z, -r0.z
  54: frc r0.z, |r0.z|
  55: movc r0.z, r1.z, r0.z, -r0.z
  56: mad r0.z, r0.z, r1.y, -r1.x
  57: sincos r0.z, null, r0.z
  58: dp2 r0.x, r0.xyxx, r0.xyxx
  59: sqrt r0.x, r0.x
  60: mad r0.x, |r0.z|, r0.x, -cb0[3].y
  61: deriv_rtx r0.y, r0.x
  62: deriv_rty r0.z, r0.x
  63: add r0.y, |r0.z|, |r0.y|
  64: div r0.x, r0.x, r0.y
  65: add_sat r0.x, -r0.x, l(1.000000)
  66: add r0.x, -r0.x, r0.w
  67: max r0.y, r0.w, l(0.000000)
  68: min r0.x, r0.x, r0.y
  69: round_ni r1.w, r0.x
  70: eq r0.y, r1.w, l(0.000000)
  71: discard_nz r0.y
  72: mul r1.xyz, r0.xxxx, cb0[2].xyzx
  73: mul o0.xyzw, r1.xyzw, v3.xyzw
  74: ret 
// Approximately 0 instruction slots used


//////////////////////////////////////////////////////
Keywords: DEBUG_DISPLAY
-- Vertex shader for "d3d11":
// No shader variant for this keyword set. The closest match will be used instead.

-- Hardware tier variant: Tier 1
-- Fragment shader for "d3d11":
// Stats: 69 math, 5 temp registers, 9 branches
Constant Buffer "$Globals" (2144 bytes) on slot 0 {
  ScalarInt _DebugMaterialMode at 1968
  ScalarInt _DebugSceneOverrideMode at 1984
  Vector4 _DebugColor at 2048
  Vector4 _DebugColorInvalidMode at 2064
}
Constant Buffer "UnityPerMaterial" (64 bytes) on slot 1 {
  Float _Radius at 16
  Float _LineWidth at 20
  Float _Rotation at 24
  Vector4 _Color at 32
  Float _RemovedSegments at 48
  Float _SegmentSpacing at 52
  Float _SegmentCount at 56
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// INTERP                   0   xyz         1     NONE   float       
// INTERP                   1   xyzw        2     NONE   float   xy  
// INTERP                   2   xyzw        3     NONE   float   xyzw
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_TARGET                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_constantbuffer CB0[130], immediateIndexed
      dcl_constantbuffer CB1[4], immediateIndexed
      dcl_input_ps linear v2.xy
      dcl_input_ps linear v3.xyzw
      dcl_output o0.xyzw
      dcl_temps 5
   0: add r0.xy, v2.xyxx, l(-0.500000, -0.500000, 0.000000, 0.000000)
   1: mul r0.zw, r0.xxxy, l(0.000000, 0.000000, -1.000000, 1.000000)
   2: dp2 r1.x, r0.zwzz, r0.zwzz
   3: sqrt r1.x, r1.x
   4: add r1.x, r1.x, -cb1[1].x
   5: mov r1.x, |r1.x|
   6: add r1.y, r1.x, -cb1[1].y
   7: deriv_rtx r1.z, r1.x
   8: deriv_rty r1.x, r1.x
   9: add r1.x, |r1.x|, |r1.z|
  10: div r1.x, r1.y, r1.x
  11: add_sat r1.x, -r1.x, l(1.000000)
  12: div r1.y, l(6.283180), cb1[3].z
  13: mul r1.z, cb1[1].z, l(0.017453)
  14: sincos r2.x, r3.x, r1.z
  15: mov r3.yz, r2.xxxx
  16: mad r2.xyz, r3.xyzx, l(0.500000, -0.500000, 0.500000, 0.000000), l(0.500000, 0.500000, 0.500000, 0.000000)
  17: mad r2.xyz, r2.xyzx, l(2.000000, 2.000000, 2.000000, 0.000000), l(-1.000000, -1.000000, -1.000000, 0.000000)
  18: dp2 r1.z, r0.zwzz, r2.xzxx
  19: dp2 r0.z, r0.wzww, r2.xyxx
  20: min r0.w, |r0.z|, |r1.z|
  21: max r1.w, |r0.z|, |r1.z|
  22: div r1.w, l(1.000000, 1.000000, 1.000000, 1.000000), r1.w
  23: mul r0.w, r0.w, r1.w
  24: mul r1.w, r0.w, r0.w
  25: mad r2.x, r1.w, l(0.020835), l(-0.085133)
  26: mad r2.x, r1.w, r2.x, l(0.180141)
  27: mad r2.x, r1.w, r2.x, l(-0.330299)
  28: mad r1.w, r1.w, r2.x, l(0.999866)
  29: mul r2.x, r0.w, r1.w
  30: lt r2.y, |r0.z|, |r1.z|
  31: mad r2.x, r2.x, l(-2.000000), l(1.570796)
  32: and r2.x, r2.y, r2.x
  33: mad r0.w, r0.w, r1.w, r2.x
  34: lt r1.w, r0.z, -r0.z
  35: and r1.w, r1.w, l(0xc0490fdb)
  36: add r0.w, r0.w, r1.w
  37: min r1.w, r0.z, r1.z
  38: max r0.z, r0.z, r1.z
  39: lt r1.z, r1.w, -r1.w
  40: ge r0.z, r0.z, -r0.z
  41: and r0.z, r0.z, r1.z
  42: movc r0.z, r0.z, -r0.w, r0.w
  43: add r0.z, r0.z, l(3.141000)
  44: mad r0.w, cb1[3].x, r1.y, -r0.z
  45: deriv_rtx r1.z, r0.w
  46: deriv_rty r1.w, r0.w
  47: add r1.z, |r1.w|, |r1.z|
  48: div_sat r0.w, r0.w, r1.z
  49: add r0.w, -r0.w, r1.x
  50: mul r1.x, r1.y, l(0.500000)
  51: mad r0.z, r1.y, l(0.500000), r0.z
  52: div r0.z, r0.z, r1.y
  53: ge r1.z, r0.z, -r0.z
  54: frc r0.z, |r0.z|
  55: movc r0.z, r1.z, r0.z, -r0.z
  56: mad r0.z, r0.z, r1.y, -r1.x
  57: sincos r0.z, null, r0.z
  58: dp2 r0.x, r0.xyxx, r0.xyxx
  59: sqrt r0.x, r0.x
  60: mad r0.x, |r0.z|, r0.x, -cb1[3].y
  61: deriv_rtx r0.y, r0.x
  62: deriv_rty r0.z, r0.x
  63: add r0.y, |r0.z|, |r0.y|
  64: div r0.x, r0.x, r0.y
  65: add_sat r0.x, -r0.x, l(1.000000)
  66: add r0.x, -r0.x, r0.w
  67: max r0.y, r0.w, l(0.000000)
  68: min r0.x, r0.x, r0.y
  69: round_ni r1.xyw, r0.xxxx
  70: eq r0.y, r1.w, l(0.000000)
  71: discard_nz r0.y
  72: mul r0.xyz, r0.xxxx, cb1[2].xyzx
  73: switch cb0[123].x
  74:   case l(0)
  75:   mov r2.xyzw, l(0,0,0,0)
  76:   mov r3.x, l(0)
  77:   break 
  78:   case l(1)
  79:   mov r0.w, l(1.000000)
  80:   mov r2.xyzw, r0.xyzw
  81:   mov r3.x, l(-1)
  82:   break 
  83:   case l(3)
  84:   mov r1.z, l(1.000000)
  85:   mov r2.xyzw, r1.xywz
  86:   mov r3.x, l(-1)
  87:   break 
  88:   case l(11)
  89:   mov r2.xyzw, l(1.000000,1.000000,1.000000,1.000000)
  90:   mov r3.x, l(-1)
  91:   break 
  92:   case l(8)
  93:   case l(7)
  94:   mov r2.xyzw, l(0,0,1.000000,1.000000)
  95:   mov r3.x, l(-1)
  96:   break 
  97:   default 
  98:   mov r2.xyzw, cb0[129].xyzw
  99:   mov r3.x, l(-1)
 100:   break 
 101: endswitch 
 102: ine r0.w, cb0[123].x, l(11)
 103: movc r0.w, r3.x, r0.w, l(-1)
 104: if_nz r0.w
 105:   movc r4.xyzw, cb0[124].xxxx, cb0[128].xyzw, cb0[129].xyzw
 106:   movc o0.xyzw, r3.xxxx, r2.xyzw, r4.xyzw
 107:   ret 
 108: endif 
 109: mov r1.xyz, r0.xyzx
 110: mul o0.xyzw, r1.xyzw, v3.xyzw
 111: ret 
// Approximately 0 instruction slots used


 }
}
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
Fallback "Hidden/Shader Graph/FallbackError"
}