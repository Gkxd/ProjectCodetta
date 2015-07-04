using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateWireframeMesh : MonoBehaviour {

	void Awake () {
        Mesh mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;

        Mesh newMesh = new Mesh();

        Vector3[] barycentrics = new Vector3[mesh.triangles.Length];
        Vector3[] vertices = new Vector3[mesh.triangles.Length];
        Vector2[] texCoords = new Vector2[mesh.triangles.Length];
        int[] triangles = new int[mesh.triangles.Length];


        for (int i = 0; i < mesh.triangles.Length; i += 3) {
            int t0 = mesh.triangles[i];
            int t1 = mesh.triangles[i + 1];
            int t2 = mesh.triangles[i + 2];

            triangles[i] = i;
            triangles[i + 1] = i + 1;
            triangles[i + 2] = i + 2;

            vertices[i] = mesh.vertices[t0];
            vertices[i + 1] = mesh.vertices[t1];
            vertices[i + 2] = mesh.vertices[t2];

            texCoords[i] = mesh.uv[t0];
            texCoords[i + 1] = mesh.uv[t1];
            texCoords[i + 2] = mesh.uv[t2];

            barycentrics[i] = Vector3.right;
            barycentrics[i + 1] = Vector3.up;
            barycentrics[i + 2] = Vector3.forward;
        }

        newMesh.vertices = vertices;
        newMesh.normals = barycentrics;
        newMesh.uv = texCoords;
        newMesh.triangles = triangles;

        GetComponent<SkinnedMeshRenderer>().sharedMesh.vertices = vertices;
        GetComponent<SkinnedMeshRenderer>().sharedMesh.normals = barycentrics;
        GetComponent<SkinnedMeshRenderer>().sharedMesh.uv = texCoords;
        GetComponent<SkinnedMeshRenderer>().sharedMesh.triangles = triangles;
	}
}
