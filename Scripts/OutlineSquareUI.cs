using UnityEngine;
using UnityEngine.UI;

namespace UniTextMeshProRectVisualizer
{
	internal class OutlineSquareUI : MaskableGraphic
	{
		[SerializeField] private float m_outlineSize = 5;

		public float OutlineSize { get => m_outlineSize; set => m_outlineSize = value; }

		protected override void OnPopulateMesh( VertexHelper vh )
		{
			vh.Clear();

			var rect     = rectTransform.rect;
			var outerMin = rect.min;
			var outerMax = rect.max;
			var innerMin = outerMin + new Vector2( m_outlineSize, m_outlineSize );
			var innerMax = outerMax - new Vector2( m_outlineSize, m_outlineSize );

			AddQuad
			(
				vh: vh,
				leftTop: new Vector3( innerMin.x, outerMax.y ),
				rightTop: new Vector3( innerMax.x, outerMax.y ),
				leftBottom: new Vector3( innerMin.x, innerMax.y ),
				rightBottom: new Vector3( innerMax.x, innerMax.y )
			);

			AddQuad
			(
				vh: vh,
				leftTop: new Vector3( innerMax.x, outerMax.y ),
				rightTop: new Vector3( outerMax.x, outerMax.y ),
				leftBottom: new Vector3( innerMax.x, outerMin.y ),
				rightBottom: new Vector3( outerMax.x, outerMin.y )
			);

			AddQuad
			(
				vh: vh,
				leftTop: new Vector3( innerMin.x, innerMin.y ),
				rightTop: new Vector3( innerMax.x, innerMin.y ),
				leftBottom: new Vector3( innerMin.x, outerMin.y ),
				rightBottom: new Vector3( innerMax.x, outerMin.y )
			);

			AddQuad
			(
				vh: vh,
				leftTop: new Vector3( outerMin.x, outerMax.y ),
				rightTop: new Vector3( innerMin.x, outerMax.y ),
				leftBottom: new Vector3( outerMin.x, outerMin.y ),
				rightBottom: new Vector3( innerMin.x, outerMin.y )
			);
		}

		private void AddQuad
		(
			VertexHelper vh,
			Vector2      leftTop,
			Vector2      rightTop,
			Vector2      leftBottom,
			Vector2      rightBottom
		)
		{
			var lt = UIVertex.simpleVert;
			lt.position = leftTop;
			lt.color    = color;

			var rt = UIVertex.simpleVert;
			rt.position = rightTop;
			rt.color    = color;

			var lb = UIVertex.simpleVert;
			lb.position = leftBottom;
			lb.color    = color;

			var rb = UIVertex.simpleVert;
			rb.position = rightBottom;
			rb.color    = color;

			var verts = new[]
			{
				lb,
				rb,
				rt,
				lt,
			};

			vh.AddUIVertexQuad( verts );
		}
	}
}