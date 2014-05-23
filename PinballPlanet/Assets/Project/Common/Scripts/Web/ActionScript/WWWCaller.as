package
{
	import flash.events.*;
	import flash.net.*;
	import flash.net.URLLoader;

	
	public class WWWCaller
	{
		var request:URLRequest;
		var loader:URLLoader;
		
		public static var response : String = "";
		
		public function DoWebRequest( url : String) : void
		{
			request = new URLRequest();
			request.url = url; //"http://195.178.163.254/summergames/test.php";
			request.method = URLRequestMethod.GET;
	
			loader = new URLLoader();
			loader.dataFormat = URLLoaderDataFormat.TEXT;
			//loader.addEventListener(Event.IOErrorEvent, loaderEvent);
			configureListeners( loader );
			loader.addEventListener(Event.COMPLETE, loaderCompleteHandler);
			loader.load(request);
		}	

		public function loaderCompleteHandler(e:Event):void
		{
			response += "FINISHED : " + e.target.data;
		}

	private function configureListeners(dispatcher:IEventDispatcher):void {
            //dispatcher.addEventListener(Event.COMPLETE, completeHandler);
            dispatcher.addEventListener(Event.OPEN, openHandler);
            dispatcher.addEventListener(ProgressEvent.PROGRESS, progressHandler);
            dispatcher.addEventListener(SecurityErrorEvent.SECURITY_ERROR, securityErrorHandler);
            dispatcher.addEventListener(HTTPStatusEvent.HTTP_STATUS, httpStatusHandler);
            dispatcher.addEventListener(IOErrorEvent.IO_ERROR, ioErrorHandler);
        }

        private function openHandler(event:Event):void {
			response += "Problem? : " + event;
        }

        private function progressHandler(event:ProgressEvent):void {

			response += "Problem? : " + event.bytesLoaded + " / " + event.bytesTotal;
        }

        private function securityErrorHandler(event:SecurityErrorEvent):void {
			response += "Problem? : " + event;
        }

        private function httpStatusHandler(event:HTTPStatusEvent):void {
			response += "Problem? : " + event;
        }

        private function ioErrorHandler(event:IOErrorEvent):void {
			response += "Problem? : " + event;
        }
	}
}
