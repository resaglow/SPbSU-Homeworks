(*  Artem Lobanov (c) 2014
    Getting unique image urls from the list of webpages via CPS
*)

module ImageUrlsCPS

open System
open WebR
open MapCPS

let myUrlList = ["http://vk.com/durov"]

// given a webpage HTML outputs a list of image urls is there are >= 5 such
let rec getUrlList (curIndex:int) (curList:string list) (url:string) cont = 
   let imgIndex = url.IndexOf("<img", curIndex)

   if imgIndex = -1 then if curList.Length < 5 then cont [] else cont curList
   else 
      let urlStartIndex = url.IndexOf("src=\"", imgIndex) + 5 // srcIndex + {src="}
      let urlEndIndex = url.IndexOf("\"", urlStartIndex)

      let curUrl = url.Substring(urlStartIndex, urlEndIndex - urlStartIndex)

      getUrlList (urlEndIndex + 1) (curUrl :: curList) url cont

let imageList = mapCPS myUrlList getUrl (fun htmlList -> mapCPS htmlList (getUrlList 0 []) (printfn "%A"))

ignore <| Console.ReadLine()
