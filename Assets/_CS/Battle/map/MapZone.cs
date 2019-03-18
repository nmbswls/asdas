using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapZone
{

	public int idx;
	public Vector2 size;

	public List<GameLife> enemy = new List<GameLife>();
	public List<Tower> towers = new List<Tower>();

	public MapZone(){
	}
	public MapZone(int idx){
		this.idx = idx;
	}
}

