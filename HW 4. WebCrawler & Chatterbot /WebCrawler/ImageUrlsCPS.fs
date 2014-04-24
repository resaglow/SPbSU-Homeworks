(*  Artem Lobanov (c) 2014
    Getting unique image urls from the list of webpages via CPS
*)

module ImageUrlsCPS

open System
open WebR

let myUrlList = [ "http://www.yandex.ru"; "http://www.google.com";  "http://vk.com" ]


let rec getUrlList (url:string) (curIndex:int) (curList:string list) cont = 
   let imgIndex = url.IndexOf("<img", curIndex)

   if imgIndex = -1 then if curList.Length < 5 then cont [] else cont curList
   else 
      let urlStartIndex = url.IndexOf("src=\"", imgIndex) + 5 // srcIndex + {src="}
      let urlEndIndex = url.IndexOf("\"", urlStartIndex)

      let curUrl = url.Substring(urlStartIndex, urlEndIndex - urlStartIndex)

      getUrlList url (urlEndIndex + 1) (curUrl :: curList) cont


let rec getImageList urlList cont = 
   match urlList with
   | [] -> cont []
   | firstUrl :: otherUrls -> getUrl firstUrl (fun siteText -> getUrlList siteText 0 [] (fun urlList -> getImageList otherUrls (fun otherUrlsList -> cont (Seq.toList (Seq.distinct (urlList @ otherUrlsList))))))

getImageList myUrlList (printfn "%A")

ignore (Console.ReadLine())
