using System;
using System.Collections.Generic;

public class PFQuery
{
	public PFQuery() { }
	
	public String ClassName;
	public List<PFPointer> WherePointers = new List<PFPointer>();
	public Dictionary<String, String> WhereData = new Dictionary<String, String>();
	public String Order;
	public int Limit = 10;
}
