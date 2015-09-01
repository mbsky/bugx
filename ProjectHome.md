Allow web developers to log errors on production environment and store them into xml based file.

This file will be use to generate Error reports and even replay bug scenarios.

_This project is in early stage of development, there aren't any official releases._
_But you can download the source code from repository ([svn](http://bugx.googlecode.com/svn/trunk/))_

**Goals**:
  * [HttpModule](http://msdn2.microsoft.com/en-us/library/system.web.ihttpmodule.aspx) to log error in bugx file (simply an xml file which is gzipped)
    * saved information:
      * Request.Form (including ViewState)
      * Request.QueryString
      * Request.Session
      * Request.ServerVariables
      * Request.Headers
      * HttpContext.Current.Error
  * A winform which is able to open a bugx file and replay the bug scenario on developer's computer
  * Firefox extension to monitor website online status and track new error with AJAX requests between Browser and an [HttpHandler](http://msdn2.microsoft.com/en-us/library/system.web.ihttphandler.aspx).
    * Notifications when a new bug is encountered / web server is down.
    * Customizable check interval
    * Monitor multiple server
    * Make some graphics about qos.