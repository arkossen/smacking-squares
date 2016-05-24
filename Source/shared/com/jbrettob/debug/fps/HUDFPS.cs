using com.jbrettob.core;
using UnityEngine;

namespace com.jbrettob.debug.fps
{
	// hides the component from the menu scripts
	[UnityEngine.AddComponentMenu("")]
	public class HUDFPS:CoreMonoBehaviour
	{ 
		[UnityEngine.SerializeField]
		private float updateInterval = 0.5F;
		
		/// <summary>
		/// FPS accumulated over the interval
		/// </summary>
		private float _accum = 0;
		/// <summary>
		/// Frames drawn over the interval
		/// </summary>
		private int   _frames = 0;
		/// <summary>
		/// Left time for current interval
		/// </summary>
		private float _timeleft;
		private string _format = "";
		private float _fps = 0;
		private Rect _labelRect;
		private Color _guiColor;
		
		void Start()
		{
			_timeleft = updateInterval;
			_labelRect = new Rect(5, 5, 200, 25);
			_guiColor = Color.green;
		}
		
		void OnGUI()
		{
			updateFPS();
			//updateGC();
		}
		
		private void updateFPS()
		{
			_timeleft -= Time.deltaTime;
			_accum += Time.timeScale / Time.deltaTime;
			_frames++;
			
			// Interval ended - update GUI text and start new interval
			if(_timeleft <= 0.0f)
			{
				// display two fractional digits (f2 format)
				_fps = _accum / _frames;
				_format = System.String.Format("{0:F2} FPS", _fps);
			 
				if(_fps < 30)
				{
					_guiColor = Color.yellow;
				}
				else
				{
					if(_fps < 10)
					{
						_guiColor = Color.red;
					}
					else
					{
						_guiColor = Color.green;
					}
					_timeleft = updateInterval;
					_accum = 0.0f;
					_frames = 0;
				}
			}
			
			GUI.color = _guiColor;
			GUI.Label(_labelRect, _format);
		}
		
		private void updateGC()
		{
			// TODO: Work GC out, to show how much memory the games has been using
			long totalMemory = System.GC.GetTotalMemory(false);
			float totalMemoryInMegaBytes = (float)(totalMemory / 1024) / 1024;
			print("Total Memory: " + totalMemoryInMegaBytes);
		}
	}
}