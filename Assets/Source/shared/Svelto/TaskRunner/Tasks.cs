using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Svelto.Tasks
{
	abstract public class Tasks: IEnumerable
	{
		protected List<IEnumerator> 	registeredEnumerators { get; private set; }

		public bool						isRunning 		{ protected set; get; }
		public int						registeredTasks { get { return registeredEnumerators.Count; } }
		private bool _debug;
		
		public Tasks()
		{
			registeredEnumerators = new List<IEnumerator>();
		}
		
		public void Add(ITask task)
		{
			if (task == null)
				throw new ArgumentNullException();
			
			Add(new AsyncTask(task));
		}
		
		public void Add(IEnumerable enumerable)
		{
			if ((enumerable is Tasks) && (enumerable as Tasks).registeredTasks == 0)
				Debug.LogError("Avoid to Register zero size tasks");
					
			if (enumerable == null)
				throw new ArgumentNullException();
			
			registeredEnumerators.Add(enumerable.GetEnumerator());
		}
 
		public void Add(IEnumerator enumerator)
		{
			if (enumerator == null)
				throw new ArgumentNullException();
			
			registeredEnumerators.Add(enumerator);
		}
		
		abstract public IEnumerator GetEnumerator();

		public bool debug
		{
			get { return _debug; }
			set { _debug = value; }
		}
	}
}

