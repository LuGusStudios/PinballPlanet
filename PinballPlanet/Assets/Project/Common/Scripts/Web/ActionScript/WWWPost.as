package
{
	import flash.events.*;
	import flash.net.*;
	import flash.net.URLLoader;

	
	public class WWWPost
	{
		var request:URLRequest;
		var loader:URLLoader;
		
		public static var response : String = "";

		public function Post( url : String, params : String )
		{
			request = new URLRequest();
			request.url = url;
			request.method = URLRequestMethod.POST;

			var variables : URLVariables = new URLVariables(params);
			request.data = variables;
	
			loader = new URLLoader();
			loader.dataFormat = URLLoaderDataFormat.TEXT;
			//loader.addEventListener(Event.IOErrorEvent, loaderEvent);
			configureListeners( loader );
			loader.addEventListener(Event.COMPLETE, loaderCompleteHandler);
			loader.load(request);
		}

		/*
		public function AddScore()
		{
			request = new URLRequest();
			request.url = "http://accept.ketnet.be/ws/game/addscore";
			request.method = URLRequestMethod.POST;

			var variables : URLVariables = new URLVariables();
			variables.game_id = "50741f7871483";
			variables.score = 50;
			request.data = variables;
	
			loader = new URLLoader();
			loader.dataFormat = URLLoaderDataFormat.TEXT;
			//loader.addEventListener(Event.IOErrorEvent, loaderEvent);
			configureListeners( loader );
			loader.addEventListener(Event.COMPLETE, loaderCompleteHandler);
			loader.load(request);
		}	
		*/

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
