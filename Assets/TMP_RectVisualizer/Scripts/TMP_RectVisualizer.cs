using KoganeUnityLib.TMP_RectVisualizer_Internal;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KoganeUnityLib
{
	/// <summary>
	/// TMP_RectVisualizer で使用するパラメータを管理するクラス
	/// </summary>
	public sealed class TMP_RectVisualizerData
	{
		//==============================================================================
		// プロパティ
		//==============================================================================
		public float	OutlineSize		{ get; set; }	// アウトラインのサイズ
		public Color	OutlineColor	{ get; set; }	// アウトラインの色

		//==============================================================================
		// 関数
		//==============================================================================
		public TMP_RectVisualizerData(){ }

		public TMP_RectVisualizerData
		(
			float	outlineSize	,
			Color	outlineColor
		)
		{
			OutlineSize		= outlineSize	;
			OutlineColor	= outlineColor	;
		}
	}

	/// <summary>
	/// テキストの表示範囲を可視化するクラス
	/// </summary>
	public static class TMP_RectVisualizer
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private static readonly List<GameObject> m_list = new List<GameObject>();

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// テキストの表示範囲を可視化します
		/// </summary>
		public static void Show( float outlineSize, Color outlineColor )
		{
			var data = new TMP_RectVisualizerData
			(
				outlineSize		: outlineSize	,
				outlineColor	: outlineColor
			);

			Show( data );
		}

		/// <summary>
		/// テキストの表示範囲を可視化します
		/// </summary>
		public static void Show( TMP_RectVisualizerData data )
		{
			// すでに可視化している場合はいったん非表示にします
			Hide();

			// すべてのシーンのルートオブジェクトを取得します
			var rootObjects = GetAllScenes()
				.SelectMany( c => c.GetRootGameObjects() )
			;

			// すべての Text コンポーネントを取得します
			var textList = rootObjects
				.SelectMany( c => c.GetComponentsInChildren<Text>( true ) )
				.OfType<MaskableGraphic>()
			;

			// すべての TextMeshProUGUI コンポーネントを取得します
			var textMeshProUGUIList	= rootObjects
				.SelectMany( c => c.GetComponentsInChildren<TextMeshProUGUI>( true ) )
				.OfType<MaskableGraphic>()
			;

			// Text と TextMeshProUGUI のリストを結合します
			var list = textList.Concat( textMeshProUGUIList );

			// アウトラインのサイズと色をキャッシュします
			var outlineSize		= data.OutlineSize	;
			var outlineColor	= data.OutlineColor	;

			foreach ( var n in list )
			{
				// アウトラインを表示するためのゲームオブジェクトを作成します
				var go = new GameObject( "Outline" );

				// テキストオブジェクトの子供にします
				go.transform.SetParent( n.transform );

				// アウトラインのゲームオブジェクトは Hierarchy に表示しないかつ
				// シーンに保存しない設定にします
				go.hideFlags = HideFlags.HideAndDontSave;

				// アウトラインを表示するためのコンポーネントをアタッチします
				go.AddComponent<CanvasRenderer>();

				var squareUI = go.AddComponent<OutlineSquareUI>();

				// アウトラインのサイズと色を設定します
				// また、タップ判定から除外します
				squareUI.raycastTarget	= false			;
				squareUI.outlineSize	= outlineSize	;
				squareUI.color			= outlineColor	;

				// アウトラインのサイズをテキストオブジェクトに合わせます
				var rectTransform = go.GetComponent<RectTransform>();

				rectTransform.anchoredPosition	= Vector3.zero	;
				rectTransform.anchorMin			= Vector2.zero	;
				rectTransform.anchorMax			= Vector2.one	;
				rectTransform.offsetMin			= Vector2.zero	;
				rectTransform.offsetMax			= Vector2.zero	;

				// 非表示にするためのリストに追加します
				m_list.Add( go );
			}
		}

		/// <summary>
		/// テキストの表示範囲を非表示にします
		/// </summary>
		public static void Hide()
		{
			for ( int i = 0; i < m_list.Count; i++ )
			{
				var go = m_list[ i ];

				if ( go == null ) continue;

				GameObject.Destroy( go );
			}

			m_list.Clear();
		}

		/// <summary>
		/// 読み込まれているシーンの一覧を返します
		/// </summary>
		private static IEnumerable<Scene> GetAllScenes()
		{
			for ( int i = 0; i < SceneManager.sceneCount; i++ )
			{
				yield return SceneManager.GetSceneAt( i );
			}
		}
	}
}