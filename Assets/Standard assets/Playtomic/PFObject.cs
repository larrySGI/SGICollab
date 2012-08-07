using System;
using System.Collections.Generic;

public class PFObject
{
	public PFObject () { }

	public string ObjectId;
	public string ClassName;
	public List<PFPointer> Pointers = new List<PFPointer>();
	public Dictionary<string, string> Data = new Dictionary<string, string>();
	public DateTime UpdatedAt;
	public DateTime CreatedAt;
	public String Password;
}
