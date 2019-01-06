using UnityEngine;

namespace KoganeUnityLib.TMP_RectVisualizer_Example
{
	public class Example : MonoBehaviour
	{
		private void Update()
		{
			if ( Input.GetKeyDown( KeyCode.Z ) )
			{
				TMP_RectVisualizer.Show( 2, Color.red );
			}
			else if ( Input.GetKeyDown( KeyCode.X ) )
			{
				TMP_RectVisualizer.Hide();
			}
		}
	}
}