//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// UIDragDropItem is a base script for your own Drag & Drop operations.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
		public enum Restriction
		{
				None,
				Horizontal,
				Vertical,
				PressAndHold,
		}

		public enum Scene
		{
				NeuronSelection,
				SortingStation,
				RoomInventory
		}

		/// <summary>
		/// What kind of restriction is applied to the drag & drop logic before dragging is made possible.
		/// </summary>

		public Restriction restriction = Restriction.None;
		public Scene scene = Scene.NeuronSelection;
		public Vector3 start;
		public bool putSomewhereElse = false;
	
		/// <summary>
		/// Whether a copy of the item will be dragged instead of the item itself.
		/// </summary>

		public bool cloneOnDrag = false;
		public bool inventoryClone = false;
		public Vector3 scaleInInventory;

		public float coeff;

		/// <summary>
		/// How long the user has to press on an item before the drag action activates.
		/// </summary>

		[HideInInspector]
		public float
				pressAndHoldDelay = 0.2f;

#region Common functionality

		protected Transform mTrans;
		protected Transform mParent;
		protected Collider mCollider;
		protected UIButton mButton;
		protected UIRoot mRoot;
		protected UIGrid mGrid;
		protected UITable mTable;
		protected int mTouchID = int.MinValue;
		protected float mPressTime = 0f;
		protected UIDragScrollView mDragScrollView = null;
		protected Restriction originalRestriction;

		/// <summary>
		/// Cache the transform.
		/// </summary>

		protected virtual void Start ()
		{
				mTrans = transform;
				mCollider = collider;
				mButton = GetComponent<UIButton> ();
				mDragScrollView = GetComponent<UIDragScrollView> ();
				originalRestriction = restriction;
		}

		/// <summary>
		/// Record the time the item was pressed on.
		/// </summary>

		void OnPress (bool isPressed)
		{
				if (isPressed) {
						mPressTime = RealTime.time; 
				}
		}

		/// <summary>
		/// Start the dragging operation.
		/// </summary>

		void OnDragStart ()
		{

				if (!enabled || mTouchID != int.MinValue) {
						Debug.Log (enabled + " " + mTouchID);

						if (!inventoryClone)  // dilara test
								return;
				}

				// If we have a restriction, check to see if its condition has been met first
				if (restriction != Restriction.None) {
						if (restriction == Restriction.Horizontal) {
								Vector2 delta = UICamera.currentTouch.totalDelta;
								if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y))
										return;
						} else if (restriction == Restriction.Vertical) {
								Vector2 delta = UICamera.currentTouch.totalDelta;
								if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y))
										return;
						} else if (restriction == Restriction.PressAndHold) {
								if (mPressTime + pressAndHoldDelay > RealTime.time) {
										Debug.Log ("Ready to move");					
										return;
								}		
						}
				}

				if (cloneOnDrag) {
						GameObject clone = NGUITools.AddChild (transform.parent.gameObject, gameObject);
						clone.transform.localPosition = transform.localPosition;
						clone.transform.localRotation = transform.localRotation;
						clone.transform.localScale = transform.localScale;

						UIButtonColor bc = clone.GetComponent<UIButtonColor> ();
						if (bc != null)
								bc.defaultColor = GetComponent<UIButtonColor> ().defaultColor;

						UICamera.currentTouch.dragged = clone;

						UIDragDropItem item = clone.GetComponent<UIDragDropItem> ();
						item.Start ();
						item.OnDragDropStart ();
				} else if (inventoryClone) {
						OnDragDropStartInventory ();
				} else {
						OnDragDropStart ();
				}
		}

		/// <summary>
		/// Perform the dragging.
		/// </summary>

		void OnDrag (Vector2 delta)
		{
				if (!enabled || mTouchID != UICamera.currentTouchID)
						return;
				OnDragDropMove ((Vector3)delta * mRoot.pixelSizeAdjustment);
		}

		/// <summary>
		/// Notification sent when the drag event has ended.
		/// </summary>

		void OnDragEnd ()
		{
				if (!enabled || mTouchID != UICamera.currentTouchID)
						return;
				OnDragDropRelease (UICamera.hoveredObject, null);
		}

