//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Simple example of an OnDrop event accepting a game object. In this case we check to see if there is a DragDropObject present,
/// and if so -- create its prefab on the surface, then destroy the object.
/// </summary>
/// 
/// 



[AddComponentMenu("NGUI/Examples/Drag and Drop Surface (Example)")]
public class TestSurface : MonoBehaviour
{
		public bool rotatePlacedObject = false;

		public SurfaceType surfaceType;

		public int maxObjCount;

		public int curObjCount = 0;

		/// <summary>
		/// Flip factor of the surface. If assigned -1 in the editor, flips the game object sideways
		/// </summary>
		public int flip = 1;


}