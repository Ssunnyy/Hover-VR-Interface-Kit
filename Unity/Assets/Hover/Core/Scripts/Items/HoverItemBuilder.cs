﻿using Hover.Core.Items.Managers;
using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemBuilder : MonoBehaviour {

		public HoverItem.HoverItemType ItemType = HoverItem.HoverItemType.Selector;
		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;
		public bool ClickToBuild = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ButtonRendererPrefab == null ) {
				ButtonRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaButtonRectRenderer-Default");
			}

			if ( SliderRendererPrefab == null ) {
				SliderRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaSliderRectRenderer-Default");
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( ClickToBuild ) {
				ClickToBuild = false;
				UnityUtil.FindOrAddHoverManagerPrefab();
				PerformBuild();
				DestroyImmediate(this, false);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PerformBuild() {
			gameObject.AddComponent<TreeUpdater>();

			HoverItem item = gameObject.AddComponent<HoverItem>();
			item.ItemType = ItemType;

			HoverItemHighlightState highState = gameObject.AddComponent<HoverItemHighlightState>();

			gameObject.AddComponent<HoverItemSelectionState>();

			HoverRendererUpdater rendUp = gameObject.AddComponent<HoverRendererUpdater>();
			rendUp.ButtonRendererPrefab = ButtonRendererPrefab;
			rendUp.SliderRendererPrefab = SliderRendererPrefab;
			highState.ProximityProvider = rendUp;

			rendUp.TreeUpdate(); //forces the renderer prefab to load
		}

	}

}
