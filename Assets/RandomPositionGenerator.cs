using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class Vector2Extension {
	public static Vector2 Rotate(this Vector2 v, float degrees) {
		float radians = degrees * Mathf.Deg2Rad;
		float sin = Mathf.Sin(radians);
		float cos = Mathf.Cos(radians);

		float tx = v.x;
		float ty = v.y;

		return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
	}
}

public class RandomPositionGenerator {

	public float distancePerSecond = 2f;
	public float radius = 0.7f;
	bool radiusDeltaDirection;
	public float yBoundary = 0.5f;

	float direction = 90; // (degree)
	float previousTime = 0;
	float x = 0.0f, y = 0.0f; // our-defined coordinate
	Vector3 NextPosition=new Vector3(0,0,0.8f);
	List<NoteNode> previosNode;


	public RandomPositionGenerator() {
		x = 0.5f * Mathf.PI * radius;
		previosNode = new List<NoteNode> ();
	}

	public Vector3 Generate(float time) {
		//STEP0 Position
		Vector3 thisPosition=NextPosition;
		radius = Random.Range (0.65f, 0.75f);

		float distance = Mathf.Clamp((time - previousTime) * distancePerSecond, 0.1f, 0.4f);
		float deltaX = Mathf.Cos ((direction-90f) * Mathf.Deg2Rad) * distance /** 0.3f*/; // Cylinder X displacement
		//float deltaY = Mathf.Sin ((direction-90f) * Mathf.Deg2Rad) * distance /** 2.0f-0.7f+adjustY*/; // height displacement
		float deltaY = Mathf.Abs(Mathf.Sin ((direction-90f) * Mathf.Deg2Rad)) * distance * 6.0f-0.7f/*+adjustY*/; // height displacement


		if (deltaY > 0.3) {
			deltaY = 0.3f;
		} else if (deltaY < -0.3) {
			deltaY = -0.3f;
		}
		x += deltaX;
		y += deltaY;

		direction += Random.Range(-30, 30);
		if (y > yBoundary) {
			y = yBoundary;
			if (direction < 180) {
				direction = 90 + 15;
			} else { // direction > 180
				direction = 270 - 15;
			}
		}
		if (y < -yBoundary) {
			y = -yBoundary;
			if (direction < 180) {
				direction = 90 - 15;
			} else { // (direction > 180)
				direction = 270 + 15;
			}
		}

//		if(x > 120 * 
		if (x < 0) {
			x = 0;
			if (direction > 270) {
				direction = 0 + 45;
			} else { // (direction < 270)
				direction = 180 - 45;
			}
		} else if (x > (Mathf.PI * radius)) {
			x = Mathf.PI * radius;
			if (direction < 90) {
				direction = 360 - 45;
			} else { // (direction > 90)
				direction = 180 + 45;
			}
		}
//		Target:DIRECTION POSITION(REAL WORLD) 
		// Step 2. Update direction    // NEXT
		if (direction < 0)
			direction += 360;
		if (direction >= 360)
			direction -= 360;

		NoteNode thisNode=new NoteNode();
		thisNode.time = previousTime;

		previousTime = time;

		// Step 3. Return new position
//		return ToRealWorld (x, y);

		//DIRECTION HERE IS NEXT DIRECTION

		NextPosition=ToRealWorld(x,y);

		NoteNode nextNode;
		nextNode = new NoteNode ();
		nextNode.pos = NextPosition;
		nextNode.time = time;
		int breakCount = 0;
		int OverLapIndex = isOverLapping (nextNode);
		Debug.Log ("OverLapIndex=" + OverLapIndex);
		while (OverLapIndex > 0) 
		{
			breakCount++;
			NextPosition-=(previosNode[OverLapIndex].pos-NextPosition).normalized*0.1f;
			NextPosition.y = Mathf.Clamp(NextPosition.y, -0.5f, 0.5f);

			//NextPosition
			nextNode.pos=NextPosition;
			OverLapIndex=isOverLapping(nextNode);
			Debug.Log ("OverLapIndex=" + OverLapIndex);
			if (breakCount > 3)
				break;
		}


		thisNode.pos = thisPosition;
		if (previosNode.Count < 12) {
			previosNode.Add (thisNode);
		} else {
			previosNode.RemoveAt (0);
			previosNode.Add (thisNode);
		}

		return thisPosition;
	}

