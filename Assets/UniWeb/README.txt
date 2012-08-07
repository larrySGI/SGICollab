UniWeb
------


UniWeb allows you to use a common HTTP api across Unity Web players, iOS
and desktop builds.

If you're happy with the builtin WWW class, you don't need UniWeb. If you 
want to read response codes, set headers,  cache responses and other regular
HTTP stuff, then UniWeb is what you need. The .NET WebRequest classes are
another alternative, however they currently do not work on iOS devices or
web player builds.

NB: To use UniWeb in the Web Player, you must have a server running on
the host which supplies a crossdomain.xml file. See: http://bit.ly/h6QY0M


How to do a HTTP GET request.
-----------------------------

var request = new HTTP.Request("GET", url);
//set headers
request.SetHeader("Hello", "World");
request.Send();
while(!request.isDone) yield return new WaitForEndOfFrame();
if(request.exception != null) 
    Debug.LogError(request.exception);
else {
    var response = request.response;
    //inspect response code
    Debug.Log(response.status);
    //inspect headers
    Debug.Log(response.GetHeader("Content-Type"));
    //Get the body as a byte array
    Debug.Log(response.bytes);
    //Or as a string
    Debug.Log(response.Text);
}


How to do a HTTP POST request.
------------------------------

A post request is much the same as the GET request, however you assign
a value to the request.bytes field, or the request.Text property.

var request = new HTTP.Request("POST", url);
request.Text = "Hello from UniWeb!";
request.Send();


How to cache things to disk.
----------------------------

One of the great things about HTTP is the ability to cache items to disk.
UniWeb will do this automaically unless you set the request.useCache variable to false.


Support
-------

Free support is available from support@differentmethods.com.

