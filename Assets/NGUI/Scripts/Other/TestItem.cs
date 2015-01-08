//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;


[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class TestItem : UIDragDropItem
{
		/// <summary>
		/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
		/// </summary>

		public GameObject prefab;

		public Vector3 sizeInRoom;

		public SurfaceType surfaceType;

		public ItemType itemType;

		public int occupancy;

		public List<Vector3> dragableAreasPositions;
		public List<Vector3> dragableAreasScales;
		public int maxObjPermission = 0;

		protected override void OnDragDropRelease (GameObject surface, Transform t)
		{
				
				if (surface != null) {
						TestSurface dds = surface.GetComponent<TestSurface> ();

						if (dds != null) {
								Debug.Log (gameObject);	
								
								Debug.Log (surface.GetComponent<SpriteRenderer> ().sprite.border);


								Debug.Log (surface.renderer.bounds.size + "  " + surface.renderer.bounds.max + " " + surface.renderer.bounds.min);
												
								// Destroy this icon as it's no longer needed
								//NGUITools.Destroy (gameObject);
								return;
						}
				}
				base.OnDragDropRelease (surface, null);
		}


}
