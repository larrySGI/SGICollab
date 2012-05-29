using System;

public class PFPointer
{
	public string FieldName;
	public PFObject PObject;
	
	public PFPointer(string fieldname, PFObject po)
	{
		FieldName = fieldname;
		PObject = po;
	}
}
