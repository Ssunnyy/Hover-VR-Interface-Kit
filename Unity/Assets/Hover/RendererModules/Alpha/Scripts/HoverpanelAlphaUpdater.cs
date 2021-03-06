using System.Collections.Generic;
using Hover.Core.Items;
using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using Hover.InterfaceModules.Panel;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverpanelInterface))]
	[RequireComponent(typeof(HoverpanelRowTransitioner))]
	public class HoverpanelAlphaUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		private readonly List<IItemData> vItemDataResults;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverpanelAlphaUpdater() {
			vItemDataResults = new List<IItemData>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateWithTransitions();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithTransitions() {
			HoverpanelRowTransitioner row = gameObject.GetComponent<HoverpanelRowTransitioner>();
			HoverpanelInterface panel = gameObject.GetComponent<HoverpanelInterface>();

			FadeRow(panel.PreviousRow, 1-row.TransitionProgressCurved);
			FadeRow(panel.ActiveRow, row.TransitionProgressCurved);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FadeRow(HoverLayoutRectRow pRow, float pAlpha) {
			if ( pRow == null || !pRow.gameObject.activeSelf ) {
				return;
			}

			pRow.GetComponentsInChildren(true, vItemDataResults);

			for ( int i = 0 ; i < vItemDataResults.Count ; i++ ) {
				FadeItem(vItemDataResults[i], pAlpha);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FadeItem(IItemData pItemData, float pAlpha) {
			HoverAlphaRendererUpdater rendUp = 
				pItemData.gameObject.GetComponentInChildren<HoverAlphaRendererUpdater>();

			if ( rendUp == null ) {
				return;
			}

			float currAlpha = (pItemData.IsEnabled ? rendUp.EnabledAlpha : rendUp.DisabledAlpha);

			rendUp.Controllers.Set(HoverAlphaRendererUpdater.MasterAlphaName, this);
			rendUp.MasterAlpha = Mathf.Lerp(0, currAlpha, pAlpha);
		}

	}

}
