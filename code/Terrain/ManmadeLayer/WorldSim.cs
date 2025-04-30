using Sandbox;

public sealed class WorldSim : Component
{
	[Property] Terrain terrain;

	public float spacing;

	[Property] Astar astar;
	Vector3[] Towns = new Vector3[]
	{
		new Vector3( 10000, 10000, 10000 ),
		new Vector3( 20000, 58000, 10000 ),
		new Vector3( 80000, 30000, 10000 ),
		new Vector3( 90000, 45000, 10000 ),
		new Vector3( 90000, 800000, 10000 ),
		new Vector3( 70000, 120000, 10000 ),
		new Vector3( 60000, 35000, 10000 ),
		new Vector3( 20000, 70000, 10000 ),
	};

	[Button("Generate Town Paths")]

	public void PickTownSpots()
	{
		
		
		for ( int i = 0; i < Towns.Length; i++ )
		{
				if (i <  Towns.Length+1)
			{ astar.FindPath( Towns[i], Towns[i + 1] );
				astar.FindPath( Towns[i], Towns[i + 2] ); }
				
			
		}
	}
}
