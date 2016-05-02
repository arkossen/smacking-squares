/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

using UnityEngine;
using System.Collections;

namespace com.jbrettob.core
{
	/// <summary>
	/// Interface transform.
	/// </summary>
	public interface ITransform
	{
		Transform transform  { get; set; }
	
		float x { get; set; }
		float y { get; set; }
		float z { get; set; }
	
		float rotationX { get; set;}
		float rotationY { get; set;}
		float rotationZ { get; set;}
	}
}