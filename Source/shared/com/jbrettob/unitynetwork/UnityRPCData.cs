using com.jbrettob.core;
using System.Collections.Generic;

public class UnityRPCData:CoreObject {
    private string _eventName;
    private List<string> _data;
	
    public void parseData(string eventName, string data) {
        string[] newData = data.Split(',');
		
        _data = new List<string>();
        for (int i = 0; i < newData.Length; i++) {
            _data.Add(newData [i]);
        }
		
        _eventName = eventName;
    }
	
    public int getInt(int index) {
        return int.Parse(_data [index].ToString());
    }
	
    public float getFloat(int index) {
        return float.Parse(_data [index].ToString());
    }
	
    public string getString(int index) {
        return _data [index].ToString();
    }
	
    public string eventName {
        get { return _eventName; }
    }
	
    public int Length {
        get { return _data.Count; }
    }

	public override string ToString() {
		return string.Format("[UnityRPCData: eventName={0}, _data={1}]", eventName, _data);
	}
}