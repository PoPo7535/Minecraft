using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public ChunkMapData chunkMapData;

    #region �޽�������
    private int vertexIndex = 0;
    private readonly List<Vector3> meshVertices = new List<Vector3>();
    private readonly List<int> meshTriangles = new List<int>();
    private readonly List<Vector2> meshUv = new List<Vector2>();
    #endregion

    #region ������Ʈ ������
    public readonly GameObject ChunkObject; // ûũ�� ������ ��� ���ӿ�����Ʈ
    private readonly MeshRenderer meshRenderer;
    private readonly MeshFilter meshFilter;
    #endregion

    #region ûũ ������
    public enum ChunkState { CoroutineStart, CoroutineUpdate, CoroutineEnd };
    public ChunkState chunkState;
    public ChunkCoord coord;
    #endregion

    private World world
    {
        get { return World.Instance; }
    }
    public Chunk(ChunkCoord coord)
    {
        this.coord = coord;
        chunkState = ChunkState.CoroutineStart;

        chunkMapData = new ChunkMapData(coord);

        ChunkObject = new GameObject();
        meshRenderer = ChunkObject.AddComponent<MeshRenderer>();
        meshFilter = ChunkObject.AddComponent<MeshFilter>();
        meshRenderer.material = World.Instance.TextureAtlas;
        ChunkObject.transform.SetParent(World.Instance.transform);
        ChunkObject.transform.position =
            new Vector3(coord.x * VoxelData.ChunkWidth, 0f, coord.z * VoxelData.ChunkWidth);
        ChunkObject.name = $"Chunk [{coord.x}, {coord.z}]";
        ChunkObject.SetActive(false);
    }
    public IEnumerator CreateChunkMesh()
    {
        chunkState = ChunkState.CoroutineUpdate;
        ChunkObject.SetActive(true);
        for (int i = 0; i < 4; ++i)
        {
            if (null == world.GetChunkFromCoord(ChunkHelperData.VectorCoord(i, coord)))
            {
                world.CreateNewChunk(ChunkHelperData.VectorCoord(i, coord));
            }
        }

        for (int y = 0; y < VoxelData.ChunkHeight; ++y)
        {
            if (y % 3 == 0) 
                yield return null;
            for (int x = 0; x < VoxelData.ChunkWidth; ++x)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; ++z)
                {
                    CreateMeshkData(new Vector3Int(x, y, z));
                }
            }
        }

        meshFilter.mesh.vertices = meshVertices.ToArray();
        meshFilter.mesh.triangles = meshTriangles.ToArray();
        meshFilter.mesh.uv = meshUv.ToArray();
        meshFilter.mesh.RecalculateNormals();

        chunkState = ChunkState.CoroutineEnd;
    }
    public void ApplyMeshData()
    {
        // �޽ÿ� �����͵� �ʱ�ȭ
        meshFilter.mesh.Clear();
        meshVertices.Clear();
        meshTriangles.Clear();
        meshUv.Clear();
        vertexIndex = 0;

        for (int y = 0; y < VoxelData.ChunkHeight; ++y)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; ++x)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; ++z)
                {
                    CreateMeshkData(new Vector3Int(x, y, z));
                }
            }
        }
        meshFilter.mesh.vertices = meshVertices.ToArray();
        meshFilter.mesh.triangles = meshTriangles.ToArray();
        meshFilter.mesh.uv = meshUv.ToArray();
        meshFilter.mesh.RecalculateNormals();
    }

    private void CreateMeshkData(in Vector3Int voxelPos)
    {
        Voxel currentVoxel = chunkMapData.GetVoxelState(voxelPos);
        if (CodeData.BLOCK_Air == currentVoxel.id)
            return;

        for (int face = 0; face < 6; ++face)
        {
            Voxel neighborVoxel = chunkMapData.GetVoxelState(voxelPos + VoxelData.faceChecks[face]);

            if (false == neighborVoxel?.blockProperties.renderNeighborFaces)
                continue;

            for (int i = 0; i < 4; ++i)
                meshVertices.Add(VoxelData.voxelVerts[VoxelData.voxelTris[face, i]] + voxelPos);

            int dir = VoxelData.DirectionNormaliz(currentVoxel.direction, face);
            int atlasesCode = CodeData.GetBlockTextureAtlases(currentVoxel.id, dir);
            AddTextureUV(atlasesCode);

            foreach (int i in ChunkHelperData.vertexData)
                meshTriangles.Add(vertexIndex + i);

            vertexIndex += 4;
        }
    }
    private void AddTextureUV(int atlasesCode)
    {
        // ��Ʋ�� ���� �ؽ��� ����, ���� ����
        (int w, int h) = (VoxelData.TextureAtlasWidth, VoxelData.TextureAtlasHeight);

        int x = atlasesCode % w;
        int y = h - (atlasesCode / w) - 1;

        float nw = VoxelData.NormalizedTextureAtlasWidth;
        float nh = VoxelData.NormalizedTextureAtlasHeight;

        float uvX = x * nw;
        float uvY = y * nh;

        // �ش� �ؽ����� uv�� LB-LT-RB-RT ������ �߰�
        meshUv.Add(new Vector2(uvX, uvY));
        meshUv.Add(new Vector2(uvX, uvY + nh));
        meshUv.Add(new Vector2(uvX + nw, uvY));
        meshUv.Add(new Vector2(uvX + nw, uvY + nh));
    }

    private Chunk CheckProximityChunk(Vector2Int chunkPos)
    {
        Chunk chunk = world.GetChunkFromCoord(chunkPos);
        if (chunk == null)
            return null;
        return chunk;
    }
    public void ModifyChunkData(in Vector3Int voxelPos, ushort blockCode, in Vector2Int dir = new Vector2Int())
    {
        #region �÷��̾� �Է°� ����
        chunkMapData.SetVoxelState(voxelPos, blockCode, dir);
        #endregion

        #region �޽������� ����
        ApplyMeshData();
        if (voxelPos.x == 0)
            CheckProximityChunk(ChunkHelperData.VectorCoord(0, coord)).ApplyMeshData();
        else if (voxelPos.x == 15)
            CheckProximityChunk(ChunkHelperData.VectorCoord(1, coord)).ApplyMeshData();
        if (voxelPos.z == 0)
            CheckProximityChunk(ChunkHelperData.VectorCoord(2, coord)).ApplyMeshData();
        else if (voxelPos.z == 15)
            CheckProximityChunk(ChunkHelperData.VectorCoord(3, coord)).ApplyMeshData();
        #endregion
    }
}