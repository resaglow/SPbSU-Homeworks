// Web requests in CPS style
// Victor Polozov (c) 2014
// see WebR.getUrl function

module WebR

open System
open System.IO
open System.Net
open System.Text

// Async store
//  StringBuilder requestData
//  byte[] BufferRead
//  HttpWebRequest request
//  HttpWebResponse response
//  Stream streamResponse
type RequestState = StringBuilder * byte[] * HttpWebRequest * HttpWebResponse * Stream
let private BUFFER_SIZE = 1024

// Download page in CPS style
let getUrl url fn =
    // Callback for HTML stream read
    let rec readCallback (asyncResult : IAsyncResult) =
        let myRequestState = asyncResult.AsyncState :?> RequestState
        let (requestData, BufferRead, _, _, responseStream) = myRequestState
        let read = responseStream.EndRead( asyncResult );

        // Read the HTML page and then print it to the console. 
        if read > 0 then
            requestData.Append(Encoding.ASCII.GetString(BufferRead, 0, read)) |> ignore
            let asynchronousResult = responseStream.BeginRead( BufferRead, 0, BUFFER_SIZE, new AsyncCallback(readCallback), myRequestState)
            ()
        else
            // Finally call callback
            fn <| requestData.ToString()
            responseStream.Close()


    // Create WebRequest
    let webRequest = WebRequest.Create(new Uri(url)) :?> HttpWebRequest

    // Do a web request
    let asyncResult = 
        
        let initialRequestState : RequestState = (new StringBuilder(""), Array.create BUFFER_SIZE (byte 0), webRequest, null, null)
        webRequest.BeginGetResponse(
            new AsyncCallback(fun asyncResult -> 
                let (requestData, BufferRead, myHttpWebRequest, _, responseStream) = asyncResult.AsyncState :?> RequestState 
                let response = myHttpWebRequest.EndGetResponse(asyncResult) :?> HttpWebResponse
 
                // Stream to read HTML from network
                let responseStream = response.GetResponseStream()
                // Update state
                let myRequestState : RequestState = (requestData, BufferRead, myHttpWebRequest, response, responseStream)
                  
                // Let's read HTML itself. With readCallback callback.
                let asynchronousInputRead = 
                    responseStream.BeginRead(BufferRead, 0, BUFFER_SIZE, 
                        new AsyncCallback(readCallback), 
                        myRequestState)
                ()
            ),
            initialRequestState)

    // return unit
    ()
