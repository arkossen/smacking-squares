/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

using System;
using System.Collections.Generic;
using com.jbrettob.debug;
using UnityEngine;

namespace com.jbrettob.core {
	/// <summary>
	/// Abstract mono behaviour, which contains most used funtionalities.
	/// </summary>
	public abstract class CoreMonoBehaviour:MonoBehaviour, ICoreMonoBehaviour, IDebuggable {
		[System.NonSerialized]
		public new Transform
			transform;
		//
		private bool _debug = false;
		
		#region IAbstractMonoBehaviour implementation

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		public virtual void Awake() {
			transform = gameObject.transform;
		}
		
		/// <summary>
		/// Invoke the specified callback and time.
		/// </summary>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		/// <param name='time'>
		/// Time.
		/// </param>
		public void Invoke(Action callback, float time) {
			base.Invoke(callback.Method.Name, time);
		}
		
		/// <summary>
		/// Invokes the repeating.
		/// </summary>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		/// <param name='time'>
		/// Time.
		/// </param>
		/// <param name='repeatRate'>
		/// Repeat rate.
		/// </param>
		public void InvokeRepeating(Action callback, float time, float repeatRate) {
			base.InvokeRepeating(callback.Method.Name, time, repeatRate);
		}
	
		/// <summary>
		/// Gets the interface component.
		/// </summary>
		/// <returns>
		/// The interface component.
		/// </returns>
		/// <typeparam name='I'>
		/// The 1st type parameter.
		/// </typeparam>
		public I GetInterfaceComponent<I>() where I:class {
			return GetComponent(typeof(I)) as I;
		}
		#endregion
	
		/// <summary>
		/// Finds the objects of interface.
		/// </summary>
		/// <returns>
		/// The objects of interface.
		/// </returns>
		/// <typeparam name='I'>
		/// The 1st type parameter.
		/// </typeparam>
		public static List<I> FindObjectsOfInterface<I>() where I:class {
			List<I> list = new List<I>();
			MonoBehaviour[] monoBehaviours = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
	
			foreach (MonoBehaviour behaviour in monoBehaviours) {
				I component = behaviour.GetComponent(typeof(I)) as I;
	
				if (component != null) {
					list.Add(component);
				}
			}
			
			return list;
		}
		
		#region IDebuggable implementation
		public void logDebug(string message) {
			logDebug(message, this);
		}
		
		public void logDebug(string message, Component component) {
			if (DebugLoggerManager.USE_LOG) {
				Debug.Log(DebugLoggerManager.getInstance.logDebug("[" + component + "] " + message), component);
			} else {
				Debug.Log("[" + component + "] " + message, component);
			}
		}
		
		public void logWarning(string message) {
			logWarning(message, this);
		}
		
		public void logWarning(string message, Component component) {
			if (DebugLoggerManager.USE_LOG) {
				Debug.LogWarning(DebugLoggerManager.getInstance.logWarning("[" + component + "] " + message));
			} else {
				Debug.LogWarning("[" + component + "] " + message);
			}
		}
		
		public void logError(string message) {
			logError(message, this);
		}
		
		public void logError(string message, Component component) {
			if (DebugLoggerManager.USE_LOG) {
				Debug.LogError(DebugLoggerManager.getInstance.logError("[" + component + "] " + message));
			} else {
				Debug.LogError("[" + component + "] " + message);
			}
		}
		
		public bool debug {
			get {
				return _debug;
			}
	
			set {
				_debug = value;
			}
		}
		#endregion
	
		#region ITransform implementation
		public virtual float x {
			get { return transform.position.x; }
			set { transform.position = new Vector3(value, y, z); }
		}
		public virtual float y {
			get { return transform.position.y; }
			set { transform.position = new Vector3(x, value, z); }
		}
		public virtual float z {
			get { return transform.position.z; }
			set { transform.position = new Vector3(x, y, value); }
		}
	
		public virtual float rotationX {
			get { return transform.eulerAngles.x; }
			set { transform.eulerAngles = new Vector3(value, rotationY, rotationZ); }
		}
		public virtual float rotationY {
			get { return transform.eulerAngles.y; }
			set { transform.eulerAngles = new Vector3(rotationX, value, rotationZ); }
		}
		public virtual float rotationZ {
			get { return transform.rotation.eulerAngles.z; }
			set { transform.eulerAngles = new Vector3(rotationX, rotationY, value); }
		}
		#endregion
	
		#region IDestruct implementation
		public virtual void destruct() {
			if (transform != null) {
				transform = null;
			}
			
			if (gameObject != null) {
				Destroy(gameObject);
			} else {
				Destroy(this);
			}
		}
		#endregion
	}
}