#endregion

		/// <summary>
		/// Perform any logic related to starting the drag & drop operation.
		/// </summary>

		protected virtual void OnDragDropStart ()
		{
				// Automatically disable the scroll view
				if (mDragScrollView != null)
						mDragScrollView.enabled = false;

				// Disable the collider so that it doesn't intercept events
				if (mButton != null)
						mButton.isEnabled = false;
				else if (mCollider != null)
						mCollider.enabled = false;

				mTouchID = UICamera.currentTouchID;
				mParent = mTrans.parent;
				mRoot = NGUITools.FindInParents<UIRoot> (mParent);
				mGrid = NGUITools.FindInParents<UIGrid> (mParent);
				mTable = NGUITools.FindInParents<UITable> (mParent);

				// Re-parent the item
				if (UIDragDropRoot.root != null)
						mTrans.parent = UIDragDropRoot.root;

				Vector3 pos = mTrans.localPosition;
				pos.z = 0f;
				mTrans.localPosition = pos;

				TweenPosition tp = GetComponent<TweenPosition> ();
				if (tp != null)
						tp.enabled = false;

				SpringPosition sp = GetComponent<SpringPosition> ();
				if (sp != null)
						sp.enabled = false;

				// Notify the widgets that the parent has changed
				NGUITools.MarkParentAsChanged (gameObject);

				if (mTable != null)
						mTable.repositionNow = true;
				if (mGrid != null)
						mGrid.repositionNow = true;
		}

		protected virtual void OnDragDropStartInventory ()
		{

				// Automatically disable the scroll view
				if (mDragScrollView != null)
						mDragScrollView.enabled = false;
		
				// Disable the collider so that it doesn't intercept events
				if (mButton != null)
						mButton.isEnabled = false;
				else if (mCollider != null)
						mCollider.enabled = false;
		
				mTouchID = UICamera.currentTouchID;
				mParent = mTrans.parent;
				mRoot = NGUITools.FindInParents<UIRoot> (mParent);
				mGrid = NGUITools.FindInParents<UIGrid> (mParent);
				mTable = NGUITools.FindInParents<UITable> (mParent);
		
				// Re-parent the item
				if (UIDragDropRoot.root != null)
						mTrans.parent = UIDragDropRoot.root;
		
				Vector3 pos = mTrans.localPosition;
				pos.z = 0f;
				mTrans.localPosition = pos;

				UIDragDropContainer container = NGUITools.FindInParents<UIDragDropContainer> (mParent);

				if (container != null && container.resizeChild)
						mTrans.localScale = coeff * mTrans.localScale;

		
				TweenPosition tp = GetComponent<TweenPosition> ();
				if (tp != null)
						tp.enabled = false;
		
				SpringPosition sp = GetComponent<SpringPosition> ();
				if (sp != null)
						sp.enabled = false;
		
				// Notify the widgets that the parent has changed
				NGUITools.MarkParentAsChanged (gameObject);
		
				if (mTable != null)
						mTable.repositionNow = true;
				if (mGrid != null)
						mGrid.repositionNow = true;
		}

		/// <summary>
		/// Adjust the dragged object's position.
		/// </summary>

		protected virtual void OnDragDropMove (Vector3 delta)
		{
				mTrans.localPosition += delta;
		}

		/// <summary>
		/// Drop the item onto the specified object.
		/// </summary>

		protected virtual void OnDragDropRelease (GameObject surface, Transform t)
		{
				if (surface == null) {
						putSomewhereElse = true;
				} else {
						Debug.Log (surface.name);
						putSomewhereElse = false;
						Debug.Log ("Could not put it anywhere");

				}
				if (!cloneOnDrag) {
						mTouchID = int.MinValue;

						// Re-enable the collider
						if (mButton != null)
								mButton.isEnabled = true;
						else if (mCollider != null)
								mCollider.enabled = true;

						// Is there a droppable container?
						UIDragDropContainer container = surface ? NGUITools.FindInParents<UIDragDropContainer> (surface) : null;

						if (container != null) {
								Debug.Log ("Container name " + container.name);
								// Container found -- parent this object to the container
								if (UIDragDropRoot.root != null && mTrans.parent == UIDragDropRoot.root) {
										Transform tr = (t != null) ? t : container.transform;
										mTrans.parent = (container.reparentTarget != null) ? container.reparentTarget : tr;

										Vector3 pos = mTrans.localPosition;
										pos.z = 0f;
										pos.y = container.bottomLine.y - GetComponentInChildren<UI2DSprite> ().height / 200;

										mTrans.localPosition = pos;
										if (container.scaleToOne) {
												mTrans.localScale = scaleInInventory;
												gameObject.GetComponent<BoxCollider> ().size = new Vector3 (60f, 60f, 0);
												gameObject.GetComponent<BoxCollider> ().center = new Vector3 (0, 0, 0);
												Debug.Log ("LOCAL SCALE " + mTrans.localScale);
										}   
										
										Debug.Log (mTrans.localScale);
								}

								Debug.Log ("ORIGINAL RESTRICTION " + originalRestriction);

								if (container.noRestriction) {
										restriction = Restriction.None;
								} else {
										restriction = originalRestriction;
								}

						} else {
								// No valid container under the mouse -- revert the item's parent
								Transform tr = (t != null) ? t : mParent;
								mTrans.parent = tr;
                   
						}

						// Update the grid and table references
						mParent = mTrans.parent;
						mGrid = NGUITools.FindInParents<UIGrid> (mParent);
						mTable = NGUITools.FindInParents<UITable> (mParent);

						// Re-enable the drag scroll view script
						if (mDragScrollView != null)
								mDragScrollView.enabled = true;

						// Notify the widgets that the parent has changed
						NGUITools.MarkParentAsChanged (gameObject);

						if (mTable != null)
								mTable.repositionNow = true;
						if (mGrid != null)
								mGrid.repositionNow = true;
				} else
						NGUITools.Destroy (gameObject);
		}
}
