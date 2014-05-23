using UnityEngine;
using System.Collections.Generic;

public class GrassDeformation : MonoBehaviour 
{
	public GameObject[] grassObjects; 
	
	//protected Mesh mesh = null;
	//protected Vector3[] originalVertices;
	
	protected List< Mesh > _meshes;
	protected List< Vector3[] > _originalVertices;
	
	//public int factor = 1;
	
	// Use this for initialization
	void Start () 
	{
		_meshes = new List<Mesh>();
		_originalVertices = new List<Vector3[]>();
		
		for( int i = 0; i < grassObjects.Length; ++i )
		{
			Mesh mesh = grassObjects[i].GetComponent<MeshFilter>().mesh;
			_meshes.Add(mesh);
			
			//_originalVertices.Add( (Vector3[]) mesh.vertices.Clone() );
			Vector3[] vertices = new Vector3[ mesh.vertices.Length ];
			int tel = 0;
			foreach( Vector3 vertex in mesh.vertices )
			{
				vertices[tel] = vertex;
				++tel;
			}
			
			_originalVertices.Add( vertices );
		}
	}
	
	protected float targetFactor = 0;
	protected float currentFactor = 0;
	public float factorMultiplier = 2; // number of units the displacement can maximally become
	protected float timeSinceFactorChange = 0;
	public float secondsBetweenFactorChange = 3;
	
	// Update is called once per frame
	void Update () 
	{
		
		// every 5 seconds: change target
		timeSinceFactorChange += Time.deltaTime;
		if( timeSinceFactorChange > secondsBetweenFactorChange )
		{
			targetFactor = (Random.value - 0.5f) * factorMultiplier * 2; // * 2 to compensate for the negative values
			//Debug.Log("New Target Value " + targetFactor);
			
			timeSinceFactorChange = 0;
		}
		
		currentFactor = Mathf.Lerp(currentFactor, targetFactor, Time.deltaTime);
		//Debug.Log(currentFactor);
		
		//if( Time.timeSince
		
		for( int objIndex = 0; objIndex < grassObjects.Length; ++objIndex)
		{
			// FIXME: better way to approach this:
			// instead of constantly copying the original values:
			// store the target values! 
			// then, we can just Vector3.Lerp() to the targets every frame
			// every time the factor/target changes, we will have to re-calculate all the targets,
			// but that's still going to be a lot faster than what we're doing now (timeslicing might still be needed though... otherwhise framedrops every secondsbetweenfactorchange - seconds)
			
			//Vector3[] vertices = (Vector3[]) _originalVertices[objIndex].Clone();
			
			
			Vector3[] vertices = new Vector3[ _originalVertices[objIndex].Length ];
			
			int tel = 0;
			foreach( Vector3 vertex in _originalVertices[objIndex] )
			{
				vertices[tel] = vertex;
				tel++;
			}
			
			
			
			Mesh mesh = _meshes[objIndex];
			
			//Debug.Log( mesh.bounds.center + " -> " + mesh.bounds.min + " , " + mesh.bounds.max );
			
			
			for( int i = 0; i < vertices.Length; ++i)
			{
				Vector3 vertex = vertices[i];
				
				//Debug.DrawLine(vertex, vertex + new Vector3(currentFactor, 0, 0), Color.red );
				
				
                //float distanceToRightEdge = Mathf.Abs( vertex.x - mesh.bounds.max.x);
				float distanceToBottom = Mathf.Abs(vertex.y - mesh.bounds.min.y);
				// convert to the [0,1] zone
				// dividing by the maximum distance will do this automagically
				float distancePercentage = distanceToBottom / mesh.bounds.max.y;
				
				// put vertex in world space (so it will certainly change in the x-direction, even if it is rotated!)
				vertex = grassObjects[objIndex].transform.TransformPoint(vertex);
				
				//Debug.DrawLine(vertex, vertex + new Vector3(currentFactor, 0, 0), Color.green );
				//Debug.Log( vertex );
				
				
				// x coordinate should differ with at max 2 units
				// coordinate in [0,1] to the second power is still in [0,1]
				vertex.x += Mathf.Pow(distancePercentage,2)  * currentFactor ;// * 2;
				
				vertices[i] = grassObjects[objIndex].transform.InverseTransformPoint(vertex);
				//mesh.bounds.
			}
			
			
			
			mesh.vertices = vertices;
			//mesh.RecalculateBounds();
			//mesh.RecalculateNormals();		
		}
		
	}
}
