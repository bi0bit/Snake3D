using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Extenstions.MeshExtensions
{
	public static class MeshExtensions
	{
		public static Mesh CombineMesh(this IEnumerable<MeshFilter> meshes)
		{
			var meshFilters = meshes as MeshFilter[] ?? meshes.ToArray();
			var combine = new CombineInstance[meshFilters.Length];

			var i = 0;
			foreach (var meshFilter in meshFilters)
			{
				combine[i].mesh = meshFilter.sharedMesh;
				combine[i].transform = meshFilter.transform.localToWorldMatrix;
				i++;
			}

			var mesh = new Mesh();
			mesh.CombineMeshes(combine);
			return mesh;
		}
		
		public static float CalculateTotals(this Mesh mesh, ref float[] sizes, ref float[] cumulativeSizes)
		{
			sizes = GetTriSizes(mesh.triangles, mesh.vertices);
			cumulativeSizes = new float[sizes.Length];
			float total = 0;

			for (int i = 0; i < sizes.Length; i++)
			{
				total += sizes[i];
				cumulativeSizes[i] = total;
			}

			return total;
		}
		
		public static Vector3 GetRandomPointOnMeshNonCached(this Mesh mesh)
		{
			float[] sizes = null;
			float[] cumulativeSizes = null;
			float total = CalculateTotals(mesh, ref sizes, ref cumulativeSizes);
			
			return GetRandomPointOnMesh(mesh, sizes, cumulativeSizes, total);
		}
		
		public static Vector3 GetRandomPointOnMesh(this Mesh mesh, float[] sizes, float[] cumulativeSizes, float total)
		{
			float randomSample = Random.value * total;

			int triIndex = -1;

			for (int i = 0; i < sizes.Length; i++)
			{
				if (randomSample <= cumulativeSizes[i])
				{
					triIndex = i;
					break;
				}
			}

			if (triIndex == -1) Debug.LogError("triIndex should never be -1");

			Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
			Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
			Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];


			float r = Random.value;
			float s = Random.value;

			if (r + s >= 1)
			{
				r = 1 - r;
				s = 1 - s;
			}

			Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
			return pointOnMesh;
		}

		private static float[] GetTriSizes(int[] tris, Vector3[] verts)
		{
			int triCount = tris.Length / 3;
			float[] sizes = new float[triCount];
			for (var i = 0; i < triCount; i++)
			{
				sizes[i] = .5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]]
					- verts[tris[i * 3]]).magnitude;
			}

			return sizes;
		}
	}
}