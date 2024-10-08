using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
    public static readonly int TextureAtlasWidth = 20;
    public static readonly int TextureAtlasHeight = 20;

    // �ؽ��� ��Ʋ�� ������ �� ��, ������ �ؽ��İ� ���� ũ�� ����
    public static readonly float NormalizedTextureAtlasWidth = 1f / TextureAtlasWidth;
    public static readonly float NormalizedTextureAtlasHeight = 1f / TextureAtlasHeight;

    public static readonly byte ChunkWidth = 16;
    public static readonly byte ChunkHeight = 80;

    /// <summary> ť���� 8�� ���ؽ��� ��� ��ġ </summary>
    public static readonly Vector3[] voxelVerts = new Vector3[8]
    {   // ������ ����
        // Front
        new Vector3(0.0f, 0.0f, 0.0f), // LB
        new Vector3(1.0f, 0.0f, 0.0f), // RB
        new Vector3(1.0f, 1.0f, 0.0f), // RT
        new Vector3(0.0f, 1.0f, 0.0f), // LT

        // Back
        new Vector3(0.0f, 0.0f, 1.0f), // LB
        new Vector3(1.0f, 0.0f, 1.0f), // RB
        new Vector3(1.0f, 1.0f, 1.0f), // RT
        new Vector3(0.0f, 1.0f, 1.0f), // LT
    };

    public static readonly Vector3[] itemVerts = new Vector3[8]
{   // ������ ����
        // Front
        new Vector3(0.0f, 0.0f, 0.5f), // LB
        new Vector3(1.0f, 0.0f, 0.5f), // RB
        new Vector3(1.0f, 1.0f, 0.5f), // RT
        new Vector3(0.0f, 1.0f, 0.5f), // LT

        // Back
        new Vector3(0.0f, 0.0f, 0.5f), // LB
        new Vector3(1.0f, 0.0f, 0.5f), // RB
        new Vector3(1.0f, 1.0f, 0.5f), // RT
        new Vector3(0.0f, 1.0f, 0.5f), // LT
};
    /// <summary> ť���� �� ���� �̷�� �ﰢ������ ���ؽ� �ε��� ������ </summary>
    public static readonly int[,] voxelTris = new int[6, 4]
    {   // ������ �̾� �ش�.
    {0, 3, 1, 2 }, // Back Face   (-Z)
    {5, 6, 4, 7 }, // Front Face  (+Z)
    {3, 7, 2, 6 }, // Top Face    (+Y)
    {1, 5, 0, 4 }, // Bottom Face (-Y)
    {4, 7, 0, 3 }, // Left Face   (-X)
    {1, 2, 5, 6 }, // RIght Face  (+X)
    };

    /// <summary> voxelTris�� ���ؽ� �ε��� ������ ���� ���ǵ� UV ��ǥ ������ </summary>
    public static readonly Vector2[] voxelUvs = new Vector2[4]
    {   // uvs ���� �ݻ�Ǵ� ����
    new Vector2(0.0f, 0.0f), // LB
    new Vector2(0.0f, 1.0f), // LT
    new Vector2(1.0f, 0.0f), // RB
    new Vector2(1.0f, 1.0f), // RT
    };

    /// <summary> ��6��ü ������ ���� ��ǥ�� </summary>
    public static readonly Vector3Int[] faceChecks = new Vector3Int[6]
    {
    new Vector3Int( 0,  0, -1), // Back Face   (-Z)
    new Vector3Int( 0,  0, +1), // Front Face  (+Z)
    new Vector3Int( 0, +1,  0), // Top Face    (+Y)
    new Vector3Int( 0, -1,  0), // Bottom Face (-Y)
    new Vector3Int(-1,  0,  0), // Left Face   (-X)
    new Vector3Int(+1,  0,  0), // RIght Face  (+X)
    };


    private static int[,] directionNormaliz = new int[4, 6]
    {
        {0, 1, 2, 3, 4, 5}, // -z
        {1, 0, 2, 3, 5, 4}, // +z
        {4, 5, 2, 3, 0, 1}, // -x
        {5, 4, 2, 3, 1, 0}  // +x
    };
    public static int DirectionNormaliz(int dir, int face)
    {
        return directionNormaliz[dir, face];
    }
}
