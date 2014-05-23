package
{

    import flash.external.ExternalInterface;

    import flash.system.Security;

 	import flash.display.LoaderInfo;
 	
 	import com.unity.UnityNative;

    public class BrowserCommunicator
    {

        //Exposed so that it can be called from the browser JavaScript.
        public static function callFromJavascript() : void
        {

            trace("Javascript successfully called ActionScript function.");

        }

 

        //Sets up an ExternalInterface callback and calls a Javascript function.

        public static function showLoginform( url:String ) : void
        {

            if (ExternalInterface.available)

            {
/*
                try

                {

                    ExternalInterface.addCallback("callFromJavascript", callFromJavascript);

                }

                catch (error:SecurityError)

                {

                    trace("A SecurityError occurred: " + error.message);

                }

                catch (error:Error)

                {

                    trace("An Error occurred: " + error.message);

                }

 */

                ExternalInterface.call('showLoginform', url);

            }

            else

            {

                trace("External interface not available");

            }

        } 
        
        public static function GetURL() : String
        {
        	var output:String = "";
        	var params:Object = LoaderInfo(UnityNative.stage.loaderInfo).parameters;
        	var key:String;
        	
        	for( key in params )
        		output += key + "@" + params[key] + "$";
        		
        	return output;
        }

    }

}