	Vector3 ToRealWorld(float x, float y) {    
		float worldDegree = (x / (2 * Mathf.PI * radius)) * 360;

		return new Vector3(
			radius * Mathf.Cos (worldDegree * Mathf.Deg2Rad),
			y,
			radius * Mathf.Sin (worldDegree * Mathf.Deg2Rad)
		);					// Return Real World  pos
	}

	float RealWorldToX(Vector3 pos) {
		float worldDegree = Mathf.Acos (pos.x/radius) / Mathf.Deg2Rad;
		return worldDegree / 360 * (2 * Mathf.PI * radius);
	}
	float RealWorldToY(Vector3 pos) {
		return pos.y;
	}

	Vector3 directionToVector3(float dir) {
		float distance = 1;
		float deltaX = Mathf.Cos ((direction-90f) * Mathf.Deg2Rad) * distance * 0.3f; // Cylinder X displacement
		float deltaY = Mathf.Abs(Mathf.Sin ((direction-90f) * Mathf.Deg2Rad)) * distance * 2.0f-0.7f/*+adjustY*/; // height displacement

		return (ToRealWorld(x+deltaX, y+deltaY) - ToRealWorld(x, y)).normalized;
	}

	public Vector3 GetNextDirection() {
		return directionToVector3 (direction);
	}
	int isOverLapping(NoteNode _node){
		int isOverlap = -1;
		Debug.Log ("List Count=" + previosNode.Count);
		for (int i = previosNode.Count-1; i >= 0; i--) 
		{
			if (_node.time - previosNode [i].time > 1.5f) {
				previosNode.RemoveRange (0, i);
				return isOverlap;
			} else 
			{
				if (Vector3.Distance (_node.pos, previosNode [i].pos) < 0.5f) 
				{
					isOverlap = i;
					return isOverlap;
				}
			}
		}
		return isOverlap;
	} 

	/////////////////////////////////////////


//	float previousTime = 0;
//	float previousDegree = 90;
//	float previousWorldDegree = 90;
//
//	public float heightBoundary = 0.5f;
//	public float radius = 0.8f;
//	public float distancePerSecond = 2f;
//	// (currentTime-previousTime)*distancePerSecond
//
//	public Vector3 Generate(float time) {
//		float distance = Mathf.Clamp((time - previousTime) * distancePerSecond, 0.5f, 0.8f);
////		float distance = (time - previousTime) * distancePerSecond;
//		/*
//		if (time - previousTime > 0.5f) {
//			if (previousDegree > 180) {
//				previousDegree = 90;
//
//			}
//			if (previousDegree < 180) {
//				previousDegree = 270;
//			}
//			distance *= 2;
//		}*/
//		float adjustY = 0;
//		if (previousWorldDegree > 160 || previousWorldDegree < 20) {
//			if (previousWorldDegree < 20) {
//				previousDegree = 90;
//				adjustY = 0.2f;
//			}
//			else if (previousWorldDegree > 160) {
//				previousDegree = 270;
//				adjustY = -0.2f;
//			}
//			distance *= 2.5f;
//		}
//
//
//		float degree = Random.Range (-30, 30) + previousDegree;
//		if ((degree > 150 && degree < 180) || degree < 30)
//			degree = 90;
//		else if ((degree > 180 && degree < 210) || degree > 330)
//			degree = 270;
////		if (degree > 360)
////			degree -= 360;
//
//		float newX = Mathf.Cos ((degree-90f) * Mathf.Deg2Rad) * distance * 0.3f; // degree
//		float newY = Mathf.Abs(Mathf.Sin ((degree-90f) * Mathf.Deg2Rad)) * distance * 2.0f-0.7f/*+adjustY*/; // height
//
//		newY = Mathf.Clamp (newY, -0.7f, 0.5f);
//
//		// Check the boundary
////		if (newY > heightBoundary) {
////			if(newX > previo
////			degree += 90;
////			newY = Mathf.Sin (degree) * distance;
////		} else if (newY < -heightBoundary) {
////		}
////
//
//		float worldDegree = (newX / (2 * Mathf.PI * radius)) * 360;
//
//
//
//		previousTime = time;
//		previousDegree = degree;
//		previousWorldDegree += worldDegree;
//		Debug.Log ("Degree: " + degree + ", WorldDegree: " + previousWorldDegree);
//		return new Vector3 (
//			radius * Mathf.Cos (previousWorldDegree * Mathf.Deg2Rad),
//			newY,
//			radius * Mathf.Sin (previousWorldDegree * Mathf.Deg2Rad)
//		);
//
//	}
}
