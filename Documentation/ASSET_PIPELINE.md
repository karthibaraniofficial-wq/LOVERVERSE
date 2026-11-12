# LOVEVERSE - Asset Pipeline & Conventions

## Asset Naming Rules
- Character Prefabs: `CHR_Player_Male_01`, `CHR_Player_Female_01`
- Environment Assets: `ENV_Garden_Gazebo_01`, `ENV_Path_Stone_01`
- Materials: `MAT_Character_Skin`, `MAT_Garden_Grass`
- Textures: `TEX_Garden_Grass_Albedo`, `TEX_Water_Normal`
- Animations: `ANIM_Locomotion_Run`, `ANIM_Romance_Hug_A`, `ANIM_Romance_Hug_B`
- Audio: `BGM_Garden_Sunset`, `SFX_Step_Grass_01`, `UI_Click_Soft`

## Geometry & LOD Guidelines
- Main Character: 15,000 - 25,000 triangles (LOD0), 6,000 triangles (LOD1 for mobile).
- Foliage & Trees: LOD groups with wind-vertex displacement.
- Colliders: Primitive box/capsule colliders for environment geometry.
