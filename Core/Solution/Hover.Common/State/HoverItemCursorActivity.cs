﻿using System;
using System.Collections.Generic;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Renderers;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemCursorActivity : MonoBehaviour {

		[Serializable]
		public struct Highlight {
			public HovercursorData Data;
			public Vector3 NearestWorldPos;
			public float Distance;
			public float Progress;
		}

		public Highlight? NearestHighlight { get; private set; }
		public List<Highlight> Highlights { get; private set; }
		
		public HovercursorDataProvider CursorDataProvider;
		public HoverRendererController ProximityProvider;
		public bool AllowCursorHighlighting = true;

		private readonly BaseInteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemCursorActivity() {
			vSettings = new BaseInteractionSettings(); //TODO: access from somewhere
			Highlights = new List<Highlight>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				FindObjectOfType<HovercursorDataProvider>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Highlights.Clear();
			NearestHighlight = null;

			if ( !AllowCursorHighlighting || !ProximityProvider ) {
				return;
			}

			foreach ( HovercursorData data in CursorDataProvider.Cursors ) {
				Highlight high = CalculateHighlight(data);
				Highlights.Add(high);

				if ( NearestHighlight == null ||
							high.Distance < ((Highlight)NearestHighlight).Distance ) {
					NearestHighlight = high;
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Highlight? GetHighlight(CursorType pType) {
			foreach ( Highlight high in Highlights ) {
				if ( high.Data.Type == pType ) {
					return high;
				}
			}

			return null;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Highlight CalculateHighlight(HovercursorData pData) {
			var high = new Highlight();
			high.Data = pData;
			
			if ( !Application.isPlaying ) {
				return high;
			}
			
			Vector3 cursorWorldPos = pData.transform.position;
			
			high.NearestWorldPos = ProximityProvider.GetNearestWorldPosition(cursorWorldPos);
			high.Distance = (cursorWorldPos-high.NearestWorldPos).magnitude;
			high.Progress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, high.Distance*vSettings.ScaleMultiplier);
			
			return high;
		}

	}

}